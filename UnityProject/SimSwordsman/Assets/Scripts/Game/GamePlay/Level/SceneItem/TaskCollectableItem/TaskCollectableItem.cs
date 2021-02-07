using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskCollectableItem : MonoBehaviour
	{
        public CollectedObjType collectedObjType = CollectedObjType.None;
        public List<Transform> collectPos = new List<Transform>();

        protected SkeletonAnimation m_SpineAnim;
        protected bool m_HasSpine = false;

        private List<Transform> m_UsedCollectPos = new List<Transform>();

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
            m_UsedCollectPos.Clear();

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

            m_UsedCollectPos.Clear();
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

        public Transform GetRandomCollectPos()
        {
            List<Transform> list = new List<Transform>();
            foreach (Transform t in collectPos)
            {
                if (!m_UsedCollectPos.Contains(t))
                {
                    list.Add(t);
                }
            }

            if (list.Count > 0)
            {
                int index = Random.Range(0, list.Count);
                return list[index];
            }

            return collectPos[0];
        }

        public void OnCollectPosTaken(Transform t)
        {
            if(!m_UsedCollectPos.Contains(t))
                m_UsedCollectPos.Add(t);
        }

        public void ClearUsedPos()
        {
            m_UsedCollectPos.Clear();
        }
    }
	
}