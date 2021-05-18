using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace GameWish.Game
{
	public class ChallengeTalkPanel :AbstractAnimPanel
	{
        [SerializeField] 
        private Text m_ContentText;
        [SerializeField] 
        protected Button m_NextButton;

        private Tweener m_TextTweener;
        private bool m_IsUpdateContent;

        protected int m_StoryIndex = 0;
        protected List<string> m_NowStoryIDList = new List<string>();

        private string m_Content;

        protected override void OnUIInit()
        {
            base.OnUIInit();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }


        protected override void OnClose()
        {
            base.OnClose();
        }


        /// <summary>
        /// 更新对话内容
        /// </summary>
        /// <param name="config"></param>
        protected void UpdateContent()
        {
            KillTweener(m_TextTweener);
            m_Content = TDLanguageTable.Get(m_NowStoryIDList[m_StoryIndex]);

            m_TextTweener = m_ContentText
                .DoText(m_Content, 0.03f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    m_IsUpdateContent = false;
                });
            m_IsUpdateContent = true;
        }

        /// <summary>
        /// 下一步回调
        /// </summary>
        protected void NextBtCallBack()
        {
            //if (!m_SkipButton.gameObject.activeInHierarchy)
            //{
            //    m_SkipButton.gameObject.SetActive(true);
            //}
            //if (m_IsUpdateContent)
            //{
            //    KillTweener(m_TextTweener);

            //    m_ContentText.text = m_Content;
            //    if (m_UpdateContentTweener == null)
            //    {
            //        m_UpdateContentTweener = DOTween.To((x) => { }, 0, 1, 0.5f)
            //        .OnComplete(() =>
            //        {
            //            m_UpdateContentTweener = null;
            //            m_IsUpdateContent = false;
            //        });
            //    }
            //}
            //else
            //{
            //    NextStory();
            //}
        }


        private void NextStory()
        {
            //if (m_StoryIndex < m_NowStoryIDList.Count - 1)
            //{
            //    m_StoryIndex++;
            //    UpdateStoryLimit();
            //}
            //else
            //{
            //    StoryPanelOver();
            //}
        }

        private void KillTweener(Tweener tweener)
        {
            if (tweener != null && tweener.IsActive() && tweener.IsPlaying())
            {
                tweener.Kill();
            }
        }

    }
	
}