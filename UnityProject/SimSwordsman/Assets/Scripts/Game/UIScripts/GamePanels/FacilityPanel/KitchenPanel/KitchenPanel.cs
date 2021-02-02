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
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_NextKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel+1);
            m_CostItems = m_NextKitchLevelInfo.GetUpgradeResCosts();
        }

        private void RefreshPanelInfo()
        {
            m_KitchenContTxt.text = TDFacilityConfigTable.GetFacilityConfigInfo(m_CurFacilityType).desc;
            m_FacilityIcon.sprite = FindSprite("Kitchen" + m_CurLevel);

            RefreshPanelText();
            UpdateFoodItems();
        }
        private void RefreshPanelText()
        {
            m_CurLevelTxt.text = CommonUIMethod.GetGrade(m_CurLevel);
            m_CurFoodLimitTxt.text = m_CurKitchLevelInfo.GetCurFoodLimit().ToString();
            m_CurRecoverySpeedTxt.text = string.Format("{0}/分钟", m_CurKitchLevelInfo.GetCurFoodAddSpeed());

            m_NextFoodLimitTxt.text = string.Format("+{0}", m_CurKitchLevelInfo.GetNextFoodLimit() - m_CurKitchLevelInfo.GetCurFoodLimit());
            m_NextRecoverySpeedTxt.text = string.Format("+{0}", m_CurKitchLevelInfo.GetNextFoodAddSpeed() - m_CurKitchLevelInfo.GetCurFoodAddSpeed());

            RefreshResInfo();

        }
        private void RefreshResInfo()
        {
            if (m_CostItems.Count == 1)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_NextKitchLevelInfo.upgradeCoinCost.ToString();
                m_Res2Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_CostItems[1].value.ToString();
                m_Res2Img.sprite = FindSprite(GetIconName(m_CostItems[1].itemId));
                m_Res3Value.text = m_NextKitchLevelInfo.upgradeCoinCost.ToString();
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
        }

        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();
            RefreshPanelInfo();

            EventSystem.S.Register(EventID.OnFoodBuffTick, OnFoodBuffTick);
            EventSystem.S.Register(EventID.OnFoodBuffEnd, OnFoodBuffEnd);
            EventSystem.S.Register(EventID.OnFoodBuffStart, OnFoodBuffStart);
        }
        // 开始buff
        private void OnFoodBuffStart(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            foreach (var item in m_Items)
            {
                if (item.ID == cd.ID)
                {
                    item.StartEffect(cd.GetProgress(), (string)param[1]);
                    break;
                }
            }
        }
        // 结束buff
        private void OnFoodBuffEnd(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            foreach (var item in m_Items)
            {
                if (item.ID == cd.ID)
                {
                    item.StopEffect();
                    break;
                }
            }
        }
        // buff倒计时
        private void OnFoodBuffTick(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            foreach (var item in m_Items)
            {
                if (item.ID == cd.ID)
                {
                    item.Countdown(cd.GetProgress(), (string)param[1]);
                    break;
                }
            }
        }

        private void BindAddListenerEvent()
        {
            //音效
            foreach (var item in transform.GetComponentsInChildren<Button>(true))
            {
                item.onClick.AddListener(() => AudioMgr.S.PlaySound(Define.SOUND_UI_BTN));
            }
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

                RefreshPanelInfo();
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();

            EventSystem.S.UnRegister(EventID.OnFoodBuffTick, OnFoodBuffTick);
            EventSystem.S.UnRegister(EventID.OnFoodBuffEnd, OnFoodBuffEnd);
            EventSystem.S.UnRegister(EventID.OnFoodBuffStart, OnFoodBuffStart);
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