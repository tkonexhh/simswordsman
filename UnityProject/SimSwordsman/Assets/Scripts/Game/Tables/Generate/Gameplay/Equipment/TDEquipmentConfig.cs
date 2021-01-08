//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDEquipmentConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Name;   
        private string m_Desc;   
        private string m_Quality;   
        private string m_UpgradeCondition;   
        private string m_Bonus;   
        private string m_BuildCondition;   
        private string m_SellingPrice;   
        private EInt m_UnlockLevel = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 名称
        /// </summary>
        public  string  name {get { return m_Name; } }
       
        /// <summary>
        /// 描述
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       
        /// <summary>
        /// 品质
        /// </summary>
        public  string  quality {get { return m_Quality; } }
       
        /// <summary>
        /// 升级消耗,id|数量
        /// </summary>
        public  string  upgradeCondition {get { return m_UpgradeCondition; } }
       
        /// <summary>
        /// 加成
        /// </summary>
        public  string  bonus {get { return m_Bonus; } }
       
        /// <summary>
        /// 打造消耗,id|数量
        /// </summary>
        public  string  buildCondition {get { return m_BuildCondition; } }
       
        /// <summary>
        /// 出售价格（单价）
        /// </summary>
        public  string  sellingPrice {get { return m_SellingPrice; } }
       
        /// <summary>
        /// 解锁等级
        /// </summary>
        public  int  unlockLevel {get { return m_UnlockLevel; } }
       

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
                    m_Name = dataR.ReadString();
                    break;
                case 2:
                    m_Desc = dataR.ReadString();
                    break;
                case 3:
                    m_Quality = dataR.ReadString();
                    break;
                case 4:
                    m_UpgradeCondition = dataR.ReadString();
                    break;
                case 5:
                    m_Bonus = dataR.ReadString();
                    break;
                case 6:
                    m_BuildCondition = dataR.ReadString();
                    break;
                case 7:
                    m_SellingPrice = dataR.ReadString();
                    break;
                case 8:
                    m_UnlockLevel = dataR.ReadInt();
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
          ret.Add("Name", 1);
          ret.Add("Desc", 2);
          ret.Add("Quality", 3);
          ret.Add("UpgradeCondition", 4);
          ret.Add("Bonus", 5);
          ret.Add("BuildCondition", 6);
          ret.Add("SellingPrice", 7);
          ret.Add("UnlockLevel", 8);
          return ret;
        }
    } 
}//namespace LR