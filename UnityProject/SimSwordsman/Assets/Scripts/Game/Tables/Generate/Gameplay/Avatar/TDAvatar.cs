//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDAvatar
    {
        
       
        private EInt m_Id = 0;   
        private string m_UnlockingCondition;   
        private string m_HeadIcon;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 解锁条件
        /// </summary>
        public  string  unlockingCondition {get { return m_UnlockingCondition; } }
       
        /// <summary>
        /// 头像icon
        /// </summary>
        public  string  headIcon {get { return m_HeadIcon; } }
       

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
                    m_UnlockingCondition = dataR.ReadString();
                    break;
                case 2:
                    m_HeadIcon = dataR.ReadString();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(3);
          
          ret.Add("Id", 0);
          ret.Add("UnlockingCondition", 1);
          ret.Add("HeadIcon", 2);
          return ret;
        }
    } 
}//namespace LR