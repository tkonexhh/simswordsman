//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDCharacterQualityConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDCharacterQualityConfigTable.Parse, "CharacterQualityConfig");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<string, TDCharacterQualityConfig> m_DataCache = new Dictionary<string, TDCharacterQualityConfig>();
        private static List<TDCharacterQualityConfig> m_DataList = new List<TDCharacterQualityConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDCharacterQualityConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDCharacterQualityConfig.GetFieldHeadIndex(), "CharacterQualityConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDCharacterQualityConfig memberInstance = new TDCharacterQualityConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDCharacterQualityConfig"));
        }

        private static void OnAddRow(TDCharacterQualityConfig memberInstance)
        {
            string key = memberInstance.quality;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDCharacterQualityConfigTable Id already exists {0}", key));
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

        public static List<TDCharacterQualityConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDCharacterQualityConfig GetData(string key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDCharacterQualityConfig", key));
                return null;
            }
        }
    }
}//namespace LR