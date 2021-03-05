using System;
using UnityEngine;

namespace GameWish.Game
{
    public delegate void GetSpriteCallBack(Sprite go);
    public abstract class RewardBase
    {
        protected int m_KeyID;
        public int Count { get; protected set; }
        public int KeyID => m_KeyID;

        //protected bool m_isInitSuccess = true;
        protected Action m_CallBackAction;

        public RewardItemType RewardItem;

        public RewardBase(RewardItemType type, int id, int count)
        {
            RewardItem = type;
            m_KeyID = id;
            Count = count;
        }

        public abstract string SpriteName();

        public abstract void AcceptReward();

        public abstract Sprite GetSprite();

        public abstract void GetSpriteAsyn(GetSpriteCallBack callBack);

        public abstract string RewardName();

        public abstract void SetCallBackAction(Action action);
    }
}