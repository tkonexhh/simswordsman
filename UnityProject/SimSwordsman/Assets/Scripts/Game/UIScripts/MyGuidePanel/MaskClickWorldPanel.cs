using System;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using UnityEngine.UI;
using DG.Tweening;

namespace GameWish.Game
{
    public class MaskClickWorldPanel : AbstractPanel
    {
        [SerializeField]
        private CircleShaderControl m_CircleShaderControl;

        [SerializeField]
        private RectTransform m_TargetRect;

        [SerializeField]
        private Button m_Targetbtn;

        [SerializeField]
        private RectTransform m_Hand;

        [SerializeField]
        private Text m_GuideTipsText;

        private Action clickAction;
        //private int m_GuidestepId = -1;

        protected override void OnOpen()
        {
            base.OnOpen();
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            if (args.Length > 0)
            {
                m_Hand.anchoredPosition = (Vector3)args[0];
            }
            if (args.Length > 1)
            {
                m_TargetRect.anchoredPosition = (Vector3)args[1];
                m_CircleShaderControl.Init(m_TargetRect);
            }

            if (args.Length > 2)
            {
                m_GuideTipsText.text = TDLanguageTable.Get((string)args[2]);
            }

            if (args.Length > 3)
            {
                clickAction = (Action)args[3];
                m_Targetbtn.onClick.AddListener(OnClick);
            }
            //EventSystem.S.Send(EventID.OnGuidePanelOpen, m_GuidestepId);
            EventSystem.S.Send(EventID.InGuideProgress, false);
        }
        protected override void OnClose()
        {
            m_CircleShaderControl.EndGuide();
            //m_Hand.DOKill();
            //m_Hand2.DOKill();

            base.OnClose();
        }

        void OnClick()
        {
            EventSystem.S.Send(EventID.InGuideProgress, true);
            m_Targetbtn.onClick.RemoveAllListeners();
            CloseSelfPanel();
            clickAction?.Invoke();
        }
    }
}

