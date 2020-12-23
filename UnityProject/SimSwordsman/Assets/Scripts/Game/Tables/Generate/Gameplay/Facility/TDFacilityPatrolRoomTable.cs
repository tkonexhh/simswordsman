//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDFacilityPatrolRoomTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDFacilityPatrolRoomTable.Parse, "FacilityPatrolRoom");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDFacilityPatrolRoom> m_DataCache = new Dictionary<int, TDFacilityPatrolRoom>();
        private static List<TDFacilityPatrolRoom> m_DataList = new List<TDFacilityPatrolRoom >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDFacilityPatrolRoom.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDFacilityPatrolRoom.GetFieldHeadIndex(), "FacilityPatrolRoomTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDFacilityPatrolRoom memberInstance = new TDFacilityPatrolRoom();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDFacilityPatrolRoom"));
        }

        private static void OnAddRow(TDFacilityPatrolRoom memberInstance)
        {
            int key = memberInstance.level;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDFacilityPatrolRoomTable Id already exists {0}", key));
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

        public static List<TDFacilityPatrolRoom> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDFacilityPatrolRoom GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDFacilityPatrolRoom", key));
                return null;
            }
        }
    }
}//namespace LR