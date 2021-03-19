using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Qarth;
using Spine.Unity;

namespace GameWish.Game
{
    public class RawMatItemChick : RawMatItem
    {
        [SerializeField]
        private GameObject m_Body = null;

        private PolyNavAgent m_NavAgent = null;
        private SkeletonAnimation m_SpineAnim = null;
        private Vector3 m_InitPos;
        private float m_MoveInterval = 2f;
        private float m_Time = 0;

        private bool m_IsWorking = false;

        public override void OnInit()
        {
            m_Body.SetActive(false);

            base.OnInit();

            m_InitPos = m_Body.transform.position;
            m_IsWorking = false;
        }

        public override void OnStateChanged(RawMatStateID state)
        {
            base.OnStateChanged(state);

            if (state == RawMatStateID.Idle)
            {
                m_IsWorking = false;
                m_Body.SetActive(false);
            }
            else if (state == RawMatStateID.Working)
            {
                m_IsWorking = true;
                PlayAnim("idle", true, null);
            }
            else if (state == RawMatStateID.BubbleShowing)
            {
                m_Body.SetActive(true);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_IsWorking == false)
                return;

            m_Time += Time.deltaTime;
            if (m_Time >= m_MoveInterval)
            {
                m_Time = 0;

                Move();
            }
        }

        private void Move()
        {
            if (m_NavAgent == null)
            {
                m_NavAgent = GetComponent<PolyNavAgent>();
            }

            if (m_SpineAnim == null)
                m_SpineAnim = m_Body.GetComponent<SkeletonAnimation>();

            PlayAnim("run", true, null);
            Vector2 randomDelta = UnityEngine.Random.insideUnitCircle * 1f;
            Vector2 pos = m_InitPos + new Vector3(randomDelta.x, randomDelta.y, 0);
            m_NavAgent.SetDestination(pos, (arrive) =>
            {
                PlayAnim("idle", true, null);
            });

            FaceTo(pos.x);
        }

        protected void PlayAnim(string name, bool loop, System.Action onAnimEnd)
        {
            if (m_SpineAnim != null)
            {
                SpineHelper.PlayAnim(m_SpineAnim, name, loop, onAnimEnd);
            }
        }

        protected void FaceTo(float x)
        {
            if (x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

}