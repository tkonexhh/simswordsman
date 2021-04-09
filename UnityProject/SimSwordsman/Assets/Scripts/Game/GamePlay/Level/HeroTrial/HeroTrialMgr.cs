using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class HeroTrialMgr : MonoBehaviour, IMgr
    {
        private BattleField m_BattleField = null;
        private HeroTrialData m_DbData = null;

        #region IMgr
        public void OnInit()
        {
            m_BattleField = FindObjectOfType<BattleField>();
            m_BattleField.Init();

            m_DbData = GameDataMgr.S.GetClanData().heroTrialData;

            RegisterEvents();
        }

        public void OnUpdate()
        {

        }

        public void OnDestroyed()
        {

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
        #endregion
    }

}