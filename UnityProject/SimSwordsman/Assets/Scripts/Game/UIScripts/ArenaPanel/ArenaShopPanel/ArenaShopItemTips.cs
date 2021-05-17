using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ArenaShopItemTips : MonoBehaviour
    {
        [SerializeField] private RewardItemIcon m_RewardItemIcon;
        [SerializeField] private Text m_TxtName;
        [SerializeField] private Text m_TxtTip;
        [SerializeField] private Text m_TxtPrice;
        [SerializeField] private Text m_TxtNum;

        private AbstractPanel m_Panel;

        public void Init(AbstractPanel panel)
        {
            m_Panel = panel;
        }

        public void SetItem(ArenaShopItemInfo itemInfo)
        {
            var reward = itemInfo.reward;
            m_RewardItemIcon.SetReward(reward, m_Panel);
            m_TxtName.text = reward.RewardName();
            var tips = reward.RewardTips();
            int count = tips.Length / 11;
            for (int i = 0; i < count; i++)
            {
                // Debug.LogError(i + "-" + (10 * (i + 1) - 1));
                tips = tips.Insert(11 * (i + 1), "\n");
            }
            m_TxtTip.text = tips;
            m_TxtPrice.text = string.Format("价格:<color=#A2423E>{0}</color>", itemInfo.price);
            m_TxtNum.text = string.Format("数量:<color=#A2423E>{0}</color>", itemInfo.reward.Count);
        }
    }

}