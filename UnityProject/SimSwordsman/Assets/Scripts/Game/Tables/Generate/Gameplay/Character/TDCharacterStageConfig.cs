//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCharacterStageConfig
    {
        
       
        private EInt m_Stage = 0;   
        private EInt m_FromLevel = 0;   
        private EInt m_ToLevel = 0;   
        private string m_BaseAtk;   
        private EInt m_GrowAtk = 0;   
        private string m_UnlockContent;   
        private EInt m_StartExp = 0;   
        private EInt m_GrowExp = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  stage {get { return m_Stage; } }
       
        /// <summary>
        /// 起始等级
        /// </summary>
        public  int  fromLevel {get { return m_FromLevel; } }
       
        /// <summary>
        /// 终点等级
        /// </summary>
        public  int  toLevel {get { return m_ToLevel; } }
       
        /// <summary>
        /// 基础功力
        /// </summary>
        public  string  baseAtk {get { return m_BaseAtk; } }
       
        /// <summary>
        /// 成长功力
        /// </summary>
        public  int  growAtk {get { return m_GrowAtk; } }
       
        /// <summary>
        /// 升阶解锁内容
        /// </summary>
        public  string  unlockContent {get { return m_UnlockContent; } }
       
        /// <summary>
        /// 起始经验
        /// </summary>
        public  int  startExp {get { return m_StartExp; } }
       
        /// <summary>
        /// 成长经验
        /// </summary>
        public  int  growExp {get { return m_GrowExp; } }
       

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
                    m_Stage = dataR.ReadInt();
                    break;
                case 1:
                    m_FromLevel = dataR.ReadInt();
                    break;
                case 2:
                    m_ToLevel = dataR.ReadInt();
                    break;
                case 3:
                    m_BaseAtk = dataR.ReadString();
                    break;
                case 4:
                    m_GrowAtk = dataR.ReadInt();
                    break;
                case 5:
                    m_UnlockContent = dataR.ReadString();
                    break;
                case 6:
                    m_StartExp = dataR.ReadInt();
                    break;
                case 7:
                    m_GrowExp = dataR.ReadInt();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(8);
          
          ret.Add("Stage", 0);
          ret.Add("FromLevel", 1);
          ret.Add("ToLevel", 2);
          ret.Add("BaseAtk", 3);
          ret.Add("GrowAtk", 4);
          ret.Add("UnlockContent", 5);
          ret.Add("StartExp", 6);
          ret.Add("GrowExp", 7);
          return ret;
        }
    } 
}//namespace LR