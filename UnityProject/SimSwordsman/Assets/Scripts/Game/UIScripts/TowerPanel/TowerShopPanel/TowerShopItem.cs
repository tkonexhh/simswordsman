using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class TowerShopItem : MonoBehaviour
    {
        [SerializeField] private Image m_ImgRewardIcon;
        [SerializeField] private Text m_TxtRewardName;
        [SerializeField] private Text m_TxtCost;
        [SerializeField] private Button m_BtnBuy;

        [SerializeField] private Text m_TxtID;

        private TowerShopItemInfo m_ItemInfo;
        private int m_Index;

        private void Awake()
        {
            m_BtnBuy.onClick.AddListener(OnClickBuy);
        }

        public void SetItem(int index, TowerShopItemInfo itemInfo)
        {
            m_Index = index;
            m_TxtID.text = itemInfo.id.ToString();
            m_ItemInfo = itemInfo;
            m_ItemInfo.buyed = GameDataMgr.S.GetPlayerData().towerData.GetShopDataByIndex(index).buyed;
            // m_ImgRewardIcon.sprite=m_ItemInfo.reward.SpriteName
            m_TxtRewardName.text = m_ItemInfo.reward.RewardName();
            m_TxtCost.text = m_ItemInfo.price.ToString();
            m_BtnBuy.gameObject.SetActive(!itemInfo.buyed);
        }

        private void OnClickBuy()
        {
            if (GameDataMgr.S.GetPlayerData().towerData.AddCoin(-m_ItemInfo.price))
            {
                MainGameMgr.S.TowerSystem.BuyShopItem(m_Index, m_ItemInfo);
                m_BtnBuy.gameObject.SetActive(false);
            }

        }
    }

}