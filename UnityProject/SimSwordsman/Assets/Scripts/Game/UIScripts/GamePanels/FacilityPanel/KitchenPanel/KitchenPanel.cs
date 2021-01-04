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
        private Text m_KitchenTitleTxt;
        [SerializeField]
        private Text m_KitchenContTxt;
        [SerializeField]
        private Text m_UpgradeRequiredCoinTxt;
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
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_UpgradeBtn;

        [SerializeField]
        private Transform m_KitchenContTra;

        [SerializeField]
        private GameObject m_FoodItemPrefab;

        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private KitchLevelInfo m_CurKitchLevelInfo = null;


        private List<FoodItem> m_FoodItems = new List<FoodItem>();

        protected override void OnUIInit() 
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }
        
        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
        }

        private void RefreshPanelInfo()
        {

            m_KitchenTitleTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KITCHEN_NAME);
            m_KitchenContTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KITCHEN_DESCRIBLE);

            RefreshPanelText();
            UpdateFoodItems();
        }
        private void RefreshPanelText()
        {
            m_CurLevelTxt.text = m_CurLevel.ToString();
            m_CurFoodLimitTxt.text = m_CurKitchLevelInfo.GetCurFoodAddSpeed().ToString();
            m_CurRecoverySpeedTxt.text = m_CurKitchLevelInfo.GetCurFoodAddSpeed().ToString();

            m_NextFoodLimitTxt.text = m_CurKitchLevelInfo.GetNextFoodLimit().ToString();
            m_NextRecoverySpeedTxt.text = m_CurKitchLevelInfo.GetNextFoodAddSpeed().ToString();

            m_UpgradeRequiredCoinTxt.text = m_CurKitchLevelInfo.upgradeCoinCost.ToString();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();
            RefreshPanelInfo();

            EventSystem.S.Register(EventID.OnFoodBuffInterval, OnFoodBuffInterval);
            EventSystem.S.Register(EventID.OnFoodBuffEnd, OnFoodBuffEnd);
            EventSystem.S.Register(EventID.OnFoodBuffStart, OnFoodBuffStart);
        }
        // 开始buff
        private void OnFoodBuffStart(int key, object[] param)
        {
            int id = (int)param[0];
            foreach (var item in m_FoodItems)
            {
                if (item.ID == id)
                {
                    item.StartEffect((string)param[1]);
                    break;
                }
            }
        }
        // 结束buff
        private void OnFoodBuffEnd(int key, object[] param)
        {
            int id = (int)param[0];
            foreach (var item in m_FoodItems)
            {
                if (item.ID == id)
                {
                    item.StopEffect();
                    break;
                }
            }
        }
        // buff倒计时
        private void OnFoodBuffInterval(int key, object[] param)
        {
            int id = (int)param[0];
            foreach (var item in m_FoodItems)
            {
                if (item.ID == id)
                {
                    item.Countdown((string)param[1]);
                    break;
                }
            }
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(double.Parse(m_UpgradeRequiredCoinTxt.text));
                if (isReduceSuccess)
                {
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    RefreshPanelText();
                }
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();

            EventSystem.S.UnRegister(EventID.OnFoodBuffInterval, OnFoodBuffInterval);
            EventSystem.S.UnRegister(EventID.OnFoodBuffEnd, OnFoodBuffEnd);
            EventSystem.S.UnRegister(EventID.OnFoodBuffStart, OnFoodBuffStart);
        }

        public void UpdateFoodItems()
        {
            if (GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Count == m_FoodItems.Count)
            {
                for (int i = 0; i < GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Count; i++)
                {
                    ItemICom itemICom = m_FoodItems[i].GetComponent<ItemICom>();
                    itemICom.OnInit(this, null, GameDataMgr.S.GetPlayerData().unlockFoodItemIDs[i]);
                    m_FoodItems[i].gameObject.SetActive(true);
                }
            }
            else if(GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Count > m_FoodItems.Count)
            {
                for (int i = 0; i < GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Count; i++)
                {
                    if (i >= m_FoodItems.Count)
                    {
                        GameObject obj = Instantiate(m_FoodItemPrefab, m_KitchenContTra);
                        FoodItem item = obj.GetComponent<FoodItem>();
                        m_FoodItems.Add(item);
                    }
                    else
                        m_FoodItems[i].gameObject.SetActive(true);
                    ItemICom itemICom = m_FoodItems[i].GetComponent<ItemICom>();
                    itemICom.OnInit(this, null, GameDataMgr.S.GetPlayerData().unlockFoodItemIDs[i]);
                }
            }
            else
            {
                for (int i = 0; i < m_FoodItems.Count; i++)
                {
                    if (i >= GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Count)
                        m_FoodItems[i].gameObject.SetActive(false);
                    else
                    {
                        ItemICom itemICom = m_FoodItems[i].GetComponent<ItemICom>();
                        itemICom.OnInit(this, null, GameDataMgr.S.GetPlayerData().unlockFoodItemIDs[i]);
                    }
                }
            }
           
        }
    }

}