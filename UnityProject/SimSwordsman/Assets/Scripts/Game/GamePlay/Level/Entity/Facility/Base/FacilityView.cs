using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class FacilityView : MonoBehaviour, IEntityView, IFacilityClickedHandler
    {
        public GameObject stateLockedObj;
        public GameObject stateReadyToUnlockObj;
        public GameObject state1Obj;
        public GameObject state2Obj;
        public GameObject state3Obj;
        public GameObject navObstacleObj = null;
        public GameObject roadObj = null;
        public GameObject door = null;

        [SerializeField]
        protected FacilityType facilityType;
        protected FacilityController m_Controller = null;

        public void Init()
        {
        }

        public virtual void OnClicked()
        {
            switch (m_Controller.GetState())
            {
                case FacilityState.Locked:
                    break;
                case FacilityState.ReadyToUnlock:
                    UIMgr.S.OpenPanel(UIID.ConstructionFacilitiesPanel, m_Controller.GetFacilityType(), m_Controller.GetSubId());
                    break;
                case FacilityState.State1:
                case FacilityState.State2:
                case FacilityState.State3:
                    OpenUIElement();
                    break;
                default:
                    break;
            }
        }

        protected virtual void OpenUIElement()
        {
            
        }

        public virtual FacilityController GenerateContoller()
        {
            return null;
        }

        public virtual void SetViewByState()
        {
            stateLockedObj.SetActive(false);
            stateReadyToUnlockObj.SetActive(false);
            state1Obj.SetActive(false);
            state2Obj.SetActive(false);
            state3Obj.SetActive(false);
            navObstacleObj?.SetActive(false);
            ShowRoad(false);

            switch (m_Controller.GetState())
            {
                case FacilityState.Locked:
                    stateLockedObj.SetActive(true);
                    break;
                case FacilityState.ReadyToUnlock:
                    stateReadyToUnlockObj.SetActive(true);
                    break;
                case FacilityState.State1:
                    state1Obj.SetActive(true);
                    navObstacleObj?.SetActive(true);
                    ShowRoad(true);
                    break;
                case FacilityState.State2:
                    state2Obj.SetActive(true);
                    navObstacleObj?.SetActive(true);
                    ShowRoad(true);
                    break;
                case FacilityState.State3:
                    state3Obj.SetActive(true);
                    navObstacleObj?.SetActive(true);
                    ShowRoad(true);
                    break;
            }
        }

        public void SetController(FacilityController facilityController)
        {
            m_Controller = facilityController;
        }

        private void ShowRoad(bool show)
        {
            if (roadObj != null)
            {
                roadObj.SetActive(show);
            }
        }

        public Vector3 GetDoorPos()
        {
            if (door != null)
            {
                return door.transform.position;
            }

            return Vector3.zero;
        }
    }

}