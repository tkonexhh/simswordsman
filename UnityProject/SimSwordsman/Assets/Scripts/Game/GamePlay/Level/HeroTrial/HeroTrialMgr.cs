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
            m_StateMachine.UpdateState(Time.deltaTime);
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
        }

        public void OnExitHeroTrial()
        {
            UnregisterEvents();

            EventSystem.S.Send(EventID.OnExitHeroTrial);
        }

        public void StartTrial(int characterId)
        {
            int trialStartDay = DateTime.Today.DayOfYear;
            ClanType clanType = GetNextClanType(m_DbData.clanType);
            m_DbData.OnTrialStart(trialStartDay, characterId, clanType);
            SetState(m_DbData.state);
        }

        public void EndTrial()
        {
            m_DbData.OnTrialEnd();
            SetState(m_DbData.state);
        }
        #endregion

        #region Public Get
        public bool IsUnlocked(ref string msg)
        {
            bool isLobbyLevelEnough = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby) >= 5;
            if (isLobbyLevelEnough == false)
            {
                msg = "需要讲武堂等级达到5级";
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
                msg = "需要至少一个天才级弟子且等级达到200级";
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
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EngineEventID.OnDateUpdate, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnSelectedConfirmEvent, HandleEvent);
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

            int trialStartDay = GameDataMgr.S.GetClanData().heroTrialData.trialStartDay;
            if (trialStartDay != DateTime.Today.DayOfYear)
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
        #endregion
    }

}