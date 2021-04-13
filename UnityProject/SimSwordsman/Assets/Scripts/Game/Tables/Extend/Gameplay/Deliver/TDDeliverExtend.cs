using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDDeliver
    {
        public void Reset()
        {

        }

        private int m_NormalRewadCount = 2;
        private int m_RareRewadCount = 1;

        public List<RewardBase> GetReward() 
        {
            List<RewardBase> rewardList = new List<RewardBase>();

            string[] totalNormalRewardList = SplitRewardStrList(normalReward, ';');
            string[] totalRareRewardList = SplitRewardStrList(rareReward, ';');

            if (totalNormalRewardList != null && totalNormalRewardList.Length >= m_NormalRewadCount) 
            {
                List<int> normalRandomIndexList = GetRandomIndex(totalNormalRewardList.Length, m_NormalRewadCount);

                for (int i = 0; i < normalRandomIndexList.Count; i++)
                {
                    int index = normalRandomIndexList[i];

                    RewardBase rewadBase = GetRewardBaseByRewardStr(totalNormalRewardList[index], '|');

                    rewardList.Add(rewadBase);
                }
            }

            if (totalRareRewardList != null && totalRareRewardList.Length >= m_RareRewadCount) 
            {
                int rareIndex = UnityEngine.Random.Range(0, totalRareRewardList.Length);
                RewardBase rareRewadBase = GetRewardBaseByRewardStr(totalRareRewardList[rareIndex], '|');
                rewardList.Add(rareRewadBase);
            }

            return rewardList;
        }
        private RewardBase GetRewardBaseByRewardStr(string str,char splitChar) 
        {
            try
            {
                string[] rewadList = SplitRewardStrList(str, splitChar);
                RewardItemType itemtype = EnumUtil.ConvertStringToEnum<RewardItemType>(rewadList[0]);
                if (rewadList.Length == 3)
                {
                    int rewardID = int.Parse(rewadList[1]);
                    int count = int.Parse(rewadList[2]);
                    return RewardMgr.S.GetRewardBase(itemtype, rewardID, count);
                }
                else if (rewadList.Length == 2)
                {
                    int count = int.Parse(rewadList[1]);
                    return RewardMgr.S.GetRewardBase(itemtype, 1, count);
                }

                return null;

            } catch (Exception ex) 
            {
                Debug.LogError("deliver data ´íÎó£º" + ex.ToString());
                return null;
            }
        }
        private List<int> GetRandomIndex(int totalLength,int needCount) 
        {
            List<int> resultList = new List<int>();
            List<int> tmpList = new List<int>();
            for (int i = 0; i < totalLength; i++)
            {
                tmpList.Add(i);
            }
            int tmpIndex = 0;
            if (totalLength >= needCount) 
            {
                for (int i = 0; i < needCount; i++)
                {
                    if (tmpList.Count <= 0) 
                    {
                        break;
                    }
                    tmpIndex = UnityEngine.Random.Range(0, tmpList.Count);
                    resultList.Add(tmpIndex);
                    tmpList.RemoveAt(tmpIndex);
                }
            }

            return resultList;
        }
        private string[] SplitRewardStrList(string str,char splitChar) 
        {
            string[] rewardStrList = str.Split(splitChar);

            return rewardStrList;
        }
    }
}