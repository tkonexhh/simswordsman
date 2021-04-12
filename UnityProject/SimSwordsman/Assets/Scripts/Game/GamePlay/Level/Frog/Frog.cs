using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Qarth;

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
		private SkeletonAnimation m_SkeletonAnimation;
		[SerializeField]
		private Transform[] m_PointArray;
		[SerializeField]
		private Rigidbody2D m_Rigidbody2D;
		[SerializeField]
		private int m_TimeRange;
		private Transform m_Target;

		// Start is called before the first frame update
		void Start()
	    {
			//SpineHelper.PlayAnim(m_SkeletonAnimation, "move", true, onAnimEnd);
			//m_Rigidbody2D.velocity = new Vector2();
			//transform.DOMove(m_PointArray[2].position, 5f, false).SetEase(Ease.Flash);
			//transform.DOMove(m_PointArray[2].position, 0.1f, false).SetEase(Ease.Flash);
			//int index = UnityEngine.Random.Range(0, m_PointArray.Length - 1);
			//Transform target = m_PointArray[index];
			//if (target.position.x - transform.position.x < 0)
			//	transform.localScale = new Vector3(1, 1, 1);
			//else
			//	transform.localScale = new Vector3(-1, 1, 1);
			////transform.DOMove(target.position, 5f, false).SetEase(Ease.Linear);
			//TimeRange = UnityEngine.Random.Range(5, 5);
			//SpineHelper.PlayAnim(m_SkeletonAnimation, "move", true, onAnimEnd);

			RandomAnimation();
			//SpineHelper.PlayAnim(m_SkeletonAnimation, aniName[aniIndex], true, onAnimEnd);
		}

		private void RandomAnimation()
		{
			int aniIndex = UnityEngine.Random.Range(0, 2);

			switch ((AnimName)aniIndex)
			{
				case AnimName.move:
					int index = UnityEngine.Random.Range(0, m_PointArray.Length);
					m_Target = m_PointArray[index];
					MoveToTarget();
					break;
				case AnimName.idle:
					m_TimeRange = UnityEngine.Random.Range(5, 5);
					SetAnimationForTime(m_TimeRange);
					break;
				default:
					break;
			}
		}


		private void SetAnimationForTime(int time)
		{
			SpineHelper.PlayAnim(m_SkeletonAnimation, "idle", true);
			Timer.S.Post2Really((i) => {
				if (i == time)
				{
					RandomAnimation();
				}
			}, 1, time);
		}
		

		private void MoveToTarget()
		{
			SpineHelper.PlayAnim(m_SkeletonAnimation, "move", true,AnimEnd);
			if (m_Target.position.x - transform.position.x < 0)
				transform.localScale = new Vector3(1, 1, 1);
			else
				transform.localScale = new Vector3(-1, 1, 1);
			transform.DOMove(m_Target.position, 25f, false).SetEase(Ease.Linear);
   //         if (Vector2.Distance(target.position, transform.position)<0.1f)
   //         {
			//	RandomAnimation(); 
			//}
		}

        private void AnimEnd()
        {
			if (Vector2.Distance(m_Target.position, transform.position) < 0.1f)
			{
				RandomAnimation();
			}
		}

        // Update is called once per frame
        void Update()
	    {

			//int index = UnityEngine.Random.Range(0, m_PointArray.Length - 1);
			//Transform target = m_PointArray[index];
   //         if (target.position.x-transform.position.x<0)
   //         {
			//	transform.localScale = new Vector3(1,1,1);
			//}
   //         else
   //         {
			//	transform.localScale = new Vector3(-1, 1, 1);
			//}
			//transform.DOMove(m_PointArray[2].position, 5f, false).SetEase(Ease.Flash);
			//transform.Translate(m_PointArray[3].localPosition);
		}
	}
	
}