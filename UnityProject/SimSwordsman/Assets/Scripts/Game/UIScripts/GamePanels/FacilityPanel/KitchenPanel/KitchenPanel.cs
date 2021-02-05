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
            if (m_NextKitchLevelInfo!=null)
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
            m_CurRecoverySpeedTxt.text = string.Format("{0}秒", m_CurKitchLevelInfo.GetCurFoodAddSpeed());

            m_NextFoodLimitTxt.text = string.Format("+{0}", m_CurKitchLevelInfo.GetNextFoodLimit() - m_CurKitchLevelInfo.GetCurFoodLimit());
            m_NextRecoverySpeedTxt.text = string.Format("+{0}", m_CurKitchLevelInfo.GetNextFoodAddSpeed() - m_CurKitchLevelInfo.GetCurFoodAddSpeed());

            RefreshResInfo();

        }
        private void RefreshResInfo()
        {
            if (m_CostItems == null)
                return;

            if (m_CostItems.Count == 1)
            {
                int havaItem = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(m_CostItems[0].itemId);
                m_Res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItem, m_CostItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(m_CostItems[0].value);
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = GetCurCoin() + Define.SLASH + CommonUIMethod.GetTenThousand(m_NextKitchLevelInfo.upgradeCoinCost);
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
                m_Res3Value.text = GetCurCoin() + Define.SLASH + CommonUIMethod.GetTenThousand(m_NextKitchLevelInfo.upgradeCoinCost);
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
        }

        private int GetCurItem(int hava,int number)
        {
            if (hava >= number)
                return number;
            return hava;
        }

        private string GetCurCoin()
        {
            long coin = GameDataMgr.S.GetPlayerData().GetCoinNum();
            if (coin >= m_NextKitchLevelInfo.upgradeCoinCost)
                return CommonUIMethod.GetTenThousand(m_NextKitchLevelInfo.upgradeCoinCost);
            return CommonUIMethod.GetTenThousand((int)coin);
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
            bool isHaveCoin = GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_NextKitchLevelInfo.upgradeCoinCost);
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
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (m_NextKitchLevelInfo.GetUpgradeCondition() > lobbyLevel)
            {
                FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_NEEDLOBBY));
                return false;
            }

            if (CheckPropIsEnough())
                return true;
            return false;
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
                if (!CheackIsBuild())
                    return;
                if (m_NextKitchLevelInfo == null)
                    return;
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextKitchLevelInfo.upgradeCoinCost);
                if (isReduceSuccess)
                {
                    AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    //解锁食物
                    int unlockfoodid = TDFacilityKitchenTable.GetData(m_CurLevel).unlockRecipe;
                    if (unlockfoodid != -1 && !GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Contains(unlockfoodid))
                        GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Add(unlockfoodid);
                    RefreshPanelInfo();
                }
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