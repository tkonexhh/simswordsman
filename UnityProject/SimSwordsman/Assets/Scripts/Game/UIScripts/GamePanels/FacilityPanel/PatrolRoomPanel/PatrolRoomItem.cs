using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PatrolRoomItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Text m_PatrolRoomPos;
        [SerializeField]
        private Text m_CurPatrolRoom;
        [SerializeField]
        private Image m_DiscipleImg;
        [SerializeField]
        private Button m_PatrolRoomBtn;

        private int m_CountDown = 0;
        private int m_CurLevel;
        private FacilityType m_CurFacility;
        private PatrolRoomSlot m_PatrolRoomSlot = null;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            BindAddListenerEvent();
            m_CurFacility = (FacilityType)obj[0];
            m_PatrolRoomSlot = t as PatrolRoomSlot;
            m_PatrolRoomPos.text = "练功位:" + m_PatrolRoomSlot.Index;
            RefreshPracticeFieldState();
        }

        public SlotState GetPracticeFieldState()
        {
            return m_PatrolRoomSlot.slotState;
        }
        public void IncreaseCountDown(int time)
        {
            CountDownItem countDown = null;
            countDown = TimeUpdateMgr.S.IsHavaITimeObserver(m_PatrolRoomSlot.FacilityType.ToString() + m_PatrolRoomSlot.Index);
            if (countDown != null)
                countDown.IncreasTickTime(time);
        }
        private void BindAddListenerEvent()
        {
            m_PatrolRoomBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.PatrolRoomChooseDisciplePanel, m_PatrolRoomSlot, m_CurFacility);
            });
        }

        public void SetButtonEvent(Action<object> action)
        {
        }

        public void RefreshPracticeFieldState()
        {

            switch (m_PatrolRoomSlot.slotState)
            {
                case SlotState.None:
                    break;
                case SlotState.Free:
                    m_PatrolRoomBtn.enabled = true;
                    m_CurPatrolRoom.text = "安排弟子";
                    break;
                case SlotState.NotUnlocked:
                    m_PatrolRoomBtn.enabled = false;
                    // m_CurPatrolRoom.text = "巡逻位" + m_PatrolRoomSlot.UnlockLevel + "级后解锁";
                    break;
                case SlotState.Busy:
                    RefreshFixedInfo();
                    m_CurPatrolRoom.text = m_PatrolRoomSlot.CharacterItem.name + "正在巡逻";
                    //(m_PracticeFieldInfo.StartTime);
                    m_PatrolRoomBtn.enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void RefreshFixedInfo()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacility);
        }
    }
}