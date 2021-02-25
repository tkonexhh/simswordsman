using Qarth;
using System;
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

        private List<FacilityType> m_HavaWorkingBubbleFacility = new List<FacilityType>();
        ~FacilityController()
        {
            EventSystem.S.UnRegister(EventID.OnAddRawMaterialEvent, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnSendWorkingBubbleFacility, HandleAddListenerEvent);
        }
        public FacilityController(FacilityType facilityType/*, int subId*/, FacilityView view)
        {
            EventSystem.S.Register(EventID.OnAddRawMaterialEvent, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnSendWorkingBubbleFacility, HandleAddListenerEvent);
            
            m_FacilityType = facilityType;
            //m_SubId = subId;

            m_View = view;
            m_View.SetController(this);

            FacilityItemDbData dbItem = GameDataMgr.S.GetClanData().GetFacilityItem(m_FacilityType/*, subId*/);
            m_Model = new FacilityModel(this, dbItem);

            SetState(dbItem.facilityState, true);

            if (m_FacilityState == FacilityState.Unlocked)
            {
                m_View.SetViewByLevel();
            }
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnAddRawMaterialEvent:
                    if (m_FacilityType == FacilityType.BulletinBoard)
                        break;
                    CheckUpgradeConditions();
                    break;
                case EventID.OnSendWorkingBubbleFacility:
                    if ((bool)param[1] && !m_HavaWorkingBubbleFacility.Contains((FacilityType)param[0]))
                        m_HavaWorkingBubbleFacility.Add((FacilityType)param[0]);
                    else if (!(bool)param[1] && m_HavaWorkingBubbleFacility.Contains((FacilityType)param[0]))
                        m_HavaWorkingBubbleFacility.Remove((FacilityType)param[0]);
                    break;
            }
        }

        private void CheckUpgradeConditions()
        {
            int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_FacilityType);
            FacilityLevelInfo facilityLevelInfo = null;
            switch (m_FacilityState)
            {
                case FacilityState.Locked:
                    break;
                case FacilityState.ReadyToUnlock:
                    facilityLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_FacilityType, curLevel);
                    break;
                case FacilityState.Unlocked:
                    facilityLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_FacilityType, curLevel + 1);
                    break;
                default:
                    break;
            }
           
            List<CostItem> costItems = null;

            if (facilityLevelInfo != null)
                costItems = facilityLevelInfo.GetUpgradeResCosts();
            if (facilityLevelInfo == null || costItems == null)
                return;
            
            if (CheackIsBuild(facilityLevelInfo, costItems) && !m_HavaWorkingBubbleFacility.Contains(m_FacilityType))
            {
                m_View.SetTips(true);
            }
            else
            {
                m_View.SetTips(false);
            }

        }

        private bool CheackIsBuild(FacilityLevelInfo facilityLevelInfo, List<CostItem> costItems)
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (facilityLevelInfo.GetUpgradeCondition() > lobbyLevel)
                return false;

            if (CheckPropIsEnough(facilityLevelInfo, costItems))
                return true;
            return false;
        }  
        private bool CheckPropIsEnough(FacilityLevelInfo facilityLevelInfo, List<CostItem> costItems)
        {

            for (int i = 0; i < costItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)costItems[i].itemId, costItems[i].value);
                if (!isHave)
                    return false;
            }
            bool isHaveCoin = GameDataMgr.S.GetPlayerData().CheckHaveCoin(facilityLevelInfo.upgradeCoinCost);
            if (isHaveCoin)
                return true;
            else
                return false;
        }
        public void SetState(FacilityState facilityState, bool isFile = false)
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