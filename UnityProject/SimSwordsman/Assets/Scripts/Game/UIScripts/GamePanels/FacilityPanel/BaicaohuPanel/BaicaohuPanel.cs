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
        private Text m_UpGradeCondition;

        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Transform m_ItemTra;

        [SerializeField]
        private GameObject m_RedPoint;
        [SerializeField]
        private GameObject m_ItemPrefab;
        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private BaicaohuInfo m_CurBaicaohuInfo = null;
        private List<CostItem> m_CostItems;
        private FacilityLevelInfo m_NextFacilityLevelInfo = null;
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
                        item.Countdown(cd.GetProgress(), (string)param[1]);
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
                        item.StartEffect(cd.GetProgress(), (string)param[1]);
                        break;
                    }
                }
            }
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
                if (!CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems))
                    return;
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
                if (isReduceSuccess)
                {
                    AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    RefreshPanelInfo();
                    HideSelfWithAnim();
                }
            });
        }

        private void RefreshPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems, false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
            m_CurLevelTxt.text = CommonUIMethod.GetGrade(m_CurBaicaohuInfo.level);
            m_BaicaohuCont.text = TDFacilityConfigTable.GetFacilityConfigInfo(m_CurFacilityType).desc;
            m_FacilityIcon.sprite = FindSprite("Baicaohu" + m_CurLevel);
            m_NextUnlockName.text = ((BaicaohuInfo)m_NextFacilityLevelInfo).GetCurMedicinalPowderName();
            RefreshPanelText();
            UpdateItems();
        }
        private void GetInformationForNeed()
        {
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurBaicaohuInfo = (BaicaohuInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_Res1Img.gameObject.SetActive(false);
                m_Res2Img.gameObject.SetActive(false);
                m_Res3Img.gameObject.SetActive(false);
                m_NextUnlockName.text = Define.COMMON_DEFAULT_STR;
                m_UpGradeCondition.text = Define.COMMON_DEFAULT_STR;
            }
            else
            {
                m_NextFacilityLevelInfo = (BaicaohuInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
                m_CostItems = m_NextFacilityLevelInfo.GetUpgradeResCosts();
            }
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
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_Res1Value, m_Res1Img, m_Res2Value, m_Res2Img, m_Res3Value, m_Res3Img, m_NextFacilityLevelInfo, this);
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
                for (int i = 0; i < TDFacilityBaicaohuTable.dataList.Count; i++)
                {
                    temp = TDFacilityBaicaohuTable.dataList[i].unlockHerbID.Split(';');
                    foreach (var id in temp)
                    {
                        GameObject obj = Instantiate(m_ItemPrefab, m_ItemTra);
                        BaicaohuItem item = obj.GetComponent<BaicaohuItem>();
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