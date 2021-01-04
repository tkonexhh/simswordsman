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
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(5);
          
          ret.Add("Id", 0);
          ret.Add("Name", 1);
          ret.Add("IconName", 2);
          ret.Add("Desc", 3);
          ret.Add("Price", 4);
          return ret;
        }
    } 
}//namespace LR