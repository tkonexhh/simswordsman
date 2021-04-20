using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class DisciplePanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Button m_BlackBtn;
        [SerializeField]
        private Button m_CloseBtn;

        [Header("Left")]
        [SerializeField]
        private Toggle m_AllTog;
        [SerializeField]
        private Text m_AllValue;
        [SerializeField]
        private Toggle m_CivilianTog;
        [SerializeField]
        private Text m_CivilianValue;
        [SerializeField]
        private Toggle m_EliteTog;
        [SerializeField]
        private Text m_EliteValue;
        [SerializeField]
        private Toggle m_GeniusTog;
        [SerializeField]
        private Text m_GeniusValue;
        [SerializeField]
        private Toggle m_HeroTog;
        [SerializeField]
        private Text m_HeroValue;

        [Header("Right")]
        [SerializeField]
        private Transform m_DiscipleContList;
        [SerializeField]
        private GameObject m_DiscipleItem;

        private List<CharacterItem> m_CharacterList = null;

        private Dictionary<CharacterItem, GameObject> m_DiscipleDic = new Dictionary<CharacterItem, GameObject>();

        #region 作弊使用
        [SerializeField]
        private Button m_Disciple;
        #endregion
        protected override void OnUIInit()
        {
            base.OnUIInit();

            GetInformationForNeed();

            InitPanelInfo();

            BindAddListenerEvevnt();
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            CloseSelfPanel();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            OpenDependPanel(EngineUI.MaskPanel,-1,null);
            RegisterEvents();
        }
        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            UnregisterEvents();
        }

        /// <summary>
        /// 创建弟子
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="action"></param>
        /// <param name="characterItem"></param>
        private void CreateDisciple(Transform parent, CharacterItem characterItem)
        {
            if (m_DiscipleItem == null)
                return;
            GameObject disciple =  Instantiate(m_DiscipleItem, parent);
            Disciple discipleItem = disciple.GetComponent<Disciple>();

            m_DiscipleDic.Add(characterItem, disciple);
            discipleItem.OnInit(characterItem,this);
            discipleItem.SetButtonEvent(AddListenerBtn);
        }

        /// <summary>
        /// 弟子按钮Btn回调
        /// </summary>
        /// <param name="obj"></param>
        private void AddListenerBtn(object obj)
        {
            UIMgr.S.OpenPanel(UIID.DicipleDetailsPanel, obj);
        }

        private void InitPanelInfo()
        {
            RefreshFixedInfo();
            CommonUIMethod.BubbleSortForType(m_CharacterList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);
            if (m_CharacterList != null)
                foreach (var item in m_CharacterList)
                    CreateDisciple(m_DiscipleContList, item);

            RefreshDiscipleLine();
        }

        /// <summary>
        /// 刷新弟子线显示
        /// </summary>
        private void RefreshDiscipleLine()
        {
            CommonUIMethod.BubbleSortForType(m_CharacterList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);
            int i = -1;
            foreach (var item in m_DiscipleDic.Values)
            {
                if (item.activeInHierarchy)
                {
                    i++;
                    item.GetComponent<Disciple>().SetShowLine(i%2);
                }
            }
        }

        private void RefreshFixedInfo()
        {
            m_AllValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_ALL);
            m_CivilianValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_NORMAL);
            m_EliteValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_GOOD);
            m_GeniusValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_PREFECT);
            m_HeroValue.text = "英雄";
        }

        private void GetInformationForNeed()
        {
            m_CharacterList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }

        private void BindAddListenerEvevnt()
        {

            if (PlatformHelper.isTestMode)
            {
                m_Disciple.onClick.AddListener(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        MainGameMgr.S.CharacterMgr.AddCharacterLevel(i, 20);
                    }
                });
            }

            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_BlackBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_AllTog.onValueChanged.AddListener((e)=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel,"All");

                foreach (var item in m_DiscipleDic.Values)
                    item.SetActive(true);

                RefreshDiscipleLine();
            });
            m_CivilianTog.onValueChanged.AddListener((e) => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "Civilian");

                SwitchDisciple(CharacterQuality.Normal);
            });
            m_EliteTog.onValueChanged.AddListener((e) => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "Elite");

                SwitchDisciple(CharacterQuality.Good);
            });
            m_GeniusTog.onValueChanged.AddListener((e) => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "Genius");

                SwitchDisciple(CharacterQuality.Perfect);
            });
            m_HeroTog.onValueChanged.AddListener((e) => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "Hero");

                SwitchDisciple(CharacterQuality.Hero);
            });
        }

        /// <summary>
        /// 根据选择切换弟子类型
        /// </summary>
        /// <param name="characterQuality"></param>
        private void SwitchDisciple(CharacterQuality characterQuality)
        {
            foreach (var item in m_DiscipleDic)
            {
                if (item.Key.quality == characterQuality)
                    item.Value.SetActive(true);
                else
                    item.Value.SetActive(false);
            }
            RefreshDiscipleLine();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnRefreshMainMenuPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnDiscipleReduce, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshMainMenuPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnDiscipleReduce, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnRefreshMainMenuPanel:
                    break;
                case (int)EventID.OnDiscipleReduce:
                    int id = (int)param[0];
                    GameObject obj = null;
                    CharacterItem cha = null;
                    foreach (var item in m_DiscipleDic.Keys)
                    {
                        if (item.id == id)
                        {
                            obj = m_DiscipleDic[item];
                            cha = item;
                        }
                    }
                    m_DiscipleDic.Remove(cha);
                    DestroyImmediate(obj);
                    break;
            }
        }
    }
}