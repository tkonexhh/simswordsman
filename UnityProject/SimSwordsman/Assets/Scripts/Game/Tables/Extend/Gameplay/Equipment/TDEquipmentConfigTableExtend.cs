using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDEquipmentConfigTable
    {
        public static Dictionary<int, Equipment> m_EquipDic = new Dictionary<int, Equipment>();
        public static Dictionary<int, List<CostItem>> MakeNeedItemIDsDic = new Dictionary<int, List<CostItem>>();

        static void CompleteRowAdd(TDEquipmentConfig tdData)
        {
            if (!m_EquipDic.ContainsKey(tdData.id))
                m_EquipDic.Add(tdData.id, new Equipment(tdData));

            if (!MakeNeedItemIDsDic.ContainsKey(tdData.id))
            {
                if (!string.IsNullOrEmpty(tdData.buildCondition))
                {
                    string[] strs = tdData.buildCondition.Split(';');
                    List<CostItem> infos = new List<CostItem>();
                    foreach (var item in strs)
                    {
                        string[] str = item.Split('|');
                        infos.Add(new CostItem(int.Parse(str[0]), int.Parse(str[1])));
                    }

                    MakeNeedItemIDsDic.Add(tdData.id, infos);
                }
            }
        }
        public static Equipment GetEquipmentInfo(int id)
        {
            if (m_EquipDic.ContainsKey(id))
                return m_EquipDic[id];
            return null;
        }
        /// <summary>
        /// 获取武器信息
        /// </summary>
        /// <param name="arms"></param>
        /// <returns></returns>
        public static Equipment GetEquipmentInfo(ArmsType arms)
        {
            int id = (int)arms;
            if (m_EquipDic.ContainsKey(id))
                return m_EquipDic[id];
            return null;
        }
        /// <summary>
        /// 获取铠甲信息
        /// </summary>
        /// <param name="armor"></param>
        /// <returns></returns>
        public static Equipment GetEquipmentInfo(ArmorType armor)
        {
            int id = (int)armor;
            if (m_EquipDic.ContainsKey(id))
                return m_EquipDic[id];
            return null;
        }
        /// <summary>
        /// 获取装备图标名称
        /// </summary>
        /// <param name="equipType">暂留</param>
        /// <param name="equipID"></param>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static string GetIconName(int equipID)
        {
            if (m_EquipDic.ContainsKey(equipID))
                return m_EquipDic[equipID].IconName;
            return null;
        }
        //public static float GetBonus(ItemBase itemBase)
        //{
        //    ArmsItem armsItem = itemBase as ArmsItem;
        //    if (m_EquipDic.ContainsKey((int)armsItem.ArmsID))
        //        return m_EquipDic[(int)armsItem.ArmsID].GetBonusForClassID((int)armsItem.ClassID);
        //    return 0;
        //}
        /// <summary>
        /// 获取装备升级消耗
        /// </summary>
        public static UpgradeCondition GetEquipUpGradeConsume(int equipID,int step)
        {
            if (m_EquipDic.ContainsKey(equipID))
                return m_EquipDic[equipID].GetUpgradeConditionForClassID(step);
            return null;
        }

        public static int GetSellingPrice(ArmsType arms, Step step)
        {
            int id = (int)arms;
            if (m_EquipDic.ContainsKey(id))
                return m_EquipDic[id].GetSellingPriceForClassID((int)step);
            return 0;
        }

        public static int GetSellingPrice(ArmorType armor, Step step)
        {
            int id = (int)armor;
            if (m_EquipDic.ContainsKey(id))
                return m_EquipDic[id].GetSellingPriceForClassID((int)step);
            return 0;
        }
    }
}