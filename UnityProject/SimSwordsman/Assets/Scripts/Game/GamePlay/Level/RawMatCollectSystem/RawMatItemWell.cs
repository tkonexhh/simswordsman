using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Qarth;
using Spine.Unity;

namespace GameWish.Game
{
    public class RawMatItemWell : RawMatItem
    {
        [SerializeField]
        private GameObject m_Body = null;

        private SkeletonAnimation m_SpineAnim = null;

        public override void OnInit()
        {
            base.OnInit();

            if (m_SpineAnim == null)
                m_SpineAnim = m_Body.GetComponent<SkeletonAnimation>();
        }

        public override void OnStateChanged(RawMatStateID state)
        {
            base.OnStateChanged(state);

            if (state == RawMatStateID.Idle)
            {
                m_SpineAnim.skeleton.SetToSetupPose();
                m_SpineAnim.AnimationState.ClearTracks();
            }
        }

        public override void OnCharacterArriveCollectPos()
        {
            PlayAnim("idle", true, null);
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