using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{

    public class UpgradeCondition : BuildCondition
    {
        /// <summary>
        /// ����ID
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
        /// ����ID
        /// </summary>
        public int PropID { set; get; }
        /// <summary>
        /// ����
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
        /// װ��ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// װ������
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// װ������
        /// </summary>
        public string Desc { set; get; }
        /// <summary>
        /// װ��Ʒ��
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
        /// ���������۸�
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
        /// ������������
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
        /// �����ӳ�����
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
        /// ������������
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
        /// ����classID��ȡ��������
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
        /// ����classID��ȡ�����Ĺ����ӳ�
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
        /// ����ClassID��ȡװ���ĳ��ۼ۸�
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
    /// װ��Ʒ��
    /// </summary>
    public enum EquipQuailty
    {
        /// <summary>
        /// ����
        /// </summary>
        Primary,
        /// <summary>
        /// �м�
        /// </summary>
        Intermediate,
        /// <summary>
        /// �߼�
        /// </summary>
        Senior,
    }


}

