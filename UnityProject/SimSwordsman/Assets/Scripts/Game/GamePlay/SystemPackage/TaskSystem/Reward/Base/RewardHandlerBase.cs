using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class RewardHandlerBase : IRewardHandler
	{
        protected int m_TaskId;

        public virtual void Init(int id)
        {
            m_TaskId = id;
        }

        public virtual void OnRewardClaimed()
        {

        }
    }
	
}