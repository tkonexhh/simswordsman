using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ClanBase : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect m_ScrollRect;
        [SerializeField]
        private Button[] m_Buttons;
        private int m_CurLevel = -1;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private Dictionary<int, LevelConfigInfo> m_CurChapterAllLevelConfigInfo = null;
        // Start is called before the first frame update
        public virtual void Start()
        {
            m_ScrollRect.DoScrollVertical(0, 0.6f);
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        public void SetPanelInfo(ChapterConfigInfo chapterConfigInfo, Dictionary<int, LevelConfigInfo> chapterAllLevelConfigInfo)
        {
            m_CurChapterConfigInfo = chapterConfigInfo;
            m_CurChapterAllLevelConfigInfo = chapterAllLevelConfigInfo;
            m_CurLevel = MainGameMgr.S.ChapterMgr.GetLevelProgressNumber(m_CurChapterConfigInfo.chapterId);

            if (m_Buttons.Length == m_CurChapterAllLevelConfigInfo.Count)
            {
                for (int i = 0; i < m_Buttons.Length; i++)
                {
                    if (i < m_CurLevel - 1)
                    {
                        m_Buttons[i].enabled = false;
                        m_Buttons[i].GetComponent<BtnFunc>().RefreshBtnInfo(ChallengeBtnState.Over, i);
                    }
                    else if (i == m_CurLevel - 1)
                    {
                        m_Buttons[i].enabled = false;
                        m_Buttons[i].GetComponent<BtnFunc>().RefreshBtnInfo(ChallengeBtnState.Battle, i);
                    }
                    else
                    {
                        m_Buttons[i].enabled = false;
                        m_Buttons[i].GetComponent<BtnFunc>().RefreshBtnInfo(ChallengeBtnState.Lock, i);
                    }
                    m_Buttons[i].onClick.AddListener(() =>
                    {
                        UIMgr.S.OpenPanel(UIID.IdentifyChallengesPanel, m_CurChapterConfigInfo, m_CurChapterAllLevelConfigInfo[i]);
                    });
                }
            }
        }
    }
}