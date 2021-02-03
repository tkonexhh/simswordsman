using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ForgeHousePanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_ForgeHouseCont;
        [SerializeField]
        private Text m_CurLevelTxt;
        [SerializeField]
        private Image m_FacilityIcon;

        [SerializeField]
        private Image m_Res1Img;
        [SerializeField]
        private Text m_Res1Value;
        [SerializeField]
        private Image m_Res2Img;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3Img;
        [SerializeField]
        private Text m_Res3Value;

        [SerializeField]
        private Text m_NextUnlockName;

        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Transform m_ItemTra;

        [SerializeField]
        private GameObject m_ItemPrefab;

        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private ForgeHouseInfo m_CurForgeHouseInfo = null;
        private List<CostItem> m_CostItems;
        private FacilityLevelInfo m_NextFacilityLevelInfo = null;
        private List<ForgeHouseItem> m_Items = new List<ForgeHouseItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        private void OnTick(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("ForgeHousePanel"))
            {
                foreach (var item in m_Items)
                {
                    if (item.ID == cd.ID)
                    {
                        item.Countdown(cd.GetProgress(), (string)param[1]);
                        break;
                    }
                }
            }
        }

        private void OnEnd(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("ForgeHousePanel"))
            {
                foreach (var item in m_Items)
                {
                    if (item.ID == cd.ID)
                    {
                        item.StopEffect();
                        break;
                    }
                }
            }
        }

        private void OnStart(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("ForgeHousePanel"))
            {
                foreach (var item in m_Items)
                {
                    if (item.ID == cd.ID)
                    {
                        item.StartEffect(cd.GetProgress(), (string)param[1]);
                        break;
                    }
                }
            }
        }
        private bool CheckPropIsEnough()
        {
            for (int i = 0; i < m_CostItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)m_CostItems[i].itemId, m_CostItems[i].value);
                if (!isHave)
                {
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
                    return false;
                }
            }
            bool isHaveCoin = GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_NextFacilityLevelInfo.upgradeCoinCost);
            if (isHaveCoin)
                return true;
            else
            {
                FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_COIN));
                return false;
            }
        }
        private bool CheackIsBuild()
        {
            if (CheckPropIsEnough())
                return true;
            return false;
        }
        private void BindAddListenerEvent()
        {
            //ÒôÐ§
            foreach (var item in transform.GetComponentsInChildren<Button>(true))
            {
                item.onClick.AddListener(() => AudioMgr.S.PlaySound(Define.SOUND_UI_BTN));
            }
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                if (!CheackIsBuild())
                    return;
                if (m_NextFacilityLevelInfo == null)
                    return;
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
                if (isReduceSuccess)
                {
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    RefreshPanelInfo();
                }
            });
        }

        private void RefreshPanelInfo()
        {
            m_ForgeHouseCont.text = TDFacilityConfigTable.GetFacilityConfigInfo(m_CurFacilityType).desc;
            m_FacilityIcon.sprite = FindSprite("ForgeHouse" + m_CurLevel);

            RefreshPanelText();
            UpdateItems();
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurForgeHouseInfo = (ForgeHouseInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_NextFacilityLevelInfo = (ForgeHouseInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
            if (m_NextFacilityLevelInfo != null)
                m_CostItems = m_NextFacilityLevelInfo.GetUpgradeResCosts();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            RegisterEvent(EventID.OnCountdownerEnd, OnEnd);
            RegisterEvent(EventID.OnCountdownerStart, OnStart);
            RegisterEvent(EventID.OnCountdownerTick, OnTick);

            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();
            RefreshPanelInfo();
        }

        private void RefreshPanelText()
        {
            if (m_CostItems == null)
                return;

            if (m_CostItems.Count == 1)
            {
                int havaItem = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(m_CostItems[0].itemId);
                m_Res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItem, m_CostItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(m_CostItems[0].value);
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = GetCurCoin() + Define.SLASH + CommonUIMethod.GetTenThousand(m_NextFacilityLevelInfo.upgradeCoinCost);
                m_Res2Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {
                int havaItemFirst = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(m_CostItems[0].itemId);
                int havaItemSec = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(m_CostItems[1].itemId);
                m_Res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemFirst, m_CostItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(m_CostItems[0].value);
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemSec, m_CostItems[1].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(m_CostItems[1].value);
                m_Res2Img.sprite = FindSprite(GetIconName(m_CostItems[1].itemId));
                m_Res3Value.text = GetCurCoin() + Define.SLASH + CommonUIMethod.GetTenThousand(m_NextFacilityLevelInfo.upgradeCoinCost);
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
        }
        private int GetCurItem(int hava, int number)
        {
            if (hava >= number)
                return number;
            return hava;
        }
        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }
        private string GetCurCoin()
        {
            long coin = GameDataMgr.S.GetPlayerData().GetCoinNum();
            if (coin >= m_NextFacilityLevelInfo.upgradeCoinCost)
                return CommonUIMethod.GetTenThousand(m_NextFacilityLevelInfo.upgradeCoinCost);
            return CommonUIMethod.GetTenThousand((int)coin);
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnCountdownerEnd, OnEnd);
            EventSystem.S.UnRegister(EventID.OnCountdownerStart, OnStart);
            EventSystem.S.UnRegister(EventID.OnCountdownerTick, OnTick);

            CloseSelfPanel();
        }

        private void UpdateItems()
        {
            string[] temp = null;
            if (m_Items.Count == 0)
            {
                for (int i = 0; i < TDFacilityForgeHouseTable.dataList.Count; i++)
                {
                    temp = TDFacilityForgeHouseTable.dataList[i].unlockEquip.Split(';');
                    foreach (var id in temp)
                    {
                        GameObject obj = Instantiate(m_ItemPrefab, m_ItemTra);
                        ForgeHouseItem item = obj.GetComponent<ForgeHouseItem>();
                        item.ID = int.Parse(id);
                        item.UnlockLevel = TDFacilityForgeHouseTable.dataList[i].level;
                        m_Items.Add(item);
                    }
                }
            }
            for (int i = 0; i < m_Items.Count; i++)
            {
                ItemICom itemICom = m_Items[i].GetComponent<ItemICom>();
                itemICom.OnInit(this, null);
            }
        }
    }
	
}