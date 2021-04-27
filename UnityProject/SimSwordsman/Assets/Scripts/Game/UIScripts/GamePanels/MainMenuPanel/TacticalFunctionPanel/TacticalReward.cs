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
        #region Public
        public void RefreshRewardInfo(TaskReward taskReward, bool random = true)
        {
            GetSprite(taskReward, m_RewardImg, m_KungFuName);
            if (taskReward.count1 == -1)
            {
                m_RewardText.text = CommonUIMethod.GetTenThousandOrMillion(taskReward.count1);
            }
            else
            {
                if (random)
                    m_RewardText.text = CommonUIMethod.GetTenThousandOrMillion(Random.Range(taskReward.count1, taskReward.count2 + 1));
                else
                    m_RewardText.text = CommonUIMethod.GetTenThousandOrMillion(taskReward.count1);
            }
        }
        public void RefreshRewardInfo(TaskRewardType rewardType, RewardBase rewardBase, bool random = true)
        {
            switch (rewardType)
            {
                case TaskRewardType.Food:
                    RefreshRewardInfo(new TaskReward(rewardType, -2, rewardBase.Count), random);
                    break;
                case TaskRewardType.Coin:
                    RefreshRewardInfo(new TaskReward(rewardType, -1, rewardBase.Count), random);
                    break;
                case TaskRewardType.Item:
                case TaskRewardType.Medicine:
                case TaskRewardType.Kongfu:
                case TaskRewardType.Armor:
                case TaskRewardType.Arms:
                    RefreshRewardInfo(new TaskReward(rewardType, (int)rewardBase.KeyID, rewardBase.Count), random);
                    break;
            }
        }
        public void RefreshRewardInfo(RewardItemType rewardItemType, int value)
        {
            switch (rewardItemType)
            {
                case RewardItemType.Exp_Role:
                    m_RewardImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "PanelCommon_RoleExp");
                    break;
                case RewardItemType.Exp_Kongfu:
                    m_RewardImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "PanelCommon_KungfuExp");
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
                    m_RewardImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, TDItemConfigTable.GetIconName((int)taskReward.id));
                    //image.sprite = m_TacticalFunctionPanel.FindSprite(TDItemConfigTable.GetIconName((int)taskReward.id));
                    break;
                case TaskRewardType.Armor:
                case TaskRewardType.Arms:
                    image.sprite = SpriteHandler.S.GetSprite(AtlasDefine.EquipmentAtlas, TDEquipmentConfigTable.GetIconName((int)taskReward.id));
                    //image.sprite = m_TacticalFunctionPanel.FindSprite(TDEquipmentConfigTable.GetIconName((int)taskReward.id));
                    break;
                case TaskRewardType.Kongfu:
                    SetKungfuSprite(taskReward, image, res1KungfuName);
                    break;
                case TaskRewardType.Medicine:
                    image.sprite = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, TDHerbConfigTable.GetHerbIconNameById((int)taskReward.id));
                    //image.sprite = m_TacticalFunctionPanel.FindSprite(TDHerbConfigTable.GetHerbIconNameById((int)taskReward.id));
                    break;
                case TaskRewardType.Food:
                    image.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "Baozi");
                    break;
                case TaskRewardType.Coin:
                    image.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "Coin");
                    break;
            }
        }
        private void SetKungfuSprite(TaskReward item, Image image, Image kungfuName)
        {
            kungfuName.gameObject.SetActive(true);
            switch (GetKungfuQuality((KungfuType)item.id))
            {
                case KungfuQuality.Normal:
                    m_RewardImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Introduction");
                    break;
                case KungfuQuality.Master:
                    m_RewardImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Advanced");
                    break;
                case KungfuQuality.Super:
                    m_RewardImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Excellent");
                    break;
                default:
                    break;
            }
            kungfuName.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, TDKongfuConfigTable.GetIconName((KungfuType)item.id));
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        #endregion

    }
}