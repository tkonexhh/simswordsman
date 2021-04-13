//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public static partial class TDTowerShopTable
    {
        private static TDTableMetaData m_MetaData = new TDTableMetaData(TDTowerShopTable.Parse, "TowerShop");
        public static TDTableMetaData metaData
        {
            get { return m_MetaData; }
        }
        
        private static Dictionary<int, TDTowerShop> m_DataCache = new Dictionary<int, TDTowerShop>();
        private static List<TDTowerShop> m_DataList = new List<TDTowerShop >();
        
        public static void Parse(byte[] fileData)
        {
            m_DataCache.Clear();
            m_DataList.Clear();
            DataStreamReader dataR = new DataStreamReader(fileData);
            int rowCount = dataR.GetRowCount();
            int[] fieldIndex = dataR.GetFieldIndex(TDTowerShop.GetFieldHeadIndex());
    #if (UNITY_STANDALONE_WIN) || UNITY_EDITOR || UNITY_STANDALONE_OSX
            dataR.CheckFieldMatch(TDTowerShop.GetFieldHeadIndex(), "TowerShopTable");
    #endif
            for (int i = 0; i < rowCount; ++i)
            {
                TDTowerShop memberInstance = new TDTowerShop();
                memberInstance.ReadRow(dataR, fieldIndex);
                OnAddRow(memberInstance);
                memberInstance.Reset();
                CompleteRowAdd(memberInstance);
            }
            Log.i(string.Format("Parse Success TDTowerShop"));
        }

        private static void OnAddRow(TDTowerShop memberInstance)
        {
            int key = memberInstance.goodsID;
            if (m_DataCache.ContainsKey(key))
            {
                Log.e(string.Format("Invaild,  TDTowerShopTable Id already exists {0}", key));
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

        public static List<TDTowerShop> dataList
        {
            get 
            {
                return m_DataList;
            }    
        }

        public static TDTowerShop GetData(int key)
        {
            if (m_DataCache.ContainsKey(key))
            {
                return m_DataCache[key];
            }
            else
            {
                Log.w(string.Format("Can't find key {0} in TDTowerShop", key));
                return null;
            }
        }
    }
}//namespace LR