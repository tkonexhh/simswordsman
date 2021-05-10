using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Qarth;
using Spine;

namespace GameWish.Game
{
    public enum AnimName
    {
        move = 0,
        idle
    }

    public class Frog : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> m_PointArray;
        [SerializeField]
        private Rigidbody2D m_Rigidbody2D;

        private Transform m_Target;
        private SkeletonAnimation m_SkeletonAnimation;
        private MeshRenderer m_MeshRenderer;
        private Vector3 m_Vector;

        private int m_ResidenceTime = 3;
        private float m_CheckRang = 1.2f;

        private void Awake()
        {
            m_SkeletonAnimation = GetComponent<SkeletonAnimation>();
            m_MeshRenderer = GetComponent<MeshRenderer>();
        }
        void Start()
        {
            m_SkeletonAnimation.state.Event += state_Event;

            RandomAnimation();
        }

        private void state_Event(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == "jump_start")
            {
                transform.DOMove(transform.position+m_Vector, 1f, false).SetEase(Ease.Linear);
            }
            else if (e.Data.Name == "jump_end")
            {
                transform.DOMove(transform.position, 1f, false).SetEase(Ease.Linear);
                //Debug.LogError("¾àÀë= "+ Vector2.Distance(m_Target.position, transform.position));
            }
        }

        private void RandomAnimation()
        {
            int aniIndex = UnityEngine.Random.Range(0, 2);

            switch ((AnimName)aniIndex)
            {
                case AnimName.move:
                    int index = UnityEngine.Random.Range(0, m_PointArray.Count);
                    m_Target = m_PointArray[index];
                    float dis = Vector2.Distance(m_Target.position, transform.position);
                    m_Vector = (m_Target.position - transform.position).normalized * 2;
                    MoveToTarget();
                    break;
                case AnimName.idle:
                    m_Target = null;
                    int timeRange = UnityEngine.Random.Range(1, m_ResidenceTime+1);
                    SetAnimationForTime(timeRange);
                    break;
                default:
                    break;
            }
        
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Disappear"))
            {
                m_MeshRenderer.enabled = true;
            }
            else if(collision.gameObject.name.Contains("FrogBound"))
            {
                //Destroy(gameObject);
                m_MeshRenderer.enabled = false;
                RandomAnimation();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Disappear"))
            {
                m_MeshRenderer.enabled = false;
            }
            else if (collision.gameObject.name.Contains("FrogBound"))
            {
                m_MeshRenderer.enabled = true;
            }
        }

        private void SetAnimationForTime(int time)
        {
            SpineHelper.PlayAnim(m_SkeletonAnimation, "idle", true);
            Timer.S.Post2Really((i) =>
            {
                if (i == time)
                {
                    RandomAnimation();
                }
            }, 1, time);
        }


        private void MoveToTarget()
        {
            SpineHelper.PlayAnim(m_SkeletonAnimation, "move", true, AnimationEnd);
            if (m_Target.position.x - transform.position.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
            //transform.DOMove(m_Target.position.normalized/**0.5f*/, 25f, false).SetEase(Ease.Linear);
        }

        private void AnimationEnd()
        {
            if (m_Target == null)
            {
                Log.e("Target is null");
                RandomAnimation();
                return;
            }
            if (Vector2.Distance(m_Target.position, transform.position) < m_CheckRang)
            {
                RandomAnimation();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }

}