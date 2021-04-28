using Qarth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class SplashScreenPanel : AbstractPanel
    {
        [SerializeField]
        private List<SplashItem> m_SplashParentList = new List<SplashItem>();
        [SerializeField]
        private Button m_NextBtn;

        private SplashItem m_CurrentSplashItem = null;

        private int m_SplashItemIndex = 0;

        private Action m_EndAction = null;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            m_NextBtn.onClick.AddListener(() =>
            {
                m_NextBtn.gameObject.SetActive(false);
                ShowSplashItem();
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            if (args != null && args.Length > 0)
            {
                m_EndAction = (Action)args[0];
            }
            m_NextBtn.gameObject.SetActive(false);
            for (int i = 0; i < m_SplashParentList.Count; i++)
            {
                m_SplashParentList[i].gameObject.SetActive(false);
            }
            ShowSplashItem();
        }
        public void ShowNextBtn()
        {
            m_NextBtn.gameObject.SetActive(true);
        }
        private void ShowSplashItem()
        {
            if (m_CurrentSplashItem != null)
            {
                m_CurrentSplashItem.gameObject.SetActive(false);
            }
            if (m_SplashItemIndex < m_SplashParentList.Count)
            {
                m_CurrentSplashItem = m_SplashParentList[m_SplashItemIndex++];
                m_CurrentSplashItem.gameObject.SetActive(true);
                m_CurrentSplashItem.Init(this);
            }
            else
            {
                if (m_EndAction != null)
                {
                    m_EndAction();
                }
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (m_CurrentSplashItem != null)
                {
                    m_CurrentSplashItem.JumpToNext();
                }
            }
#elif UNITY_ANDROID
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (m_CurrentSplashItem != null)
                {
                    m_CurrentSplashItem.JumpToNext();
                }
            }
#endif
        }
    }
}
