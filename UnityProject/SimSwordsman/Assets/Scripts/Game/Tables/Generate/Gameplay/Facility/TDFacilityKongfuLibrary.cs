//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityKongfuLibrary
    {
        
       
        private EInt m_Level = 0;   
        private string m_UpgradeRes;   
        private EInt m_UpgradeCost = 0;   
        private EInt m_UpgradePreconditions = 0;   
        private string m_UnlockKongfu;   
        private string m_KongfuList;   
        private EInt m_Duration = 0;   
        private EInt m_Seat = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  level {get { return m_Level; } }
       
        /// <summary>
        /// 升级资源
        /// </summary>
        public  string  upgradeRes {get { return m_UpgradeRes; } }
       
        /// <summary>
        /// 升级花费
        /// </summary>
        public  int  upgradeCost {get { return m_UpgradeCost; } }
       
        /// <summary>
        /// 升级条件
        /// </summary>
        public  int  upgradePreconditions {get { return m_UpgradePreconditions; } }
       
        /// <summary>
        /// 显示解锁功夫
        /// </summary>
        public  string  unlockKongfu {get { return m_UnlockKongfu; } }
       
        /// <summary>
        /// 功夫池
        /// </summary>
        public  string  kongfuList {get { return m_KongfuList; } }
       
        /// <summary>
        /// 抄经时长，秒
        /// </summary>
        public  int  duration {get { return m_Duration; } }
       
        /// <summary>
        /// 抄经位
        /// </summary>
        public  int  seat {get { return m_Seat; } }
       

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
                    m_Level = dataR.ReadInt();
                    break;
                case 1:
                    m_UpgradeRes = dataR.ReadString();
                    break;
                case 2:
                    m_UpgradeCost = dataR.ReadInt();
                    break;
                case 3:
                    m_UpgradePreconditions = dataR.ReadInt();
                    break;
                case 4:
                    m_UnlockKongfu = dataR.ReadString();
                    break;
                case 5:
                    m_KongfuList = dataR.ReadString();
                    break;
                case 6:
                    m_Duration = dataR.ReadInt();
                    break;
                case 7:
                    m_Seat = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(8);
          
          ret.Add("Level", 0);
          ret.Add("UpgradeRes", 1);
          ret.Add("UpgradeCost", 2);
          ret.Add("UpgradePreconditions", 3);
          ret.Add("UnlockKongfu", 4);
          ret.Add("KongfuList", 5);
          ret.Add("Duration", 6);
          ret.Add("Seat", 7);
          return ret;
        }
    } 
}//namespace LR