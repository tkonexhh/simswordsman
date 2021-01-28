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
        private string m_Desc;   
        private string m_MakeRes;   
        private EInt m_MakeTime = 0;   
        private string m_EffectDesc;   
        private EFloat m_EffectParam = 0.0f;   
        private EInt m_Price = 0;   
        private string m_Icon;  
        
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
        public  float  effectParam {get { return m_EffectParam; } }
       
        /// <summary>
        /// 售价
        /// </summary>
        public  int  price {get { return m_Price; } }
       
        /// <summary>
        /// icon名
        /// </summary>
        public  string  icon {get { return m_Icon; } }
       

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
                    m_Desc = dataR.ReadString();
                    break;
                case 3:
                    m_MakeRes = dataR.ReadString();
                    break;
                case 4:
                    m_MakeTime = dataR.ReadInt();
                    break;
                case 5:
                    m_EffectDesc = dataR.ReadString();
                    break;
                case 6:
                    m_EffectParam = dataR.ReadFloat();
                    break;
                case 7:
                    m_Price = dataR.ReadInt();
                    break;
                case 8:
                    m_Icon = dataR.ReadString();
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
          ret.Add("Desc", 2);
          ret.Add("MakeRes", 3);
          ret.Add("MakeTime", 4);
          ret.Add("EffectDesc", 5);
          ret.Add("EffectParam", 6);
          ret.Add("Price", 7);
          ret.Add("Icon", 8);
          return ret;
        }
    } 
}//namespace LR