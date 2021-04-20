//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDArenaConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDArenaConfigTable.Parse, "ArenaConfig");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDArenaConfig> m_DataCache = new Dictionary<int, TDArenaConfig>();
        private static List<TDArenaConfig> m_DataList = new List<TDArenaConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDArenaConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDArenaConfig.GetFieldHeadIndex(), "ArenaConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDArenaConfig memberInstance = new TDArenaConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDArenaConfig"));
        }

        private static void OnAddRow(TDArenaConfig memberInstance)
        {
            int key = memberInstance.id;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDArenaConfigTable Id already exists {0}", key));
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

        public static List<TDArenaConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDArenaConfig GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDArenaConfig", key));
                return null;
            }
        }
    }
}//namespace LR