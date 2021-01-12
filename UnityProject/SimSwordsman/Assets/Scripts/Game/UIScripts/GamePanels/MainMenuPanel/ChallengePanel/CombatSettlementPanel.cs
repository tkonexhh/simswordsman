using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class CombatSettlementPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_CombatSettlementCont;
        [SerializeField]
        private Button m_ExitBtn;

        [SerializeField]
        private Transform m_RewardContainer;
        [SerializeField]
        private GameObject m_RewardinfoItem;

        private bool m_IsSuccess;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private List<CharacterController> m_SelectedDiscipleList = null;
        private List<HerbType> m_SeletedHerb = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnCharacterUpgrade, HandleListnerEvent);
            EventSystem.S.Register(EventID.OnKongfuLibraryUpgrade, HandleListnerEvent);
            m_SelectedDiscipleList = MainGameMgr.S.BattleFieldMgr.OurCharacterList;

            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(()=> 
            {
                PanelPool.S.DisplayPanel();
                UIMgr.S.ClosePanelAsUIID(UIID.CombatInterfacePanel);
                OnPanelHideComplete();
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });
        }
        private void HandleListnerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnCharacterUpgrade:
                    PanelPool.S.AddPromotion(new DiscipleRiseStage((EventID)key, (int)param[0],(int)param[1]));
                    // UIMgr.S.OpenTopPanel(UIID.PromotionPanel,null, key, m_CurCharacterItem, param[0]);
                    break;
                case EventID.OnKongfuLibraryUpgrade:
                    PanelPool.S.AddPromotion(new WugongBreakthrough((EventID)key, (int)param[0], (CharacterKongfuDBData)param[1]));
                    //UIMgr.S.OpenTopPanel(UIID.PromotionPanel, null, key, m_CurCharacterItem, param[0]);
                    break;
            }
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_LevelConfigInfo = (LevelConfigInfo)args[0];
            m_IsSuccess = (bool)args[1];

            if (m_IsSuccess)
                m_CombatSettlementCont.text = "Ê¤Àû";
            else
                m_CombatSettlementCont.text = "Ê§°Ü";

            foreach (var item in m_SelectedDiscipleList)
                CreateRewardIInfoItem(item);
        }

        private void CreateRewardIInfoItem(CharacterController item)
        {
            ItemICom rewardItemICom = Instantiate(m_RewardinfoItem, m_RewardContainer).GetComponent<ItemICom>();
            rewardItemICom.OnInit(item, null, m_LevelConfigInfo, m_IsSuccess);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            EventSystem.S.UnRegister(EventID.OnCharacterUpgrade, HandleListnerEvent);
            EventSystem.S.UnRegister(EventID.OnKongfuLibraryUpgrade, HandleListnerEvent);
        }
    }
}