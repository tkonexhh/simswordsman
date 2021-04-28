using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ArenaRestPanel : AbstractAnimPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnShop;
        [SerializeField] private Text m_TextDesc;
        [SerializeField] private Text m_TextOpenTime;
        [SerializeField] private Text m_TextCountdownTime;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(OnClickClose);
            m_BtnShop.onClick.AddListener(OnClickShop);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);

        }

        protected override void OnClose()
        {
            CloseDependPanel(EngineUI.MaskPanel);
        }

       
        private void OnClickClose()
        {
            CloseSelfPanel();
        }

        private void OnClickShop()
        {

        }
    }

}