using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Qarth;

namespace GameWish.Game
{
    public class CharacterView : MonoBehaviour, IEntityView
    {
        //For debug
        public string state;
        public string battleState;

        private CharacterController m_Controller = null;
        private SkeletonAnimation m_SpineAnim;
        private PolyNavAgent m_NavAgent;
        private System.Action m_OnReachDestinationCallback = null;
        private bool m_IsMoving = false;
        [SerializeField]private GameObject m_Body = null;
        private Vector3 m_SimPos;
        private Vector3 m_DeltaPos = Vector3.zero;

        public void Init()
        {
            m_SpineAnim = GetComponentInChildren<SkeletonAnimation>();

            if(m_Body == null)
                m_Body = m_SpineAnim.gameObject;

            m_NavAgent = GetComponentInChildren<PolyNavAgent>();
            //m_NavAgent.enabled = false;
            m_NavAgent.OnDestinationReached += OnReach;

            m_SpineAnim.state.Event += HandleEvent;
        }

        private void HandleEvent(Spine.TrackEntry trackEntry, Spine.Event e)
        {
            //Debug.LogError("evnet name is: " + e.Data.Name);
            EventSystem.S.Send(EventID.OnBattleAtkEvent, m_Controller);
        }

        private void Update()
        {
            if (m_Controller.CurState != CharacterStateID.Battle)
            {
                if (m_IsMoving)
                {
                    int dir = m_NavAgent.Velocity.x > 0 ? -1 : 1;
                    FaceTo(dir);
                }
            }
            //For debug
#if UNITY_EDITOR
            state = m_Controller.CurState.ToString();
            battleState = ((CharacterStateBattle)(m_Controller.GetState(CharacterStateID.Battle))).CurState.ToString();
#endif
        }

        #region Public 

        public void SetController(CharacterController controller)
        {
            m_Controller = controller;
        }

        public void PlayIdleAnim()
        {
            SpineHelper.PlayAnim(m_SpineAnim, "idle", true, null);
        }

        public void PlayWalkAnim()
        {
            SpineHelper.PlayAnim(m_SpineAnim, "move", true, null);
        }

        public void PlayAnim(string animName, bool loop, System.Action callback)
        {
            SpineHelper.PlayAnim(m_SpineAnim, animName, loop, callback);
        }

        //public void PlayAtkedAnim(string animName, System.Action callback)
        //{
        //    SpineHelper.PlayAnim(m_SpineAnim, animName, false, callback);
        //}

        public void PlayDeadAnim()
        {
            SpineHelper.PlayAnim(m_SpineAnim, "hunting", false, null);
        }

        public void MoveTo(Vector2 targetPos, System.Action callback)
        {
            m_IsMoving = true;
            //m_NavAgent.enabled = true;
            PlayWalkAnim();

            m_OnReachDestinationCallback = callback;

            m_NavAgent.maxSpeed = m_Controller.CharacterModel.MoveSpeed;
            m_NavAgent.SetDestination(targetPos);
        }

        public void Move(Vector2 deltaPos)
        {
            m_DeltaPos.x = deltaPos.x;
            m_DeltaPos.y = deltaPos.y;
            m_DeltaPos.z = 0;
            transform.position += m_DeltaPos;
        }

        public void OnEnterBattleField(Vector3 pos, CharacterCamp camp)
        {
            m_SimPos = transform.position;
            transform.position = pos;
            m_NavAgent.enabled = false;

            if (camp == CharacterCamp.OurCamp)
            {
                FaceToRight();
            }
            else
            {
                FaceToLeft();
            }
        }

        public void OnExitBattleField()
        {
            transform.position = m_SimPos;
            m_NavAgent.enabled = true;
        }

        public void FaceToLeft()
        {
            FaceTo(1);
        }

        public void FaceToRight()
        {
            FaceTo(-1);
        }

        public int GetFaceDir()
        {
            return m_Body.transform.localScale.x < 0 ? 1 : -1;
        }
        #endregion

        #region Private
        private void OnReach()
        {
            m_IsMoving = false;
            //m_NavAgent.enabled = false;

            if (m_OnReachDestinationCallback != null)
            {
                m_OnReachDestinationCallback.Invoke();
            }
        }

        private void FaceTo(int dir)
        {
            m_Body.transform.localScale = new Vector3(dir, 1, 1);
        }
        #endregion
    }

}