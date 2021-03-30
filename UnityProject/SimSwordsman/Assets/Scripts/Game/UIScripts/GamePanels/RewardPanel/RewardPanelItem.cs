using Qarth;
using UnityEngine;
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
        //[SerializeField]
        //private GameObject m_NewSkillHeroUnlock;

        private ResLoader m_ResLoader;
        private GameObject obj;

        private RewardBase m_RewardBaseData = null;


        private void OnDisable()
        {
        }

        public void Init(RewardPanel rewardPanel, RewardBase reward)
        {
            m_RewardBaseData = reward;

            m_ResLoader = ResLoader.Allocate();

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

            m_RewardName.text = reward.RewardName();
            m_Count.text = CommonUIMethod.GetStrForColor("#D5C17B", "X" + reward.Count); 
            //m_CountBg.SetActive(reward.Count > 1);
            Instantiate(m_LootSingle, transform).transform.localPosition = Vector3.zero;
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