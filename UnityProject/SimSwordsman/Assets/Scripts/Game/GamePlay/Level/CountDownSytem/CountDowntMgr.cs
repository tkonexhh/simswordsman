using Qarth;
using System.Collections.Generic;


namespace GameWish.Game
{
    public class CountDowntMgr : TSingleton<CountDowntMgr>
    {
        private int m_CountDownIndex = 0;

        private List<CountDown> m_CountDownList = new List<CountDown>();

        public CountDown SpawnCountDownItemTest(int totalTime, OnCountDownCallBackDel UpdateCallBack = null, OnCountDownCallBackDel EndCallBack = null)
        {
            CountDown item = ObjectPool<CountDown>.S.Allocate();

            item.Init(m_CountDownIndex++, totalTime, UpdateCallBack, EndCallBack);

            m_CountDownList.Add(item);

            return item;
        }

        public void StopCountDownItemTest(int countDownIndex)
        {
            CountDown item = m_CountDownList.Find(x => x.CountDownIndex == countDownIndex);

            if (item != null)
            {
                item.Stop();
                m_CountDownList.Remove(item);
            }
        }

        public CountDown GetCountDownItemByID(int countDownID)
        {
            CountDown item = m_CountDownList.Find(x => x.CountDownIndex == countDownID);
            return item;
        }
    }
}
