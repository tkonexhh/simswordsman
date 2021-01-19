//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDScenarioTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDScenarioTable.Parse, "Scenario");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDScenario> m_DataCache = new Dictionary<int, TDScenario>();
        private static List<TDScenario> m_DataList = new List<TDScenario >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDScenario.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDScenario.GetFieldHeadIndex(), "ScenarioTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDScenario memberInstance = new TDScenario();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDScenario"));
        }

        private static void OnAddRow(TDScenario memberInstance)
        {
            int key = memberInstance.chapterID;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDScenarioTable Id already exists {0}", key));
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

        public static List<TDScenario> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDScenario GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDScenario", key));
                return null;
            }
        }
    }
}//namespace LR