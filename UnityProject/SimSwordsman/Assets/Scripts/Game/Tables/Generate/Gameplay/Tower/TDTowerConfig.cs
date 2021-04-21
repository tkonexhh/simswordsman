//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTowerConfig
    {
        
       
        private EInt m_FloorNum = 0;   
        private string m_Rwardtype;   
        private EInt m_FcoinNum = 0;   
        private EFloat m_AtkNum = 0.0f;   
        private EInt m_CanRevive = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 层数
        /// </summary>
        public  int  floorNum {get { return m_FloorNum; } }
       
        /// <summary>
        /// 奖励类型
        /// </summary>
        public  string  rwardtype {get { return m_Rwardtype; } }
       
        /// <summary>
        /// 伏魔币数量
        /// </summary>
        public  int  fcoinNum {get { return m_FcoinNum; } }
       
        /// <summary>
        /// 功力系数
        /// </summary>
        public  float  atkNum {get { return m_AtkNum; } }
       
        /// <summary>
        /// 是否可以复活
        /// </summary>
        public  int  canRevive {get { return m_CanRevive; } }
       

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
                    m_FloorNum = dataR.ReadInt();
                    break;
                case 1:
                    m_Rwardtype = dataR.ReadString();
                    break;
                case 2:
                    m_FcoinNum = dataR.ReadInt();
                    break;
                case 3:
                    m_AtkNum = dataR.ReadFloat();
                    break;
                case 4:
                    m_CanRevive = dataR.ReadInt();
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
          
          ret.Add("FloorNum", 0);
          ret.Add("Rwardtype", 1);
          ret.Add("FcoinNum", 2);
          ret.Add("AtkNum", 3);
          ret.Add("CanRevive", 4);
          return ret;
        }
    } 
}//namespace LR