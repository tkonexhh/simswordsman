using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ArenaRewardItemIcon : MonoBehaviour
    {
        [SerializeField] private Image m_ImgRewardIcon;
        [SerializeField] private Image m_ImgKungfuName;

        public void SetReward(RewardBase reward, AbstractPanel panel)
        {
            if (reward == null)
            {
                return;
            }

            if (reward.RewardItem == RewardItemType.Kongfu)
            {
                KungfuQuality kungfuQuality = TDKongfuConfigTable.GetKungfuConfigInfo((KungfuType)reward.KeyID).KungfuQuality;

                switch (kungfuQuality)
                {
                    case KungfuQuality.Normal:
                        m_ImgRewardIcon.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Introduction");
                        break;
                    case KungfuQuality.Master:
                        m_ImgRewardIcon.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Advanced");
                        break;
                    case KungfuQuality.Super:
                        m_ImgRewardIcon.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Excellent");
                        break;
                    default:
                        break;
                }
                m_ImgKungfuName.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, reward.SpriteName());
                m_ImgKungfuName.SetNativeSize();
                m_ImgKungfuName.gameObject.SetActive(true);
            }
            else
            {
                m_ImgRewardIcon.sprite = panel.FindSprite(reward.SpriteName());
                m_ImgKungfuName.gameObject.SetActive(false);
            }

            m_ImgRewardIcon.SetNativeSize();

        }
    }

}