using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	/// <summary>
	/// ïÚ³µÂ·¾¶
	/// </summary>
	public class DeliverPath : MonoBehaviour
	{
		public Transform path1;
		private List<Vector3> m_GoOutPath1List = new List<Vector3>();
		public List<Vector3> GoOutPath1List
        {
			get {
				if (m_GoOutPath1List == null || m_GoOutPath1List.Count <= 0) 
				{
                    for (int i = 0; i < path1.childCount; i++)
                    {
						Transform t = path1.transform.GetChild(i);
						m_GoOutPath1List.Add(t.position);
                    }
				}
				return m_GoOutPath1List;
			}
		}
		private List<Vector3> m_ComeBackPath1List = new List<Vector3>();
		public List<Vector3> ComeBackPath1List
        {
			get {
				if (m_ComeBackPath1List == null || m_ComeBackPath1List.Count <= 0) {
					m_ComeBackPath1List.AddRange(GoOutPath1List);
					m_ComeBackPath1List.Reverse();
				}

				return m_ComeBackPath1List;
			}
		}
	}	
}