﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using System;

namespace GameWish.Game
{
    public class MainMenuPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_VillaName;
        [SerializeField]
        private Text m_CoinValue;
        [SerializeField]
        private Text m_BaoziValue;
        [SerializeField]
        private GameObject m_BaoziCountDownObj;
        [SerializeField]
        private Text m_BaoziCountDown;
        [SerializeField]
        private Text m_WareHouseNumber;


        [SerializeField]
        private Button m_VillaBtn;
        [SerializeField]
        private Button m_CreateCoinBtn;
        [SerializeField]
        private Button m_CreateBaoziBtn;
        [SerializeField]
        private Button m_SignInBtn;
        [SerializeField]
        private Button m_BulletinBoardBtn;
        [SerializeField]
        private Button m_WareHouseBtn;

        [Header("菜单按钮")]
        [SerializeField]
        private Button m_DiscipleBtn;
        [SerializeField]
        private Button TrialRunBtn;
        [SerializeField]
        private Button m_ChallengeBtn;
        [SerializeField]
        private Button m_VoldemortTowerBtn;    
        [SerializeField]
        private Button m_MythicalAnimalsBtn;

        [SerializeField]
        private Button m_VisitorBtn1;
        [SerializeField]
        private Button m_VisitorBtn2;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            int limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;

            if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= limit)
            {
                m_BaoziCountDownObj.SetActive(false);
            }

            if (string.IsNullOrEmpty(GameDataMgr.S.GetClanData().GetClanName()))
                m_VillaName.transform.parent.gameObject.SetActive(false);
            else
            {
                m_VillaName.transform.parent.gameObject.SetActive(true);
                m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();
            }

            m_CreateBaoziBtn.onClick.AddListener(()=> {
                GameDataMgr.S.GetPlayerData().AddFoodNum(100);
            });

            m_VillaBtn.onClick.AddListener(() => {

            });
            m_BulletinBoardBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.BulletinBoardPanel);
            });
            m_WareHouseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.WarehousePanel);
            });
            m_DiscipleBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                UIMgr.S.OpenPanel(UIID.DisciplePanel);
            });
            TrialRunBtn.onClick.AddListener(() => {
         
            });
            m_SignInBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.SignInPanel);
            });
            m_ChallengeBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.ChallengePanel); 
            });
            m_VoldemortTowerBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                FloatMessage.S.ShowMsg("暂未开放，敬请期待");
            });
            m_CreateCoinBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(50000);
            });

            m_VisitorBtn1.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.VisitorPanel, 0);
            });
            m_VisitorBtn2.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.VisitorPanel, 1);
            });
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            //GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(5000);
            RefreshPanelInfo();

            RegisterEvents();
        }

        protected override void OnClose()
        {
            base.OnClose();

            UnregisterEvents();
        }

        private void RefreshPanelInfo()
        {
            m_CoinValue.text = CommonUIMethod.GetTenThousand((int)GameDataMgr.S.GetPlayerData().GetCoinNum());
            m_BaoziValue.text = GameDataMgr.S.GetPlayerData().GetFoodNum().ToString()+Define.SLASH+ GetFoodUpperLimit().ToString();
            m_WareHouseNumber.text = GetWareHouseAllPeopleNumber();
        }

        private int GetFoodUpperLimit()
        {
            int foodMaxLimit = 0;
            FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Kitchen);
            if (facility.GetState() == FacilityState.Unlocked)
            {
                int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen);
                foodMaxLimit = TDFacilityKitchenTable.GetFoodMaxLimit(curLevel);
            }
            return foodMaxLimit;
        }

        private string GetWareHouseAllPeopleNumber()
        {
            int CurLevelMaxNumber = 0;
            int CurLevelNumber = MainGameMgr.S.CharacterMgr.GetAllCharacterList().Count;
            //for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
            //    CurLevelMaxNumber+=TDFacilityLivableRoomTable.GetCapability(i, TDFacilityLivableRoomTable.GetMaxLevel(i));

            for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
            {
                FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController((FacilityType)i);
                if (facility.GetState()== FacilityState.Unlocked)
                    CurLevelMaxNumber += TDFacilityLivableRoomTable.GetCapability(i, MainGameMgr.S.FacilityMgr.GetFacilityCurLevel((FacilityType)i));
            }

            return CurLevelNumber.ToString() + Define.SLASH + CurLevelMaxNumber.ToString();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnRefreshMainMenuPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnCloseParentPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnCheckVisitorBtn, CheckVisitorBtn);
            EventSystem.S.Register(EventID.OnClanNameChange, ChangeClanName);
            EventSystem.S.Register(EventID.OnFoodRefreshEvent, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshMainMenuPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCheckVisitorBtn, CheckVisitorBtn);
            EventSystem.S.UnRegister(EventID.OnClanNameChange, ChangeClanName);
            EventSystem.S.UnRegister(EventID.OnFoodRefreshEvent, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefreshMainMenuPanel:
                    RefreshPanelInfo();
                    break;
                case EventID.OnCloseParentPanel:
                    CloseSelfPanel();
                    break;
                case EventID.OnFoodRefreshEvent:
                    if ((bool)param[1])
                    {
                        m_BaoziCountDownObj.gameObject.SetActive(false);
                    }
                    else
                    {
                        m_BaoziCountDown.text = (string)param[0];
                        m_BaoziCountDownObj.gameObject.SetActive(true);
                    }
                    break;

            }
        }
        private void ChangeClanName(int key, object[] param)
        {
            if (!m_VillaName.transform.parent.gameObject.activeSelf)
                m_VillaName.transform.parent.gameObject.SetActive(true);
            m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();
        }

        private void CheckVisitorBtn(int key, params object[] param)
        {
            switch ((int)param[0])
            {
                case 0:
                    m_VisitorBtn1.transform.parent.gameObject.SetActive(false);
                    m_VisitorBtn2.transform.parent.gameObject.SetActive(false);
                    break;
                case 1:
                    m_VisitorBtn1.transform.parent.gameObject.SetActive(true);
                    m_VisitorBtn2.transform.parent.gameObject.SetActive(false);
                    break;
                case 2:
                    m_VisitorBtn1.transform.parent.gameObject.SetActive(true);
                    m_VisitorBtn2.transform.parent.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}

