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

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnAD.onClick.AddListener(OnClickAD);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
        }

        private void OnClickAD()
        {
            AdsManager.S.PlayRewardAD("TowerShopRefesh", (b) =>
            {
                CloseSelfPanel();
                GameDataMgr.S.GetPlayerData().towerData.RandomShopData();
            });
        }

    }

}