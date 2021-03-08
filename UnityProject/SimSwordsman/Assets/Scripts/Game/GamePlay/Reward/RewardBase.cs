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
        }

        // public RewardBase

        public abstract string SpriteName();

        public abstract void AcceptReward();

        public abstract string RewardName();
    }
}