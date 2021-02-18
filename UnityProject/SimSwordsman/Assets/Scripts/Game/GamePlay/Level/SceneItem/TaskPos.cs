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
        [SerializeField] private Transform m_ChickPos;
        [SerializeField] private List<Transform> m_PosList = new List<Transform>();
        private List<Transform> m_UsePosList = new List<Transform>();

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
                case CollectedObjType.CloudRock:
                case CollectedObjType.Deer:
                case CollectedObjType.Ganoderma:
                case CollectedObjType.Iron:
                case CollectedObjType.QingRock:
                case CollectedObjType.SilverWood:
                case CollectedObjType.Snake:
                case CollectedObjType.Vine:
                case CollectedObjType.WuWood:
                    if (m_UsePosList.Count == m_PosList.Count)
                    {
                        m_UsePosList.Clear();
                    }

                    List<Transform> list = new List<Transform>();
                    list.AddRange(m_PosList);
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (m_UsePosList.Contains(list[i]))
                        {
                            list.RemoveAt(i);
                        }
                    }
                    int index = Random.Range(0, list.Count);
                    pos = list[index].position;

                    m_UsePosList.Add(list[index]);
                    break;
                case CollectedObjType.Well:
                    pos = m_WellPos.position;
                    break;
                case CollectedObjType.Chicken:
                    pos = m_ChickPos.position;
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