//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDHeroTrialConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Clan;   
        private string m_NameDes;   
        private string m_JobName;   
        private string m_OrdinaryEnemy;   
        private string m_EliteEnemy;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 门派
        /// </summary>
        public  string  clan {get { return m_Clan; } }
       
        /// <summary>
        /// 帮派名称
        /// </summary>
        public  string  nameDes {get { return m_NameDes; } }
       
        /// <summary>
        /// 职位
        /// </summary>
        public  string  jobName {get { return m_JobName; } }
       
        /// <summary>
        /// 敌方刷新-普通敌方
        /// </summary>
        public  string  ordinaryEnemy {get { return m_OrdinaryEnemy; } }
       
        /// <summary>
        /// 敌方刷新-精英敌方
        /// </summary>
        public  string  eliteEnemy {get { return m_EliteEnemy; } }
       

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
                    m_Clan = dataR.ReadString();
                    break;
                case 2:
                    m_NameDes = dataR.ReadString();
                    break;
                case 3:
                    m_JobName = dataR.ReadString();
                    break;
                case 4:
                    m_OrdinaryEnemy = dataR.ReadString();
                    break;
                case 5:
                    m_EliteEnemy = dataR.ReadString();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(6);
          
          ret.Add("Id", 0);
          ret.Add("Clan", 1);
          ret.Add("NameDes", 2);
          ret.Add("JobName", 3);
          ret.Add("OrdinaryEnemy", 4);
          ret.Add("EliteEnemy", 5);
          return ret;
        }
    } 
}//namespace LR