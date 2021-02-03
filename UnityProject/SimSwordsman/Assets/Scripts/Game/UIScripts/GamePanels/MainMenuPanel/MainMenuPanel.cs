using System.Collections;
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
        private Button m_VillaBtn;
        [SerializeField]
        private Button m_CreateCoinBtn;
        [SerializeField]
        private Button m_CreateBaoziBtn;
        [SerializeField]
        private Button m_SignInBtn;

        [Header("菜单按钮")]
        [SerializeField]
        private Button m_DiscipleBtn;
        [SerializeField]
        private Button m_BulletinBorardBtn;
        [SerializeField]
        private Button m_ChallengeBtn;
        [SerializeField]
        private Button m_VoldemortTowerBtn;

        [SerializeField]
        private Button m_VisitorBtn1;
        [SerializeField]
        private Button m_VisitorBtn2;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            if (string.IsNullOrEmpty(GameDataMgr.S.GetClanData().GetClanName()))
                m_VillaName.transform.parent.gameObject.SetActive(false);
            else
            {
                m_VillaName.transform.parent.gameObject.SetActive(true);
                m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();
            }

            m_CreateBaoziBtn.onClick.AddListener(()=> {

                //GameDataMgr.S.GetClanData().SetClanName("修仙门派");

                for (int i = (int)RawMaterial.QingRock; i < (int)RawMaterial.SnakeTeeth; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)i), 2000);
                }
                //for (int i = (int)HerbType.ChiDanZhuangQiWan; i < (int)HerbType.LianHuaQingShenLu; i++)
                //{
                //    MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)i), 200);
                //}
                GameDataMgr.S.GetPlayerData().AddFoodNum(100);
            });

            m_VillaBtn.onClick.AddListener(() => {

                //for (int i = (int)RawMaterial.QingRock; i < (int)RawMaterial.SnakeTeeth; i++)
                //{
                //    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)i), 2000);
                //}

                ////功夫

                //添加铠甲
                //for (int i = (int)ArmorType.ZiTenJia; i < (int)ArmorType.RuanWeiJia; i++)
                //{
                //    MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)i, (Step)1), 2000);

                //}
                //添加武器
                //for (int i = (int)ArmsType.ShaZhuDao; i < (int)ArmsType.YanYueDao; i++)
                //{
                //    MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)i, (Step)2), 2000);

                //}
            });

            m_DiscipleBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                UIMgr.S.OpenPanel(UIID.DisciplePanel);
            });
            m_BulletinBorardBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.BulletinBoardPanel);
            });
            m_SignInBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.SignInPanel);
            });
            m_ChallengeBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.ChallengePanel); 
            });
            m_VoldemortTowerBtn.onClick.AddListener(() => { });
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
            m_CoinValue.text = GameDataMgr.S.GetPlayerData().GetCoinNum().ToString();
            m_BaoziValue.text = GameDataMgr.S.GetPlayerData().GetFoodNum().ToString();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnAddCoinNum, HandleEvent);
            EventSystem.S.Register(EventID.OnReduceCoinNum, HandleEvent);
            EventSystem.S.Register(EventID.OnAddFoodNum, FoodNumChange);
            EventSystem.S.Register(EventID.OnReduceFoodNum, FoodNumChange);
            EventSystem.S.Register(EventID.OnCloseParentPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnCheckVisitorBtn, CheckVisitorBtn);
            EventSystem.S.Register(EventID.OnClanNameChange, ChangeClanName);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnAddCoinNum, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnReduceCoinNum, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnAddFoodNum, FoodNumChange);
            EventSystem.S.UnRegister(EventID.OnReduceFoodNum, FoodNumChange);
            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCheckVisitorBtn, CheckVisitorBtn);
            EventSystem.S.UnRegister(EventID.OnClanNameChange, ChangeClanName);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnAddCoinNum:
                    RefreshPanelInfo();
                    break;
                case EventID.OnReduceCoinNum:
                    RefreshPanelInfo();
                    break;
                case EventID.OnCloseParentPanel:
                    CloseSelfPanel();
                    break;

            }
        }

        private void FoodNumChange(int key, object[] param)
        {
            m_BaoziValue.text = GameDataMgr.S.GetPlayerData().foodNum.ToString();
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

