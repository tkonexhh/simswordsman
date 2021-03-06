//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDWork
    {
        
       
        private EInt m_WorkID = 0;   
        private EInt m_HomeLevel = 0;   
        private string m_CollectObjType;   
        private string m_UnlockDesc;   
        private string m_FunctionDesc;   
        private string m_WorkName;   
        private string m_WorkTalk;   
        private string m_Reward;   
        private string m_RewardRatio;   
        private EInt m_SpeRewardRate = 0;   
        private string m_SpeReward;   
        private EInt m_WorkTime = 0;   
        private EInt m_WorkInterval = 0;   
        private EInt m_WaitingTime = 0;   
        private EInt m_StoreAmount = 0;   
        private EInt m_MeanWhileWorkman = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 工作ID
        /// </summary>
        public  int  workID {get { return m_WorkID; } }
       
        /// <summary>
        /// 解锁条件，讲武堂等级
        /// </summary>
        public  int  homeLevel {get { return m_HomeLevel; } }
       
        /// <summary>
        /// 收集资源的类型
        /// </summary>
        public  string  collectObjType {get { return m_CollectObjType; } }
       
        /// <summary>
        /// 解锁文本
        /// </summary>
        public  string  unlockDesc {get { return m_UnlockDesc; } }
       
        /// <summary>
        /// 功能文本
        /// </summary>
        public  string  functionDesc {get { return m_FunctionDesc; } }
       
        /// <summary>
        /// 工作名称
        /// </summary>
        public  string  workName {get { return m_WorkName; } }
       
        /// <summary>
        /// 工作文本
        /// </summary>
        public  string  workTalk {get { return m_WorkTalk; } }
       
        /// <summary>
        /// 工作奖励
        /// </summary>
        public  string  reward {get { return m_Reward; } }
       
        /// <summary>
        /// 奖励系数随讲武堂
        /// </summary>
        public  string  rewardRatio {get { return m_RewardRatio; } }
       
        /// <summary>
        /// 特殊奖励概率，基数10000
        /// </summary>
        public  int  speRewardRate {get { return m_SpeRewardRate; } }
       
        /// <summary>
        /// 特殊奖励
        /// </summary>
        public  string  speReward {get { return m_SpeReward; } }
       
        /// <summary>
        /// 工作时长，秒
        /// </summary>
        public  int  workTime {get { return m_WorkTime; } }
       
        /// <summary>
        /// 生成工作间隔，秒
        /// </summary>
        public  int  workInterval {get { return m_WorkInterval; } }
       
        /// <summary>
        /// 自动采集等待，秒
        /// </summary>
        public  int  waitingTime {get { return m_WaitingTime; } }
       
        /// <summary>
        /// 最大保留数量
        /// </summary>
        public  int  storeAmount {get { return m_StoreAmount; } }
       
        /// <summary>
        /// 手动最大同时工作人数
        /// </summary>
        public  int  meanWhileWorkman {get { return m_MeanWhileWorkman; } }
       

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
                    m_WorkID = dataR.ReadInt();
                    break;
                case 1:
                    m_HomeLevel = dataR.ReadInt();
                    break;
                case 2:
                    m_CollectObjType = dataR.ReadString();
                    break;
                case 3:
                    m_UnlockDesc = dataR.ReadString();
                    break;
                case 4:
                    m_FunctionDesc = dataR.ReadString();
                    break;
                case 5:
                    m_WorkName = dataR.ReadString();
                    break;
                case 6:
                    m_WorkTalk = dataR.ReadString();
                    break;
                case 7:
                    m_Reward = dataR.ReadString();
                    break;
                case 8:
                    m_RewardRatio = dataR.ReadString();
                    break;
                case 9:
                    m_SpeRewardRate = dataR.ReadInt();
                    break;
                case 10:
                    m_SpeReward = dataR.ReadString();
                    break;
                case 11:
                    m_WorkTime = dataR.ReadInt();
                    break;
                case 12:
                    m_WorkInterval = dataR.ReadInt();
                    break;
                case 13:
                    m_WaitingTime = dataR.ReadInt();
                    break;
                case 14:
                    m_StoreAmount = dataR.ReadInt();
                    break;
                case 15:
                    m_MeanWhileWorkman = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(16);
          
          ret.Add("WorkID", 0);
          ret.Add("HomeLevel", 1);
          ret.Add("CollectObjType", 2);
          ret.Add("UnlockDesc", 3);
          ret.Add("FunctionDesc", 4);
          ret.Add("WorkName", 5);
          ret.Add("WorkTalk", 6);
          ret.Add("Reward", 7);
          ret.Add("RewardRatio", 8);
          ret.Add("SpeRewardRate", 9);
          ret.Add("SpeReward", 10);
          ret.Add("WorkTime", 11);
          ret.Add("WorkInterval", 12);
          ret.Add("WaitingTime", 13);
          ret.Add("StoreAmount", 14);
          ret.Add("MeanWhileWorkman", 15);
          return ret;
        }
    } 
}//namespace LR