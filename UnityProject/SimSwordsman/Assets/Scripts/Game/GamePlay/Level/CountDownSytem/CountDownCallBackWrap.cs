using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public delegate void OnCountDownCallBackDel();
    public class CountDownCallBackWrap
    {
        private LinkedList<OnCountDownCallBackDel> m_CallBackList = null;
        private OnCountDownCallBackDel m_TempCall = null;
        private LinkedListNode<OnCountDownCallBackDel> m_HeadNode = null;
        public bool Fire()
        {
            if (m_CallBackList == null)
            {
                return false;
            }
            m_TempCall = null;
            m_HeadNode = null;

            m_HeadNode = m_CallBackList.First;

            while (m_HeadNode != null)
            {
                m_TempCall = m_HeadNode.Value;

                m_HeadNode = m_HeadNode.Next;

                m_TempCall();
            }

            return true;
        }
        public bool Add(OnCountDownCallBackDel listener)
        {
            if (m_CallBackList == null)
            {
                m_CallBackList = new LinkedList<OnCountDownCallBackDel>();
            }

            if (m_CallBackList.Contains(listener))
            {
                return false;
            }

            m_CallBackList.AddFirst(listener);
            return true;
        }
        public void Remove(OnCountDownCallBackDel listener)
        {
            if (m_CallBackList == null)
            {
                return;
            }

            m_CallBackList.Remove(listener);
        }
        public void Clear()
        {
            m_CallBackList.Clear();
            m_TempCall = null;
            m_HeadNode = null;
        }
    }
}