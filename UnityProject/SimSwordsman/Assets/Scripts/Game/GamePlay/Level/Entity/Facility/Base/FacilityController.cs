using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class FacilityController : IEntityController
    {
        protected FacilityView m_View = null;
        protected FacilityModel m_Model = null;

        protected FacilityType m_FacilityType;
        protected FacilityState m_FacilityState;
        protected int m_SubId; // TODO: delete

        #region IEntityController
        public virtual void Init()
        {

        }

        public virtual void Release()
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void Update()
        {
        }
        #endregion

        public FacilityController(FacilityType facilityType/*, int subId*/, FacilityView view)
        {
            m_FacilityType = facilityType;
            //m_SubId = subId;

            m_View = view;
            m_View.SetController(this);

            FacilityItemDbData dbItem = GameDataMgr.S.GetClanData().GetFacilityItem(m_FacilityType/*, subId*/);
            m_Model = new FacilityModel(this, dbItem);

            SetState(dbItem.facilityState,true);

            if (m_FacilityState == FacilityState.Unlocked)
            {
                m_View.SetViewByLevel();
            }
        }

        public void SetState(FacilityState facilityState,bool isFile = false)
        {
            m_FacilityState = facilityState;

            m_View?.SetViewByState(isFile);
        }

        public void OnUpgrade()
        {
            m_View.SetViewByLevel();
        }

        public FacilityState GetState()
        {
            return m_FacilityState;
        }

        public FacilityType GetFacilityType()
        {
            return m_FacilityType;
        }

        public int GetSubId()
        {
            return m_SubId;
        }

        public Vector3 GetDoorPos()
        {
            return m_View.GetDoorPos();
        }
    }

}