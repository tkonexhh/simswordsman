//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDKongfuAnimationConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_AnimationName;   
        private string m_AtkRange;   
        private string m_AtkDelay;   
        private string m_FootSE;   
        private string m_CastSE;   
        private string m_HitSE;   
        private string m_AttackSound;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 动画名称
        /// </summary>
        public  string  animationName {get { return m_AnimationName; } }
       
        /// <summary>
        /// 攻击距离
        /// </summary>
        public  string  atkRange {get { return m_AtkRange; } }
       
        /// <summary>
        /// 攻击延时
        /// </summary>
        public  string  atkDelay {get { return m_AtkDelay; } }
       
        /// <summary>
        /// 脚步特效
        /// </summary>
        public  string  footSE {get { return m_FootSE; } }
       
        /// <summary>
        /// 释放特效
        /// </summary>
        public  string  castSE {get { return m_CastSE; } }
       
        /// <summary>
        /// 受击特效
        /// </summary>
        public  string  hitSE {get { return m_HitSE; } }
       
        /// <summary>
        /// 攻击声音
        /// </summary>
        public  string  attackSound {get { return m_AttackSound; } }
       

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
                    m_AnimationName = dataR.ReadString();
                    break;
                case 2:
                    m_AtkRange = dataR.ReadString();
                    break;
                case 3:
                    m_AtkDelay = dataR.ReadString();
                    break;
                case 4:
                    m_FootSE = dataR.ReadString();
                    break;
                case 5:
                    m_CastSE = dataR.ReadString();
                    break;
                case 6:
                    m_HitSE = dataR.ReadString();
                    break;
                case 7:
                    m_AttackSound = dataR.ReadString();
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
          
          ret.Add("Id", 0);
          ret.Add("AnimationName", 1);
          ret.Add("AtkRange", 2);
          ret.Add("AtkDelay", 3);
          ret.Add("FootSE", 4);
          ret.Add("CastSE", 5);
          ret.Add("HitSE", 6);
          ret.Add("AttackSound", 7);
          return ret;
        }
    } 
}//namespace LR