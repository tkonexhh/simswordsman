//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCollectConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Type;   
        private string m_LobbyLevelRequired;   
        private EInt m_ItemId = 0;   
        private EInt m_SpecialRate = 0;   
        private EInt m_SpecialItemId = 0;   
        private EInt m_ProductTime = 0;   
        private EInt m_MaxStore = 0;   
        private EInt m_CollectMin = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 类型
        /// </summary>
        public  string  type {get { return m_Type; } }
       
        /// <summary>
        /// 解锁等级
        /// </summary>
        public  string  lobbyLevelRequired {get { return m_LobbyLevelRequired; } }
       
        /// <summary>
        /// 获取物品id
        /// </summary>
        public  int  itemId {get { return m_ItemId; } }
       
        /// <summary>
        /// 稀有物品概率
        /// </summary>
        public  int  specialRate {get { return m_SpecialRate; } }
       
        /// <summary>
        /// 稀有物品Id
        /// </summary>
        public  int  specialItemId {get { return m_SpecialItemId; } }
       
        /// <summary>
        /// 生产时间，分钟
        /// </summary>
        public  int  productTime {get { return m_ProductTime; } }
       
        /// <summary>
        /// 存储最大数量
        /// </summary>
        public  int  maxStore {get { return m_MaxStore; } }
       
        /// <summary>
        /// 生成气泡最小数量
        /// </summary>
        public  int  collectMin {get { return m_CollectMin; } }
       

        public void ReadRow(DataStreamReader dataR, int[] filedIndex)
        {
          //var schemeNames = dataR.GetSchemeName();
          int col = 0;
          while(true)
          {
            col = dataR.MoreFieldOnRow();
            if (col == -1)
            {
              break;
            }
            switch (filedIndex[col])
            { 
            
                case 0:
                    m_Id = dataR.ReadInt();
                    break;
                case 1:
                    m_Type = dataR.ReadString();
                    break;
                case 2:
                    m_LobbyLevelRequired = dataR.ReadString();
                    break;
                case 3:
                    m_ItemId = dataR.ReadInt();
                    break;
                case 4:
                    m_SpecialRate = dataR.ReadInt();
                    break;
                case 5:
                    m_SpecialItemId = dataR.ReadInt();
                    break;
                case 6:
                    m_ProductTime = dataR.ReadInt();
                    break;
                case 7:
                    m_MaxStore = dataR.ReadInt();
                    break;
                case 8:
                    m_CollectMin = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(9);
          
          ret.Add("Id", 0);
          ret.Add("Type", 1);
          ret.Add("LobbyLevelRequired", 2);
          ret.Add("ItemId", 3);
          ret.Add("SpecialRate", 4);
          ret.Add("SpecialItemId", 5);
          ret.Add("ProductTime", 6);
          ret.Add("MaxStore", 7);
          ret.Add("CollectMin", 8);
          return ret;
        }
    } 
}//namespace LR