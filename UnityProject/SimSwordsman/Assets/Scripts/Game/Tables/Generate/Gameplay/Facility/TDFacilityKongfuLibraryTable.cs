//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDFacilityKongfuLibraryTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDFacilityKongfuLibraryTable.Parse, "FacilityKongfuLibrary");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDFacilityKongfuLibrary> m_DataCache = new Dictionary<int, TDFacilityKongfuLibrary>();
        private static List<TDFacilityKongfuLibrary> m_DataList = new List<TDFacilityKongfuLibrary >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDFacilityKongfuLibrary.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDFacilityKongfuLibrary.GetFieldHeadIndex(), "FacilityKongfuLibraryTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDFacilityKongfuLibrary memberInstance = new TDFacilityKongfuLibrary();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDFacilityKongfuLibrary"));
        }

        private static void OnAddRow(TDFacilityKongfuLibrary memberInstance)
        {
            int key = memberInstance.level;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDFacilityKongfuLibraryTable Id already exists {0}", key));
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

        public static List<TDFacilityKongfuLibrary> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDFacilityKongfuLibrary GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDFacilityKongfuLibrary", key));
                return null;
            }
        }
    }
}//namespace LR