//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDItemConfig
    {
        
       
        private EInt m_Id = 0;   
        private string m_Name;   
        private string m_IconName;   
        private string m_Desc;   
        private EInt m_Price = 0;   
        private string m_UnlockDesc;   
        private string m_FunctionDesc;  
        
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
        /// 说明
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       
        /// <summary>
        /// 出售价格
        /// </summary>
        public  int  price {get { return m_Price; } }
       
        /// <summary>
        /// 解锁文本
        /// </summary>
        public  string  unlockDesc {get { return m_UnlockDesc; } }
       
        /// <summary>
        /// 功能文本
        /// </summary>
        public  string  functionDesc {get { return m_FunctionDesc; } }
       

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
                    m_Price = dataR.ReadInt();
                    break;
                case 5:
                    m_UnlockDesc = dataR.ReadString();
                    break;
                case 6:
                    m_FunctionDesc = dataR.ReadString();
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
          
          ret.Add("Id", 0);
          ret.Add("Name", 1);
          ret.Add("IconName", 2);
          ret.Add("Desc", 3);
          ret.Add("Price", 4);
          ret.Add("UnlockDesc", 5);
          ret.Add("FunctionDesc", 6);
          return ret;
        }
    } 
}//namespace LR