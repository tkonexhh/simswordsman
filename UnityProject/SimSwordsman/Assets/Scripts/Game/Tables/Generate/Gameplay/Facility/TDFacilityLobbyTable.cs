//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDFacilityLobbyTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDFacilityLobbyTable.Parse, "FacilityLobby");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDFacilityLobby> m_DataCache = new Dictionary<int, TDFacilityLobby>();
        private static List<TDFacilityLobby> m_DataList = new List<TDFacilityLobby >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDFacilityLobby.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDFacilityLobby.GetFieldHeadIndex(), "FacilityLobbyTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDFacilityLobby memberInstance = new TDFacilityLobby();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDFacilityLobby"));
        }

        private static void OnAddRow(TDFacilityLobby memberInstance)
        {
            int key = memberInstance.level;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDFacilityLobbyTable Id already exists {0}", key));
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

        public static List<TDFacilityLobby> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDFacilityLobby GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDFacilityLobby", key));
                return null;
            }
        }
    }
}//namespace LR