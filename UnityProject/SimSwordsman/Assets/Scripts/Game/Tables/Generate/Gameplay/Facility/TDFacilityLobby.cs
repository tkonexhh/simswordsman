//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityLobby
    {
        
       
        private EInt m_Level = 0;   
        private string m_UpgradeRes;   
        private EInt m_UpgradeCost = 0;   
        private string m_UpgradePreconditions;   
        private string m_UnlockContent;   
        private EInt m_WorkPay = 0;   
        private EInt m_DefExp = 0;   
        private EInt m_DefKongfuExp = 0;   
        private EInt m_CommonTaskMax = 0;  
        
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
        /// 升级条件，拥有n名m级弟子
        /// </summary>
        public  string  upgradePreconditions {get { return m_UpgradePreconditions; } }
       
        /// <summary>
        /// 解锁内容
        /// </summary>
        public  string  unlockContent {get { return m_UnlockContent; } }
       
        /// <summary>
        /// 工作报酬
        /// </summary>
        public  int  workPay {get { return m_WorkPay; } }
       
        /// <summary>
        /// 防守经验
        /// </summary>
        public  int  defExp {get { return m_DefExp; } }
       
        /// <summary>
        /// 防守功夫经验
        /// </summary>
        public  int  defKongfuExp {get { return m_DefKongfuExp; } }
       
        /// <summary>
        /// 日常任务最大数
        /// </summary>
        public  int  commonTaskMax {get { return m_CommonTaskMax; } }
       

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
                    m_UpgradePreconditions = dataR.ReadString();
                    break;
                case 4:
                    m_UnlockContent = dataR.ReadString();
                    break;
                case 5:
                    m_WorkPay = dataR.ReadInt();
                    break;
                case 6:
                    m_DefExp = dataR.ReadInt();
                    break;
                case 7:
                    m_DefKongfuExp = dataR.ReadInt();
                    break;
                case 8:
                    m_CommonTaskMax = dataR.ReadInt();
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
          
          ret.Add("Level", 0);
          ret.Add("UpgradeRes", 1);
          ret.Add("UpgradeCost", 2);
          ret.Add("UpgradePreconditions", 3);
          ret.Add("UnlockContent", 4);
          ret.Add("WorkPay", 5);
          ret.Add("DefExp", 6);
          ret.Add("DefKongfuExp", 7);
          ret.Add("CommonTaskMax", 8);
          return ret;
        }
    } 
}//namespace LR