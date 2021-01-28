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
        private Text m_ChallengeBattleTitle;

        [SerializeField]
        private Button m_CloseBtn;


        [SerializeField]
        private Transform m_Parent;
        //private List<Button> m_CheckpointBtns = null;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private Dictionary<int, LevelConfigInfo> m_CurChapterAllLevelConfigInfo = null;

        private Dictionary<int, Button> m_LevelBtnDic = new Dictionary<int, Button>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            //BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            EventSystem.S.Register(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.Register(EventID.OnCloseParentPanel, HandlingListeningEvents);
            m_CurChapterConfigInfo = args[0] as ChapterConfigInfo;
            m_CurChapterAllLevelConfigInfo = MainGameMgr.S.ChapterMgr.GetAllLevelConfigInfo(m_CurChapterConfigInfo.chapterId);
            //InitPanelInfo();
            LoadClanPrefabs(m_CurChapterConfigInfo.clanType.ToString());
        }

        public void LoadClanPrefabs(string prefabsName)
        {
            AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
            prefabsName = prefabsName + "Panel";
            loader.InstantiateAsync(prefabsName, (obj) =>
            {
                //m_CharacterLoaderDic.Add(id, loader);
                obj.transform.SetParent(m_Parent);
                RectTransform rectTransform =((RectTransform)obj.transform);
                //记录
                rectTransform.localPosition = Vector3.zero;
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.offsetMin = Vector2.zero;

                obj.transform.localScale = new Vector3(1,1,1);
                obj.GetComponent<ClanBase>().SetPanelInfo(m_CurChapterConfigInfo, m_CurChapterAllLevelConfigInfo);
            });
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
                case EventID.OnCloseParentPanel:
                    HideSelfWithAnim();
                    break;
                default:
                    break;
            }
        }

        private void InitPanelInfo()
        {
            //m_LevelConfigInfo = MainGameMgr.S.ChapterMgr.GetLevelInfo(m_CurChapterConfigInfo.chapterId);
            m_ChallengeBattleTitle.text = CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType);
            int CurLevel = MainGameMgr.S.ChapterMgr.GetLevelProgressLevelID(m_CurChapterConfigInfo.chapterId);

            foreach (var item in m_CurChapterAllLevelConfigInfo.Values)
            {
                if (item.level < CurLevel)
                    continue;
                //Transform chapterItem = Instantiate(m_CheckpointItem, m_CheckpointTrans).transform;
                //chapterItem.GetComponentInChildren<Text>().text = item.level.ToString();
                //Button challengeBtn = chapterItem.GetComponent<Button>();
                //if (!m_LevelBtnDic.ContainsKey(item.level))
                //    m_LevelBtnDic.Add(item.level, challengeBtn);
                //challengeBtn.onClick.AddListener(() =>
                //{
                //    UIMgr.S.OpenPanel(UIID.IdentifyChallengesPanel, m_CurChapterConfigInfo, item);
                //});
            }
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(()=>{
                HideSelfWithAnim();
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });

            foreach (var item in m_LevelBtnDic.Values)
            {
                item.onClick.AddListener(() =>
                {
                    UIMgr.S.OpenPanel(UIID.IdentifyChallengesPanel);
                });
            }
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandlingListeningEvents);
            CloseSelfPanel();
        }
    }

}