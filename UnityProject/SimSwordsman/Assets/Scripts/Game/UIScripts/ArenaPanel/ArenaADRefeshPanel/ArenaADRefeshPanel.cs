using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ArenaADRefeshPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnAD;
        [SerializeField] private Button m_BtnFree;
        [SerializeField] private Text m_TxtTip;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnAD.onClick.AddListener(OnClickAD);
            m_BtnFree.onClick.AddListener(OnClickFree);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);

            int count = GameDataMgr.S.GetPlayerData().recordData.arenaShopRefesh.dailyCount;
            m_TxtTip.text = count < 1 ? "点击免费可以免费刷新一次商店" : "点击观看广告可以免费刷新一次商店";
            m_BtnFree.gameObject.SetActive(count < 1);
            m_BtnAD.gameObject.SetActive(!m_BtnFree.gameObject.activeSelf);
        }

        private void OnClickFree()
        {
            RefeshShop();
        }

        private void OnClickAD()
        {
            AdsManager.S.PlayRewardAD("ArenaShopRefesh", (b) =>
            {
                RefeshShop();
            });
        }

        private void RefeshShop()
        {
            GameDataMgr.S.GetPlayerData().recordData.AddArenaShopRefesh();
            CloseSelfPanel();
            GameDataMgr.S.GetPlayerData().arenaData.RandomShopData();
        }

    }

}