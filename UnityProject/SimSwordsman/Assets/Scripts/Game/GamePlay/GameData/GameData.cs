using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class GameData : IDataClass
    {
        public PlayerData playerInfoData = null;
        public ClanData clanData = null;
        public CommonTaskData commonTaskData = null;

        public GameData()
        {
            SetDirtyRecorder(GameDataHandler.s_DataDirtyRecorder);
        }

        public override void InitWithEmptyData()
        {
            playerInfoData = new PlayerData();
            playerInfoData.InitWithEmptyData();

            clanData = new ClanData();
            clanData.SetDefaultValue();

            commonTaskData = new CommonTaskData();
            commonTaskData.SetDefaultValue();
        }

        public override void OnDataLoadFinish()
        {
            playerInfoData.SetDirtyRecorder(m_Recorder);
            clanData.SetDirtyRecorder(m_Recorder);
            commonTaskData.SetDirtyRecorder(m_Recorder);

            clanData.OnDataLoadFinish();
        }
    }
}
