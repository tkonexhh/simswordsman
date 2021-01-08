using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class BaicaohuPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_BaicaohuCont;
        [SerializeField]
        private Text m_CurLevelTxt;

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
        private BaicaohuInfo m_CurBaicaohuInfo = null;
        //private List<MedicinalPowderType> m_MedicinalPowderTypes = null;

        private List<BaicaohuItem> m_Items = new List<BaicaohuItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        private void OnTick(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("BaicaohuPanel"))
            {
                foreach (var item in m_Items)
                {
                    if (item.ID == cd.ID)
                    {
                        item.Countdown(cd.Progress, (string)param[1]);
                        break;
                    }
                }
            }
        }

        private void OnEnd(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("BaicaohuPanel"))
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
            if (cd.stringID.Equals("BaicaohuPanel"))
            {
                foreach (var item in m_Items)
                {
                    if (item.ID == cd.ID)
                    {
                        item.StartEffect(cd.Progress, (string)param[1]);
                        break;
                    }
                }
            }
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                //检查等级要求
                var info = TDFacilityBaicaohuTable.GetLevelInfo(m_CurLevel);
                if (MainGameMgr.S.FacilityMgr.GetLobbyCurLevel() < info.upgradeNeedLobbyLevel)
                {
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "主城层级不足！");
                    return;
                }
                //判断材料
                var costsList = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Baicaohu, m_CurLevel).upgradeResCosts.facilityCosts;
                if (!MainGameMgr.S.InventoryMgr.HaveEnoughItem(costsList))
                {
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "材料不足！");
                    return;
                }
                //判断铜钱
                if (GameDataMgr.S.GetPlayerData().GetCoinNum() < info.upgradeCoinCost)
                {
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "铜钱不足！");
                    return;
                }

                MainGameMgr.S.InventoryMgr.ReduceItems(costsList);
                GameDataMgr.S.GetPlayerData().ReduceCoinNum(info.upgradeCoinCost);
                EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                GetInformationForNeed();
                //解锁食物
                //int unlockfoodid = TDFacilityKitchenTable.GetData(m_CurLevel).unlockRecipe;
                //if (unlockfoodid != -1 && !GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Contains(unlockfoodid))
                //    GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Add(unlockfoodid);

                RefreshPanelText();
            });
        }

        private void RefreshPanelInfo()
        {
            m_BaicaohuCont.text = TDFacilityConfigTable.GetFacilityConfigInfo(m_CurFacilityType).desc;

            RefreshPanelText();
            UpdateItems();
        }
        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurBaicaohuInfo = (BaicaohuInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            //m_MedicinalPowderTypes = m_CurBaicaohuInfo.GetNextUnlockMedicinalPowderType();
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
            m_NextUnlockName.text = TDHerbConfigTable.GetData((int)m_CurBaicaohuInfo.GetNextUnlockMedicinalPowderType()[0]).name;
          
            m_UpgradeRequiredCoinTxt.text = m_CurBaicaohuInfo.upgradeCoinCost.ToString();
            m_CurLevelTxt.text = m_CurLevel.ToString();

            //升级所需资源
            var costsList = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel).upgradeResCosts.facilityCosts;
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
            if (m_Items.Count == 0)
            {
                for (int i = 0; i < TDHerbConfigTable.dataList.Count; i++)
                {
                    GameObject obj = Instantiate(m_ItemPrefab, m_ItemTra);
                    BaicaohuItem item = obj.GetComponent<BaicaohuItem>();
                    m_Items.Add(item);
                }
            }
            for (int i = 0; i < m_Items.Count; i++)
            {
                ItemICom itemICom = m_Items[i].GetComponent<ItemICom>();
                itemICom.OnInit(this, null, TDHerbConfigTable.dataList[i].id);
            }
        }
    }

}