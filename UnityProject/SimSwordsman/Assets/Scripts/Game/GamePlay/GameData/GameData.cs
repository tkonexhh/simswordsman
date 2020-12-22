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
        //public ShopData shopData = null;
        public MainTaskData mainTaskData = null;

        public GameData()
        {
            SetDirtyRecorder(GameDataHandler.s_DataDirtyRecorder);
        }

        public override void InitWithEmptyData()
        {
            playerInfoData = new PlayerData();
            playerInfoData.SetDefaultValue();

            clanData = new ClanData();
            clanData.SetDefaultValue();
            //shopData = new ShopData();
            //shopData.SetDefaultValue();
            
            mainTaskData = new MainTaskData();
            mainTaskData.SetDefaultValue();
        }

        public override void OnDataLoadFinish()
        {
            playerInfoData.SetDirtyRecorder(m_Recorder);
            clanData.SetDirtyRecorder(m_Recorder);
            //shopData.SetDirtyRecorder(m_Recorder);
            mainTaskData.SetDirtyRecorder(m_Recorder);
        }       
    }
}
