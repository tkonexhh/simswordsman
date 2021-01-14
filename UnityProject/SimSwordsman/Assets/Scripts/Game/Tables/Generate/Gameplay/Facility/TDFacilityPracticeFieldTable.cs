//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDFacilityPracticeFieldTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDFacilityPracticeFieldTable.Parse, "FacilityPracticeField");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDFacilityPracticeField> m_DataCache = new Dictionary<int, TDFacilityPracticeField>();
        private static List<TDFacilityPracticeField> m_DataList = new List<TDFacilityPracticeField >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDFacilityPracticeField.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDFacilityPracticeField.GetFieldHeadIndex(), "FacilityPracticeFieldTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDFacilityPracticeField memberInstance = new TDFacilityPracticeField();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDFacilityPracticeField"));
        }

        private static void OnAddRow(TDFacilityPracticeField memberInstance)
        {
            int key = memberInstance.id;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDFacilityPracticeFieldTable Id already exists {0}", key));
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

        public static List<TDFacilityPracticeField> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDFacilityPracticeField GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDFacilityPracticeField", key));
                return null;
            }
        }
    }
}//namespace LR