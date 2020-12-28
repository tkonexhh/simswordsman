//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDKongfuConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_KongfuName;   
        private string m_Quality;   
        private string m_Desc;   
        private string m_AtkRange;   
        private string m_UpgradeExp;   
        private string m_AnimationName;   
        private EInt m_Price = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 功夫名称
        /// </summary>
        public  string  kongfuName {get { return m_KongfuName; } }
       
        /// <summary>
        /// 品质
        /// </summary>
        public  string  quality {get { return m_Quality; } }
       
        /// <summary>
        /// 描述
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       
        /// <summary>
        /// 加成比例
        /// </summary>
        public  string  atkRange {get { return m_AtkRange; } }
       
        /// <summary>
        /// 升级经验
        /// </summary>
        public  string  upgradeExp {get { return m_UpgradeExp; } }
       
        /// <summary>
        /// 动画名称
        /// </summary>
        public  string  animationName {get { return m_AnimationName; } }
       
        /// <summary>
        /// 出售价格
        /// </summary>
        public  int  price {get { return m_Price; } }
       

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
                    m_KongfuName = dataR.ReadString();
                    break;
                case 2:
                    m_Quality = dataR.ReadString();
                    break;
                case 3:
                    m_Desc = dataR.ReadString();
                    break;
                case 4:
                    m_AtkRange = dataR.ReadString();
                    break;
                case 5:
                    m_UpgradeExp = dataR.ReadString();
                    break;
                case 6:
                    m_AnimationName = dataR.ReadString();
                    break;
                case 7:
                    m_Price = dataR.ReadInt();
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
          ret.Add("KongfuName", 1);
          ret.Add("Quality", 2);
          ret.Add("Desc", 3);
          ret.Add("AtkRange", 4);
          ret.Add("UpgradeExp", 5);
          ret.Add("AnimationName", 6);
          ret.Add("Price", 7);
          return ret;
        }
    } 
}//namespace LR