//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Name;   
        private string m_Desc;   
        private string m_PreFacility;   
        private EInt m_LobbyLevelRequire = 0;   
        private EInt m_UnlockCost = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// Value
        /// </summary>
        public  string  name {get { return m_Name; } }
       
        /// <summary>
        /// 描述
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       
        /// <summary>
        /// 开荒前置建筑
        /// </summary>
        public  string  preFacility {get { return m_PreFacility; } }
       
        /// <summary>
        /// 建造条件-大厅等级
        /// </summary>
        public  int  lobbyLevelRequire {get { return m_LobbyLevelRequire; } }
       
        /// <summary>
        /// 建造/升级消耗
        /// </summary>
        public  int  unlockCost {get { return m_UnlockCost; } }
       

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
                    m_Name = dataR.ReadString();
                    break;
                case 2:
                    m_Desc = dataR.ReadString();
                    break;
                case 3:
                    m_PreFacility = dataR.ReadString();
                    break;
                case 4:
                    m_LobbyLevelRequire = dataR.ReadInt();
                    break;
                case 5:
                    m_UnlockCost = dataR.ReadInt();
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
          ret.Add("Name", 1);
          ret.Add("Desc", 2);
          ret.Add("PreFacility", 3);
          ret.Add("LobbyLevelRequire", 4);
          ret.Add("UnlockCost", 5);
          return ret;
        }
    } 
}//namespace LR