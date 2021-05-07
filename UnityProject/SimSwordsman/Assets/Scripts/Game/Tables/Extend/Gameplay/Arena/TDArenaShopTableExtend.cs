using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDArenaShopTable
    {
        private static List<ArenaShopItemInfo> m_ItemNormalLst = new List<ArenaShopItemInfo>();
        private static List<ArenaShopItemInfo> m_ItemGoodLst = new List<ArenaShopItemInfo>();
        private static List<ArenaShopItemInfo> m_ItemPerfectLst = new List<ArenaShopItemInfo>();
        private static Dictionary<int, ArenaShopItemInfo> m_ItemMap = new Dictionary<int, ArenaShopItemInfo>();
        static void CompleteRowAdd(TDArenaShop tdData)
        {
            ArenaShopItemInfo itemInfo = new ArenaShopItemInfo(tdData);
            switch (itemInfo.quality)
            {
                case TowerShopItemQuality.Normal:
                    m_ItemNormalLst.Add(itemInfo);
                    break;
                case TowerShopItemQuality.Good:
                    m_ItemGoodLst.Add(itemInfo);
                    break;
                case TowerShopItemQuality.Perfect:
                    m_ItemPerfectLst.Add(itemInfo);
                    break;
            }
            m_ItemMap.Add(itemInfo.id, itemInfo);

            itemInfo = null;
        }

        public static ArenaShopItemInfo GetShopItemInfoByID(int id)
        {
            ArenaShopItemInfo info;
            m_ItemMap.TryGetValue(id, out info);
            return info;
        }

        public static List<ArenaShopItemInfo> GetRandomShopItem()
        {
            //在每档奖池中分别随机抽取1、2、5件物品
            List<ArenaShopItemInfo> rewards = new List<ArenaShopItemInfo>();

            rewards.Add(m_ItemPerfectLst[RandomHelper.Range(0, m_ItemPerfectLst.Count)]);
            rewards.AddRange(ReservoirSampling(m_ItemGoodLst, 2));
            rewards.AddRange(ReservoirSampling(m_ItemNormalLst, 5));

            return rewards;
        }

        private static List<T> ReservoirSampling<T>(List<T> list, int m)
        {
            List<T> cache = new List<T>(m);
            for (int i = 0; i < m; i++)
            {
                cache.Add(list[i]);
            }
            int currentIndex;
            for (int i = m; i < list.Count; i++)
            {
                currentIndex = RandomHelper.Range(0, i + 1);
                if (currentIndex < m)
                {
                    cache[currentIndex] = list[i];
                }
            }
            return cache;
        }


    }

    public class ArenaShopItemInfo
    {
        public int id;
        public RewardBase reward;
        public TowerShopItemQuality quality;
        public int price;
        public bool buyed = false;

        public ArenaShopItemInfo(TDArenaShop tdData)
        {
            this.id = tdData.goodsID;
            this.price = tdData.price;
            this.reward = RewardMgr.S.GetRewardBase(tdData.goodsInfo);
            try
            {
                quality = (TowerShopItemQuality)Enum.Parse(typeof(TowerShopItemQuality), tdData.quaTag);
            }
            catch
            {
                Log.e("Parse TDArenaShop Error" + tdData.goodsID);
                quality = TowerShopItemQuality.Normal;
            }


        }
    }

}
