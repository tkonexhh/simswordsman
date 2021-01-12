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

        private CharacterItem m_CharacterItem = null;
        private int stage = 0;
        private CharacterKongfuDBData kungfu = null;

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
                    m_CharacterItem = (CharacterItem)args[1];
                    stage = (int)args[2];
                    m_PromotionTitle.text = "��������";
                    m_PromotionInfo.text = m_CharacterItem.name + "����" + CommonUIMethod.GetTextNumber(stage) + "�ε���";
                    break;
                case EventID.OnKongfuLibraryUpgrade:
                    m_CharacterItem = (CharacterItem)args[1];
                    kungfu = (CharacterKongfuDBData)args[2];
                    m_PromotionTitle.text = "�书ͻ��";
                    m_PromotionInfo.text = m_CharacterItem.name + "��"+ kungfu.kongfuType.ToString()+"����" + CommonUIMethod.GetTextNumber(kungfu.level) + "��";
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