using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskPos : MonoBehaviour
	{
        [SerializeField] private Transform m_HuntingPos;
        [SerializeField] private Transform m_FishingPos;

        public Vector2 GetTaskPos(SimGameTaskType collectingTaskSubType)
        {
            Vector2 pos = Vector2.zero;

            switch (collectingTaskSubType)
            {
                case SimGameTaskType.Fish:
                    pos = m_FishingPos.position;
                    break;
                case SimGameTaskType.Hunt:
                    pos = m_HuntingPos.position;
                    break;
            }

            return pos;
        }
    }
	
}