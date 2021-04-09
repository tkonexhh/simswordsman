using DG.Tweening;
using Qarth;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class DeliverCar : MonoBehaviour
	{
        #region �ֶ�
		/// <summary>
		/// ����ID
		/// </summary>
        public int m_DeliverID = -1;
		/// <summary>
		/// ��ɫ���Ｏ�ϵ�
		/// Key:��ɫid
		/// value:�Ƿ񵽴Ｏ�ϵ�
		/// </summary>
		public Dictionary<int, bool> CharacterReachGatherPointDic = new Dictionary<int, bool>();
		/// <summary>
		/// ���ϵ��б�
		/// </summary>
		public List<Transform> GatherPointList = new List<Transform>();
		[SerializeField]
		private SkeletonAnimation m_SpineAnim;
		private bool m_IsMoving = false;
		private bool m_IsInit = false;
        private int m_PathIndex = 0;
		private float m_MoveSpeed = 1.5f;
		/// <summary>
		/// ��ǰ�ڳ���ʻ·��
		/// </summary>
		private List<Vector3> m_CurrentPathList = new List<Vector3>();
		private DeliverCarDir m_DeliverCarDir = DeliverCarDir.GoOut;
		/// <summary>
		/// �ڳ��ƶ�����
		/// </summary>
		public Vector3 MoveDir
        {
			get 
			{
				if (m_PathIndex < m_CurrentPathList.Count)
					return (transform.position - m_CurrentPathList[m_PathIndex]).normalized;
				else
					return Vector3.zero;
			}
		}
		/// <summary>
		/// �ڳ��Ƿ������ƶ�
		/// </summary>
		public bool IsMoving
        {
			get {
				return m_IsMoving;
			}
		}
		public int DeliverID
        {
			get {
				return m_DeliverID;
			}
		}
        #endregion

        #region public
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="deliverID">����ID</param>
        /// <param name="characterIDList">��ɫID</param>
        /// <param name="startWorldPos">�ڳ���ʼλ��</param>
        public void Init(int deliverID, List<int> characterIDList, Vector3 startWorldPos)
		{
			if (m_IsInit) return;

			m_IsInit = true;

			m_IsMoving = false;

			m_PathIndex = 0;

			m_DeliverCarDir = DeliverCarDir.GoOut;

			transform.position = startWorldPos;

			m_DeliverID = deliverID;

			CharacterReachGatherPointDic.Clear();
			for (int i = 0; i < characterIDList.Count; i++)
			{
				CharacterReachGatherPointDic.Add(characterIDList[i], false);
			}

			EventSystem.S.Register(EventID.OnCharacterReachDeliverCarGatherPoint, OnCharacterReachDeliverCarGatherPointCallBack);

			UpdateDeliverCarAnimation(true);
		}
		/// <summary>
		/// ��ʼ�������
		/// </summary>
		public void StartMoveGoOut()
		{
			//if (IsAllCharacterReachGatherPoint() && m_IsMoving == false)
			//{
				m_IsMoving = true;

				m_DeliverCarDir = DeliverCarDir.GoOut;

				m_CurrentPathList = DeliverSystemMgr.S.DeliverPath.GoOutPath1List;

				EventSystem.S.Send(EventID.OnDeliverCarStartGoOut, DeliverID);

				Move();
			//}
		}
		/// <summary>
		/// �ڳ���ʼ����
		/// </summary>
		public void StartMoveComeBack()
		{
			m_PathIndex = 0;
			m_IsMoving = true;
			m_DeliverCarDir = DeliverCarDir.ComeBack;
			m_CurrentPathList = DeliverSystemMgr.S.DeliverPath.ComeBackPath1List;

			EventSystem.S.Send(EventID.OnDeliverCarStartComeBack, DeliverID);

			Move();
		}
		/// <summary>
		/// ��ȡ���ϵ�λ��
		/// </summary>
		/// <param name="characterID">��ɫID</param>
		/// <returns></returns>
		public Vector3 GetGatherPointPosition(int characterID)
		{
			int index = 0;
			foreach (var item in CharacterReachGatherPointDic)
			{
				if (item.Key == characterID)
				{
					break;
				}
				index++;
			}

			if (index >= 0 && index < GatherPointList.Count)
			{
				return GatherPointList[index].position;
			}

			return GatherPointList[0].position;
		}
		#endregion

		#region private
		private void Move() 
		{
			if (m_PathIndex >= m_CurrentPathList.Count) 
			{
				m_IsMoving = false;

				if (m_DeliverCarDir == DeliverCarDir.ComeBack) 
				{
					EventSystem.S.Send(EventID.OnDeliverCarArrive, DeliverID);

					OnRecycle();
				}
				return;
			}
			UpdateDeliverCarAnimation(false);
			float duration = Vector2.Distance(m_CurrentPathList[m_PathIndex], transform.position) / m_MoveSpeed;
			transform.DOMove(m_CurrentPathList[m_PathIndex], duration).SetEase(Ease.Linear).OnComplete(()=>
			{
				m_PathIndex++;

				Move();
			});
        }
		/// <summary>
		/// �����ڳ�spine����
		/// </summary>
		/// <param name="isIdle">�Ƿ񲥷�Idle����</param>
		private void UpdateDeliverCarAnimation(bool isIdle) 
		{
			if (isIdle) 
			{
				m_SpineAnim.transform.localScale = new Vector3(-1, 1, 1); 
				SpineHelper.PlayAnim(m_SpineAnim, "positive_idle", true, null);
				return;
			}
			float valueDown = Vector2.Angle(new Vector2(1, 1), MoveDir);
			float valueUp = Vector2.Angle(new Vector2(-1, -1), MoveDir);
			float valueRight = Vector2.Angle(new Vector2(-1, 1), MoveDir);
			float valueLeft = Vector2.Angle(new Vector2(1, -1), MoveDir);

			if (valueDown < valueUp && valueDown < valueRight && valueDown < valueLeft)
			{
				//Down
				m_SpineAnim.transform.localScale = new Vector3(1, 1, 1);
				SpineHelper.PlayAnim(m_SpineAnim, "positive_move", true, null);
			}
			else if (valueUp < valueDown && valueUp < valueRight && valueUp < valueLeft)
			{
				//up
				m_SpineAnim.transform.localScale = new Vector3(1, 1, 1);
				SpineHelper.PlayAnim(m_SpineAnim, "negative_move", true, null);
			}
			else if (valueRight < valueDown && valueRight < valueUp && valueRight < valueLeft)
			{
				//right
				m_SpineAnim.transform.localScale = new Vector3(-1, 1, 1);
				SpineHelper.PlayAnim(m_SpineAnim, "positive_move", true, null);
			}
			else {
				//left
				m_SpineAnim.transform.localScale = new Vector3(-1, 1, 1);
				SpineHelper.PlayAnim(m_SpineAnim, "negative_move", true, null);
			}
		}
		/// <summary>
		/// ����
		/// </summary>
        private void OnRecycle()
        {
			m_IsInit = false;

			EventSystem.S.UnRegister(EventID.OnCharacterReachDeliverCarGatherPoint, OnCharacterReachDeliverCarGatherPointCallBack);

			DeliverSystemMgr.S.RemoveDeliverCar(DeliverID);

			GameObjectPoolMgr.S.Recycle(this.gameObject);
		}
        private void OnCharacterReachDeliverCarGatherPointCallBack(int key, object[] param)
        {
			if (param != null && param.Length > 0) 
			{
				int characterID = int.Parse(param[0].ToString());

				if (CharacterReachGatherPointDic.ContainsKey(characterID)) 
				{
					CharacterReachGatherPointDic[characterID] = true;

					//StartMoveGoOutSide();

					if (IsAllCharacterReachGatherPoint() && m_IsMoving == false) 
					{
						DeliverSystemMgr.S.AddDeliverCarGoOut(this);
					}
				}
			}			
        }
		/// <summary>
		/// �Ƿ����еĽ�ɫ�������˼��ϵ�
		/// </summary>
		/// <returns></returns>
		private bool IsAllCharacterReachGatherPoint() 
		{
            foreach (var item in CharacterReachGatherPointDic)
            {
				if (item.Value == false)
					return false;
            }

			return true;
		}
        #endregion
    }

	public enum DeliverCarDir {
		/// <summary>
		/// ��������
		/// </summary>
		GoOut,
		/// <summary>
		/// ���������
		/// </summary>
		ComeBack,
	}
}