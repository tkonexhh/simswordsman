//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDTowerConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDTowerConfigTable.Parse, "TowerConfig");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDTowerConfig> m_DataCache = new Dictionary<int, TDTowerConfig>();
        private static List<TDTowerConfig> m_DataList = new List<TDTowerConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDTowerConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDTowerConfig.GetFieldHeadIndex(), "TowerConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDTowerConfig memberInstance = new TDTowerConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDTowerConfig"));
        }

        private static void OnAddRow(TDTowerConfig memberInstance)
        {
            int key = memberInstance.floorNum;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDTowerConfigTable Id already exists {0}", key));
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

        public static List<TDTowerConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDTowerConfig GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDTowerConfig", key));
                return null;
            }
        }
    }
}//namespace LR