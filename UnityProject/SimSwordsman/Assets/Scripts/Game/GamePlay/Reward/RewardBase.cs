using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public delegate void GetSpriteCallBack(Sprite go);
    public abstract class RewardBase
    {
        private RewardItemType m_Type;
        public int m_KeyID;
        public int m_Count;

        protected bool m_isInitSuccess = true;
        protected Action m_CallBackAction;

        public RewardItemType Type { get => m_Type; }

        public RewardBase(RewardItemType type, int id, int count)
        {
            m_Type = type;
            m_KeyID = id;
            m_Count = count;
        }


        public abstract void AcceptReward();

        public abstract Sprite GetSprite();

        public abstract void GetSpriteAsyn(GetSpriteCallBack callBack);

        public abstract string RewardName();

        public abstract string RewardCount();

        public abstract void SetCallBackAction(Action action);
    }

}