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
        private string m_TriggerType;   
        private EInt m_HomeLevel = 0;   
        private string m_TaskTitle;   
        private string m_TaskDescription;   
        private string m_Type;   
        private EInt m_Time = 0;   
        private string m_Reward;   
        private EInt m_SpecialRewardRate = 0;   
        private string m_SpecialReward;   
        private EInt m_ExpReward = 0;   
        private EInt m_KongfuExpReward = 0;   
        private string m_Enemy;   
        private EInt m_RoleAmount = 0;   
        private EInt m_RoleLevelRequired = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 任务ID
        /// </summary>
        public  int  taskID {get { return m_TaskID; } }
       
        /// <summary>
        /// 触发类型
        /// </summary>
        public  string  triggerType {get { return m_TriggerType; } }
       
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
        /// 特殊奖励，多个只获取一个
        /// </summary>
        public  string  specialReward {get { return m_SpecialReward; } }
       
        /// <summary>
        /// 经验奖励
        /// </summary>
        public  int  expReward {get { return m_ExpReward; } }
       
        /// <summary>
        /// 功夫经验奖励
        /// </summary>
        public  int  kongfuExpReward {get { return m_KongfuExpReward; } }
       
        /// <summary>
        /// 敌人配置
        /// </summary>
        public  string  enemy {get { return m_Enemy; } }
       
        /// <summary>
        /// 可派弟子数量
        /// </summary>
        public  int  roleAmount {get { return m_RoleAmount; } }
       
        /// <summary>
        /// 弟子等级要求
        /// </summary>
        public  int  roleLevelRequired {get { return m_RoleLevelRequired; } }
       

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
                    m_TriggerType = dataR.ReadString();
                    break;
                case 2:
                    m_HomeLevel = dataR.ReadInt();
                    break;
                case 3:
                    m_TaskTitle = dataR.ReadString();
                    break;
                case 4:
                    m_TaskDescription = dataR.ReadString();
                    break;
                case 5:
                    m_Type = dataR.ReadString();
                    break;
                case 6:
                    m_Time = dataR.ReadInt();
                    break;
                case 7:
                    m_Reward = dataR.ReadString();
                    break;
                case 8:
                    m_SpecialRewardRate = dataR.ReadInt();
                    break;
                case 9:
                    m_SpecialReward = dataR.ReadString();
                    break;
                case 10:
                    m_ExpReward = dataR.ReadInt();
                    break;
                case 11:
                    m_KongfuExpReward = dataR.ReadInt();
                    break;
                case 12:
                    m_Enemy = dataR.ReadString();
                    break;
                case 13:
                    m_RoleAmount = dataR.ReadInt();
                    break;
                case 14:
                    m_RoleLevelRequired = dataR.ReadInt();
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
          
          ret.Add("TaskID", 0);
          ret.Add("TriggerType", 1);
          ret.Add("HomeLevel", 2);
          ret.Add("TaskTitle", 3);
          ret.Add("TaskDescription", 4);
          ret.Add("Type", 5);
          ret.Add("Time", 6);
          ret.Add("Reward", 7);
          ret.Add("SpecialRewardRate", 8);
          ret.Add("SpecialReward", 9);
          ret.Add("ExpReward", 10);
          ret.Add("KongfuExpReward", 11);
          ret.Add("Enemy", 12);
          ret.Add("RoleAmount", 13);
          ret.Add("RoleLevelRequired", 14);
          return ret;
        }
    } 
}//namespace LR