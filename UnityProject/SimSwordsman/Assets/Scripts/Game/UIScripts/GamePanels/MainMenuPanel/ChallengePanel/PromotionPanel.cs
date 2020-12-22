using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class PromotionPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_PromotionTitle;
        [SerializeField]
        private Text m_PromotionInfo;
        [SerializeField]
        private Button m_ExitBtn;

        private CharacterController m_CharacterController = null;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            switch ((EventID)args[0])
            {
                case EventID.OnCharacterUpgrade:
                    m_CharacterController = (CharacterController)args[1];
                    m_PromotionTitle.text = "µÜ×ÓÉý¶Î";
                    //m_PromotionInfo.text = m_CharacterController.CharacterModel.c
                    break;
                case EventID.OnKongfuLibraryUpgrade:
                    break;
                default:
                    break;
            }
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

    }
	
}