using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerADRefeshPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnAD;
        [SerializeField] private Button m_BtnFree;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnAD.onClick.AddListener(OnClickAD);
            m_BtnFree.onClick.AddListener(OnClickFree);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);

            int count = GameDataMgr.S.GetPlayerData().recordData.towerShopRefesh.dailyCount;
            m_BtnFree.gameObject.SetActive(count < 1);
            m_BtnAD.gameObject.SetActive(!m_BtnFree.gameObject.activeSelf);
        }

        private void OnClickFree()
        {
            RefeshShop();
        }

        private void OnClickAD()
        {
            AdsManager.S.PlayRewardAD("TowerShopRefesh", (b) =>
            {
                RefeshShop();
            });
        }

        private void RefeshShop()
        {
            GameDataMgr.S.GetPlayerData().recordData.AddTowerShopRefesh();
            CloseSelfPanel();
            GameDataMgr.S.GetPlayerData().towerData.RandomShopData();
        }

    }

}