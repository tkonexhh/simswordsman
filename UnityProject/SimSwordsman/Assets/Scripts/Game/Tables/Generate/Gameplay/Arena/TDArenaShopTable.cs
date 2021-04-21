//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDArenaShopTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDArenaShopTable.Parse, "ArenaShop");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDArenaShop> m_DataCache = new Dictionary<int, TDArenaShop>();
        private static List<TDArenaShop> m_DataList = new List<TDArenaShop >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDArenaShop.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDArenaShop.GetFieldHeadIndex(), "ArenaShopTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDArenaShop memberInstance = new TDArenaShop();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDArenaShop"));
        }

        private static void OnAddRow(TDArenaShop memberInstance)
        {
            int key = memberInstance.goodsID;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDArenaShopTable Id already exists {0}", key));
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

        public static List<TDArenaShop> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDArenaShop GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDArenaShop", key));
                return null;
            }
        }
    }
}//namespace LR