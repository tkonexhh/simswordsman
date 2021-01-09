using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskPos : MonoBehaviour
	{
        [SerializeField] private Transform m_ClanDoorPos;
        [SerializeField] private Transform m_FishingPos;
        [SerializeField] private Transform m_WellPos;
        [SerializeField] private List<Transform> m_PosList = new List<Transform>();

        public Vector2 GetTaskPos(CollectedObjType objType)
        {
            Vector2 pos = Vector2.zero;

            switch (objType)
            {
                case CollectedObjType.Fish:
                    pos = m_FishingPos.position;
                    break;
                case CollectedObjType.Bear:
                case CollectedObjType.Boar:
                case CollectedObjType.Chicken:
                case CollectedObjType.CloudRock:
                case CollectedObjType.Deer:
                case CollectedObjType.Ganoderma:
                case CollectedObjType.Iron:
                case CollectedObjType.QingRock:
                case CollectedObjType.SilverWood:
                case CollectedObjType.Snake:
                case CollectedObjType.Vine:
                case CollectedObjType.WuWood:
                    int index = Random.Range(0, m_PosList.Count);
                    pos = m_PosList[index].position;
                    break;
                case CollectedObjType.Well:
                    pos = m_WellPos.position;
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