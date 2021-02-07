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
        private Button m_BlackBtn;
        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_RefuseBtn;
        [SerializeField]
        private Button m_CloseBtn;

        public Action OnSuccessBtnEvent;
        public Action OnRefuseBtnEvent;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_LogTitle.text = (string)args[0];
            m_LogCont.text = (string)args[1];
            if (args.Length > 2)
            {
                m_AcceptBtn.transform.GetComponentInChildren<Text>().text = (string)args[2];
            }
            if (args.Length > 3)
            {
                m_RefuseBtn.transform.GetComponentInChildren<Text>().text = (string)args[3];
            }
        }

        private void BindAddListenerEvent()
        {
            m_RefuseBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
                OnRefuseBtnEvent?.Invoke();
            });
            m_BlackBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
            });
            m_AcceptBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
                OnSuccessBtnEvent?.Invoke();
            });

            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
                OnRefuseBtnEvent?.Invoke();
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