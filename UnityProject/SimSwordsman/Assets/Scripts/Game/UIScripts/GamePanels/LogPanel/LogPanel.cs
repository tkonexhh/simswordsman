using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class LogPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_LogTitle;
        [SerializeField]
        private Text m_LogCont;

        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_RefuseBtn;

        public Action OnSuccessBtnEvent;
        public Action OnRefuseBtnEvent;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_LogTitle.text = (string)args[0];
            m_LogCont.text = (string)args[1];
            m_AcceptBtn.transform.GetComponentInChildren<Text>().text = (string)args[2];
            m_RefuseBtn.transform.GetComponentInChildren<Text>().text = (string)args[3];
        }

        private void BindAddListenerEvent()
        {
            m_RefuseBtn.onClick.AddListener(()=> {
                HideSelfWithAnim();
                OnRefuseBtnEvent?.Invoke();
            });
            m_AcceptBtn.onClick.AddListener(()=> {
                HideSelfWithAnim();
                OnSuccessBtnEvent?.Invoke();
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}