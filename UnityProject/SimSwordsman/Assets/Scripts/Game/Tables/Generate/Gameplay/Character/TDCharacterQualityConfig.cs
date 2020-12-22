//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCharacterQualityConfig
    {
        
       
        private string m_Quality;   
        private EInt m_MaxLevel = 0;   
        private EInt m_KongfuSlot = 0;   
        private string m_KongfuNeedLevel;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 品质
        /// </summary>
        public  string  quality {get { return m_Quality; } }
       
        /// <summary>
        /// 最大等级
        /// </summary>
        public  int  maxLevel {get { return m_MaxLevel; } }
       
        /// <summary>
        /// 功夫数
        /// </summary>
        public  int  kongfuSlot {get { return m_KongfuSlot; } }
       
        /// <summary>
        /// 功夫解锁等级
        /// </summary>
        public  string  kongfuNeedLevel {get { return m_KongfuNeedLevel; } }
       

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
                    m_Quality = dataR.ReadString();
                    break;
                case 1:
                    m_MaxLevel = dataR.ReadInt();
                    break;
                case 2:
                    m_KongfuSlot = dataR.ReadInt();
                    break;
                case 3:
                    m_KongfuNeedLevel = dataR.ReadString();
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
          
          ret.Add("Quality", 0);
          ret.Add("MaxLevel", 1);
          ret.Add("KongfuSlot", 2);
          ret.Add("KongfuNeedLevel", 3);
          return ret;
        }
    } 
}//namespace LR