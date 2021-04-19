using System;
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
        [SerializeField] private Button m_BtnFullClose;
        [SerializeField] private Button m_BtnADRefesh;
        [SerializeField] private Text m_TxtFcoin;
        [SerializeField] private Text m_TxtRefeshTime;
        [SerializeField] private TowerShopItem[] m_ItemInfos;


        private float m_Timer = 0;
        private float m_RefeshTime = 1;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(OnClickClose);
            m_BtnFullClose.onClick.AddListener(OnClickClose);
            m_BtnADRefesh.onClick.AddListener(OnClickADRefesh);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
            RegisterEvent(EventID.OnRefeshTowerShop, OnRefeshTowerShop);
            RegisterEvent(EventID.OnRefeshTowerCoin, RefeshFCoin);
            OnRefeshTowerShop(0);
            RefeshFCoin(0);

            m_BtnADRefesh.gameObject.SetActive(GameDataMgr.S.GetPlayerData().recordData.towerShopRefesh.dailyCount < 2);
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
                m_ItemInfos[i].SetItem(this, i, TDTowerShopTable.GetShopItemInfoByID(shopInfoLst[i].id));
            }
        }

        private void RefeshFCoin(int key, params object[] args)
        {
            m_TxtFcoin.text = GameDataMgr.S.GetPlayerData().towerData.coin.ToString();
        }

        private void Update()
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_RefeshTime)
            {
                m_Timer = 0;
                RefeshRemainTime();
            }

        }

        private void RefeshRemainTime()
        {
            var nowTime = DateTime.Now;
            var tomorrowTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day).AddDays(1);
            var timeDelta = tomorrowTime - nowTime;
            m_TxtRefeshTime.text = DateFormatHelper.FormatRemainTimeOnlySymbol((long)timeDelta.TotalSeconds);
        }

        private void OnClickClose()
        {
            CloseSelfPanel();
        }
    }

}