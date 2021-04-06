using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using DG.Tweening;

namespace GameWish.Game
{
	public class DeliverCar : MonoBehaviour
	{
		public int DeliverID = -1;

		public Dictionary<int, bool> CharacterIDDic = new Dictionary<int, bool>();

		private bool m_IsInit = false;

		public void Init(int deliverID,List<int> characterIDList,Vector3 startWorldPos) 
		{
			if (m_IsInit) return;
			
			m_IsInit = true;

			transform.position = startWorldPos;

			DeliverID = deliverID;

			CharacterIDDic.Clear();
            for (int i = 0; i < characterIDList.Count; i++)
            {
				CharacterIDDic.Add(characterIDList[i], false);
            }

			EventSystem.S.Register(EventID.OnCharacterReachDeliverPos, OnCharacterReachDeliverPosCallBack);
		}

        private void OnDisable()
        {
			m_IsInit = false;
			EventSystem.S.UnRegister(EventID.OnCharacterReachDeliverPos, OnCharacterReachDeliverPosCallBack);
		}

        private void OnCharacterReachDeliverPosCallBack(int key, object[] param)
        {
			if (param != null && param.Length > 0) 
			{
				int characterID = int.Parse(param[0].ToString());

				if (CharacterIDDic.ContainsKey(characterID)) 
				{
					CharacterIDDic[characterID] = true;

					StartMove();
				}
			}			
        }

        public void StartMove() 
		{
			if (IsAllCharacterReachTargetPos()) 
			{
				//移动
				transform.DOMove(new Vector3(20, 20, 0), 10).OnComplete(()=> 
				{
					//移动出屏幕后，回收
					GameObjectPoolMgr.S.Recycle(this.gameObject);
				});
			}
		}

		private bool IsAllCharacterReachTargetPos() 
		{
            foreach (var item in CharacterIDDic)
            {
				if (item.Value == false)
					return false;
            }

			return true;
		}
	}	
}