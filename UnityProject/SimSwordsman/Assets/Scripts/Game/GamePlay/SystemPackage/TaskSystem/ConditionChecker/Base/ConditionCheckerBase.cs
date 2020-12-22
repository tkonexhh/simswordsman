using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ConditionCheckerBase<T> : IConditionChecker
    {
        protected int m_TaskId;
        //protected int m_TaskSubId;
        protected T m_TargetValue;

        public virtual void Init(int id, T targetValue)
        {
            m_TaskId = id;
            //m_TaskSubId = subId;
            m_TargetValue = targetValue;
        }

        public virtual string GetCurrentValue()
        {
            return string.Empty;
        }

        public virtual string GetTargetValue()
        {
            return m_TargetValue.ToString();
        }

        public virtual bool IsFinished()
        {
            return false;
        }

        public virtual float GetProgressPercent()
        {
            return float.Parse(GetCurrentValue())/(float.Parse(GetTargetValue()));
        }
    }

}