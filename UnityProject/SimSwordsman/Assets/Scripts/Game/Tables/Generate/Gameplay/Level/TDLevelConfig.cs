//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDLevelConfig
    {
        
       
        private EInt m_Level = 0;   
        private EInt m_Chapter = 0;   
        private string m_EnemyHeadIcon;   
        private string m_Enemies;   
        private string m_Desc;   
        private string m_Reward;   
        private EInt m_RecommendAtkValue = 0;   
        private string m_BattleName;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// ID
        /// </summary>
        public  int  level {get { return m_Level; } }
       
        /// <summary>
        /// 所属章节
        /// </summary>
        public  int  chapter {get { return m_Chapter; } }
       
        /// <summary>
        /// 敌人头像
        /// </summary>
        public  string  enemyHeadIcon {get { return m_EnemyHeadIcon; } }
       
        /// <summary>
        /// 敌人配置，格式ID|数量|功力;…
        /// </summary>
        public  string  enemies {get { return m_Enemies; } }
       
        /// <summary>
        /// 描述
        /// </summary>
        public  string  desc {get { return m_Desc; } }
       
        /// <summary>
        /// 奖励
        /// </summary>
        public  string  reward {get { return m_Reward; } }
       
        /// <summary>
        /// 推荐功力
        /// </summary>
        public  int  recommendAtkValue {get { return m_RecommendAtkValue; } }
       
        /// <summary>
        /// 战斗名称
        /// </summary>
        public  string  battleName {get { return m_BattleName; } }
       

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
                    m_Level = dataR.ReadInt();
                    break;
                case 1:
                    m_Chapter = dataR.ReadInt();
                    break;
                case 2:
                    m_EnemyHeadIcon = dataR.ReadString();
                    break;
                case 3:
                    m_Enemies = dataR.ReadString();
                    break;
                case 4:
                    m_Desc = dataR.ReadString();
                    break;
                case 5:
                    m_Reward = dataR.ReadString();
                    break;
                case 6:
                    m_RecommendAtkValue = dataR.ReadInt();
                    break;
                case 7:
                    m_BattleName = dataR.ReadString();
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
          
          ret.Add("Level", 0);
          ret.Add("Chapter", 1);
          ret.Add("EnemyHeadIcon", 2);
          ret.Add("Enemies", 3);
          ret.Add("Desc", 4);
          ret.Add("Reward", 5);
          ret.Add("RecommendAtkValue", 6);
          ret.Add("BattleName", 7);
          return ret;
        }
    } 
}//namespace LR