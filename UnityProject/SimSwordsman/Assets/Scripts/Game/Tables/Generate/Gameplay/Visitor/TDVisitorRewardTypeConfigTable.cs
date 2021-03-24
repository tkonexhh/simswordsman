//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDVisitorRewardTypeConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDVisitorRewardTypeConfigTable.Parse, "VisitorRewardTypeConfig");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDVisitorRewardTypeConfig> m_DataCache = new Dictionary<int, TDVisitorRewardTypeConfig>();
        private static List<TDVisitorRewardTypeConfig> m_DataList = new List<TDVisitorRewardTypeConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDVisitorRewardTypeConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDVisitorRewardTypeConfig.GetFieldHeadIndex(), "VisitorRewardTypeConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDVisitorRewardTypeConfig memberInstance = new TDVisitorRewardTypeConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDVisitorRewardTypeConfig"));
        }

        private static void OnAddRow(TDVisitorRewardTypeConfig memberInstance)
        {
            int key = memberInstance.lobbyLevel;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDVisitorRewardTypeConfigTable Id already exists {0}", key));
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

        public static List<TDVisitorRewardTypeConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDVisitorRewardTypeConfig GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDVisitorRewardTypeConfig", key));
                return null;
            }
        }
    }
}//namespace LR