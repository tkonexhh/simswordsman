using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * 
 * 权重随机
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
	public class RandomWeightHelper<T>
	{
		private Dictionary<T, int> m_WeightItemDic = new Dictionary<T, int>();
		private Dictionary<T, RandomRange> m_WeightItemSectionDic = new Dictionary<T, RandomRange>();

		private int m_AllWeight = 0;

		public RandomWeightHelper()
		{

		}

		#region Public
		/// <summary>
		/// 添加权重子项
		/// </summary>
		/// <param name="id"></param>
		/// <param name="weight"></param>
		public void AddWeightItem(T id, int weight)
		{
			if (!m_WeightItemDic.ContainsKey(id))
			{
				m_WeightItemDic.Add(id, weight);
			}
		}
		/// <summary>
		/// 获取随机key
		/// </summary>
		/// <returns></returns>
		public T GetRandomWeightValue()
		{
			if (m_WeightItemDic.Count > 0)
			{
				GetWeightItemSectionDic();
				int randomNumber = Random.Range(0, GetAllWeight());
				return GetRandomKey(randomNumber);
			}
			else
			{
				Debug.Log("随机池中无种子");
				return default;
			}
		}
		#endregion

		#region Private
		private int GetAllWeight()
		{
			int allWeight = 0;
			if (m_WeightItemDic.Count <= 0)
			{
				return 1;
			}

			foreach (var item in m_WeightItemDic)
			{
				allWeight += item.Value;
			}
			return allWeight;
		}

		private T GetRandomKey(int randomNumber)
		{
			T key = default;
			if (m_WeightItemSectionDic.Count <= 0)
			{
				Debug.Log("随机区域未空");
				return key;
			}
			foreach (var item in m_WeightItemSectionDic)
			{
				if (item.Value.Start <= randomNumber && item.Value.End > randomNumber)
				{
					key = item.Key;
				}
			}
			return key;
		}
		/// <summary>
		/// 获取权重区域
		/// </summary>
		private void GetWeightItemSectionDic()
		{
			List<T> keys = new List<T>();
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
		#endregion
	}
}