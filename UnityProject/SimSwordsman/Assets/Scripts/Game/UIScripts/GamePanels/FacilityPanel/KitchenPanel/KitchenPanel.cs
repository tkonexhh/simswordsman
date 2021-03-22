using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
    public class KitchenPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_KitchenContTxt;

        [SerializeField]
        private Image m_FacilityIcon;

        [SerializeField]
        private Text m_CurLevelTxt;
        [SerializeField]
        private Text m_CurFoodLimitTxt;
        [SerializeField]
        private Text m_NextFoodLimitTxt;
        [SerializeField]
        private Text m_CurRecoverySpeedTxt;
        [SerializeField]
        private Text m_NextRecoverySpeedTxt;  
        [SerializeField]
        private Text m_UpgradeTitle;

        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_UpgradeBtn;

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
        private Transform m_KitchenContTra;

        [SerializeField]
        private GameObject m_FoodItemPrefab;
        [SerializeField]
        private GameObject m_RedPoint;

        private FacilityType m_CurFacilityType = FacilityType.None;
        private List<CostItem> m_CostItems;
        private int m_CurLevel;
        private KitchLevelInfo m_CurKitchLevelInfo = null;
        private KitchLevelInfo m_NextKitchLevelInfo = null;


        private List<FoodItem> m_Items = new List<FoodItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        private void GetInformationForNeed()
        {
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_Res1Img.gameObject.SetActive(false);
                m_Res2Img.gameObject.SetActive(false);
                m_Res3Img.gameObject.SetActive(false);
                m_NextFoodLimitTxt.text = Define.COMMON_DEFAULT_STR;
                m_NextRecoverySpeedTxt.text = Define.COMMON_DEFAULT_STR;
                m_NextRecoverySpeedTxt.text = Define.COMMON_DEFAULT_STR;
                m_UpgradeTitle.text = Define.COMMON_DEFAULT_STR;
            }
            else
            {
                m_NextKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
                m_CostItems = m_NextKitchLevelInfo.GetUpgradeResCosts();
            }
        }

        private void RefreshPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_NextKitchLevelInfo, m_CostItems, false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);

            m_KitchenContTxt.text = TDFacilityConfigTable.GetFacilityConfigInfo(m_CurFacilityType).desc;
            m_FacilityIcon.sprite = FindSprite("Kitchen" + m_CurLevel);

            RefreshPanelText();
            UpdateFoodItems();
        }
        private void RefreshPanelText()
        {
            m_CurLevelTxt.text = CommonUIMethod.GetGrade(m_CurLevel);
            m_CurFoodLimitTxt.text = m_CurKitchLevelInfo.GetCurFoodLimit().ToString();
            m_CurRecoverySpeedTxt.text = string.Format("{0}Ãë", m_CurKitchLevelInfo.GetCurFoodAddSpeed());
            if (m_NextKitchLevelInfo!=null)
            {
                m_NextFoodLimitTxt.text = string.Format("+{0}", m_NextKitchLevelInfo.GetCurFoodLimit() - m_CurKitchLevelInfo.GetCurFoodLimit());
                m_NextRecoverySpeedTxt.text = string.Format("{0}", m_NextKitchLevelInfo.GetCurFoodAddSpeed() - m_CurKitchLevelInfo.GetCurFoodAddSpeed());
            }
            RefreshResInfo();

        }
        private void RefreshResInfo()
        {
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_Res1Value, m_Res1Img, m_Res2Value, m_Res2Img, m_Res3Value, m_Res3Img, m_NextKitchLevelInfo, this);
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();
            RefreshPanelInfo();
        }

        protected override void OnClose()
        {
            base.OnClose();

            m_Items.ForEach(x=>x.OnClose());
        }
        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (!CommonUIMethod.CheackIsBuild(m_NextKitchLevelInfo, m_CostItems))
                    return;
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextKitchLevelInfo.upgradeCoinCost);
                if (isReduceSuccess)
                {
                    AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    //½âËøÊ³Îï
                    int unlockfoodid = TDFacilityKitchenTable.GetData(m_CurLevel).unlockRecipe;
                    if (unlockfoodid != -1 && !GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Contains(unlockfoodid))
                        GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Add(unlockfoodid);
                    RefreshPanelInfo();
                    DataAnalysisMgr.S.CustomEvent(DotDefine.facility_upgrade, m_CurFacilityType.ToString() + ";" + m_CurLevel);
                    HideSelfWithAnim();
                }
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        void UpdateFoodItems()
        {
            for (int i = 0; i < TDFoodConfigTable.dataList.Count; i++)
            {
                if (i >= m_Items.Count)
                {
                    GameObject obj = Instantiate(m_FoodItemPrefab, m_KitchenContTra);
                    FoodItem item = obj.GetComponent<FoodItem>();
                    m_Items.Add(item);
                }
                ItemICom itemICom = m_Items[i].GetComponent<ItemICom>();
                itemICom.OnInit(this, null, TDFoodConfigTable.dataList[i].id);
            }
        }
    }

}