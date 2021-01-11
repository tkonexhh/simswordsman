//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDSystemConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_System;   
        private string m_SyeName;   
        private EInt m_LobbyLevelRequired = 0;   
        private string m_OtherRequirement;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 系统
        /// </summary>
        public  string  system {get { return m_System; } }
       
        /// <summary>
        /// 系统名称
        /// </summary>
        public  string  syeName {get { return m_SyeName; } }
       
        /// <summary>
        /// 解锁条件，大厅等级
        /// </summary>
        public  int  lobbyLevelRequired {get { return m_LobbyLevelRequired; } }
       
        /// <summary>
        /// 其他条件
        /// </summary>
        public  string  otherRequirement {get { return m_OtherRequirement; } }
       

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
                    m_System = dataR.ReadString();
                    break;
                case 2:
                    m_SyeName = dataR.ReadString();
                    break;
                case 3:
                    m_LobbyLevelRequired = dataR.ReadInt();
                    break;
                case 4:
                    m_OtherRequirement = dataR.ReadString();
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
          
          ret.Add("Id", 0);
          ret.Add("System", 1);
          ret.Add("SyeName", 2);
          ret.Add("LobbyLevelRequired", 3);
          ret.Add("OtherRequirement", 4);
          return ret;
        }
    } 
}//namespace LR