using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDArmorConfigTable
    {
        public static Dictionary<int, Equipment> m_ArmorDic = new Dictionary<int, Equipment>();

        static void CompleteRowAdd(TDArmorConfig tdData)
        {
            if (!m_ArmorDic.ContainsKey(tdData.id))
                m_ArmorDic.Add(tdData.id, new Equipment(tdData));
        }
        /// <summary>
        /// 获取铠甲信息
        /// </summary>
        /// <param name="armor"></param>
        /// <returns></returns>
        public static Equipment GetEquipmentInfo(Armor armor)
        {
            if (m_ArmorDic.ContainsKey((int)armor))
                return m_ArmorDic[(int)armor];
            return null;
        }
        public static UpgradeCondition GetUpgradeConditions(PropType equipType, int equipID, int classID)
        {
            if (m_ArmorDic.ContainsKey(equipID))
                return m_ArmorDic[equipID].GetUpgradeConditionForClassID(classID);
            return null;
        }
        public static float GetBonus(ItemBase itemBase)
        {
            ArmorItem armorItem = itemBase as ArmorItem;
            if (m_ArmorDic.ContainsKey((int)armorItem.ArmorID))
                return m_ArmorDic[(int)armorItem.ArmorID].GetBonusForClassID((int)armorItem.ClassID);
            return 0;
        }
        public static int GetSellingPrice(Armor armor, Step step)
        {
            if (m_ArmorDic.ContainsKey((int)armor))
                return m_ArmorDic[(int)armor].GetSellingPriceForClassID((int)step);
            return 0;
        }
    }
}