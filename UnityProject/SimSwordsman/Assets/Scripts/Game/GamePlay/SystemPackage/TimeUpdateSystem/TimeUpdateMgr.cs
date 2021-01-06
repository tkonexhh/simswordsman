using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public class TimeUpdateMgr : TSingleton<TimeUpdateMgr>
    {
        private int m_TimerId = -1;
        private bool m_IsStart = false;

        private List<ITimeObserver> m_Observers = new List<ITimeObserver>();
        private List<ITimeObserver> m_FinishedObservers = new List<ITimeObserver>();

        public void AddObserver(ITimeObserver ob)
        {
            if (!m_Observers.Contains(ob))
            {
                m_Observers.Add(ob);

                ob.OnStart();
            }
            else
            {
                Log.w("This time observer has been added before");
            }
            if (!m_IsStart && m_Observers.Count>0)
                Start();
        }

        /// <summary>
        /// 检测当前有无
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public CountDownMgr IsHavaITimeObserver(string ob)
        {
            CountDownMgr countDownMgr = null;
            m_Observers.ForEach(i=> {
                CountDownMgr temp = (CountDownMgr)i;
                if (temp != null && temp.GetID() == ob)
                    countDownMgr = temp;
            });
            return countDownMgr;
        }

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
        }

        public void AddFinishedObservers(ITimeObserver ob)
        {
            if (!m_FinishedObservers.Contains(ob))
            {
                m_FinishedObservers.Add(ob);
            }
        }

        public void RemoveObserver(ITimeObserver ob)
        {
            if (m_Observers.Contains(ob))
            {
                m_Observers.Remove(ob);
            }
            else
            {
                Log.w("This time observer can't be found");
            }
        }

        public void Start()
        {
            //foreach (ITimeObserver ob in m_Observers)
            //{
            //    ob.OnStart();
            //}

            m_TimerId = Timer.S.Post2Really(Tick, 1, -1);
            m_IsStart = true;
        }

        public void End()
        {
            foreach (ITimeObserver ob in m_Observers)
            {
                ob.OnFinished();
            }
        }

        public void CanclePost2Really()
        {
            Timer.S.Cancel(m_TimerId);

        }

        public void Tick(int count)
        {
            if (m_Observers.Count == 0)
            {
                if (Timer.S.Cancel(m_TimerId))
                    m_IsStart = false;
            }

            foreach (ITimeObserver ob in m_Observers)
            {
                int interval = ob.GetTickInterval();
                if (count > 0 && count % interval == 0)
                {
                    ob.OnTick(count);
                }

                //if (ob.GetTotalSeconds() > 0 && ob.GetTickCount() >= ob.GetTotalSeconds())
                if (ob.GetTickCount() >= ob.GetTotalSeconds())
                {
                    if (!m_FinishedObservers.Contains(ob))
                    {
                        m_FinishedObservers.Add(ob);
                    }
                }
            }

            // Remove finised observer
            if (m_FinishedObservers.Count > 0)
            {
                foreach (ITimeObserver ob in m_FinishedObservers)
                {
                    if (m_Observers.Contains(ob))
                    {
                        ob.OnFinished();
                        m_Observers.Remove(ob);
                    }
                }

                m_FinishedObservers.Clear();
            }
            EventSystem.S.Send(EventID.OnTimeRefresh);
        }

        public void Pause()
        {
            foreach (ITimeObserver ob in m_Observers)
            {
                ob.OnPause();
            }
        }

        public void Resume()
        {
            foreach (ITimeObserver ob in m_Observers)
            {
                ob.OnResume();
            }
        }
    }
}