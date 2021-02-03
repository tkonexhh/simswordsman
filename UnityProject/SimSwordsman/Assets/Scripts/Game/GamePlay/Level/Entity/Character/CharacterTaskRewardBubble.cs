using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
	public class CharacterTaskRewardBubble : MonoBehaviour, IInputObserver
	{
        private Collider2D m_Collider;
        private CharacterController m_CharacterController = null;

        private void Awake()
        {
            InputMgr.S.AddTouchObserver(this);

            m_Collider = GetComponentInChildren<Collider2D>();
        }

        public void SetController(CharacterController controller)
        {
            m_CharacterController = controller;
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnCharacterTaskRewardClicked, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnCharacterTaskRewardClicked, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            if (m_CharacterController == null)
                return;

            if (key == (int)EventID.OnCharacterTaskRewardClicked)
            {
                int taskId = (int)param[0];
                if (taskId == m_CharacterController.CurTask?.TaskId)
                {
                    m_CharacterController.SetCurTask(null);
                    m_CharacterController.SetState(CharacterStateID.Wander);

                    GameObjectPoolMgr.S.Recycle(gameObject);
                }
            }
        }

        #region Input
        public bool On_Drag(Gesture gesture, bool isTouchStartFromUI)
        {
            return false;
        }

        public bool On_LongTap(Gesture gesture)
        {
            return false;
        }

        public bool On_Swipe(Gesture gesture)
        {
            return false;
        }

        public bool On_TouchDown(Gesture gesture)
        {
            return false;
        }

        public bool On_TouchUp(Gesture gesture)
        {
            return false;
        }

        public bool BlockInput()
        {
            return true;
        }

        public int GetSortingLayer()
        {
            return 200;
        }

        public bool On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return false;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 200, 1 << LayerMask.NameToLayer("Bubble"));
            if (hit.collider != null && hit.collider == m_Collider)
            {
                int taskId = m_CharacterController.CurTask.TaskId;
                MainGameMgr.S.CommonTaskMgr.ClaimReward(taskId);
                if (taskId != 9001 && taskId != 9002)
                    UIMgr.S.OpenTopPanel(UIID.RewardPanel, null, new List<RewardBase>() { RewardMgr.S.GetRewardBase(TDCommonTaskTable.GetData(taskId).reward) });

                EventSystem.S.Send(EventID.OnCharacterTaskRewardClicked, taskId);
                return true;
            }

            return false;
        }
        #endregion



    }

}