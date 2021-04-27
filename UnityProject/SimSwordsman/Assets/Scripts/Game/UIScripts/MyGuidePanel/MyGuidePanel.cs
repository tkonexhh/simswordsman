using Qarth;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum GuideMethod
{
    Method1,
    Method2,
    Method3,
    NoBlack,
    NoMessage,
}
namespace GameWish.Game
{
    public class MyGuidePanel : AbstractPanel
    {
        [SerializeField]
        private CircleShaderControl m_CircleShaderControl;

        [SerializeField]
        private RectTransform m_TargetRect;

        [SerializeField]
        private Transform m_Hand;

        [SerializeField]
        private GameObject m_GuideTips1;
        [SerializeField]
        private Text m_GuideTipsText1;

        [SerializeField]
        private GameObject m_GuideTips2;
        [SerializeField]
        private Text m_GuideTipsText2;

        private GuideMethod m_GuestMethod = GuideMethod.Method1;
        private int m_GuidestepId = -1;

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
                m_TargetRect = args[0] as RectTransform;
                m_Hand.transform.position = m_TargetRect.position;
            }
            if (args.Length > 1)
            {
                m_GuidestepId = (int)args[1];
            }

            if (args.Length > 2)
            {
                bool isNotForce = (bool)args[2];
                Action action = (Action)args[3];
                Button button = m_CircleShaderControl.transform.GetComponent<Button>();
                if (isNotForce)//弱点击
                {
                    Timer.S.Post2Really(x => { canClick = true; }, 1);
                    if (button == null)
                        button = m_CircleShaderControl.gameObject.AddComponent<Button>();
                    else
                    {
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() =>
                        {
                            if (canClick)
                                action?.Invoke();
                        });
                    }
                }
                else
                {
                    if (button != null)
                        Destroy(button);
                }
            }
            //是否有黑色遮罩
            if (m_GuestMethod == GuideMethod.NoBlack)
            {
                m_CircleShaderControl.InitAsNoBlack(m_TargetRect);
            }
            else
            {
                // m_CircleShaderControl.Init(m_TargetRect);

                bool isRectMask = false;
                if (args.Length >= 5)
                {
                    isRectMask = (bool)args[4];

                }
                if (isRectMask)
                {
                    m_CircleShaderControl.InitWithRectMask(m_TargetRect);
                }
                else
                {
                    m_CircleShaderControl.Init(m_TargetRect);
                }
            }

            if (m_GuestMethod == GuideMethod.NoMessage)
            {
                m_GuideTips1.gameObject.SetActive(false);
            }
            else
            {
                m_GuideTips1.gameObject.SetActive(true);
            }

            EventSystem.S.Send(EventID.OnGuidePanelOpen, m_GuidestepId);
        }
        protected override void OnClose()
        {
            m_CircleShaderControl.EndGuide();
            base.OnClose();
        }

        public void LocateMyGuideTips(string GuideTips, Vector3 guideTipsPosition, bool isFlip)
        {
            if (!isFlip)
            {
                m_GuideTips2.gameObject.SetActive(false);
                m_GuideTips1.gameObject.SetActive(true);
                ((RectTransform)m_GuideTips1.transform).anchoredPosition = guideTipsPosition;
                m_GuideTipsText1.text = GuideTips;
            }
            else
            {
                m_GuideTips1.gameObject.SetActive(false);
                m_GuideTips2.gameObject.SetActive(true);
                ((RectTransform)m_GuideTips2.transform).anchoredPosition = guideTipsPosition;
                m_GuideTipsText2.text = GuideTips;
            }
        }
    }
}

