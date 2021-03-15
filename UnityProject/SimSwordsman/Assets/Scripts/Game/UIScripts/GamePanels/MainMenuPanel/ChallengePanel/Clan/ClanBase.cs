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
            // m_ScrollRect.DoScrollVertical(0, 0.01f);
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
            List<LevelConfigInfo> levelConfigInfo = new List<LevelConfigInfo>();
            levelConfigInfo.AddRange(m_CurChapterAllLevelConfigInfo.Values);
            if (m_Buttons.Length == levelConfigInfo.Count)
            {
                for (int i = 0; i < m_Buttons.Length; i++)
                {
                    m_Buttons[i].transform.SetAsFirstSibling();
                    if (i < m_CurLevel)
                    {
                        m_Buttons[i].enabled = false;
                        m_Buttons[i].GetComponent<BtnFunc>().RefreshBtnInfo(ChallengeBtnState.Over, i, m_CurChapterConfigInfo, levelConfigInfo[i]);
                    }
                    else if (i == m_CurLevel)
                    {
                        m_Buttons[i].enabled = true;
                        BtnFunc btnFunc = m_Buttons[i].GetComponent<BtnFunc>();
                        btnFunc.RefreshBtnInfo(ChallengeBtnState.Battle, i, m_CurChapterConfigInfo, levelConfigInfo[i]);
                        //TODO scrollview跳转到此处
                        // Debug.LogError("length:" + Mathf.Max(0, m_CurLevel) + "CurLvl:" + m_CurLevel);
                        float range = ScrollViewExtension.GetScrollViewNormalizedPosition(m_ScrollRect, m_ScrollRect.content.GetChild(0).rectTransform(), Mathf.Max(0, m_CurLevel), 300);

                        m_ScrollRect.verticalNormalizedPosition = range;
                    }
                    else
                    {
                        m_Buttons[i].enabled = false;
                        m_Buttons[i].GetComponent<BtnFunc>().RefreshBtnInfo(ChallengeBtnState.Lock, i, m_CurChapterConfigInfo, levelConfigInfo[i]);
                    }
                }
            }
        }
    }
}