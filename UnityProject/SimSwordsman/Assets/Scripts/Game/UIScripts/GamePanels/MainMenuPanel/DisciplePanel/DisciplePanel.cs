using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum BgColorType
    {
        Black,
        White,
    }

    public class DisciplePanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Toggle m_AllTog;
        [SerializeField]
        private Image m_AllTogImg;
        [SerializeField]
        private Toggle m_NormalTog;
        [SerializeField]
        private Image m_NormalTogImg;
        [SerializeField]
        private Toggle m_GoodTog;
        [SerializeField]
        private Image m_GoodTogImg;
        [SerializeField]
        private Toggle m_PerfectTog;
        [SerializeField]
        private Image m_PerfectTogImg;
        [SerializeField]
        private Toggle m_HeroTog;
        [SerializeField]
        private Image m_HeroTogImg;


        [Header("Right")]
        [SerializeField]
        private Transform m_DiscipleTra;
        [SerializeField]
        private GameObject m_DiscipleItem;

        private List<CharacterItem> m_CharacterList = null;

        //private Dictionary<int, GameObject> m_DiscipleDic = new Dictionary<int, GameObject>();

        private List<DiscipleItem> m_DiscipleItemList = new List<DiscipleItem>();
        private List<DiscipleItem> m_DiscipleItemNullList = new List<DiscipleItem>();
        private const int CreateNumber = 8;
        #region 作弊使用
        [SerializeField]
        private Button m_Disciple;
        #endregion
        protected override void OnUIInit()
        {
            base.OnUIInit();

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
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            RegisterEvents();

            GetInformationForNeed();

            InitPanelInfo();
        }
        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            for (int i = 0; i < m_DiscipleItemList.Count; i++)
            {
                GameObjectPoolMgr.S.Recycle(m_DiscipleItemList[i].gameObject);
            }
            for (int i = 0; i < m_DiscipleItemNullList.Count; i++)
            {
                GameObjectPoolMgr.S.Recycle(m_DiscipleItemNullList[i].gameObject);
            }

            GameObjectPoolMgr.S.RemovePool("DiscipleList",false);

            UnregisterEvents();
        }

        protected override void BeforDestroy()
        {
            base.BeforDestroy();
   
        }

        /// <summary>
        /// 创建弟子
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="action"></param>
        /// <param name="characterItem"></param>
        private void CreateDisciple(int i, CharacterItem characterItem = null)
        {
            //双数为黑 单数为白
            if (m_DiscipleItem == null)
                return;

            GameObject disciple = GameObjectPoolMgr.S.Allocate("DiscipleList");
            disciple.transform.SetParent(m_DiscipleTra);
            disciple.transform.localPosition = Vector3.zero;
            disciple.transform.localScale = Vector3.one;
            //GameObject disciple = m_DisciplePoolt.Allocate();
            //disciple.transform.SetParent(m_DiscipleTra);
            //GameObject disciple = Instantiate(m_DiscipleItem, m_DiscipleTra);
            DiscipleItem discipleItem = disciple.GetComponent<DiscipleItem>();
            //if (characterItem==null)
            //    m_DiscipleDic.Add(characterItem.id, disciple);
            //else
            //    m_DiscipleDic.Add(-1, disciple);
            if (characterItem != null)
            {
                m_DiscipleItemList.Add(discipleItem);
            }
            else
            {
                discipleItem.gameObject.SetActive(false);
                m_DiscipleItemNullList.Add(discipleItem);
            }
            discipleItem.OnInit(characterItem, GetBgColorType(i));
        }


        private void InitPanelInfo()
        {
            GameObjectPoolMgr.S.AddPool("DiscipleList", m_DiscipleItem, -1, 8);
            RefreshFixedInfo();
            CommonUIMethod.BubbleSortForType(m_CharacterList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);

            for (int i = 0; i < m_CharacterList.Count; i++)
            {
                CreateDisciple(i, m_CharacterList[i]);
            }

            for (int i = 0; i < CreateNumber; i++)
                CreateDisciple(i);

            RefreshDiscipleNullBg();
        }

        private BgColorType GetBgColorType(int i)
        {
            if (i % 2 == 0)
            {
                return BgColorType.Black;
            }
            return BgColorType.White;
        }

        /// <summary>
        /// 刷新弟子线显示
        /// </summary>
        private void RefreshDiscipleBgColor()
        {
            int j = 0;
            for (int i = 0; i < m_DiscipleItemList.Count; i++)
            {
                if (m_DiscipleItemList[i].gameObject.activeInHierarchy)
                {
                    m_DiscipleItemList[i].RefreshBgColor(GetBgColorType(j));
                    j++;
                }
            }
            RefreshDiscipleNullBg();
        }

        private void RefreshDiscipleNullBg()
        {
            foreach (var item in m_DiscipleItemNullList)
            {
                item.gameObject.SetActive(false);
            }

            int surplus = CreateNumber - GetActiveInHierarchyNumber();
         
            if (surplus > 0)
            {
                for (int i = CommonMethod.IsEvenNumber(surplus) ? i = 0 : i = 1;  i < (surplus = CommonMethod.IsEvenNumber(surplus)? surplus: surplus+1); i++)
                {
                    m_DiscipleItemNullList[i].gameObject.SetActive(true);
                    m_DiscipleItemNullList[i].RefreshBgColor(GetBgColorType(i));
                }
            }
        }

        private int GetActiveInHierarchyNumber()
        {
            int i = 0;
            foreach (var item in m_DiscipleItemList)
            {
                if (item.gameObject.activeInHierarchy)
                {
                    i++;
                }
            }
            return i;
        }

        private void RefreshFixedInfo()
        {
            //m_AllValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_ALL);
            //m_CivilianValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_NORMAL);
            //m_EliteValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_GOOD);
            //m_GeniusValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_BTNVALUE_PREFECT);
            //m_HeroValue.text = "英雄";
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

            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            //m_BlackBtn.onClick.AddListener(() => {
            //    AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
            //    HideSelfWithAnim();
            //});
            m_AllTog.onValueChanged.AddListener((e) =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "All");

                foreach (var item in m_DiscipleItemList)
                    item.gameObject.SetActive(true);

                RefreshDiscipleBgColor();
                m_AllTogImg.gameObject.SetActive(true);
                m_NormalTogImg.gameObject.SetActive(false);
                m_GoodTogImg.gameObject.SetActive(false);
                m_PerfectTogImg.gameObject.SetActive(false);
                m_HeroTogImg.gameObject.SetActive(false);
            });
            m_NormalTog.onValueChanged.AddListener((e) =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "Civilian");

                SwitchDisciple(CharacterQuality.Normal);
            });
            m_GoodTog.onValueChanged.AddListener((e) =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "Elite");

                SwitchDisciple(CharacterQuality.Good);
            });
            m_PerfectTog.onValueChanged.AddListener((e) =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_switchlabel, "Genius");

                SwitchDisciple(CharacterQuality.Perfect);
            });
            m_HeroTog.onValueChanged.AddListener((e) =>
            {
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
            foreach (var item in m_DiscipleItemList)
            {
                if (item.GetCharacterItem().quality != characterQuality)
                    item.gameObject.SetActive(false);
                //GameObjectPoolMgr.S.Recycle("DiscipleList", item.gameObject);
                else
                {
                    item.gameObject.SetActive(true);
                }

                //    //item.gameObject.SetActive();
                //else
                //    //item.gameObject.SetActive(false);
            }
            //m_AllTogImg
            switch (characterQuality)
            {
                case CharacterQuality.Normal:
                    m_NormalTogImg.gameObject.SetActive(true);
                    m_GoodTogImg.gameObject.SetActive(false);
                    m_PerfectTogImg.gameObject.SetActive(false);
                    m_HeroTogImg.gameObject.SetActive(false);
                    break;
                case CharacterQuality.Good:
                    m_NormalTogImg.gameObject.SetActive(false);
                    m_GoodTogImg.gameObject.SetActive(true);
                    m_PerfectTogImg.gameObject.SetActive(false);
                    m_HeroTogImg.gameObject.SetActive(false);
                    break;
                case CharacterQuality.Perfect:
                    m_NormalTogImg.gameObject.SetActive(false);
                    m_GoodTogImg.gameObject.SetActive(false);
                    m_PerfectTogImg.gameObject.SetActive(true);
                    m_HeroTogImg.gameObject.SetActive(false);
                    break;
                case CharacterQuality.Hero:
                    m_NormalTogImg.gameObject.SetActive(false);
                    m_GoodTogImg.gameObject.SetActive(false);
                    m_PerfectTogImg.gameObject.SetActive(false);
                    m_HeroTogImg.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
            m_AllTogImg.gameObject.SetActive(false);
            RefreshDiscipleBgColor();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnRefreshDisciple, HandleEvent);
            EventSystem.S.Register(EventID.OnDiscipleReduce, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshDisciple, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnDiscipleReduce, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnRefreshDisciple:
                    int DiscipelId = (int)param[0];
                    for (int i = 0; i < m_DiscipleItemList.Count; i++)
                    {
                        if (m_DiscipleItemList[i].GetCharacterItem().id == DiscipelId)
                        {
                            DestroyImmediate(m_DiscipleItemList[i].gameObject);
                            m_DiscipleItemList.RemoveAt(i);
                        }
                    }
                    RefreshDiscipleBgColor();
                    break;
                case (int)EventID.OnDiscipleReduce:
                    int id = (int)param[0];
                    GameObject obj = null;
                    CharacterItem cha = null;
                    //foreach (var item in m_DiscipleDic.Keys)
                    //{
                    //    if (item.id == id)
                    //    {
                    //        obj = m_DiscipleDic[item];
                    //        cha = item;
                    //    }
                    //}
                    //m_DiscipleDic.Remove(cha);
                    DestroyImmediate(obj);
                    break;
            }
        }
    }
}