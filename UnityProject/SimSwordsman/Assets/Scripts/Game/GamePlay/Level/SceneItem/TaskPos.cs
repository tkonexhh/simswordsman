using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskPos : MonoBehaviour
	{
        [SerializeField] private Transform m_ClanDoorPos;
        [SerializeField] private Transform m_HuntingPos;
        [SerializeField] private Transform m_FishingPos;

        public Vector2 GetTaskPos(CollectedObjType objType)
        {
            Vector2 pos = Vector2.zero;

            switch (objType)
            {
                case CollectedObjType.Fish:
                    pos = m_FishingPos.position;
                    break;
                case CollectedObjType.Bear:
                    pos = m_HuntingPos.position;
                    break;
            }

            return pos;
        }

        public Vector2 GetDoorPos()
        {
            return new Vector2(m_ClanDoorPos.position.x, m_ClanDoorPos.position.y);
        }
    }
	
}