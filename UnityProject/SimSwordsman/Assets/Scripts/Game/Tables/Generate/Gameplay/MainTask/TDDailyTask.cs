//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDDailyTask
    {
        
       
        private EInt m_TaskID = 0;   
        private EInt m_HomeLevel = 0;   
        private string m_IconRes;   
        private string m_TaskTitle;   
        private string m_TaskDescription;   
        private string m_Type;   
        private EInt m_TaskCount = 0;   
        private string m_Reward;  
        
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
        /// 任务icon
        /// </summary>
        public  string  iconRes {get { return m_IconRes; } }
       
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
        /// 数量参数
        /// </summary>
        public  int  taskCount {get { return m_TaskCount; } }
       
        /// <summary>
        /// 奖励
        /// </summary>
        public  string  reward {get { return m_Reward; } }
       

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
                    m_IconRes = dataR.ReadString();
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
                    m_TaskCount = dataR.ReadInt();
                    break;
                case 7:
                    m_Reward = dataR.ReadString();
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
          
          ret.Add("TaskID", 0);
          ret.Add("HomeLevel", 1);
          ret.Add("IconRes", 2);
          ret.Add("TaskTitle", 3);
          ret.Add("TaskDescription", 4);
          ret.Add("Type", 5);
          ret.Add("TaskCount", 6);
          ret.Add("Reward", 7);
          return ret;
        }
    } 
}//namespace LR