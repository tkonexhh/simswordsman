//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDWeaponConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Name;   
        private string m_UpgradePreconditions;   
        private string m_Desc;  
        
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
        /// Value
        /// </summary>
        public  string  upgradePreconditions {get { return m_UpgradePreconditions; } }
       
        /// <summary>
        /// Value
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       

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
                    m_UpgradePreconditions = dataR.ReadString();
                    break;
                case 3:
                    m_Desc = dataR.ReadString();
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
          ret.Add("Name", 1);
          ret.Add("UpgradePreconditions", 2);
          ret.Add("Desc", 3);
          return ret;
        }
    } 
}//namespace LR