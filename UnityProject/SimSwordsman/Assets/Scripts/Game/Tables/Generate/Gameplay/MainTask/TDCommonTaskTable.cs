//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDCommonTaskTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDCommonTaskTable.Parse, "CommonTask");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDCommonTask> m_DataCache = new Dictionary<int, TDCommonTask>();
        private static List<TDCommonTask> m_DataList = new List<TDCommonTask >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDCommonTask.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDCommonTask.GetFieldHeadIndex(), "CommonTaskTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDCommonTask memberInstance = new TDCommonTask();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDCommonTask"));
        }

        private static void OnAddRow(TDCommonTask memberInstance)
        {
            int key = memberInstance.taskID;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDCommonTaskTable Id already exists {0}", key));
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

        public static List<TDCommonTask> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDCommonTask GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDCommonTask", key));
                return null;
            }
        }
    }
}//namespace LR