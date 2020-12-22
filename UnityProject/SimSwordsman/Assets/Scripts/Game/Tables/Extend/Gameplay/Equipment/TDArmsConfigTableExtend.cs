using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDArmsConfigTable
    {
        public static Dictionary<int, Equipment> m_ArmsDic = new Dictionary<int, Equipment>();

        static void CompleteRowAdd(TDArmsConfig tdData)
        {
            if (!m_ArmsDic.ContainsKey(tdData.id))
                m_ArmsDic.Add(tdData.id, new Equipment(tdData));
        }
        public static Equipment GetEquipmentInfo(Arms arms)
        {
            if (m_ArmsDic.ContainsKey((int)arms))
                return m_ArmsDic[(int)arms];
            return null;
        }
        public static UpgradeCondition GetUpgradeConditions(PropType equipType, int equipID, int classID)
        {
            if (m_ArmsDic.ContainsKey(equipID))
                return m_ArmsDic[equipID].GetUpgradeConditionForClassID(classID);
            return null;
        }
        public static float GetBonus(ItemBase itemBase)
        {
            ArmsItem armsItem = itemBase as ArmsItem;
            if (m_ArmsDic.ContainsKey((int)armsItem.ArmsID))
                return m_ArmsDic[(int)armsItem.ArmsID].GetBonusForClassID((int)armsItem.ClassID);
            return 0;
        }
        public static int GetSellingPrice(Arms arms, Step step)
        {
            if (m_ArmsDic.ContainsKey((int)arms))
                return m_ArmsDic[(int)arms].GetSellingPriceForClassID((int)step);
            return 0;
        }
    }
}