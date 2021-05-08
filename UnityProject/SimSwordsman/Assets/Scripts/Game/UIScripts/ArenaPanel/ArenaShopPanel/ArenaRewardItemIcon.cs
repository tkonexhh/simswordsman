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

                Sprite sprite = null;
                switch (kungfuQuality)
                {
                    case KungfuQuality.Normal:
                        sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Introduction");
                        break;
                    case KungfuQuality.Master:
                        sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Advanced");
                        break;
                    case KungfuQuality.Super:
                        sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Excellent");
                        break;
                    default:
                        break;
                }

                if (sprite != null)
                {
                    m_ImgRewardIcon.sprite = sprite;
                }
                else
                {
                    Log.e("Sprite not found: " + kungfuQuality.ToString());
                }

                m_ImgKungfuName.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, reward.SpriteName());
                m_ImgKungfuName.SetNativeSize();
                m_ImgKungfuName.gameObject.SetActive(true);
            }
            else
            {
                Sprite sprite = panel.FindSprite(reward.SpriteName());
                if (sprite != null)
                {
                    m_ImgRewardIcon.sprite = sprite;
                }
                else
                {
                    Log.e("Sprite not found: " + reward.SpriteName());
                }
                m_ImgKungfuName.gameObject.SetActive(false);
            }

            m_ImgRewardIcon.SetNativeSize();

        }
    }

}