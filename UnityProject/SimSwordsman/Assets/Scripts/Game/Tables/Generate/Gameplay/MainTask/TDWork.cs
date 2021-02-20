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
        private string m_WorkName;   
        private string m_WorkTalk;   
        private string m_Reward;   
        private string m_SpeReward;   
        private EInt m_WorkTime = 0;   
        private EInt m_WordInterval = 0;   
        private EInt m_StoreAmount = 0;   
        private EInt m_MeanWhileWorkman = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 工作ID
        /// </summary>
        public  int  workID {get { return m_WorkID; } }
       
        /// <summary>
        /// 讲武堂等级
        /// </summary>
        public  int  homeLevel {get { return m_HomeLevel; } }
       
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
        /// 特殊奖励
        /// </summary>
        public  string  speReward {get { return m_SpeReward; } }
       
        /// <summary>
        /// 工作时长，秒
        /// </summary>
        public  int  workTime {get { return m_WorkTime; } }
       
        /// <summary>
        /// 工作间隔，分钟
        /// </summary>
        public  int  wordInterval {get { return m_WordInterval; } }
       
        /// <summary>
        /// 最大保留数量
        /// </summary>
        public  int  storeAmount {get { return m_StoreAmount; } }
       
        /// <summary>
        /// 最大同时工作人数
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
                    m_WorkName = dataR.ReadString();
                    break;
                case 3:
                    m_WorkTalk = dataR.ReadString();
                    break;
                case 4:
                    m_Reward = dataR.ReadString();
                    break;
                case 5:
                    m_SpeReward = dataR.ReadString();
                    break;
                case 6:
                    m_WorkTime = dataR.ReadInt();
                    break;
                case 7:
                    m_WordInterval = dataR.ReadInt();
                    break;
                case 8:
                    m_StoreAmount = dataR.ReadInt();
                    break;
                case 9:
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
          Dictionary<string, int> ret = new Dictionary<string, int>(10);
          
          ret.Add("WorkID", 0);
          ret.Add("HomeLevel", 1);
          ret.Add("WorkName", 2);
          ret.Add("WorkTalk", 3);
          ret.Add("Reward", 4);
          ret.Add("SpeReward", 5);
          ret.Add("WorkTime", 6);
          ret.Add("WordInterval", 7);
          ret.Add("StoreAmount", 8);
          ret.Add("MeanWhileWorkman", 9);
          return ret;
        }
    } 
}//namespace LR