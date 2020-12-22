//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDChapterConfig
    {
        
       
        private EInt m_Chapter = 0;   
        private string m_Desc;   
        private string m_UnlockPrecondition;   
        private string m_ClanType;   
        private EInt m_ChapterCount = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  chapter {get { return m_Chapter; } }
       
        /// <summary>
        /// Key
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       
        /// <summary>
        /// Value
        /// </summary>
        public  string  unlockPrecondition {get { return m_UnlockPrecondition; } }
       
        /// <summary>
        /// Value
        /// </summary>
        public  string  clanType {get { return m_ClanType; } }
       
        /// <summary>
        /// 关卡数量
        /// </summary>
        public  int  chapterCount {get { return m_ChapterCount; } }
       

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
                    m_Chapter = dataR.ReadInt();
                    break;
                case 1:
                    m_Desc = dataR.ReadString();
                    break;
                case 2:
                    m_UnlockPrecondition = dataR.ReadString();
                    break;
                case 3:
                    m_ClanType = dataR.ReadString();
                    break;
                case 4:
                    m_ChapterCount = dataR.ReadInt();
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
          
          ret.Add("Chapter", 0);
          ret.Add("Desc", 1);
          ret.Add("UnlockPrecondition", 2);
          ret.Add("ClanType", 3);
          ret.Add("ChapterCount", 4);
          return ret;
        }
    } 
}//namespace LR