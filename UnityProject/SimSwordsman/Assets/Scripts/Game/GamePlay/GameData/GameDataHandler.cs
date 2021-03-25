using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class GameDataHandler : DataClassHandler<GameData>
    {
        public static DataDirtyRecorder s_DataDirtyRecorder = new DataDirtyRecorder();

        public static string s_path { get { return dataFilePath; } }

        public GameDataHandler()
        {
            Load();
            EnableAutoSave();
        }

        public GameData GetGameData()
        {
            return m_Data;
        }

        public void Save()
        {
            Save(true);
        }

        public PlayerData GetPlayerInfodata()
        {
            return m_Data.playerInfoData;
        }

        public ClanData GetClanData()
        {
            return m_Data.clanData;
        }

        public CommonTaskData GetCommonTaskData()
        {
            return m_Data.commonTaskData;
        }

        public CountdownData GetCountdownData()
        {
            return m_Data.countdownData;
        }
    }
}