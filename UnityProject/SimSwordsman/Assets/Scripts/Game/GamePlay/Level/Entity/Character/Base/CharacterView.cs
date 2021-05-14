using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Qarth;
using static Sdkbox.IAP;
using UnityEngine.Rendering;
using HedgehogTeam.EasyTouch;

namespace GameWish.Game
{
    public class CharacterView : MonoBehaviour, IEntityView,IInputObserver
    {
        //For debug
        public string state;
        public string battleState;
        public int deliverID;

        [SerializeField] private GameObject m_Body = null;
        [SerializeField] private GameObject m_HeadPos = null;
        [SerializeField] private BoneFollower m_BoneFollower_Foot; 
        [SerializeField] private GameObject m_Clean_DragSmoke;

        private CharacterController m_Controller = null;
        private SkeletonAnimation m_SpineAnim;
        private PolyNavAgent m_NavAgent;
        private System.Action m_OnReachDestinationCallback = null;
        private bool m_IsMoving = false;

        private Vector3 m_SimPos;
        private Vector3 m_DeltaPos = Vector3.zero;

        private BoxCollider2D m_Collider2D;

        private CharacterWorkProgressBar m_WorkProgressBar = null;
        private CharacterWorkTip m_WorkTip = null;

        public BoneFollower BoneFollower_Foot { get => m_BoneFollower_Foot; set => m_BoneFollower_Foot = value; }
        public GameObject Body  { get => m_Body; set => m_Body = value; }
        public GameObject HeadPos { get => m_HeadPos; set => m_HeadPos = value; }

        public CharacterWorkProgressBar WorkProgressBar { get => m_WorkProgressBar;}
        public PolyNavAgent NavAgent { get => m_NavAgent;}
        public GameObject Clean_DragSmoke { get => m_Clean_DragSmoke; set => m_Clean_DragSmoke = value; }

        public BoxCollider2D CharCollider2D { get => gameObject.GetComponent<BoxCollider2D>() != null ? gameObject.GetComponent<BoxCollider2D>() : null;  }

        public void Init()
        {
            m_SpineAnim = GetComponentInChildren<SkeletonAnimation>();

            AddTouch();

            if (m_Body == null)
                m_Body = m_SpineAnim.gameObject;

            if (gameObject.GetComponent<SortingGroup>() == null)
            {
                SortingGroup sortingGroup = gameObject.AddComponent<SortingGroup>();
                sortingGroup.sortingLayerName = "Facility";
                sortingGroup.sortingOrder = 5;
            }

            m_NavAgent = GetComponentInChildren<PolyNavAgent>();
            //m_NavAgent.enabled = false;
            m_NavAgent.OnDestinationReached += OnReach;

            SetSpineAnim();

            SetSweepingSmoke(false);
        }

        public void SetSpineAnim(bool reset = false)
        {
            if (m_SpineAnim == null || reset)
                m_SpineAnim = m_Body.GetComponent<SkeletonAnimation>();

            m_SpineAnim.Initialize(true);

            if (m_SpineAnim.state != null)
            {
                m_SpineAnim.state.Event += HandleEvent;
            }
            else
            {
                Log.e("Spine state is null: " + transform.name);
            }
        }

        public void SetSweepingSmoke(bool play)
        {
            m_Clean_DragSmoke?.SetActive(play);
        }

        public void SetSkin(int headId)
        {
            if (m_Controller.CharacterCamp == CharacterCamp.OurCamp)
            {
                string skinName = GetSkinName(headId);
                m_SpineAnim.skeleton.SetSkin(skinName);
            }
        }

        private string GetSkinName(int headId)
        {
            return "tou_" + headId;
        }

        private void HandleEvent(Spine.TrackEntry trackEntry, Spine.Event e)
        {
            // Debug.LogError("event name is: " + e.Data.Name + "-anim name is " + trackEntry.Animation.Name);
            EventSystem.S.Send(EventID.OnBattleAtkEvent, m_Controller, trackEntry.Animation.Name.ToString());
        }

        private void Update()
        {
            if (m_Controller == null)
                return;

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
            deliverID = m_Controller.GetDeliverID();
#endif
        }

        #region Public 

        public void SetController(CharacterController controller)
        {
            m_Controller = controller;
        }

