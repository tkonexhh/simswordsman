//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityLivableRoom
    {
        
       
        private EInt m_Level = 0;   
        private EInt m_HouseId = 0;   
        private string m_UpgradeCost;   
        private string m_UpgradePreconditions;   
        private EInt m_Capability = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 等级
        /// </summary>
        public  int  level {get { return m_Level; } }
       
        /// <summary>
        /// 屋舍ID
        /// </summary>
        public  int  houseId {get { return m_HouseId; } }
       
        /// <summary>
        /// 升级花费
        /// </summary>
        public  string  upgradeCost {get { return m_UpgradeCost; } }
       
        /// <summary>
        /// 升级条件
        /// </summary>
        public  string  upgradePreconditions {get { return m_UpgradePreconditions; } }
       
        /// <summary>
        /// 住宿人数
        /// </summary>
        public  int  capability {get { return m_Capability; } }
       

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
                    m_HouseId = dataR.ReadInt();
                    break;
                case 2:
                    m_UpgradeCost = dataR.ReadString();
                    break;
                case 3:
                    m_UpgradePreconditions = dataR.ReadString();
                    break;
                case 4:
                    m_Capability = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(5);
          
          ret.Add("Level", 0);
          ret.Add("HouseId", 1);
          ret.Add("UpgradeCost", 2);
          ret.Add("UpgradePreconditions", 3);
          ret.Add("Capability", 4);
          return ret;
        }
    } 
}//namespace LR