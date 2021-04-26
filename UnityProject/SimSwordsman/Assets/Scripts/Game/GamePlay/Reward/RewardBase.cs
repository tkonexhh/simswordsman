using System;
using UnityEngine;

namespace GameWish.Game
{
    public abstract class RewardBase
    {
        protected int? m_KeyID;
        public virtual int Count { get; protected set; }
        public int? KeyID => m_KeyID;

        public RewardItemType RewardItem;

        public RewardBase(RewardItemType type, int? id, int count)
        {
            RewardItem = type;
            m_KeyID = id;
            Count = count;
        }

        public void SetCount(int newCount)
        {
            Count = newCount;
        }

        public void SetKeyID(int keyID)
        {
            m_KeyID = keyID;
        }

        // public RewardBase

        public abstract string SpriteName();

        public abstract void AcceptReward(int bonus = 1);

        public abstract string RewardName();
        public abstract string RewardTips();

        public override string ToString()
        {
            return RewardItem + ":" + m_KeyID + ":" + Count;
        }
    }
}