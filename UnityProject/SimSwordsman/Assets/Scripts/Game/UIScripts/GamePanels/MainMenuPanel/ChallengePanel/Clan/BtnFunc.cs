using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameWish.Game
{
    public enum ChallengeBtnState
    {
        /// <summary>
        /// ???
        /// </summary>
        Lock,
        /// <summary>
        /// ?????
        /// </summary>
        Over,
        /// <summary>
        /// ???????
        /// </summary>
        Battle,
    }

    public class BtnFunc : MonoBehaviour
    {
        private Button m_Self;
        [SerializeField]
        private Text m_Number;
        [SerializeField]
        private Image m_Over;
        [SerializeField]
        private Image m_Battle;
        [SerializeField]
        private Image m_Lock;
        private ChallengeBtnState m_ChallengeBtnState;
        private int index;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_CurChapterLevelConfigInfo = null;
        private float m_BattlePosY;
        private Sequence m_TweenerBattle;
        public void RefreshBtnInfo(ChallengeBtnState state, int number, ChapterConfigInfo chapterConfigInfo, LevelConfigInfo levelConfigInfo)
        {
            m_BattlePosY = m_Battle.transform.localPosition.y;
            index = number;
            m_CurChapterConfigInfo = chapterConfigInfo;
            m_CurChapterLevelConfigInfo = levelConfigInfo;
            m_ChallengeBtnState = state;

            switch (m_ChallengeBtnState)
            {
                case ChallengeBtnState.Lock:
                    m_Lock.gameObject.SetActive(true);
                    m_Over.gameObject.SetActive(false);
                    m_Battle.gameObject.SetActive(false);
                    m_Battle.transform.DOKill();
                    m_TweenerBattle.Kill();
                    break;
                case ChallengeBtnState.Over:
                    m_Lock.gameObject.SetActive(false);
                    m_Over.gameObject.SetActive(true);
                    m_Battle.gameObject.SetActive(false);
                    m_TweenerBattle.Kill();
                    m_Number.text = (number + 1).ToString();
                    break;
                case ChallengeBtnState.Battle:
                    m_Lock.gameObject.SetActive(false);
                    m_Over.gameObject.SetActive(false);
                    m_Battle.gameObject.SetActive(true);
                    m_TweenerBattle = DOTween.Sequence()
                        .Append(m_Battle.transform.DOLocalMoveY(m_BattlePosY + 10, 1.2f))
                        .Insert(0f, m_Battle.transform.DOScale(Vector3.one * 1.15f, 1.2f))
                        .Append(m_Battle.transform.DOLocalMoveY(m_BattlePosY, 1.2f))
                        .Insert(0.8f, m_Battle.transform.DOScale(Vector3.one, 1.2f))
                        .SetLoops(-1);

                    m_Number.text = (number + 1).ToString();
                    break;
                default:
                    break;
            }
        }

        public int GetIndex()
        {
            return index;
        }

        void Start()
        {
            // m_BattlePosY = m_Battle.transform.localPosition.y;
            m_Self = GetComponent<Button>();
            m_Self.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (m_CurChapterConfigInfo == null)
                {
                    Log.e("Not fount m_CurChapterConfigInfo");
                    return;
                }

                if (m_CurChapterLevelConfigInfo == null)
                {
                    Log.e("Not fount m_CurChapterLevelConfigInfo");
                    return;
                }

                UIMgr.S.OpenPanel(UIID.IdentifyChallengesPanel, m_CurChapterConfigInfo, m_CurChapterLevelConfigInfo);
            });
        }
    }

}