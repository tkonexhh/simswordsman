using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class CountDowntMgr : TSingleton<CountDowntMgr>
    {
        private int m_CountDownIndex = 0;

        private List<CountDownItemTest> m_CountDownList = new List<CountDownItemTest>();

        public CountDownItemTest SpawnCountDownItemTest(int totalTime, OnCountDownCallBackDel UpdateCallBack = null, OnCountDownCallBackDel EndCallBack = null)
        {
            CountDownItemTest item = ObjectPool<CountDownItemTest>.S.Allocate();

            item.Init(m_CountDownIndex++, totalTime, UpdateCallBack, EndCallBack);

            m_CountDownList.Add(item);

            return item;
        }

        public void StopCountDownItemTest(int countDownIndex)
        {
            CountDownItemTest item = m_CountDownList.Find(x => x.CountDownIndex == countDownIndex);

            if (item != null)
            {
                item.Stop();
                m_CountDownList.Remove(item);
            }
        }

        public CountDownItemTest GetCountDownItemByID(int countDownID)
        {
            CountDownItemTest item = m_CountDownList.Find(x => x.CountDownIndex == countDownID);
            return item;
        }
    }
}