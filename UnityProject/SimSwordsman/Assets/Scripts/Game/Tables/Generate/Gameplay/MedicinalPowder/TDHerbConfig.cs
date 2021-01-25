//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDHerbConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Name;   
        private string m_IconName;   
        private string m_Desc;   
        private string m_MakeRes;   
        private EInt m_MakeTime = 0;   
        private string m_EffectDesc;   
        private EInt m_EffectParam = 0;   
        private EInt m_Price = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  id {get { return m_Id; } }
       
        /// <summary>
        /// 名称
        /// </summary>
        public  string  name {get { return m_Name; } }
       
        /// <summary>
        /// icon名
        /// </summary>
        public  string  iconName {get { return m_IconName; } }
       
        /// <summary>
        /// 药物描述
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       
        /// <summary>
        /// 制作材料
        /// </summary>
        public  string  makeRes {get { return m_MakeRes; } }
       
        /// <summary>
        /// 制作时长(分钟)
        /// </summary>
        public  int  makeTime {get { return m_MakeTime; } }
       
        /// <summary>
        /// 效果描述
        /// </summary>
        public  string  effectDesc {get { return m_EffectDesc; } }
       
        /// <summary>
        /// 效果参数
        /// </summary>
        public  int  effectParam {get { return m_EffectParam; } }
       
        /// <summary>
        /// 售价
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
                    m_Name = dataR.ReadString();
                    break;
                case 2:
                    m_IconName = dataR.ReadString();
                    break;
                case 3:
                    m_Desc = dataR.ReadString();
                    break;
                case 4:
                    m_MakeRes = dataR.ReadString();
                    break;
                case 5:
                    m_MakeTime = dataR.ReadInt();
                    break;
                case 6:
                    m_EffectDesc = dataR.ReadString();
                    break;
                case 7:
                    m_EffectParam = dataR.ReadInt();
                    break;
                case 8:
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
          Dictionary<string, int> ret = new Dictionary<string, int>(9);
          
          ret.Add("Id", 0);
          ret.Add("Name", 1);
          ret.Add("IconName", 2);
          ret.Add("Desc", 3);
          ret.Add("MakeRes", 4);
          ret.Add("MakeTime", 5);
          ret.Add("EffectDesc", 6);
          ret.Add("EffectParam", 7);
          ret.Add("Price", 8);
          return ret;
        }
    } 
}//namespace LR