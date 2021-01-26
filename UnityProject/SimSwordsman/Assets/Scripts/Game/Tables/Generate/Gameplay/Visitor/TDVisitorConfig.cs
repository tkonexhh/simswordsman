//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDVisitorConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Name;   
        private string m_RoleRes;   
        private string m_Desc;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 访客名
        /// </summary>
        public  string  name {get { return m_Name; } }
       
        /// <summary>
        /// 角色立绘
        /// </summary>
        public  string  roleRes {get { return m_RoleRes; } }
       
        /// <summary>
        /// 访客文本
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
                    m_RoleRes = dataR.ReadString();
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
          ret.Add("RoleRes", 2);
          ret.Add("Desc", 3);
          return ret;
        }
    } 
}//namespace LR