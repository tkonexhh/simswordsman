using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public class StoryWithCircleMaskPanel : StoryPanel
    {
        [SerializeField]
        private RectTransform m_TargetRect;
        [SerializeField]
		private CircleShaderControl m_CircleShaderControl;

        protected override void OnPanelOpen(params object[] args)
        {
            if (args.Length > 0)
            {
                Vector3 ptWorld = Camera.main.WorldToScreenPoint((Vector3)args[0]);
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, ptWorld, UIMgr.S.uiRoot.uiCamera, out pos);
                m_TargetRect.anchoredPosition = pos;
                m_CircleShaderControl.Init(m_TargetRect);
            }

            m_StoryIndex = 0;
            StoryEndedCallBack = (Action)args[2];

            string[] ids = ((string)args[1]).Split(',');
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
    }	
}