using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
    public class MainTaskUIHandler : MonoBehaviour
    {
        [SerializeField] private Animator m_AnimCtrl;
        [SerializeField] private Image m_ImgTaskStatus;
        [SerializeField] private Text m_TxtTaskTitle;
        [SerializeField] private Button m_BtnBG;

        private const string ANIMNAME_SHOW = "Show";
        private const string ANIMNAME_Hide = "Hide";
        private int m_AnimHash_Show;
        private int m_AnimHash_Hide;



        private int m_UIStatus = 0;//0 原始 1 展开
        private bool m_Animing = false;


        public void Init()
        {
            m_AnimHash_Show = Animator.StringToHash(ANIMNAME_SHOW);
            m_AnimHash_Hide = Animator.StringToHash(ANIMNAME_Hide);
            m_BtnBG.onClick.AddListener(OnClickBG);
        }

        private void OnClickBG()
        {
            if (m_Animing)
                return;

            m_Animing = true;
            if (m_UIStatus == 0)
            {
                m_UIStatus = 1;
                m_AnimCtrl.Play(m_AnimHash_Show);
            }
            else
            {
                m_UIStatus = 0;
                m_AnimCtrl.Play(m_AnimHash_Hide);
            }
        }

        public void OnShowComplete()
        {
            m_Animing = false;
            Debug.LogError("OnShowComplete");
        }

        public void OnHideComplete()
        {
            m_Animing = false;
            Debug.LogError("OnHideComplete");
        }
    }

}