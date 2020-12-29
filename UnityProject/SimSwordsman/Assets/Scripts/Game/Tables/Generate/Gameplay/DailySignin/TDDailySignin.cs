//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDDailySignin
    {
        
       
        private EInt m_Id = 0;   
        private string m_RewardType;   
        private EInt m_RewardID = 0;   
        private EInt m_RewardCount = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 签到奖励类型
        /// </summary>
        public  string  rewardType {get { return m_RewardType; } }
       
        /// <summary>
        /// 奖励ID
        /// </summary>
        public  int  rewardID {get { return m_RewardID; } }
       
        /// <summary>
        /// 奖励数量
        /// </summary>
        public  int  rewardCount {get { return m_RewardCount; } }
       

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
                    m_RewardType = dataR.ReadString();
                    break;
                case 2:
                    m_RewardID = dataR.ReadInt();
                    break;
                case 3:
                    m_RewardCount = dataR.ReadInt();
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
          
          ret.Add("Id", 0);
          ret.Add("RewardType", 1);
          ret.Add("RewardID", 2);
          ret.Add("RewardCount", 3);
          return ret;
        }
    } 
}//namespace LR