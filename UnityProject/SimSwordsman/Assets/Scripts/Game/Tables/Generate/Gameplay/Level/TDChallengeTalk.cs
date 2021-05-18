//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDChallengeTalk
    {
        
       
        private EInt m_DialogID = 0;   
        private EInt m_LevelID = 0;   
        private string m_Character;   
        private EInt m_Toward = 0;   
        private string m_Text;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 对白
        /// </summary>
        public  int  dialogID {get { return m_DialogID; } }
       
        /// <summary>
        /// 关卡ID
        /// </summary>
        public  int  levelID {get { return m_LevelID; } }
       
        /// <summary>
        /// 头像资源
        /// </summary>
        public  string  character {get { return m_Character; } }
       
        /// <summary>
        /// 角色朝向
        /// </summary>
        public  int  toward {get { return m_Toward; } }
       
        /// <summary>
        /// 对白文本
        /// </summary>
        public  string  text {get { return m_Text; } }
       

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
                    m_DialogID = dataR.ReadInt();
                    break;
                case 1:
                    m_LevelID = dataR.ReadInt();
                    break;
                case 2:
                    m_Character = dataR.ReadString();
                    break;
                case 3:
                    m_Toward = dataR.ReadInt();
                    break;
                case 4:
                    m_Text = dataR.ReadString();
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
          
          ret.Add("DialogID", 0);
          ret.Add("LevelID", 1);
          ret.Add("Character", 2);
          ret.Add("Toward", 3);
          ret.Add("Text", 4);
          return ret;
        }
    } 
}//namespace LR