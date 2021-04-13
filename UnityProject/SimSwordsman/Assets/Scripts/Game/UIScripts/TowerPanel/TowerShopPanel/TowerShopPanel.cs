using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerShopPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnADRefesh;

        [SerializeField] private TowerShopItem[] m_ItemInfos;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnADRefesh.onClick.AddListener(OnClickADRefesh);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
            RegisterEvent(EventID.OnRefeshTowerShop, OnRefeshTowerShop);

            OnRefeshTowerShop(0);
        }

        private void OnClickADRefesh()
        {
            UIMgr.S.OpenPanel(UIID.TowerADRefeshPanel);
        }

        private void OnRefeshTowerShop(int key, params object[] args)
        {
            var shopInfoLst = GameDataMgr.S.GetPlayerData().towerData.shopInfoLst;
            for (int i = 0; i < m_ItemInfos.Length; i++)
            {
                m_ItemInfos[i].SetItem(i, TDTowerShopTable.GetShopItemInfoByID(shopInfoLst[i].id));
            }
        }
    }

}