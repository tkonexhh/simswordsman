using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskCollectableItem : MonoBehaviour
	{
        public CollectedObjType collectedObjType = CollectedObjType.None;

        protected SkeletonAnimation m_SpineAnim;
        protected bool m_HasSpine = false;

        private void Start()
        {
            m_SpineAnim = GetComponentInChildren<SkeletonAnimation>();
            if (m_SpineAnim != null)
            {
                m_HasSpine = true;

                PlayAnim("idle", true, null);
            }

        }

        public virtual void OnStartCollected(Vector3 collecterPos)
        {
            if (m_HasSpine)
            {
                PlayAnim("attack", true, null);

                FaceTo(collecterPos.x);
            }
        }

        public virtual void OnEndCollected()
        {
            if (m_HasSpine)
            {
                PlayAnim("death", false, ()=> 
                {
                    RemoveItem();
                });
            }
            else
            {
                RemoveItem();
            }
        }

        protected void RemoveItem()
        {
            MainGameMgr.S.CommonTaskMgr.RemoveTaskCollectableItem(collectedObjType);
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