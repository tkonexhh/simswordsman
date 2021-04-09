//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDTowerRewardConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDTowerRewardConfigTable.Parse, "TowerRewardConfig");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDTowerRewardConfig> m_DataCache = new Dictionary<int, TDTowerRewardConfig>();
        private static List<TDTowerRewardConfig> m_DataList = new List<TDTowerRewardConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDTowerRewardConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDTowerRewardConfig.GetFieldHeadIndex(), "TowerRewardConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDTowerRewardConfig memberInstance = new TDTowerRewardConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDTowerRewardConfig"));
        }

        private static void OnAddRow(TDTowerRewardConfig memberInstance)
        {
            int key = memberInstance.homeLevel;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDTowerRewardConfigTable Id already exists {0}", key));
            }
            else
            {
                m_DataCache.Add(key, memberInstance);
                m_DataList.Add(memberInstance);
            }
        }    
        
        public static void Reload(byte[] fileData)
        {
            Parse(fileData);
        }

        public static int count
        {
            get 
            {
                return m_DataCache.Count;
            }
        }

        public static List<TDTowerRewardConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDTowerRewardConfig GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDTowerRewardConfig", key));
                return null;
            }
        }
    }
}//namespace LR