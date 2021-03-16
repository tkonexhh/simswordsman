using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    /// <summary>
    /// GameData对外交互类
    /// </summary>
    public class GameDataMgr : TSingleton<GameDataMgr>, IResetHandler, IDailyResetData
    {
        private GameDataHandler m_GameDataHandler = null;

        public GameDataHandler GameDataHandler
        {
            get
            {
                return m_GameDataHandler;
            }
        }

        public void Init()
        {
            m_GameDataHandler = new GameDataHandler();

            RegisterEvents();

            m_GameDataHandler.GetPlayerInfodata().Init();
        }

        public void Reset()
        {
            m_GameDataHandler.ResetAsNew();
        }

        public void ResetDailyData()
        {
            Debug.LogError("ResetDailyData");
            GetPlayerData().ResetDailyData();
        }

        private void RegisterEvents()
        {
            //EventSystem.S.Register(EventID.OnLevelCompleted, HandleEvent);
            EventSystem.S.Register(EventID.OnRefreshMainMenuPanel, HandleEvent);
        }

        private void HandleEvent(int eventId, params object[] param)
        {
            //if (eventId == (int)EventID.OnLevelCompleted)
            //{
            //    int levelIndex = (int)param[0];
            //    int starNum = (int)param[1];

            //    m_GameDataHandler.GetPlayerInfodata().OnLevelCompleted(levelIndex, starNum);
            //}
            //if (eventId == (int)EventID.OnRefreshMainMenuPanel)
            //{
            //    int delta = (int)param[0];
            //    m_GameDataHandler.GetPlayerInfodata().AddCoinNum(delta);
            //}
        }

        public void Save()
        {
            m_GameDataHandler.Save();
        }

        /// <summary>
        /// 获取所有游戏数据
        /// </summary>
        /// <returns></returns>
        public GameData GetGameData()
        {
            return m_GameDataHandler.GetGameData();
        }

        public PlayerData GetPlayerData()
        {
            return m_GameDataHandler.GetPlayerInfodata();
        }

        public ClanData GetClanData()
        {
            return m_GameDataHandler.GetClanData();
        }
        //public ShopData GetShopData()
        //{
        //    return m_GameDataHandler.GetShopData();
        //}

        public MainTaskData GetMainTaskData()
        {
            return m_GameDataHandler.GetMainTaskData();
        }

        public CommonTaskData GetCommonTaskData()
        {
            return m_GameDataHandler.GetCommonTaskData();
        }

        public CountdownData GetCountdownData()
        {
            return m_GameDataHandler.GetCountdownData();
        }
        public void OnReset()
        {
            GetPlayerData().OnReset();
        }
    }
}