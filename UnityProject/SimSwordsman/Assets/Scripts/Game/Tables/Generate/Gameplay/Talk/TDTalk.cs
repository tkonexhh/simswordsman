//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTalk
    {
        
       
        private EInt m_Id = 0;   
        private EInt m_LobbyLevel = 0;   
        private string m_TalkWords;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 解锁条件（讲武堂等级）
        /// </summary>
        public  int  lobbyLevel {get { return m_LobbyLevel; } }
       
        /// <summary>
        /// 文本
        /// </summary>
        public  string  talkWords {get { return m_TalkWords; } }
       

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
                    m_LobbyLevel = dataR.ReadInt();
                    break;
                case 2:
                    m_TalkWords = dataR.ReadString();
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
          ret.Add("LobbyLevel", 1);
          ret.Add("TalkWords", 2);
          return ret;
        }
    } 
}//namespace LR