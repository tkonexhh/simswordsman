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
        private ResLoader m_ResLoader;
        private GameObject obj;
        private void OnDisable()
        {
            if (obj!=null)
            {
                DestroyImmediate(obj);
                m_ResLoader.ReleaseRes("NewSkillHeroUnlock");
            }
        }

        public void Init(RewardPanel rewardPanel, RewardBase reward)
        {
            m_ResLoader = ResLoader.Allocate();
            obj = Instantiate(m_ResLoader.LoadSync("NewSkillHeroUnlock"), transform) as GameObject;
            //obj.transform.position = transform.position;

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
            m_Count.text = "+" + reward.Count;
        }
    
    }
}