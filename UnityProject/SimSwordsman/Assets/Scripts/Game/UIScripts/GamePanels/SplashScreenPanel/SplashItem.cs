using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class SplashItem : MonoBehaviour
    {
        [SerializeField]
        private List<SkeletonGraphic> m_SkeletonList = new List<SkeletonGraphic>();
        private int m_CurrentIndex = 0;
        private const string m_AniName = "anime";

        private SplashScreenPanel m_SplashScreenPanel;
        private bool m_IsCallEndBack = false;
        public void Init(SplashScreenPanel panel)
        {
            if (m_SkeletonList.Count <= 0)
            {
                Debug.LogError("开屏动画为空");
                return;
            }

            m_SplashScreenPanel = panel;

            for (int i = 0; i < m_SkeletonList.Count; i++)
            {
                m_SkeletonList[i].gameObject.SetActive(false);
            }

            PlayNext();
        }

        public void PlayNext()
        {
            if (m_CurrentIndex >= 0 && m_CurrentIndex < m_SkeletonList.Count)
            {
                PlaySpineAni(() =>
                {
                    PlayNext();
                });
            }
            else
            {
                Finished();
            }
        }
        private void Finished()
        {
            if (m_IsCallEndBack == false)
            {
                m_IsCallEndBack = true;
                m_SplashScreenPanel.ShowNextBtn();
            }
        }
        public void JumpToNext()
        {
            if (m_CurrentIndex >= 0 && m_CurrentIndex < m_SkeletonList.Count)
            {
                PlaySpineAni();
            }
            else
            {
                Finished();
            }
        }

        private void PlaySpineAni(Action endCallBack = null)
        {
            SkeletonGraphic spine = m_SkeletonList[m_CurrentIndex++];
            spine.gameObject.SetActive(true);
            SpineHelper.PlayAnim(spine, m_AniName, false, endCallBack);
        }
    }
}
