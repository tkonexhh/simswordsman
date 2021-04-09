//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTowerEnemyConfig
    {
        
       
        private EInt m_PoolNum = 0;   
        private string m_EnemyClan;   
        private string m_EnemyHeadIcon;   
        private string m_Boss;   
        private string m_Enemies;   
        private string m_Desc;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 池编号
        /// </summary>
        public  int  poolNum {get { return m_PoolNum; } }
       
        /// <summary>
        /// 敌人门派
        /// </summary>
        public  string  enemyClan {get { return m_EnemyClan; } }
       
        /// <summary>
        /// 敌人头像
        /// </summary>
        public  string  enemyHeadIcon {get { return m_EnemyHeadIcon; } }
       
        /// <summary>
        /// BOSS配置，格式ID
        /// </summary>
        public  string  boss {get { return m_Boss; } }
       
        /// <summary>
        /// 敌人配置，格式ID
        /// </summary>
        public  string  enemies {get { return m_Enemies; } }
       
        /// <summary>
        /// 描述
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       

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
                    m_PoolNum = dataR.ReadInt();
                    break;
                case 1:
                    m_EnemyClan = dataR.ReadString();
                    break;
                case 2:
                    m_EnemyHeadIcon = dataR.ReadString();
                    break;
                case 3:
                    m_Boss = dataR.ReadString();
                    break;
                case 4:
                    m_Enemies = dataR.ReadString();
                    break;
                case 5:
                    m_Desc = dataR.ReadString();
                    break;
                default:
                    //TableHelper.CacheNewField(dataR, schemeNames[col], m_DataCacheNoGenerate);
                    break;
            }
          }

        }
        
        public static Dictionary<string, int> GetFieldHeadIndex()
        {
          Dictionary<string, int> ret = new Dictionary<string, int>(6);
          
          ret.Add("PoolNum", 0);
          ret.Add("EnemyClan", 1);
          ret.Add("EnemyHeadIcon", 2);
          ret.Add("Boss", 3);
          ret.Add("Enemies", 4);
          ret.Add("Desc", 5);
          return ret;
        }
    } 
}//namespace LR