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

        protected override void OnUIInit()
        {
            base.OnUIInit();

            m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();

            m_VillaBtn.onClick.AddListener(() => {
                GameDataMgr.S.GetClanData().SetClanName("修仙山庄");
                m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();
            });
            m_DiscipleBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.DisciplePanel); });
            m_BulletinBorardBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.BulletinBoardPanel); });
            m_SignInBtn.onClick.AddListener(() => {
                //UIMgr.S.OpenPanel(UIID.SignInPanel); 
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(Armor.BrightLightArmor, Step.Eight),3);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(Arms.DragonCarvingKnife, Step.Eight),3);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(Arms.DragonCarvingKnife, Step.Four),3);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(Arms.HeavenReliantSword, Step.Eight),3);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(Arms.DragonCarvingKnife, Step.One),3);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem(RawMaterial.Charoite),3);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem(RawMaterial.Malachite),3);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem(RawMaterial.RedAgate),3);
                //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Arms, 3, 4));
            });
            m_ChallengeBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.ChallengePanel); });
            m_VoldemortTowerBtn.onClick.AddListener(() => { });
            m_CreateCoinBtn.onClick.AddListener(()=> {
                GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(5000);
            });
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(5000);
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
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnAddCoinNum, HandleEvent);
            EventSystem.S.Register(EventID.OnReduceCoinNum, HandleEvent);
            EventSystem.S.Register(EventID.OnCloseParentPanel, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnAddCoinNum, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnReduceCoinNum, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandleEvent);
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
    }
}

