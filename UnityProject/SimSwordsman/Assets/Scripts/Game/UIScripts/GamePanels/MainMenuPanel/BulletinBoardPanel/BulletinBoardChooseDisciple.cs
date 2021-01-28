using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum SelectedState
    {
        /// <summary>
        /// 选中
        /// </summary>
        Selected,
        /// <summary>
        /// 未选中
        /// </summary>
        NotSelected
    }

    public class BulletinBoardChooseDisciple : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Text m_ChoiceDiscipleTitle;
        [SerializeField]
        private Text m_RecommendedSkillsTitle;
        [SerializeField]
        private Text m_RecommendedSkillsValue;
        //[SerializeField]
        //private Text m_SelectedDiscipleSkillTitle;
        //[SerializeField]
        //private Text m_SelectedDiscipleSkillValue;
        //[SerializeField]
        //private Text m_State;
        //[SerializeField]
        //private Image m_StateBg;
        [SerializeField]
        private Transform m_Cont;
        [SerializeField]
        private GameObject m_ChoosePanelDisciple;
        [Header("Bottom")]
        [SerializeField]
        private Transform m_Bottom;
        [SerializeField]
        private GameObject m_ChooseSelectedDisciple;
        [SerializeField]
        private Button m_ConfirmBtn;
        [SerializeField]
        private Text m_ConfirmText;

        private SimGameTask m_CurTaskInfo;
        private CommonTaskItemInfo m_CommonTaskItemInfo;

        private List<CharacterItem> m_AllDiscipleList;

        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();

        private Dictionary<int, ChoosePanelDisciple> m_DiscipleObjDic = new Dictionary<int, ChoosePanelDisciple>();
        private List<ChooseSelectedDisciple> m_SelectedDiscipleObjList = new List<ChooseSelectedDisciple>();
        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
            EventSystem.S.Register(EventID.OnSelectedDiscipleEvent, HandAddListenerEvent);

            GetInformationForNeed();
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedDiscipleEvent:
                    HandSelectedDiscipleEvent((CharacterItem)param[0], (bool)param[1]);
                    break;
                default:
                    break;
            }
        }

        public void AddDiscipleDicDic(Dictionary<int, CharacterItem> keyValuePairs)
        {
            foreach (var item in keyValuePairs.Values)
                EventSystem.S.Send(EventID.OnSelectedDiscipleEvent, item, true);

            RefreshPanelInfo();
        }

        /// <summary>
        /// 处理选中弟子情况
        /// </summary>
        /// <param name="characterItem"></param>
        /// <param name="seleted"></param>
        private void HandSelectedDiscipleEvent(CharacterItem characterItem, bool seleted)
        {
            //选中
            if (seleted)
            {
                if (m_SelectedDiscipleDic.Count>= m_CommonTaskItemInfo.GetCharacterAmount())
                {
                    FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                    return;
                }

                if (!m_SelectedDiscipleDic.ContainsKey(characterItem.id))
                {
                    m_SelectedDiscipleDic.Add(characterItem.id, characterItem);
                    foreach (var item in m_SelectedDiscipleObjList)
                    {
                        if (item.GetSelelctedState() == SelectedState.NotSelected)
                        {
                            item.SetSelectedDisciple(characterItem, seleted);
                            break;
                        }
                    }
                }

                foreach (var item in m_DiscipleObjDic.Values)
                {
                    if (item.IsHavaSameDisciple(characterItem))
                        item.SetItemState(true);
                }
            }
            //选中取消
            else
            {
                if (m_SelectedDiscipleDic.ContainsKey(characterItem.id))
                {
                    m_SelectedDiscipleDic.Remove(characterItem.id);
                    foreach (var item in m_SelectedDiscipleObjList)
                    {
                        if (item.GetSelelctedState() == SelectedState.Selected && item.IsHavaSame(characterItem))
                        {
                            item.SetSelectedDisciple(null, seleted);
                            break;
                        } 
                    }
                }

                foreach (var item in m_DiscipleObjDic.Values)
                {
                    if (item.IsHavaSameDisciple(characterItem))
                        item.SetItemState(false);
                }
            }
        }

        private void RefreshFixedInfo()
        {
            m_ChoiceDiscipleTitle.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_SELECTEDDISCIPLEY);
            m_RecommendedSkillsTitle.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NEEDLEVEL);
            m_RecommendedSkillsValue.text = CommonUIMethod.GetGrade(m_CommonTaskItemInfo.characterLevelRequired);
            //m_SelectedDiscipleSkillTitle.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_SELECTEDDISCIPLEYSKILLS);
            //m_RecommendedSkillsValue.text = 
        }

        private void BindAddListenerEvent()
        {
            m_ConfirmBtn.onClick.AddListener(() =>
            {
                EventSystem.S.Send(EventID.OnSelectedConfirmEvent, m_SelectedDiscipleDic, m_CommonTaskItemInfo);
                HideSelfWithAnim();
            });
        }
        private void GetInformationForNeed()
        {
            m_AllDiscipleList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }

        private void CreateDisciple(CharacterItem characterItem)
        {
            GameObject obj = Instantiate(m_ChoosePanelDisciple, m_Cont);
            ChoosePanelDisciple itemICom = obj.GetComponent<ChoosePanelDisciple>();
            itemICom.OnInit(characterItem);
            m_DiscipleObjDic.Add(characterItem.id, itemICom);
        }

        private void CreateSelectedDisciple()
        {
            GameObject obj = Instantiate(m_ChooseSelectedDisciple, m_Bottom);
            ChooseSelectedDisciple itemICom = obj.GetComponent<ChooseSelectedDisciple>();
            itemICom.OnInit(this);
            m_SelectedDiscipleObjList.Add(itemICom);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurTaskInfo = args[0] as SimGameTask;
            //m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)args[1];
            m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;

            RefreshFixedInfo();

            for (int i = 0; i < m_AllDiscipleList.Count; i++)
            {
                if (m_AllDiscipleList[i].level >= m_CommonTaskItemInfo.characterLevelRequired && m_AllDiscipleList[i].IsFreeState())
                    CreateDisciple(m_AllDiscipleList[i]);
            }

            for (int i = 0; i < m_CommonTaskItemInfo.GetCharacterAmount(); i++)
                CreateSelectedDisciple();
        }
        private void RefreshPanelInfo()
        {
            foreach (var item in m_SelectedDiscipleDic.Values)
            {
                if (m_DiscipleObjDic.ContainsKey(item.id))
                    m_DiscipleObjDic[item.id].SetItemState(true);
            }
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedDiscipleEvent, HandAddListenerEvent);
        }
    }
}