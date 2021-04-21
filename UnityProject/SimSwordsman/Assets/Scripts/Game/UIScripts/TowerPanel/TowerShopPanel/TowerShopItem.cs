using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerShopItem : MonoBehaviour
    {
        [SerializeField] private RewardItemIcon m_ImgRewardIcon;
        [SerializeField] private Text m_TxtRewardNum;
        [SerializeField] private Text m_TxtCost;
        [SerializeField] private Image m_ImgQuality;
        [Header("购买")]
        [SerializeField] private Button m_BtnBuy;
        [Header("已购买")]
        [SerializeField] private GameObject m_ObjBuyed;
        [SerializeField] private GameObject m_ObjBuyedDark;

        private TowerShopItemInfo m_ItemInfo;
        private int m_Index;

        private void Awake()
        {
            m_BtnBuy.onClick.AddListener(OnClickBuy);
        }

        public void SetItem(TowerShopPanel panel, int index, TowerShopItemInfo itemInfo)
        {
            m_Index = index;
            m_ItemInfo = itemInfo;
            m_TxtRewardNum.text = "x" + m_ItemInfo.reward.Count;
            m_ItemInfo.buyed = GameDataMgr.S.GetPlayerData().towerData.GetShopDataByIndex(index).buyed;
            // Debug.LogError(m_ItemInfo.reward.SpriteName());
            m_ImgRewardIcon.SetReward(m_ItemInfo.reward, panel);
            m_TxtCost.text = m_ItemInfo.price.ToString();
            RefeshQuality(itemInfo.quality);
            RefeshBuyed(itemInfo.buyed);
        }

        private void RefeshQuality(TowerShopItemQuality quality)
        {
            string icon = "";
            switch (quality)
            {
                case TowerShopItemQuality.Normal:
                    icon = "rewardpanel_equip_ordinary";
                    break;
                case TowerShopItemQuality.Good:
                    icon = "rewardpanel_kungfu_senior";
                    break;
                case TowerShopItemQuality.Perfect:
                    icon = "rewardpanel_kungfu_god";
                    break;
            }
            m_ImgQuality.sprite = SpriteHandler.S.GetSprite(AtlasDefine.RewardPanelAtlas, icon);
        }

        private void OnClickBuy()
        {
            if (GameDataMgr.S.GetPlayerData().towerData.AddCoin(-m_ItemInfo.price))
            {
                MainGameMgr.S.TowerSystem.BuyShopItem(m_Index, m_ItemInfo);
                RefeshBuyed(true);
            }
            else
            {
                FloatMessage.S.ShowMsg("伏魔币不足，再去收集一些吧");
            }

        }

        private void RefeshBuyed(bool buyed)
        {
            m_ObjBuyed.gameObject.SetActive(buyed);
            m_ObjBuyedDark.gameObject.SetActive(buyed);
            m_BtnBuy.gameObject.SetActive(!buyed);
        }
    }

}