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

            SetState(m_DbData.state);
 
            RegisterEvents();
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
            EventSystem.S.Send(EventID.OnEnterHeroTrial);
        }

        public void OnExitHeroTrial()
        {
            EventSystem.S.Send(EventID.OnExitHeroTrial);
        }

        public void StartTrial(int trialStartDay, int characterId)
        {
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
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EngineEventID.OnDateUpdate, HandleEvent);
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
            if (key == (int)EngineEventID.OnDateUpdate)
            {
                bool needRefresh = CheckIsTrialReady();
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