        public bool HasAnim(string animName)
        {
            bool hasAnim = SpineHelper.HasAnimation(m_SpineAnim, animName);
            return hasAnim;
        }

        public void PlayIdleAnim()
        {
            //if (m_NavAgent.enabled)
            //{
            //    m_NavAgent.SetDestination(transform.position);
            //}
            m_SpineAnim.skeleton.SetToSetupPose();
            SpineHelper.PlayAnim(m_SpineAnim, "idle", true, null);
        }

        public void PlayWalkAnim()
        {
            m_SpineAnim.skeleton.SetToSetupPose();
            SpineHelper.PlayAnim(m_SpineAnim, "move", true, null);
        }

        public void PlayRunAnim()
        {
            m_SpineAnim.skeleton.SetToSetupPose();
            SpineHelper.PlayAnim(m_SpineAnim, "run", true, null);
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
            SpineHelper.PlayAnim(m_SpineAnim, "tumble", false, null);
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

        public void StopNavAgent() {
            m_NavAgent.Stop();
        }

        public void RunTo(Vector2 targetPos, System.Action callback)
        {
            m_IsMoving = true;

            PlayRunAnim();

            m_OnReachDestinationCallback = callback;

            m_NavAgent.maxSpeed = m_Controller.CharacterModel.MoveSpeed * 1.5f;
            m_NavAgent.SetDestination(targetPos);
        }

        public void FollowDeliver(Vector2 targetPos) 
        {
            m_IsMoving = true;

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

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
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

        //public void OnExitBattleField()
        //{
        //    transform.position = m_SimPos;
        //    m_NavAgent.enabled = true;
        //    m_NavAgent.SetDestination(m_SimPos);
        //}

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

        public void HideBody()
        {
            m_Body.SetActive(false);
        }

        public void ShowBody()
        {
            m_Body.SetActive(true);
        }

        public Vector3 GetTaskRewardBubblePos()
        {
            return transform.position + new Vector3(0, -0.04f, 0);
        }

        public Vector3 GetHeadPos()
        {
            if (m_HeadPos == null)
                return transform.position;

            return m_HeadPos.transform.position;
        }

        public void SetProgressBar(CharacterWorkProgressBar characterWorkProgressBar)
        {
            m_WorkProgressBar = characterWorkProgressBar;
        }

        public void SetProgressBarPrecent(float percent)
        {
            m_WorkProgressBar?.SetPercent(percent);
        }

        public void ReleaseProgressBar()
        {
            if (m_WorkProgressBar != null)
            {
                SetProgressBarPrecent(0);
                GameObjectPoolMgr.S.Recycle(m_WorkProgressBar.gameObject);
                m_WorkProgressBar = null;
            }
        }

        public void SetWorkTip(CharacterWorkTip workTip)
        {
            m_WorkTip = workTip;
        }

        public void ReleaseWorkTip()
        {
            if (m_WorkTip != null)
            {
                GameObjectPoolMgr.S.Recycle(m_WorkTip.gameObject);
                m_WorkTip = null;
            }
        }

        public string GetCurRuningAnimName()
        {
            Spine.TrackEntry trackEntry = m_SpineAnim.AnimationState.GetCurrent(0);
            string name = trackEntry.Animation.Name;

            return name;
        }

        public void AddTouch()
        {
            if (CharCollider2D == null) return;
            CharCollider2D.enabled = true;
            InputMgr.S.AddTouchObserver(this);
        }

        public void RemoveTouch()
        {
            if (CharCollider2D == null) return;
            CharCollider2D.enabled = false;
            InputMgr.S.RemoveTouchObserver(this);
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
                m_OnReachDestinationCallback = null;
            }
        }

        private void FaceTo(int dir)
        {
            m_Body.transform.localScale = new Vector3(dir, 1, 1);
        }

        #endregion

        #region Input

        public bool On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement()|| CharCollider2D == null)
                return false;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Bubble"));
            if (hit.collider != null && hit.collider == CharCollider2D)
            {
                //显示对话气泡
                WorldUIPanel.S?.ShowWorkText(m_Controller.CharacterView.transform, TDTalkTable.GetRangeWords(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel()));
                return true;
            }

            return false;
        }

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
            return 1;
        }
        #endregion

    }

}