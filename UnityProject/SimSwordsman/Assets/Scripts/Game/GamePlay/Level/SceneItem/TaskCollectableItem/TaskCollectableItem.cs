using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskCollectableItem : MonoBehaviour
	{
        public CollectedObjType collectedObjType = CollectedObjType.None;

        public string animName;

        private SkeletonAnimation m_SpineAnim;

        private void Start()
        {
            m_SpineAnim = GetComponentInChildren<SkeletonAnimation>();
        }

        public void PlayAnim()
        {
            if (m_SpineAnim != null)
            {
                SpineHelper.PlayAnim(m_SpineAnim, animName, true, null);
            }
        }
	}
	
}