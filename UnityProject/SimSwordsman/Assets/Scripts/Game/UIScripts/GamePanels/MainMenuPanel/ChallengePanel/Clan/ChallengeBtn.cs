using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ChallengeBtn : MonoBehaviour
	{
        [SerializeField]
        private BtnFunc m_NormalState = null;
        [SerializeField]
        private BtnFunc m_BossState = null;

        private BtnFunc m_BtnFunc = null;

        public void Init(bool isBossLevel)
        {
            bool isBoss = isBossLevel;

            if (isBoss)
            {
                m_NormalState.gameObject.SetActive(false);
                m_BossState.gameObject.SetActive(true);
                m_BtnFunc = m_BossState;
            }
            else
            {
                m_NormalState.gameObject.SetActive(true);
                m_BossState.gameObject.SetActive(false);
                m_BtnFunc = m_NormalState;
            }
        }

        public void RefreshBtnInfo(ChallengeBtnState state, int number, ChapterConfigInfo chapterConfigInfo, LevelConfigInfo levelConfigInfo)
        {
            m_BtnFunc.RefreshBtnInfo(state, number, chapterConfigInfo, levelConfigInfo);
        }

        public void SetEnabled(bool enable)
        {
            m_BtnFunc.GetComponent<Button>().enabled = enable;
        }
    }
	
}