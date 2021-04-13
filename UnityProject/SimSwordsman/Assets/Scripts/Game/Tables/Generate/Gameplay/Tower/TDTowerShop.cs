//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTowerShop
    {
        
       
        private EInt m_GoodsID = 0;   
        private string m_GoodsInfo;   
        private string m_QuaTag;   
        private EInt m_Price = 0;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 商品编号
        /// </summary>
        public  int  goodsID {get { return m_GoodsID; } }
       
        /// <summary>
        /// 商品信息
        /// </summary>
        public  string  goodsInfo {get { return m_GoodsInfo; } }
       
        /// <summary>
        /// 品质标签
        /// </summary>
        public  string  quaTag {get { return m_QuaTag; } }
       
        /// <summary>
        /// 兑换价格
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
                    m_GoodsID = dataR.ReadInt();
                    break;
                case 1:
                    m_GoodsInfo = dataR.ReadString();
                    break;
                case 2:
                    m_QuaTag = dataR.ReadString();
                    break;
                case 3:
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
          Dictionary<string, int> ret = new Dictionary<string, int>(4);
          
          ret.Add("GoodsID", 0);
          ret.Add("GoodsInfo", 1);
          ret.Add("QuaTag", 2);
          ret.Add("Price", 3);
          return ret;
        }
    } 
}//namespace LR