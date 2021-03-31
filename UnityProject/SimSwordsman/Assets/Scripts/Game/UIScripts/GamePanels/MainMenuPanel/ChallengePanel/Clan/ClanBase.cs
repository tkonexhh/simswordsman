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
        protected ScrollRect m_ScrollRect;
        [SerializeField]
        protected ChallengeBtn[] m_Buttons;

        [HideInInspector]
        public RectTransform bg;
        [HideInInspector]
        public RectTransform m_ViewPortRT;


        protected int m_CurLevel = -1;
        protected ChapterConfigInfo m_CurChapterConfigInfo = null;
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

        private bool m_IsInit = false;
        private void Init()
        {
            if (m_IsInit) return;

            m_IsInit = true;

            m_ViewPortRT = transform.Find("Scroll View/Viewport").GetComponent<RectTransform>();

            bg = m_ViewPortRT.Find("CheckpointTra/Bg").GetComponent<RectTransform>();
        }

        public void UpdateScrollRectValue()
        {
            Init();

            if (m_CurLevel >= 0 && m_CurLevel < m_Buttons.Length)
            {
                RectTransform targetBtnRT = m_Buttons[m_CurLevel].GetComponent<RectTransform>();

                float bgHeight = bg.rect.height;

                float canvasHeight = m_ViewPortRT.rect.height;

                Vector3 childTraLocalPos = targetBtnRT.anchoredPosition;

                childTraLocalPos.y = Mathf.Abs(childTraLocalPos.y + bgHeight * .5f);

                childTraLocalPos.y -= canvasHeight * .5f;

                float bili = Mathf.Clamp01(childTraLocalPos.y / (bgHeight - canvasHeight));

                m_ScrollRect.verticalNormalizedPosition = bili;
            }
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
                    bool isBossLevel = TDLevelConfigTable.IsBossLevel(levelConfigInfo[i].level);
                    m_Buttons[i].Init(isBossLevel);
                    m_Buttons[i].transform.SetAsFirstSibling();
                    //HACK 修改过关点击
                    if (i < m_CurLevel)
                    {
                        m_Buttons[i].SetEnabled(false);
                        m_Buttons[i].RefreshBtnInfo(ChallengeBtnState.Over, i, m_CurChapterConfigInfo, levelConfigInfo[i]);
                    }
                    else if (i == m_CurLevel)
                    {
                        m_Buttons[i].SetEnabled(true);
                        m_Buttons[i].RefreshBtnInfo(ChallengeBtnState.Battle, i, m_CurChapterConfigInfo, levelConfigInfo[i]);
                        //TODO scrollview跳转到此处
                        // Debug.LogError("length:" + Mathf.Max(0, m_CurLevel) + "CurLvl:" + m_CurLevel);
                        float range = ScrollViewExtension.GetScrollViewNormalizedPosition(m_ScrollRect, m_ScrollRect.content.GetChild(0).rectTransform(), Mathf.Max(0, m_CurLevel), 300);

                        m_ScrollRect.verticalNormalizedPosition = range;
                    }
                    else
                    {
                        m_Buttons[i].SetEnabled(false);
                        m_Buttons[i].RefreshBtnInfo(ChallengeBtnState.Lock, i, m_CurChapterConfigInfo, levelConfigInfo[i]);
                    }

                    if (PlatformHelper.isTestMode)
                    {
                        m_Buttons[i].enabled = true;
                    }
                }
            }
        }
    }
}