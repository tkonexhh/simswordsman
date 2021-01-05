//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCommonTask
    {
        
       
        private EInt m_TaskID = 0;   
        private EInt m_HomeLevel = 0;   
        private string m_TaskTitle;   
        private string m_TaskDescription;   
        private string m_Type;   
        private EInt m_Time = 0;   
        private string m_Reward;   
        private EInt m_SpecialRewardRate = 0;   
        private string m_SpecialReward;   
        private string m_TaskEvent;   
        private string m_ConditionType;   
        private string m_ConditionValue;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 任务ID
        /// </summary>
        public  int  taskID {get { return m_TaskID; } }
       
        /// <summary>
        /// 讲武堂等级
        /// </summary>
        public  int  homeLevel {get { return m_HomeLevel; } }
       
        /// <summary>
        /// 任务标题
        /// </summary>
        public  string  taskTitle {get { return m_TaskTitle; } }
       
        /// <summary>
        /// 任务描述
        /// </summary>
        public  string  taskDescription {get { return m_TaskDescription; } }
       
        /// <summary>
        /// 任务类型
        /// </summary>
        public  string  type {get { return m_Type; } }
       
        /// <summary>
        /// 任务时长（秒）
        /// </summary>
        public  int  time {get { return m_Time; } }
       
        /// <summary>
        /// 奖励
        /// </summary>
        public  string  reward {get { return m_Reward; } }
       
        /// <summary>
        /// 特殊奖励概率（基数10000）
        /// </summary>
        public  int  specialRewardRate {get { return m_SpecialRewardRate; } }
       
        /// <summary>
        /// 特殊奖励
        /// </summary>
        public  string  specialReward {get { return m_SpecialReward; } }
       
        /// <summary>
        /// 事件(刷新任务状态)
        /// </summary>
        public  string  taskEvent {get { return m_TaskEvent; } }
       
        /// <summary>
        /// 完成条件类型
        /// </summary>
        public  string  conditionType {get { return m_ConditionType; } }
       
        /// <summary>
        /// 任务参数
        /// </summary>
        public  string  conditionValue {get { return m_ConditionValue; } }
       

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
                    m_TaskID = dataR.ReadInt();
                    break;
                case 1:
                    m_HomeLevel = dataR.ReadInt();
                    break;
                case 2:
                    m_TaskTitle = dataR.ReadString();
                    break;
                case 3:
                    m_TaskDescription = dataR.ReadString();
                    break;
                case 4:
                    m_Type = dataR.ReadString();
                    break;
                case 5:
                    m_Time = dataR.ReadInt();
                    break;
                case 6:
                    m_Reward = dataR.ReadString();
                    break;
                case 7:
                    m_SpecialRewardRate = dataR.ReadInt();
                    break;
                case 8:
                    m_SpecialReward = dataR.ReadString();
                    break;
                case 9:
                    m_TaskEvent = dataR.ReadString();
                    break;
                case 10:
                    m_ConditionType = dataR.ReadString();
                    break;
                case 11:
                    m_ConditionValue = dataR.ReadString();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(12);
          
          ret.Add("TaskID", 0);
          ret.Add("HomeLevel", 1);
          ret.Add("TaskTitle", 2);
          ret.Add("TaskDescription", 3);
          ret.Add("Type", 4);
          ret.Add("Time", 5);
          ret.Add("Reward", 6);
          ret.Add("SpecialRewardRate", 7);
          ret.Add("SpecialReward", 8);
          ret.Add("TaskEvent", 9);
          ret.Add("ConditionType", 10);
          ret.Add("ConditionValue", 11);
          return ret;
        }
    } 
}//namespace LR