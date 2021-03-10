using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
/**
 * 
 * Ȩ�����
 * 
 * 
 */
namespace GameWish.Game
{

	public class RandomRange
	{
		public int Start { set; get; }
		public int End { set; get; }
		public RandomRange(int start,int end) { Start = start; End = end; }
	}
	public class RandomWeightHelper 
	{
		private Dictionary<int, int> m_WeightItemDic = new Dictionary<int, int>();
		private Dictionary<int, RandomRange> m_WeightItemSectionDic = new Dictionary<int, RandomRange>();

		private int m_AllWeight = 0;

		public RandomWeightHelper()
		{

		}
		/// <summary>
		/// ���Ȩ������
		/// </summary>
		/// <param name="id"></param>
		/// <param name="weight"></param>
		public void AddWeightItem(int id, int weight)
		{
            if (!m_WeightItemDic.ContainsKey(id))
            {
				m_WeightItemDic.Add(id,weight);
			}
		}

		private int GetAllWeight()
		{
			int allWeight = 0;
            if (m_WeightItemDic.Count<=0)
            {
				return 1;
            }

            foreach (var item in m_WeightItemDic)
            {
				allWeight += item.Value;
			}
			return allWeight;
		}
		/// <summary>
		/// ��ȡ���key
		/// </summary>
		/// <returns></returns>
		public int GetRandomWeightValue()
		{
            if (m_WeightItemDic.Count>0)
            {
				GetWeightItemSectionDic();
				int randomNumber = Random.Range(0, GetAllWeight()+1);
				return GetRandomKey(randomNumber);
			}
            else
            {
				Debug.Log("�������������");
				return 0;
            }
		}

        private int GetRandomKey(int randomNumber)
        {
			int key = 0;
            if (m_WeightItemSectionDic.Count<=0)
            {
				Debug.Log("�������δ��");
				return key;
            }
            foreach (var item in m_WeightItemSectionDic)
            {
                if (item.Value.Start<= randomNumber && item.Value.End> randomNumber)
                {
					key =  item.Key;
				}
            }
			return key;
		}
		/// <summary>
		/// ��ȡȨ������
		/// </summary>
        private void GetWeightItemSectionDic()
		{
			List<int> keys = new List<int>();
			keys.AddRange(m_WeightItemDic.Keys);
			List<int> values = new List<int>();
			values.AddRange(m_WeightItemDic.Values);
			for (int i = 0; i < keys.Count; i++)
            {
				int preWight = 0;
				int curWight = 0;

                for (int j = 0; j < values.Count; j++)
                {
					curWight += values[j];
					preWight += values[j];
					if (i <= j)
					{
						preWight -= values[j];
						break;
					}
				}
				if (!m_WeightItemSectionDic.ContainsKey(keys[i]))
					m_WeightItemSectionDic.Add(keys[i], new RandomRange(preWight, curWight));

			}
		}

	}
	
}