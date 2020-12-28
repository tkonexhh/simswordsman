//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDEnemyConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Name;   
        private string m_Clan;   
        private string m_KongfuName;   
        private string m_AnimationName;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 名字
        /// </summary>
        public  string  name {get { return m_Name; } }
       
        /// <summary>
        /// 门派
        /// </summary>
        public  string  clan {get { return m_Clan; } }
       
        /// <summary>
        /// 功夫名称
        /// </summary>
        public  string  kongfuName {get { return m_KongfuName; } }
       
        /// <summary>
        /// 动画名称
        /// </summary>
        public  string  animationName {get { return m_AnimationName; } }
       

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
                    m_Clan = dataR.ReadString();
                    break;
                case 3:
                    m_KongfuName = dataR.ReadString();
                    break;
                case 4:
                    m_AnimationName = dataR.ReadString();
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
          ret.Add("Name", 1);
          ret.Add("Clan", 2);
          ret.Add("KongfuName", 3);
          ret.Add("AnimationName", 4);
          return ret;
        }
    } 
}//namespace LR