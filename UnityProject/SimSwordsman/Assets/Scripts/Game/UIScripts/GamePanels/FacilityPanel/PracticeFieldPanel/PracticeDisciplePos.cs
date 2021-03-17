using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PracticeDisciplePos : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Text m_PracticePos;
        [SerializeField]
        private Text m_Time;
        [SerializeField]
        private Text m_CurPractice;
        [SerializeField]
        private Image m_PracticeImg;
        [SerializeField]
        private Image m_DiscipleHead;
        [SerializeField]
        private Text m_ArrangeDisciple;
        [SerializeField]
        private Text m_State;
        [SerializeField]
        private Button m_PracticeBtn;
        private FacilityType m_CurFacilityType;
        private int m_CurLevel;
        private int m_CountDown = 0;
        private PracticeFieldPanel m_PracticeFieldPanel;

        private PracticeField m_PracticeFieldInfo = null;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurFacilityType = (FacilityType)obj[0];

            m_PracticeFieldInfo = t as PracticeField;
            m_PracticeFieldPanel = obj[1] as PracticeFieldPanel;
            RefreshFixedInfo();
            BindAddListenEvent();
            m_PracticePos.text = "练功位:" + m_PracticeFieldInfo.Index;
            RefreshPracticeFieldState();
        }

        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }

        private void BindAddListenEvent()
        {
            m_PracticeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (CheckIsEmpty())
                    UIMgr.S.OpenPanel(UIID.ChooseDisciplePanel, m_PracticeFieldInfo, m_CurFacilityType, m_CurLevel);
                else
                {
                    FloatMessage.S.ShowMsg("暂时没有空闲的弟子，等会儿再试试吧");
                }

            });
        }

        private bool CheckIsEmpty()
        {
            bool isEntry = false;
            List<CharacterItem> characterItems = MainGameMgr.S.CharacterMgr.GetAllCharacterList();

            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            int maxLevel = TDFacilityLobbyTable.GetPracticeLevelMax(lobbyLevel);
            for (int i = 0; i < characterItems.Count; i++)
            {
                if (characterItems[i].IsFreeState() && characterItems[i].level < maxLevel)
                    isEntry = true;
            }
            return isEntry;
        }

        public SlotState GetPracticeFieldState()
        {
            return m_PracticeFieldInfo.slotState;
        }

        public void IncreaseCountDown(int time)
        {
            CountDownItem countDown = null;
            countDown = TimeUpdateMgr.S.IsHavaITimeObserver(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index);
            if (countDown != null)
                countDown.IncreasTickTime(time);
        }

        private void RefreshFixedInfo()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
        }

        public void RefreshPracticeFieldState()
        {
            switch (m_PracticeFieldInfo.slotState)
            {
                case SlotState.None:
                    break;
                case SlotState.Free:
                    m_PracticeBtn.enabled = true;
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_CurPractice.text = Define.COMMON_DEFAULT_STR;
                    m_Time.enabled = false;
                    //m_PracticeImg.sprite = ""
                    m_ArrangeDisciple.text = "安排弟子";
                    m_State.text = "空闲";
                    break;
                case SlotState.NotUnlocked:
                    m_PracticeBtn.enabled = false;
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_State.text = "练功场" + m_PracticeFieldInfo.UnlockLevel + "级后解锁";
                    m_PracticeImg.sprite = m_PracticeFieldPanel.FindSprite("Lock2");
                    m_Time.enabled = false;
                    m_CurPractice.text = Define.COMMON_DEFAULT_STR;
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    break;
                case SlotState.Practice:
                    m_Time.enabled = true;
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_PracticeBtn.enabled = false;
                    m_State.text = Define.COMMON_DEFAULT_STR;
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    RefreshFixedInfo();
                    m_CurPractice.text = "当前训练:" + m_PracticeFieldInfo.CharacterItem.name;
                    m_Time.text = GameExtensions.SplicingTime(GetDuration());
                    CreateCountDown();
                    LoadClanPrefabs(GetLoadDiscipleName(m_PracticeFieldInfo.CharacterItem));
                    //TimeRemaining(m_PracticeFieldInfo.StartTime);
                    break;
                default:
                    break;
            }
        }

        private void LoadClanPrefabs(string prefabsName)
        {
            m_DiscipleHead.sprite = m_PracticeFieldPanel.FindSprite(prefabsName);
        }

        private void CreateCountDown()
        {
            CountDownItem countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index);
            if (countDownMgr == null)
            {
                m_CountDown = GetDuration();
                countDownMgr = new CountDownItem(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index, m_CountDown);
            }
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            countDownMgr.OnSecondRefreshEvent = OnRefresAction;
            if (countDownMgr.OnCountDownOverEvent == null)
                countDownMgr.OnCountDownOverEvent = m_PracticeFieldInfo.overAction;
        }

        private int GetDuration()
        {
            return m_PracticeFieldInfo.GetDurationTime();
        }

        public void OnRefresAction(string obj)
        {
            if (m_Time != null)
                m_Time.text = obj;
        }

        public void SetButtonEvent(Action<object> action)
        {
            //throw new NotImplementedException();
        }
    }
}