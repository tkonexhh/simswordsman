using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class CharacterStateCD : CharacterState
    {
        protected virtual CharacterStateID targetState => CharacterStateID.Reading;
        protected virtual string animName => "write";
        protected CharacterController m_Controller = null;
        protected BaseSlot m_Slot;
        protected int m_TimerID = -1;
        protected float m_Progress = 0;

        public CharacterStateCD(CharacterStateID stateEnum) : base(stateEnum)
        {
        }

        public override void Enter(ICharacterStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_Slot = GetTargetSlot();
            if (m_Slot != null)
            {
                m_Slot.OnCharacterEnter(m_Controller);
                Vector3 practicePos = m_Slot.GetPosition();
                m_Controller.RunTo(practicePos, OnReachDestination);
            }
            else
            {
                Log.e("field slot not found");
                m_Controller.SetState(CharacterStateID.Wander);//Ã»ÓÐslot »¹ÊÇÈ¥ÏÐ¹ä ±ÜÃâ¿¨ËÀ
            }
        }

        public override void Exit(ICharacterStateHander handler)
        {
            if (m_TimerID != -1)
            {
                Timer.S.Cancel(m_TimerID);
                m_TimerID = -1;
            }

            if (m_Controller != null)
            {
                m_Controller.ReleaseWorkProgressBar();
            }

            m_Controller = null;

            if (m_Slot != null)
            {
                m_Slot.OnCharacterLeave();
                m_Slot = null;
            }
        }

        protected virtual BaseSlot GetTargetSlot()
        {
            return null;
        }

        protected void UpdateProgress()
        {
            if (m_Slot != null && m_Controller != null)
            {
                m_Progress = m_Slot.GetProgress();
                m_Controller.SetWorkProgressPercent(m_Progress);
                Debug.LogError("UpdateProgress:" + m_Progress);
                if (m_Progress >= 1)
                {
                    OnCDOver();
                    //²ÙÁ·½áÊø »Øµ½ÏÐ¹ä×´Ì¬
                    m_Controller.SetState(CharacterStateID.Wander);
                }
            }
        }

        protected virtual void OnCDOver()
        {

        }

        private void OnReachDestination()
        {
            if (m_Controller == null || m_Controller.CurState != targetState)
                return;

            m_Controller.CharacterView.PlayAnim(animName, true, null);
            m_Controller.SpawnWorkProgressBar();

            UpdateProgress();
            m_TimerID = Timer.S.Post2Really((x) =>
            {
                UpdateProgress();
            }, 1, -1);
        }
    }

}