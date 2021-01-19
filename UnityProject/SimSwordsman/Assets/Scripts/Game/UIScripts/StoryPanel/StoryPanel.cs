using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using DG.Tweening;
using System;

namespace GameWish.Game
{
    /// <summary>
    /// 剧情界面
    /// </summary>
	public partial class StoryPanel : AbstractAnimPanel
	{
        private Tweener m_TextTweener;

        private int m_StoryIndex = 0;
        private List<string> m_NowStoryIDList = new List<string>();

        private bool m_IsUpdateContent;
        private Tweener m_UpdateContentTweener;

        public Action StoryEndedCallBack;
        private string m_Content;
        
        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            
            m_StoryIndex = 0;
            StoryEndedCallBack = (Action)args[1];

            string[] ids = ((string)args[0]).Split(',');
            m_NowStoryIDList.Clear();

            foreach (var item in ids)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    m_NowStoryIDList.Add(item);
                }
            }

            m_SkipButton.gameObject.SetActive(false);

            m_NextButton.onClick.AddListener(() => { NextBtCallBack(); });
            m_SkipButton.onClick.AddListener(() => { SkipStory(); });

            UpdateContent();
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            m_NextButton.onClick.RemoveAllListeners();
            m_SkipButton.onClick.RemoveAllListeners();
        }

        protected override void OnClose()
        {
            base.OnClose();
            
            m_NowStoryIDList.Clear();
            StoryEndedCallBack = null;
            //UIMgrExtend.S.RemoveAction(m_UIID);
        }

        /// <summary>
        /// 更新对话内容
        /// </summary>
        /// <param name="config"></param>
        private void UpdateContent()
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
        private void NextBtCallBack()
        {
            if(!m_SkipButton.gameObject.activeInHierarchy)
            {
                m_SkipButton.gameObject.SetActive(true);
            }
            if (m_IsUpdateContent)
            {
                KillTweener(m_TextTweener);

                m_ContentText.text = m_Content;
                if(m_UpdateContentTweener == null)
                {
                    m_UpdateContentTweener = DOTween.To((x) => { }, 0, 1, 0.5f)
                    .OnComplete(() => 
                    {
                        m_UpdateContentTweener = null;
                        m_IsUpdateContent = false;
                    });
                }
            }
            else
            {
                NextStory();
            }
        }

        private void NextStory()
        {
            if (m_StoryIndex < m_NowStoryIDList.Count - 1)
            {
                m_StoryIndex++;
                UpdateStoryLimit();
            }
            else
            {
                StoryPanelOver();
            }
        }
        /// <summary>
        /// 剧情总更新
        /// </summary>
        private void UpdateStoryLimit()
        {
            if (m_StoryIndex >= m_NowStoryIDList.Count)
                return;

            UpdateContent();
        }

        private void KillTweener(Tweener tweener)
        {
            if (tweener != null && tweener.IsActive() && tweener.IsPlaying())
            {
                tweener.Kill();
            }
        }

        /// <summary>
        /// 界面结束
        /// </summary>
        private void StoryPanelOver()
        {
            StoryEndedCallBack?.Invoke();
            HideSelfWithAnim();
        }

        /// <summary>
        /// 跳过剧情
        /// </summary>
        private void SkipStory()
        {
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("StoryID", m_ChapterID);

            //DataAnalysisMgr.S.CustomEventDic(Define.CUSTOM_EVENT_SKIP_STORY, dic);

            StoryPanelOver();
        }
        
    }
}