using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using System.Linq;

namespace GameWish.Game
{
    public class InfoRecordBase
    {
        public int ID { set; get; }
        public string time;

        public InfoRecordBase(int id,string time)
        {
            this.ID = id;
            this.time = time;
        }

        public T ToSubType<T>() where T : InfoRecordBase
        {
            return this as T;
        }
    }



    public class FixedPointRefreshHelper : TSingleton<FixedPointRefreshHelper>
	{
        public List<InfoRecordBase> m_InfoRecord = new List<InfoRecordBase>();

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();

        }

        ~FixedPointRefreshHelper()
        {
            //EventSystem.S.UnRegister(EventID.OnFixedPointRefreshEvent, HandAddListenerEvent);
        }

        public void OnInit(List<InfoRecordBase> infoRecords)
        {
            m_InfoRecord = infoRecords;
            //EventSystem.S.Register(EventID.OnFixedPointRefreshEvent,HandAddListenerEvent);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnFixedPointRefreshEvent:

                    break;
            }
        }

        public void Rigister<T>(InfoRecordBase infoRecord) where T : InfoRecordBase
        {
            if (m_InfoRecord.Any(i=>i.ID != infoRecord.ID))
                m_InfoRecord.Add(infoRecord);

            //foreach
        }


    }
}