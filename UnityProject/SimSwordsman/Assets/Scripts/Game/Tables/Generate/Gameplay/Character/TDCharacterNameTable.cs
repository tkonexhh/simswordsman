//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDCharacterNameTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDCharacterNameTable.Parse, "CharacterName");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<string, TDCharacterName> m_DataCache = new Dictionary<string, TDCharacterName>();
        private static List<TDCharacterName> m_DataList = new List<TDCharacterName >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDCharacterName.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDCharacterName.GetFieldHeadIndex(), "CharacterNameTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDCharacterName memberInstance = new TDCharacterName();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDCharacterName"));
        }

        private static void OnAddRow(TDCharacterName memberInstance)
        {
            string key = memberInstance.familyName;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDCharacterNameTable Id already exists {0}", key));
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

        public static List<TDCharacterName> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDCharacterName GetData(string key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDCharacterName", key));
                return null;
            }
        }
    }
}//namespace LR