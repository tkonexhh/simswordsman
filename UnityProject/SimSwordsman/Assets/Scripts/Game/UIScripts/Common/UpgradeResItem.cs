using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class UpgradeResItem : MonoBehaviour
    {
        [SerializeField]
        private Image m_ResImg;
        [SerializeField]
        private Button m_ResBtn;
        [SerializeField]
        private Text m_ResTxt;
        [SerializeField]
        private GameObject m_GreenHook;

        private CostItem costItem = null;
        private FacilityLevelInfo facilityLevelInfo = null;

        private AbstractAnimPanel animPanel;
        // Start is called before the first frame update
        void Start()
        {
            m_ResBtn.onClick.AddListener(() =>
            {
                UpgradeResItemToUITipsPanel tempStruct = new UpgradeResItemToUITipsPanel();
                tempStruct.pos = m_ResImg.transform.position;
                if (costItem != null)
                {
                    PropConfigInfo propConfigInfo = TDItemConfigTable.GetPropConfigInfo(costItem.itemId);
                    tempStruct.itemID = costItem.itemId;
                    tempStruct.name = propConfigInfo.name;
                    tempStruct.need = GetNeedDesc();
                    tempStruct.desc = propConfigInfo.unlockDesc.desc;

                }
                if (facilityLevelInfo != null)
                {
                    tempStruct.name = "铜钱";
                    tempStruct.need = GetNeedCoinDesc();
                    tempStruct.desc = "弟子干活可以获得";

                }
                tempStruct.parentPanel = animPanel;
                UIMgr.S.OpenDependPanel(animPanel.panelID, UIID.UITipsPanel, null, tempStruct);
            });
        }

        private string GetNeedCoinDesc()
        {
            long coinNum = GameDataMgr.S.GetPlayerData().GetCoinNum();
            return "当前:" + CommonUIMethod.GetTenThousandOrMillion(coinNum) + "需要:" + CommonUIMethod.GetTenThousandOrMillion(facilityLevelInfo.upgradeCoinCost);
        }

        private string GetNeedDesc()
        {
            int number = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)costItem.itemId);
            return "当前:" + CommonUIMethod.GetTenThousandOrMillion(number) + "需要:" + CommonUIMethod.GetTenThousandOrMillion(costItem.value);

        }

        public void OnInit(CostItem costItem, Transform transform)
        {
            this.costItem = costItem;
            animPanel = transform.GetComponentInParent<AbstractAnimPanel>();
        }
        public void OnInit(FacilityLevelInfo facilityLevelInfo, Transform transform)
        {
            this.facilityLevelInfo = facilityLevelInfo;
            animPanel = transform.GetComponentInParent<AbstractAnimPanel>();
        }

        public void ShowResItem(string cont, Sprite sprite)
        {
            if (costItem != null)
            {
                bool isHave = CommonUIMethod.CheckResFontColor(costItem, m_ResTxt);
                if (isHave)
                    m_GreenHook.SetActive(true);
            }
            else if (facilityLevelInfo != null)
            {
                bool isHaveCoin = CommonUIMethod.CheckCoinFontColor(facilityLevelInfo, m_ResTxt);
                if (isHaveCoin)
                    m_GreenHook.SetActive(true);
            }
            //m_ResTxt.color = Color.black;
            m_ResTxt.text = cont;
            m_ResImg.sprite = sprite;

        }
    }


    public struct UpgradeResItemToUITipsPanel
    {
        public Vector3 pos;
        public string name;
        public string need;
        public string desc;
        public int itemID;
        public AbstractPanel parentPanel;
    }

}