//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDScenario
    {
        
       
        private EInt m_ChapterID = 0;   
        private string m_ChapterName;   
        private string m_CDialogID;   
        private EInt m_CfunctionID = 0;   
        private string m_RewardParam;   
        private string m_Background;   
        private EInt m_LevelID = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 章节ID
        /// </summary>
        public  int  chapterID {get { return m_ChapterID; } }
       
        /// <summary>
        /// 章节名称ID
        /// </summary>
        public  string  chapterName {get { return m_ChapterName; } }
       
        /// <summary>
        /// 章节剧情ID
        /// </summary>
        public  string  cDialogID {get { return m_CDialogID; } }
       
        /// <summary>
        /// 角标功能ID
        /// </summary>
        public  int  cfunctionID {get { return m_CfunctionID; } }
       
        /// <summary>
        /// 奖励
        /// </summary>
        public  string  rewardParam {get { return m_RewardParam; } }
       
        /// <summary>
        /// 拍照背景
        /// </summary>
        public  string  background {get { return m_Background; } }
       
        /// <summary>
        /// 解锁关卡ID
        /// </summary>
        public  int  levelID {get { return m_LevelID; } }
       

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
                    m_ChapterID = dataR.ReadInt();
                    break;
                case 1:
                    m_ChapterName = dataR.ReadString();
                    break;
                case 2:
                    m_CDialogID = dataR.ReadString();
                    break;
                case 3:
                    m_CfunctionID = dataR.ReadInt();
                    break;
                case 4:
                    m_RewardParam = dataR.ReadString();
                    break;
                case 5:
                    m_Background = dataR.ReadString();
                    break;
                case 6:
                    m_LevelID = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(7);
          
          ret.Add("ChapterID", 0);
          ret.Add("ChapterName", 1);
          ret.Add("CDialogID", 2);
          ret.Add("CfunctionID", 3);
          ret.Add("RewardParam", 4);
          ret.Add("Background", 5);
          ret.Add("LevelID", 6);
          return ret;
        }
    } 
}//namespace LR