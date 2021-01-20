using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using UnityEngine.UI;
using DG.Tweening;
using System;

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
        private Transform m_GuideHandBg;
        [SerializeField]
        private Transform m_Hand;
      
        [SerializeField]
        private Transform m_GuideMethod1;

        [SerializeField]
        private Transform m_GuideMethod2;
     
        [SerializeField]
        private Transform m_Method2Start;
        [SerializeField]
        private Transform m_Method2End;
        
        [SerializeField]
        private Transform m_GuideMethod3;

        [SerializeField]
        private GameObject m_GuideTipsImg;
        [SerializeField]
        private Text m_GuideTipsText;
        
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
            if(args.Length > 0)
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

            //if(args.Length > 4)
            //{
            //    m_Method2Start = args[3] as Transform;
            //    m_Method2End = args[4] as Transform;
            //}

            //是否有黑色遮罩
            if(m_GuestMethod == GuideMethod.NoBlack)
            {
                m_CircleShaderControl.InitAsNoBlack(m_TargetRect);
            }
            else
            {
                m_CircleShaderControl.Init(m_TargetRect);
            }

            if(m_GuestMethod == GuideMethod.NoMessage)
            {
                m_GuideTipsImg.gameObject.SetActive(false);
            }
            else
            {
                m_GuideTipsImg.gameObject.SetActive(true);
            }

            CheckMyUIState();
            EventSystem.S.Send(EventID.OnGuidePanelOpen, m_GuidestepId);
        }
        protected override void OnClose()
        {
            m_CircleShaderControl.EndGuide();
            base.OnClose();
        }
        
        private void CheckMyUIState()
        {
            m_GuideMethod1.gameObject.SetActive(false);
            m_GuideMethod2.gameObject.SetActive(false);
            m_GuideMethod3.gameObject.SetActive(false);
            switch (m_GuestMethod)
            {
                case GuideMethod.Method1:
                    m_GuideMethod1.gameObject.SetActive(true);
                    //LocadGuideHand();
                    break;
                case GuideMethod.Method2:
                    m_GuideMethod2.gameObject.SetActive(true);   
                    break;
                case GuideMethod.Method3:
                    m_GuideMethod3.gameObject.SetActive(true);
                    //LocadGuideArrow();
                    break;
                case GuideMethod.NoMessage:
                    m_GuideMethod1.gameObject.SetActive(true);
                    //LocadGuideHand();
                    break;
                default:
                    break;
            }
        }
        public void LocateMyGuideTips(string GuideTips, Vector3 guideTipsPosition)
        {
            m_GuideTipsImg.gameObject.SetActive(true);
            m_GuideTipsImg.transform.localPosition = guideTipsPosition;
            m_GuideTipsText.text = GuideTips;
        }
    }
}

