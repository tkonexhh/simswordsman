using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class HeroTrialMgr : MonoBehaviour, IMgr, IHeroTrialStateHander
    {
        private BattleField m_BattleField = null;
        private HeroTrialData m_DbData = null;

        private HeroTrialStateMachine m_StateMachine = null;
        private HeroTrialStateID m_CurState = HeroTrialStateID.None;

        private FightGroup m_FightGroup;

        private bool m_IsInTrial = false;

        private DateTime m_TrialStartTime;

        private float m_LeftTimeUpdateInterval = 1;
        private float m_LeftTimeUpdateTime = 0;

        private double m_TrialTotalTime = 10;

        public HeroTrialData DbData { get => m_DbData;}
        public BattleField BattleField { get => m_BattleField; }
        public FightGroup FightGroup { get => m_FightGroup; set => m_FightGroup = value; }

        #region IMgr
        public void OnInit()
        {
            m_DbData = GameDataMgr.S.GetClanData().heroTrialData;

            m_BattleField = FindObjectOfType<BattleField>();
            m_BattleField.Init();

            m_StateMachine = new HeroTrialStateMachine(this);
        }

        public void OnUpdate()
        {
            if (m_IsInTrial)
            {
                m_StateMachine.UpdateState(Time.deltaTime);

                if (m_CurState == HeroTrialStateID.Runing)
                {
                    m_LeftTimeUpdateTime += Time.deltaTime;
                    if (m_LeftTimeUpdateTime >= m_LeftTimeUpdateInterval)
                    {
                        m_LeftTimeUpdateTime = 0;

                        if (GetLeftTime() <= 0)
                        {
                            FinishTrial();
                        }
                    }
                }
            }
        }

        public void OnDestroyed()
        {

        }

        public HeroTrialMgr GetHeroTrialMgr()
        {
            return this;
        }
        #endregion

        #region Public Set
        public void OnEnterHeroTrial()
        {
            SetState(m_DbData.state);

            RegisterEvents();

            EventSystem.S.Send(EventID.OnEnterHeroTrial);

            m_IsInTrial = true;

            if (!string.IsNullOrEmpty(m_DbData.trialStartTime))
            {
                m_TrialStartTime = m_DbData.GetStartTime();
            }
        }

        public void OnExitHeroTrial()
        {
            UnregisterEvents();

            SetState(HeroTrialStateID.None);

            GameObject.Destroy(m_FightGroup.OurCharacter.CharacterView.gameObject);
            GameObject.Destroy(m_FightGroup.EnemyCharacter.CharacterView.gameObject);
            m_FightGroup = null;

            m_BattleField.OnBattleEnd();

            EventSystem.S.Send(EventID.OnExitHeroTrial);

            m_IsInTrial = false;
        }

        public void StartTrial(int characterId)
        {
            ClanType clanType = GetNextClanType(m_DbData.clanType);
            m_TrialStartTime = DateTime.Now;
            m_DbData.OnTrialStart(DateTime.Now, characterId, clanType);
            SetState(m_DbData.state);
        }

        public void FinishTrial()
        {
            m_DbData.OnTrialFinished();
            SetState(m_DbData.state);
        }

        public void Reset()
        {
            m_DbData.Reset();
            SetState(m_DbData.state);
        }
        #endregion

        #region Public Get
        public bool IsUnlocked(ref string msg)
        {
            bool isLobbyLevelEnough = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby) >= 5;
            if (isLobbyLevelEnough == false)
            {
                msg = "��Ҫ�����õȼ��ﵽ5��";
                return false;
            }

            bool anyPerfectCharacterReach200Level = false;
            List<CharacterItem> talentCharacterList = MainGameMgr.S.CharacterMgr.GetAllCharacterList().Where(i => i.quality == CharacterQuality.Perfect).ToList();
            if (talentCharacterList != null && talentCharacterList.Count > 0)
            {
                anyPerfectCharacterReach200Level = talentCharacterList.Any(i => i.level >= 200);
            }

            if (anyPerfectCharacterReach200Level == false)
            {
                msg = "��Ҫ����һ����ż������ҵȼ��ﵽ200��";
                return false;
            }

            return true;
        }
        #endregion

        #region Private

        private void RegisterEvents()
        {
            EventSystem.S.Register(EngineEventID.OnDateUpdate, HandleEvent);
            EventSystem.S.Register(EventID.OnSelectedConfirmEvent, HandleEvent);
            EventSystem.S.Register(EventID.OnCharacterInFightGroupDead, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EngineEventID.OnDateUpdate, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnSelectedConfirmEvent, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCharacterInFightGroupDead, HandleEvent);
        }

        private void SetState(HeroTrialStateID state)
        {
            if (m_CurState != state)
            {
                m_CurState = state;

                m_StateMachine.SetCurrentStateByID(state);
            }
        }

        private bool CheckIsTrialReady()
        {
            if (m_DbData.state != HeroTrialStateID.Idle)
                return false;

            DateTime trialStartDay = GameDataMgr.S.GetClanData().heroTrialData.GetStartTime();
            if (trialStartDay.Day != DateTime.Today.DayOfYear)
            {
                return true;
            }

            return false;
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EngineEventID.OnDateUpdate:            
                    bool needRefresh = CheckIsTrialReady();
                    break;
                case (int)EventID.OnSelectedConfirmEvent:
                    Debug.Assert(param.Length > 0, "OnSelectedConfirmEvent param pattern error");
                    Dictionary<int, CharacterItem> selectedCharacterDic = (Dictionary<int, CharacterItem>)param[0];
                    Debug.Assert(selectedCharacterDic.Count > 0, "OnSelectedConfirmEvent selectedCharacterDic count = 0");
                    CharacterItem[] items = selectedCharacterDic.Values.ToArray();

                    StartTrial(items[0].id);
                    break;

            }
        }

        private ClanType GetNextClanType(ClanType curClanType)
        {
            int value = (int)curClanType + 1;
            if (value > (int)ClanType.Xiaoyao)
            {
                value = (int)ClanType.Gaibang;
            }
            ClanType result = (ClanType)value;

            return result;
        }

        private double GetLeftTime()
        {
            TimeSpan time = DateTime.Now - m_TrialStartTime;
            double leftTime = m_TrialTotalTime - time.TotalSeconds;
            Log.i("Left time: " + leftTime);
            return leftTime;
        }
        #endregion
    }

}