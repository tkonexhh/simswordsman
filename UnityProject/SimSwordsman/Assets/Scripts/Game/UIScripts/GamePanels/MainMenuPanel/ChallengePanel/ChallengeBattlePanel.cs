using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ChallengeBattlePanel : AbstractAnimPanel
    {
        [SerializeField] private Image m_ChallengeBattleTitle;
        [SerializeField] private Button m_CloseBtn;
        [SerializeField] private Transform m_Parent;
        //private List<Button> m_CheckpointBtns = null;
        private ResLoader m_ResLoader;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private Dictionary<int, LevelConfigInfo> m_CurChapterAllLevelConfigInfo = null;

        private Dictionary<int, Button> m_LevelBtnDic = new Dictionary<int, Button>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            EventSystem.S.Register(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            m_CurChapterConfigInfo = args[0] as ChapterConfigInfo;
            m_CurChapterAllLevelConfigInfo = MainGameMgr.S.ChapterMgr.GetAllLevelConfigInfo(m_CurChapterConfigInfo.chapterId);
            InitPanelInfo();
            LoadClanPrefabs(m_CurChapterConfigInfo.clanType.ToString() + "Panel");
        }

        public void LoadClanPrefabs(string prefabsName)
        {
            if (m_ResLoader == null)
                m_ResLoader = ResLoader.Allocate();

            var prefab = m_ResLoader.LoadSync(prefabsName);
            if (prefab == null)
            {
                Log.e("LoadClanPrefabs: Not found prefabsName:" + prefabsName);
                return;
            }

            GameObject obj = Instantiate(prefab) as GameObject;
            obj.transform.SetParent(m_Parent);
            RectTransform rectTransform = ((RectTransform)obj.transform);
            rectTransform.SetSiblingIndex(0);
            //��¼
            rectTransform.localPosition = Vector3.zero;

            rectTransform.offsetMax = Vector2.zero;

            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -UIMgr.S.panelOffset);
            rectTransform.offsetMin = Vector2.zero;

            obj.transform.localScale = new Vector3(1.01f, 1.01f, 1);
            ClanBase _ClanBase = obj.GetComponent<ClanBase>();
            if (_ClanBase != null)
            {
                _ClanBase.SetPanelInfo(m_CurChapterConfigInfo, m_CurChapterAllLevelConfigInfo);
                _ClanBase.UpdateScrollRectValue();
            }
        }

        protected override void OnClose()
        {
            base.OnClose();

            EventSystem.S.UnRegister(EventID.OnChanllengeSuccess, HandlingListeningEvents);

            m_ResLoader?.ReleaseRes(m_CurChapterConfigInfo.clanType.ToString() + "Panel");
        }

        /// <summary>
        /// �����¼�������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        private void HandlingListeningEvents(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnChanllengeSuccess:
                    int level = (int)param[0];
                    if (m_LevelBtnDic.ContainsKey(level))
                    {
                        Button levelBtn = m_LevelBtnDic[level];
                        m_LevelBtnDic.Remove(level);
                        DestroyImmediate(levelBtn.gameObject);
                    }
                    break;
                default:
                    break;
            }
        }

        private void InitPanelInfo()
        {
            m_ChallengeBattleTitle.sprite = FindSprite("Challenge_Small" + m_CurChapterConfigInfo.clanType.ToString().ToLower());
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
            });
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            CloseSelfPanel();
        }

    }
}