//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDDeliver
    {
        
       
        private EInt m_Level = 0;   
        private string m_NormalReward;   
        private string m_RareReward;   
        private EInt m_Duration = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  level {get { return m_Level; } }
       
        /// <summary>
        /// 普通奖励池
        /// </summary>
        public  string  normalReward {get { return m_NormalReward; } }
       
        /// <summary>
        /// 稀有奖励池
        /// </summary>
        public  string  rareReward {get { return m_RareReward; } }
       
        /// <summary>
        /// 押镖时长，分钟
        /// </summary>
        public  int  duration {get { return m_Duration; } }
       

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
                    m_NormalReward = dataR.ReadString();
                    break;
                case 2:
                    m_RareReward = dataR.ReadString();
                    break;
                case 3:
                    m_Duration = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(4);
          
          ret.Add("Level", 0);
          ret.Add("NormalReward", 1);
          ret.Add("RareReward", 2);
          ret.Add("Duration", 3);
          return ret;
        }
    } 
}//namespace LR