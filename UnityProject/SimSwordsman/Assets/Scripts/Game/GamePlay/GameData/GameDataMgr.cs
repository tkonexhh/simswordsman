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

        public CommonTaskData GetCommonTaskData()
        {
            return m_GameDataHandler.GetCommonTaskData();
        }

        public void OnReset()
        {
            GetPlayerData().OnReset();
        }
    }
}