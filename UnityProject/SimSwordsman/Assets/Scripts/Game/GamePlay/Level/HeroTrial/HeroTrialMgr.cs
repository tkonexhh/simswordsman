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

        private double m_TrialTotalTime = 20;

        private ClanType m_TrialClanType = ClanType.None;

        private Coroutine m_Coroutine;

        public HeroTrialData DbData { get => m_DbData;}
        public int TrialDiscipleID { get => m_DbData.characterId; }
        public HeroTrialStateID CurState { get => m_CurState; }
        public double TrialTotalTime { get => m_TrialTotalTime; }
        public ClanType TrialClan { get => m_TrialClanType; }
        public BattleField BattleField { get => m_BattleField; }
        public FightGroup FightGroup { get => m_FightGroup; set => m_FightGroup = value; }

        

        #region IMgr
        public void OnInit()
        {
            m_DbData = GameDataMgr.S.GetClanData().heroTrialData;
            m_TrialClanType = m_DbData.clanType;

            m_BattleField = FindObjectOfType<BattleField>();
            m_BattleField.Init();

            m_StateMachine = new HeroTrialStateMachine(this);

            if (CheckIsTrialReady() && m_TrialClanType == ClanType.None)
            {
                m_TrialClanType = GetNextClanType(ClanType.None);
            }
        }

        public void OnUpdate()
        {
            if (m_IsInTrial)
            {
                m_StateMachine.UpdateState(Time.deltaTime);

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

            m_BattleField.ChangeBgSpriteRenderToHeroTrial();

            StartCountDown();

            if (!string.IsNullOrEmpty(m_DbData.trialStartTime))
            {
                m_TrialStartTime = m_DbData.GetStartTime();
            }
        }

        public void OnExitHeroTrial()
        {
            UnregisterEvents();

            SetState(HeroTrialStateID.None);

            if (m_FightGroup != null)
            {
                GameObject.Destroy(m_FightGroup.OurCharacter.CharacterView.gameObject);
                GameObject.Destroy(m_FightGroup.EnemyCharacter.CharacterView.gameObject);
                m_FightGroup = null;
            }

            m_BattleField.OnBattleEnd();

            EventSystem.S.Send(EventID.OnExitHeroTrial);

            if (m_Coroutine!=null)
                StopCoroutine(m_Coroutine);

            m_IsInTrial = false;
        }

        private void StartCountDown()
        {
            if (m_DbData.state == HeroTrialStateID.Runing)
            {
                m_Coroutine = StartCoroutine(CommonMethod.CountDown(() =>
                {
                    EventSystem.S.Send(EventID.OnCountDownRefresh, GetLeftTime());
                }));
            }
        }

        public void StartTrial(int characterId)
        {       
            m_TrialStartTime = DateTime.Now;
            m_DbData.OnTrialStart(DateTime.Now, characterId, m_TrialClanType);
            SetState(m_DbData.state);
            StartCountDown();
        }

        public void FinishTrial()
        {
            m_FightGroup.OurCharacter.CharacterModel.SetIsHero(m_DbData.clanType);
            CharacterController characterInGame = MainGameMgr.S.CharacterMgr.GetCharacterController(m_FightGroup.OurCharacter.CharacterId);
            if (characterInGame != null)
            {
                characterInGame.ChangeBody(CharacterQuality.Hero, m_DbData.clanType);
            }

            m_DbData.OnTrialFinished();
            SetState(m_DbData.state);

            //Refresh next trial clantype
            m_TrialClanType = GetNextClanType(m_DbData.clanType);
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
            if (m_DbData.state == HeroTrialStateID.Runing || m_DbData.state == HeroTrialStateID.Finished)
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
                    EventSystem.S.Send(EventID.OnRefreshTrialPanel);
                    break;

            }
        }

        private ClanType GetNextClanType(ClanType curClanType)
        {
            if (curClanType == ClanType.None)
                return ClanType.Gaibang;

            HeroTrialConfig config = TDHeroTrialConfigTable.GetConfig(curClanType);

            int nextId = config.id + 1;
            if (nextId > 4)
            {
                nextId = 1;
            }

            HeroTrialConfig nextConfig = TDHeroTrialConfigTable.GetConfig(nextId);

            return nextConfig.clanType;
        }

        public double GetLeftTime()
        {
            TimeSpan time = DateTime.Now - m_TrialStartTime;
            double leftTime = m_TrialTotalTime - time.TotalSeconds;
            if (leftTime < 0)
                leftTime = 0;

            return leftTime;
        }
        #endregion
    }

}