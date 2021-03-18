using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	//public class TaskCollectableChick : TaskCollectableItem
	//{
 //       private float m_MoveInterval = 2f;
 //       private float m_Time = 0;
 //       private PolyNavAgent m_NavAgent = null;
 //       private Vector3 m_InitPos;
 //       private bool m_StartCollected = false;

 //       public override void OnStartCollected(Vector3 collecterPos)
 //       {
 //           m_StartCollected = true;

 //           m_InitPos = transform.position;

 //           FaceTo(collecterPos.x);
 //       }

 //       public override void OnEndCollected()
 //       {
 //           RemoveItem();
 //       }

 //       private void Update()
 //       {
 //           if (m_StartCollected == false)
 //               return;

 //           m_Time += Time.deltaTime;
 //           if (m_Time >= m_MoveInterval)
 //           {
 //               m_Time = 0;

 //               Move();
 //           }
 //       }

 //       private void Move()
 //       {
 //           if (m_NavAgent == null)
 //           {
 //               m_NavAgent = GetComponent<PolyNavAgent>();
 //           }

 //           PlayAnim("run", true, null);
 //           Vector2 randomDelta = Random.insideUnitCircle * 1f;
 //           Vector2 pos = m_InitPos + new Vector3(randomDelta.x, randomDelta.y, 0);
 //           m_NavAgent.SetDestination(pos, (arrive)=> {
 //               PlayAnim("idle", true, null);
 //           });

 //           FaceTo(pos.x);
 //       }
 //   }
	
}