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
        private Text m_UpgradeRequiredCoinTxt;
        [SerializeField]
        private Image m_UpgradeRequiredImg1;
        [SerializeField]
        private Image m_UpgradeRequiredImg2;
        [SerializeField]
        private Text m_UpgradeRequiredTxt1;
        [SerializeField]
        private Text m_UpgradeRequiredTxt2;

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
            m_KitchenContTxt.text = TDFacilityConfigTable.GetFacilityConfigInfo(FacilityType.Kitchen).desc;
            //CommonUIMethod.GetStringForTableKey(Define.FACILITY_KITCHEN_DESCRIBLE);

            RefreshPanelText();
            UpdateFoodItems();
        }
        private void RefreshPanelText()
        {
            m_CurLevelTxt.text = m_CurLevel.ToString();
            m_CurFoodLimitTxt.text = m_CurKitchLevelInfo.GetCurFoodLimit().ToString();
            m_CurRecoverySpeedTxt.text = string.Format("{0}/分钟", m_CurKitchLevelInfo.GetCurFoodAddSpeed());

            m_NextFoodLimitTxt.text = string.Format("+{0}", m_CurKitchLevelInfo.GetNextFoodLimit() - m_CurKitchLevelInfo.GetCurFoodLimit());
            m_NextRecoverySpeedTxt.text = string.Format("+{0}", m_CurKitchLevelInfo.GetNextFoodAddSpeed() - m_CurKitchLevelInfo.GetCurFoodAddSpeed());

            m_UpgradeRequiredCoinTxt.text = m_CurKitchLevelInfo.upgradeCoinCost.ToString();

            //升级所需资源
            var costsList = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Kitchen, m_CurLevel).upgradeResCosts.facilityCosts;
            if (costsList.Count > 1)
            {
                m_UpgradeRequiredImg2.gameObject.SetActive(true);
                m_UpgradeRequiredTxt2.gameObject.SetActive(true);
                m_UpgradeRequiredImg2.sprite = Resources.Load<Sprite>("Sprites/ItemIcon/" + TDItemConfigTable.GetData(costsList[1].itemId).iconName);
                m_UpgradeRequiredTxt2.text = costsList[1].value.ToString();
            }
            else
            {
                m_UpgradeRequiredImg2.gameObject.SetActive(false);
                m_UpgradeRequiredTxt2.gameObject.SetActive(false);
            }
            m_UpgradeRequiredImg1.sprite = Resources.Load<Sprite>("Sprites/ItemIcon/" + TDItemConfigTable.GetData(costsList[0].itemId).iconName);
            m_UpgradeRequiredTxt1.text = costsList[0].value.ToString();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();
            RefreshPanelInfo();

            EventSystem.S.Register(EventID.OnFoodBuffTick, OnFoodBuffInterval);
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
                //检查等级要求
                if (MainGameMgr.S.FacilityMgr.GetLobbyCurLevel() < TDFacilityKitchenTable.GetLevelInfo(m_CurLevel).upgradeNeedLobbyLevel)
                {
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "主城层级不足！");
                    return;
                }
                //判断材料
                var costsList = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Kitchen, m_CurLevel).upgradeResCosts.facilityCosts;
                if (!MainGameMgr.S.InventoryMgr.HaveEnoughItem(costsList))
                {
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "材料不足！");
                    return;
                }
                //判断铜钱
                long coins = m_CurKitchLevelInfo.upgradeCoinCost;
                if (GameDataMgr.S.GetPlayerData().GetCoinNum() < coins)
                {
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "铜钱不足！");
                    return;
                }

                MainGameMgr.S.InventoryMgr.ReduceItems(costsList);
                GameDataMgr.S.GetPlayerData().ReduceCoinNum(coins);
                EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                GetInformationForNeed();
                //解锁食物
                int unlockfoodid = TDFacilityKitchenTable.GetData(m_CurLevel).unlockRecipe;
                if (unlockfoodid != -1 && !GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Contains(unlockfoodid))
                    GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Add(unlockfoodid);

                RefreshPanelText();
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();

            EventSystem.S.UnRegister(EventID.OnFoodBuffTick, OnFoodBuffInterval);
            EventSystem.S.UnRegister(EventID.OnFoodBuffEnd, OnFoodBuffEnd);
            EventSystem.S.UnRegister(EventID.OnFoodBuffStart, OnFoodBuffStart);
        }

        public void UpdateFoodItems()
        {
            if (m_FoodItems.Count == 0)
            {
                for (int i = 0; i < TDFoodConfigTable.dataList.Count; i++)
                {
                    GameObject obj = Instantiate(m_FoodItemPrefab, m_KitchenContTra);
                    FoodItem item = obj.GetComponent<FoodItem>();
                    m_FoodItems.Add(item);
                }
            }
            for (int i = 0; i < m_FoodItems.Count; i++)
            {
                ItemICom itemICom = m_FoodItems[i].GetComponent<ItemICom>();
                itemICom.OnInit(this, null, TDFoodConfigTable.dataList[i].id);
            }
        }
    }

}