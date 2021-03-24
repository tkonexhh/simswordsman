//Auto Generate Don't Edit it
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDVisitorRewardTypeConfig
    {
        
       
        private EInt m_LobbyLevel = 0;   
        private string m_RewardTypeOrder;   
        private string m_BuildResBase;   
        private string m_BuildResAmout;   
        private string m_RecruitOrder;   
        private string m_EquipStrengthenMaterialOrder;   
        private string m_EquipForgeMaterialOrder;  
        
        //private Dictionary<string, TDUniversally.FieldData> m_DataCacheNoGenerate = new Dictionary<string, TDUniversally.FieldData>();
      
        /// <summary>
        /// 讲武堂等级
        /// </summary>
        public  int  lobbyLevel {get { return m_LobbyLevel; } }
       
        /// <summary>
        /// 循环序列，1建材、2招募令、3强化材料、4制作材料
        /// </summary>
        public  string  rewardTypeOrder {get { return m_RewardTypeOrder; } }
       
        /// <summary>
        /// 建材进度比例基数，格式为木材1、石材1、铜钱、木材2、石材2
        /// </summary>
        public  string  buildResBase {get { return m_BuildResBase; } }
       
        /// <summary>
        /// 建材奖励数量
        /// </summary>
        public  string  buildResAmout {get { return m_BuildResAmout; } }
       
        /// <summary>
        /// 招募令序列
        /// </summary>
        public  string  recruitOrder {get { return m_RecruitOrder; } }
       
        /// <summary>
        /// 强化材料序列
        /// </summary>
        public  string  equipStrengthenMaterialOrder {get { return m_EquipStrengthenMaterialOrder; } }
       
        /// <summary>
        /// 制作材料序列
        /// </summary>
        public  string  equipForgeMaterialOrder {get { return m_EquipForgeMaterialOrder; } }
       

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
                    m_LobbyLevel = dataR.ReadInt();
                    break;
                case 1:
                    m_RewardTypeOrder = dataR.ReadString();
                    break;
                case 2:
                    m_BuildResBase = dataR.ReadString();
                    break;
                case 3:
                    m_BuildResAmout = dataR.ReadString();
                    break;
                case 4:
                    m_RecruitOrder = dataR.ReadString();
                    break;
                case 5:
                    m_EquipStrengthenMaterialOrder = dataR.ReadString();
                    break;
                case 6:
                    m_EquipForgeMaterialOrder = dataR.ReadString();
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
          
          ret.Add("LobbyLevel", 0);
          ret.Add("RewardTypeOrder", 1);
          ret.Add("BuildResBase", 2);
          ret.Add("BuildResAmout", 3);
          ret.Add("RecruitOrder", 4);
          ret.Add("EquipStrengthenMaterialOrder", 5);
          ret.Add("EquipForgeMaterialOrder", 6);
          return ret;
        }
    } 
}//namespace LR