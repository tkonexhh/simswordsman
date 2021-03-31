using Qarth;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class RewardPanelItem : MonoBehaviour
    {
        [SerializeField]
        private Image m_Icon;
        [SerializeField]
        private Image m_KungfuName;
        [SerializeField]
        private Text m_Count;
        [SerializeField]
        private Text m_RewardName;
        [SerializeField]
        private GameObject m_CountBg;
        [SerializeField]
        private GameObject m_LootSingle;
        [SerializeField] private SortingGroup m_EffectSortGroup;
        [SerializeField] private SortingGroup m_IconSortGroup;
        //[SerializeField]
        //private GameObject m_NewSkillHeroUnlock;

        private RewardPanel m_Panel;
        private RewardBase m_RewardBaseData = null;


        public void Init(RewardPanel rewardPanel, RewardBase reward)
        {
            m_RewardBaseData = reward;
            m_Panel = rewardPanel;

            if (reward.RewardItem == RewardItemType.Kongfu)
            {
                KungfuQuality kungfuQuality = TDKongfuConfigTable.GetKungfuConfigInfo((KungfuType)reward.KeyID).KungfuQuality;

                switch (kungfuQuality)
                {
                    case KungfuQuality.Normal:
                        m_Icon.sprite = rewardPanel.FindSprite("Introduction");
                        break;
                    case KungfuQuality.Super:
                        m_Icon.sprite = rewardPanel.FindSprite("Advanced");
                        break;
                    case KungfuQuality.Master:
                        m_Icon.sprite = rewardPanel.FindSprite("Excellent");
                        break;
                    default:
                        break;
                }
                m_KungfuName.sprite = rewardPanel.FindSprite(reward.SpriteName());
                m_KungfuName.gameObject.SetActive(true);
            }
            else
                m_Icon.sprite = rewardPanel.FindSprite(reward.SpriteName());

            // gameObject.AddMissingComponent<SortingGroup>().sortingOrder = m_SortOrder + 10 - 1;

            m_RewardName.text = reward.RewardName();
            m_Count.text = CommonUIMethod.GetStrForColor("#D5C17B", "X" + reward.Count);
            m_EffectSortGroup.sortingOrder = m_Panel.GetComponent<Canvas>().sortingOrder - 1;
            m_IconSortGroup.sortingOrder = m_EffectSortGroup.sortingOrder + 1;

            StartCoroutine(SetSort());
        }

        IEnumerator SetSort()
        {
            yield return new WaitForSeconds(0.15f);
            m_EffectSortGroup.sortingOrder = m_Panel.GetComponent<Canvas>().sortingOrder - 1;
            m_IconSortGroup.sortingOrder = m_EffectSortGroup.sortingOrder + 1;
        }

        public void UpdateDoubleRewardCount()
        {
            if (m_RewardBaseData != null)
            {
                int doubleRewadCount = m_RewardBaseData.Count * 2;
                m_Count.text = CommonUIMethod.GetStrForColor("#D5C17B", string.Format("X{0}", doubleRewadCount));
            }
        }
    }
}