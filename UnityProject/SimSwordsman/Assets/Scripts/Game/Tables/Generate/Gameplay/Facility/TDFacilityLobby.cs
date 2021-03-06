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
        private EInt m_WorkInterval = 0;   
        private EInt m_WorkMaxAmount = 0;   
        private EInt m_WorkTime = 0;   
        private EInt m_AutoWorkWait = 0;   
        private EInt m_WorkPay = 0;   
        private EInt m_WorkExp = 0;   
        private EInt m_DefExp = 0;   
        private EInt m_CommonTaskAmount = 0;   
        private EInt m_CTDailyMax = 0;   
        private EInt m_PracticeLevelMax = 0;  
        
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
        /// 生成工作间隔，秒
        /// </summary>
        public  int  workInterval {get { return m_WorkInterval; } }
       
        /// <summary>
        /// 最大同保留工作数
        /// </summary>
        public  int  workMaxAmount {get { return m_WorkMaxAmount; } }
       
        /// <summary>
        /// 工作时长，秒
        /// </summary>
        public  int  workTime {get { return m_WorkTime; } }
       
        /// <summary>
        /// 自动工作等待时长，秒
        /// </summary>
        public  int  autoWorkWait {get { return m_AutoWorkWait; } }
       
        /// <summary>
        /// 工作报酬
        /// </summary>
        public  int  workPay {get { return m_WorkPay; } }
       
        /// <summary>
        /// 工作经验
        /// </summary>
        public  int  workExp {get { return m_WorkExp; } }
       
        /// <summary>
        /// 防守经验
        /// </summary>
        public  int  defExp {get { return m_DefExp; } }
       
        /// <summary>
        /// 日常任务数
        /// </summary>
        public  int  commonTaskAmount {get { return m_CommonTaskAmount; } }
       
        /// <summary>
        /// 日常任务每日最大次数
        /// </summary>
        public  int  cTDailyMax {get { return m_CTDailyMax; } }
       
        /// <summary>
        /// 练功弟子等级上限
        /// </summary>
        public  int  practiceLevelMax {get { return m_PracticeLevelMax; } }
       

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
                    m_WorkInterval = dataR.ReadInt();
                    break;
                case 6:
                    m_WorkMaxAmount = dataR.ReadInt();
                    break;
                case 7:
                    m_WorkTime = dataR.ReadInt();
                    break;
                case 8:
                    m_AutoWorkWait = dataR.ReadInt();
                    break;
                case 9:
                    m_WorkPay = dataR.ReadInt();
                    break;
                case 10:
                    m_WorkExp = dataR.ReadInt();
                    break;
                case 11:
                    m_DefExp = dataR.ReadInt();
                    break;
                case 12:
                    m_CommonTaskAmount = dataR.ReadInt();
                    break;
                case 13:
                    m_CTDailyMax = dataR.ReadInt();
                    break;
                case 14:
                    m_PracticeLevelMax = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(15);
          
          ret.Add("Level", 0);
          ret.Add("UpgradeRes", 1);
          ret.Add("UpgradeCost", 2);
          ret.Add("UpgradePreconditions", 3);
          ret.Add("UnlockContent", 4);
          ret.Add("WorkInterval", 5);
          ret.Add("WorkMaxAmount", 6);
          ret.Add("WorkTime", 7);
          ret.Add("AutoWorkWait", 8);
          ret.Add("WorkPay", 9);
          ret.Add("WorkExp", 10);
          ret.Add("DefExp", 11);
          ret.Add("CommonTaskAmount", 12);
          ret.Add("CTDailyMax", 13);
          ret.Add("PracticeLevelMax", 14);
          return ret;
        }
    } 
}//namespace LR