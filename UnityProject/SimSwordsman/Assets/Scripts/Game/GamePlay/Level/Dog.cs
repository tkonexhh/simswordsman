using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Qarth;
using Spine;
using Spine.Unity;

namespace GameWish.Game
{
    public enum DogAnimName
    {
        move = 0,
        idle,

    }

    public class Dog : MonoBehaviour
	{
        [SerializeField]
        private List<Transform> m_PointArray;

        private Transform m_Target;
        private SkeletonAnimation m_SkeletonAnimation;
        private Vector3 m_Vector;
 

        private int m_ResidenceTime = 3;
        private float m_CheckRang = 1.2f;
        private int m_TargetIndex = 1;

        private void Awake()
        {
            m_SkeletonAnimation = gameObject.GetComponentInChildren<SkeletonAnimation>();
        }
        void Start()
        {
            gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);

            RandomAnimation();
        }


        private void RandomAnimation()
        {
            int aniIndex = UnityEngine.Random.Range(0, 2);

            switch ((DogAnimName)aniIndex)
            {
                case DogAnimName.idle:
                    m_Target = null;
                    int timeRange = UnityEngine.Random.Range(1, m_ResidenceTime + 1);
                    SetAnimationForTime(timeRange);
                    break;
                case DogAnimName.move:

                    SpineHelper.PlayAnim(m_SkeletonAnimation, "move", true);
                    MoveToTarget();
                    break;
                default:
                    break;
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
            m_TargetIndex = m_TargetIndex == 0 ? 1 : 0;
            m_Target = m_PointArray[m_TargetIndex];

            if (m_Target.position.x - transform.position.x < 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else
                transform.localScale = new Vector3(-1f, 1f, 1f);

            transform.DOMove(m_Target.position, 4f, false)
                .SetEase(Ease.Linear)
                .OnComplete( RandomAnimation);

        }

        //private bool IsShortDis()
        //{
        //    if (Vector2.Distance(m_Target.position, transform.position) < m_CheckRang)
        //    {
        //        RandomAnimation();
        //        return false;
        //    }
        //    return true;
        //}
    }
	
}