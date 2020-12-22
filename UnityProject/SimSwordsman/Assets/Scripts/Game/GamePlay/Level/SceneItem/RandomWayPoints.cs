using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
	public class RandomWayPoints : MonoBehaviour
	{
        public List<Transform> wayPointList = new List<Transform>();

        public Vector3 GetRandomWayPointPos(Vector3 exceptPos)
        {
            List<Transform> exceptPositionList = wayPointList.Where(i => Vector3.Distance(i.position, exceptPos) < 0.5f).ToList();
            List<Transform> list = new List<Transform>(wayPointList);
            list = list.Except(exceptPositionList).ToList();
            Vector3 pos = list[Random.Range(0, list.Count)].position;
            return pos;
        }
	}
	
}