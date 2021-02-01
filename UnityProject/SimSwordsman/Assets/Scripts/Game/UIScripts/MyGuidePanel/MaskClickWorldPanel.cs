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
        private RectTransform m_GuideTipsTran;
        [SerializeField]
        private Text m_GuideTipsText;


        private Action clickAction;
        bool canClick = false;

        protected override void OnOpen()
        {
            base.OnOpen();
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            if (args.Length > 0)
            {
                Vector3 ptWorld = Camera.main.WorldToScreenPoint((Vector3)args[0]);
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, ptWorld, UIMgr.S.uiRoot.uiCamera, out pos);
                m_Hand.anchoredPosition = pos;//如果位置与预期不一致，请修改锚点对齐方式
                m_TargetRect.anchoredPosition = pos;
                m_CircleShaderControl.Init(m_TargetRect);
            }
            if (args.Length > 1)
            {
                m_GuideTipsTran.anchoredPosition = (Vector3)args[1];
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
            if (args.Length > 4)
            {
                bool isNotForce = (bool)args[4];
                Button button = m_CircleShaderControl.transform.GetComponent<Button>();
                if (isNotForce)//弱点击
                {
                    if (button == null)
                        button = m_CircleShaderControl.gameObject.AddComponent<Button>();

                    Timer.S.Post2Really(x => { canClick = true; }, 1);
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                        if (canClick)
                            OnClick();
                    });
                }
                else
                {
                    if (button != null)
                        Destroy(button);
                }
            }
            EventSystem.S.Send(EventID.InGuideProgress, false);
        }
        protected override void OnClose()
        {
            m_CircleShaderControl.EndGuide();
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