//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityPracticeField
    {
        
       
        private EInt m_Id = 0;   
        private EInt m_Level = 0;   
        private EInt m_HouseId = 0;   
        private string m_UpgradeRes;   
        private EInt m_UpgradeCost = 0;   
        private EInt m_UpgradePreconditions = 0;   
        private EInt m_Capability = 0;   
        private EInt m_LevelUpSpeed = 0;   
        private EInt m_Duration = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 等级
        /// </summary>
        public  int  level {get { return m_Level; } }
       
        /// <summary>
        /// 练功场ID
        /// </summary>
        public  int  houseId {get { return m_HouseId; } }
       
        /// <summary>
        /// 升级资源
        /// </summary>
        public  string  upgradeRes {get { return m_UpgradeRes; } }
       
        /// <summary>
        /// 升级花费
        /// </summary>
        public  int  upgradeCost {get { return m_UpgradeCost; } }
       
        /// <summary>
        /// 升级条件
        /// </summary>
        public  int  upgradePreconditions {get { return m_UpgradePreconditions; } }
       
        /// <summary>
        /// 练功位数量
        /// </summary>
        public  int  capability {get { return m_Capability; } }
       
        /// <summary>
        /// 经验获取速度
        /// </summary>
        public  int  levelUpSpeed {get { return m_LevelUpSpeed; } }
       
        /// <summary>
        /// 练功时长，秒
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
                    m_Id = dataR.ReadInt();
                    break;
                case 1:
                    m_Level = dataR.ReadInt();
                    break;
                case 2:
                    m_HouseId = dataR.ReadInt();
                    break;
                case 3:
                    m_UpgradeRes = dataR.ReadString();
                    break;
                case 4:
                    m_UpgradeCost = dataR.ReadInt();
                    break;
                case 5:
                    m_UpgradePreconditions = dataR.ReadInt();
                    break;
                case 6:
                    m_Capability = dataR.ReadInt();
                    break;
                case 7:
                    m_LevelUpSpeed = dataR.ReadInt();
                    break;
                case 8:
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
          Dictionary<string, int> ret = new Dictionary<string, int>(9);
          
          ret.Add("Id", 0);
          ret.Add("Level", 1);
          ret.Add("HouseId", 2);
          ret.Add("UpgradeRes", 3);
          ret.Add("UpgradeCost", 4);
          ret.Add("UpgradePreconditions", 5);
          ret.Add("Capability", 6);
          ret.Add("LevelUpSpeed", 7);
          ret.Add("Duration", 8);
          return ret;
        }
    } 
}//namespace LR