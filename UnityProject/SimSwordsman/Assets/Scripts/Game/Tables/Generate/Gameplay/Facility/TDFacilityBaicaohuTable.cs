//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDFacilityBaicaohuTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDFacilityBaicaohuTable.Parse, "FacilityBaicaohu");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDFacilityBaicaohu> m_DataCache = new Dictionary<int, TDFacilityBaicaohu>();
        private static List<TDFacilityBaicaohu> m_DataList = new List<TDFacilityBaicaohu >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDFacilityBaicaohu.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDFacilityBaicaohu.GetFieldHeadIndex(), "FacilityBaicaohuTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDFacilityBaicaohu memberInstance = new TDFacilityBaicaohu();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDFacilityBaicaohu"));
        }

        private static void OnAddRow(TDFacilityBaicaohu memberInstance)
        {
            int key = memberInstance.level;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDFacilityBaicaohuTable Id already exists {0}", key));
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

        public static List<TDFacilityBaicaohu> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDFacilityBaicaohu GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDFacilityBaicaohu", key));
                return null;
            }
        }
    }
}//namespace LR