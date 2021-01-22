using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{

    public class UpgradeCondition : BuildCondition
    {
        /// <summary>
        /// 升阶ID
        /// </summary>
        public int ClassID { set; get; }


        public UpgradeCondition(int _classID, int _propID, int _number) : base(_propID, _number)
        {
            ClassID = _classID;
        }
    }

    public class BuildCondition
    {
        /// <summary>
        /// 道具ID
        /// </summary>
        public int PropID { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { set; get; }

        public BuildCondition(int _propID, int _number)
        {
            PropID = _propID;
            Number = _number;
        }
    }
    public class Equipment
    {
        /// <summary>
        /// 装备ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 装备名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 装备描述
        /// </summary>
        public string Desc { set; get; }
        /// <summary>
        /// 装备品质
        /// </summary>
        public EquipQuailty Quality { set; get; }
        private Dictionary<int, float> m_BonusDic = new Dictionary<int, float>();
        private Dictionary<int, int> m_SellingPrice = new Dictionary<int, int>();
        private Dictionary<int, UpgradeCondition> m_UpgradeConditionDic = new Dictionary<int, UpgradeCondition>();
        private List<BuildCondition> m_BuildConditionList = new List<BuildCondition>();
        public Equipment(TDEquipmentConfig tdData)
        {
            ID = tdData.id;
            Name = tdData.name;
            Desc = tdData.desc;
            Quality = (EquipQuailty)Enum.Parse(typeof(EquipQuailty), tdData.quality);

            AnalysisBonus(tdData.atkRate);
            AnalysisSellingPrice(tdData.sellingPrice);
            AnalysisUpgradeCondition(tdData.upgradeCondition);
            AnalysisBuildCondition(tdData.buildCondition);
        }



        #region Private 
        /// <summary>
        /// 解析售卖价格
        /// </summary>
        /// <param name="sellingPrice"></param>
        private void AnalysisSellingPrice(string sellingPrice)
        {
            string[] selling = sellingPrice.Split(';');
            for (int i = 1; i <= selling.Length; i++)
                if (!m_SellingPrice.ContainsKey(i))
                    m_SellingPrice.Add(i, int.Parse(selling[i - 1]));
        }
        /// <summary>
        /// 解析升级消耗
        /// </summary>
        /// <param name="upgradeCondition"></param>
        private void AnalysisUpgradeCondition(string _upgradeCondition)
        {
            string[] upgradeCondition = _upgradeCondition.Split(';');
            for (int i = 0; i < upgradeCondition.Length; i++)
            {
                string[] equipmentItem = upgradeCondition[i].Split('|');
                if (!m_UpgradeConditionDic.ContainsKey(i + 2))
                    m_UpgradeConditionDic.Add(i + 2, new UpgradeCondition(i + 2, int.Parse(equipmentItem[0]), int.Parse(equipmentItem[1])));
            }
        }

        /// <summary>
        /// 解析加成数据
        /// </summary>
        /// <param name="_bonus"></param>
        private void AnalysisBonus(string _bonus)
        {
            string[] bonus = _bonus.Split(';');
            for (int i = 1; i <= bonus.Length; i++)
                if (!m_BonusDic.ContainsKey(i))
                    m_BonusDic.Add(i, float.Parse(bonus[i - 1]));
        }

        /// <summary>
        /// 解析建造数据
        /// </summary>
        /// <param name="_bonus"></param>
        private void AnalysisBuildCondition(string _buildCondition)
        {
            if (string.IsNullOrEmpty(_buildCondition))
                return;

            string[] buildCondition = _buildCondition.Split(';');
            for (int i = 0; i < buildCondition.Length; i++)
            {
                string[] equipmentItem = buildCondition[i].Split('|');
                m_BuildConditionList.Add(new BuildCondition(int.Parse(equipmentItem[0]), int.Parse(equipmentItem[1])));
            }
        }
        #endregion

        #region Public

        /// <summary>
        /// 根据classID获取升阶条件
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public UpgradeCondition GetUpgradeConditionForClassID(int classID)
        {
            if (m_UpgradeConditionDic.ContainsKey(classID))
                return m_UpgradeConditionDic[classID];
            return null;
        }
        /// <summary>
        /// 根据classID获取武器的功力加成
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public float GetBonusForClassID(int classID)
        {
            if (m_BonusDic.ContainsKey(classID))
                return m_BonusDic[classID];
            return 0;
        }
        /// <summary>
        /// 根据ClassID获取装备的出售价格
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public int GetSellingPriceForClassID(int classID)
        {
            if (m_SellingPrice.ContainsKey(classID))
                return m_SellingPrice[classID];
            return 0;
        }
        #endregion
    }
    /// <summary>
    /// 装备品质
    /// </summary>
    public enum EquipQuailty
    {
        /// <summary>
        /// 初级
        /// </summary>
        Primary,
        /// <summary>
        /// 中级
        /// </summary>
        Intermediate,
        /// <summary>
        /// 高级
        /// </summary>
        Senior,
    }


}

