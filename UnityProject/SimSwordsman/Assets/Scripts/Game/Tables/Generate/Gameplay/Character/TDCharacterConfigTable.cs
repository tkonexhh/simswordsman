//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDCharacterConfigTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDCharacterConfigTable.Parse, "CharacterConfig");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<string, TDCharacterConfig> m_DataCache = new Dictionary<string, TDCharacterConfig>();
        private static List<TDCharacterConfig> m_DataList = new List<TDCharacterConfig >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDCharacterConfig.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDCharacterConfig.GetFieldHeadIndex(), "CharacterConfigTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDCharacterConfig memberInstance = new TDCharacterConfig();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDCharacterConfig"));
        }

        private static void OnAddRow(TDCharacterConfig memberInstance)
        {
            string key = memberInstance.quality;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDCharacterConfigTable Id already exists {0}", key));
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

        public static List<TDCharacterConfig> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDCharacterConfig GetData(string key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDCharacterConfig", key));
                return null;
            }
        }
    }
}//namespace LR