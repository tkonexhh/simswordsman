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
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            EventSystem.S.Register(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.Register(EventID.OnCloseParentPanel, HandlingListeningEvents);
            m_CurChapterConfigInfo = args[0] as ChapterConfigInfo;
            m_CurChapterAllLevelConfigInfo = MainGameMgr.S.ChapterMgr.GetAllLevelConfigInfo(m_CurChapterConfigInfo.chapterId);
            InitPanelInfo();
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
                rectTransform.SetSiblingIndex(0);
                //��¼
                rectTransform.localPosition = Vector3.zero;
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.offsetMin = Vector2.zero;

                obj.transform.localScale = new Vector3(1.01f,1.01f,1);
                obj.GetComponent<ClanBase>().SetPanelInfo(m_CurChapterConfigInfo, m_CurChapterAllLevelConfigInfo);
            });
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
            m_ChallengeBattleTitle.text = CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType);
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(()=>{
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });
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