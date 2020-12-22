using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class RewardHandlerBase<T> : IRewardHandler
	{
        protected int m_TaskId;
        protected string m_Target;
        protected T m_Value;

        public virtual void Init(int id, string target, T value)
        {
            m_TaskId = id;
            m_Target = target;

            m_Value = value;
        }

        public virtual void OnRewardClaimed()
        {

        }
    }
	
}