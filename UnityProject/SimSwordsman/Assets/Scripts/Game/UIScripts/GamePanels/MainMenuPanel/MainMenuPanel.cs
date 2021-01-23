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

            m_VillaBtn.onClick.AddListener(() => {

                for (int i = (int)RawMaterial.QingRock; i < (int)RawMaterial.SnakeTeeth; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)i), 2000);
                }
                //功夫
                for (int i = (int)KongfuType.TaiZuChangQuan; i < (int)KongfuType.ZuiQuan; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KongfuType)i), 2000);
                }
                //添加铠甲
                //for (int i = (int)ArmorType.ZiTenJia; i < (int)ArmorType.RuanWeiJia; i++)
                //{
                //    for (int j = 1; j <= 9; j++)
                //    {
                //        MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)i, (Step)j), 2000);
                //    }
                //}
                //添加武器
                //for (int i = (int)ArmsType.ShaZhuDao; i < (int)ArmsType.YanYueDao; i++)
                //{
                //    for (int j = 1; j <= 9; j++)
                //    {
                //        MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)i, (Step)j), 2000);
                //    }
                //}

                MainGameMgr.S.CharacterMgr.AddCharacterLevel(0, 500);

                //MainGameMgr.S.CharacterMgr.LearnKungfu(0, new KungfuItem(KongfuType.DuGuJiuJian));
                //MainGameMgr.S.CharacterMgr.AddCharacterLevel(0,200);
                //GameDataMgr.S.GetClanData().SetClanName("修仙山庄");
                //m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();
            });
            m_DiscipleBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                UIMgr.S.OpenPanel(UIID.DisciplePanel);
            });
            m_BulletinBorardBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.BulletinBoardPanel); });
            m_SignInBtn.onClick.AddListener(() => {
                UIMgr.S.OpenPanel(UIID.SignInPanel);
            });
            m_ChallengeBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.ChallengePanel); });
            m_VoldemortTowerBtn.onClick.AddListener(() => { });
            m_CreateCoinBtn.onClick.AddListener(()=> {
                GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(50000);
            });

            m_VisitorBtn1.onClick.AddListener(() => {
                UIMgr.S.OpenPanel(UIID.VisitorPanel, 0);
            });
            m_VisitorBtn2.onClick.AddListener(() => {
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

