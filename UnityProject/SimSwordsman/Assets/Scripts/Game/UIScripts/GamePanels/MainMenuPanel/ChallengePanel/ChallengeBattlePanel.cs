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
        [SerializeField]
        private Image m_ChallengeBattleTitle;

        [SerializeField]
        private Button m_CloseBtn;


        [SerializeField]
        private Transform m_Parent;
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
            LoadClanPrefabs(m_CurChapterConfigInfo.clanType.ToString()+"Panel");
        }

        public void LoadClanPrefabs(string prefabsName)
        {
            m_ResLoader = ResLoader.Allocate();

            GameObject obj = Instantiate(m_ResLoader.LoadSync(prefabsName)) as GameObject;

            //GameObject obj = resLoader.LoadSync(prefabsName) as GameObject;
            //m_CharacterLoaderDic.Add(id, loader);
            obj.transform.SetParent(m_Parent);
            RectTransform rectTransform = ((RectTransform)obj.transform);
            rectTransform.SetSiblingIndex(0);
            //记录
            rectTransform.localPosition = Vector3.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;

            obj.transform.localScale = new Vector3(1.01f, 1.01f, 1);
            ClanBase _ClanBase = obj.GetComponent<ClanBase>();
            _ClanBase.SetPanelInfo(m_CurChapterConfigInfo, m_CurChapterAllLevelConfigInfo);
            _ClanBase.UpdateScrollRectValue();
        }

        protected override void OnClose()
        {
            base.OnClose();
            m_ResLoader?.ReleaseRes(m_CurChapterConfigInfo.clanType.ToString() + "Panel");
        }

        /// <summary>
        /// 处理事件机函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        private void HandlingListeningEvents(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnChanllengeSuccess:
                    int  level = (int)param[0];
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
            m_CloseBtn.onClick.AddListener(()=>{
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
            });
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            CloseSelfPanel();
        }
    }
}