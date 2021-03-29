using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class TacticalReward : MonoBehaviour
	{
		[SerializeField]
		private Image m_RewardImg;
		[SerializeField]
		private Text m_RewardText;	
		[SerializeField]
		private Image m_KungFuName;

        private TacticalFunctionPanel m_TacticalFunctionPanel;
        #region Public
        public void RefreshRewardInfo(TacticalFunctionPanel tacticalFunctionPanel, TaskReward taskReward)
		{
            m_TacticalFunctionPanel = tacticalFunctionPanel;

            GetSprite(taskReward, m_RewardImg, m_KungFuName);
            if (taskReward.count1==-1)
            {
                m_RewardText.text = CommonUIMethod.GetTenThousandOrMillion(taskReward.count1);
            }
            else
            {
                m_RewardText.text = CommonUIMethod.GetTenThousandOrMillion(Random.Range(taskReward.count1, taskReward.count2 + 1));
            }

        }
        public void RefreshRewardInfo(TacticalFunctionPanel tacticalFunctionPanel, RewardItemType rewardItemType,int value)
        {
            m_TacticalFunctionPanel = tacticalFunctionPanel;
            switch (rewardItemType)
            {
                case RewardItemType.Exp_Role:
                    m_RewardImg.sprite = m_TacticalFunctionPanel.FindSprite("PanelCommon_RoleExp");
                    break;
                case RewardItemType.Exp_Kongfu:
                    m_RewardImg.sprite = m_TacticalFunctionPanel.FindSprite("PanelCommon_KungfuExp");
                    break;
                default:
                    break;
            }
            m_RewardText.text = CommonUIMethod.GetTenThousandOrMillion(value);
        }
        #endregion
        #region Private
        private void GetSprite(TaskReward taskReward, Image image, Image res1KungfuName)
        {
            switch (taskReward.rewardType)
            {
                case TaskRewardType.Item:
                    image.sprite = m_TacticalFunctionPanel.FindSprite(TDItemConfigTable.GetIconName((int)taskReward.id));
                    break;
                case TaskRewardType.Armor:
                case TaskRewardType.Arms:
                    image.sprite = m_TacticalFunctionPanel.FindSprite(TDEquipmentConfigTable.GetIconName((int)taskReward.id));
                    break;
                case TaskRewardType.Kongfu:
                    SetKungfuSprite(taskReward, image, res1KungfuName);
                    break;
                case TaskRewardType.Medicine:
                    image.sprite = m_TacticalFunctionPanel.FindSprite(TDHerbConfigTable.GetHerbIconNameById((int)taskReward.id));
                    break;
                case TaskRewardType.Food:
                    image.sprite = m_TacticalFunctionPanel.FindSprite("Baozi");
                    break;
                case TaskRewardType.Coin:
                    image.sprite = m_TacticalFunctionPanel.FindSprite("Coin");
                    break;
            }
        }
        private void SetKungfuSprite(TaskReward item, Image image, Image kungfuName)
        {
            kungfuName.gameObject.SetActive(true);
            switch (GetKungfuQuality((KungfuType)item.id))
            {
                case KungfuQuality.Normal:
                    image.sprite = m_TacticalFunctionPanel.FindSprite("Introduction");
                    break;
                case KungfuQuality.Super:
                    image.sprite = m_TacticalFunctionPanel.FindSprite("Advanced");
                    break;
                case KungfuQuality.Master:
                    image.sprite = m_TacticalFunctionPanel.FindSprite("Excellent");
                    break;
                default:
                    break;
            }
            kungfuName.sprite = m_TacticalFunctionPanel.FindSprite(TDKongfuConfigTable.GetIconName((KungfuType)item.id));
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        #endregion

    }
}