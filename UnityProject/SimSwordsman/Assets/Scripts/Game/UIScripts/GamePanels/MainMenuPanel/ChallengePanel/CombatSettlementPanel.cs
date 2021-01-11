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
            m_SelectedDiscipleList = MainGameMgr.S.BattleFieldMgr.OurCharacterList;

            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(()=> 
            {
                EventSystem.S.Send(EventID.OnExitBattle);
                HideSelfWithAnim();
                UIMgr.S.ClosePanelAsUIID(UIID.CombatInterfacePanel);
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_LevelConfigInfo = (LevelConfigInfo)args[0];
            m_IsSuccess = (bool)args[1];

            foreach (var item in m_SelectedDiscipleList)
            {
                CreateRewardIInfoItem(item);
            }
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
        }
    }
}