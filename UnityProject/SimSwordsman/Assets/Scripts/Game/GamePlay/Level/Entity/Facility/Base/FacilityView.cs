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
        public List<GameObject> stateObjList = new List<GameObject>();
        public GameObject navObstacleObj = null;
        public GameObject roadObj = null;
        public GameObject door = null;

        [SerializeField]
        protected FacilityType facilityType;
        protected FacilityController m_Controller = null;

        private ResLoader m_ResLoader;
        private int PlayParticleEffectsTime = 1;
        private GameObject m_ParticleEffects;

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
                case FacilityState.Unlocked:
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

        public virtual void SetViewByState(bool isFile = false)
        {
            stateLockedObj.SetActive(false);
            stateReadyToUnlockObj.SetActive(false);
            stateObjList.ForEach(i => i.SetActive(false));
            //state1Obj.SetActive(false);
            //state2Obj.SetActive(false);
            //state3Obj.SetActive(false);
            navObstacleObj?.SetActive(true);
            ShowRoad(false);

            switch (m_Controller.GetState())
            {
                case FacilityState.Locked:
                    stateLockedObj.SetActive(true);
                    break;
                case FacilityState.ReadyToUnlock:
                    stateReadyToUnlockObj.SetActive(true);
                    break;
                case FacilityState.Unlocked:
                    if (isFile)
                    {
                        RefreshStateObj();
                        ShowRoad(true);
                        return;
                    }

                    if (facilityType == FacilityType.Lobby || (int)facilityType == 20)
                    {
                        StartCoroutine(PlayParticleEffects(0));
                        return;
                    }
                    m_ResLoader = ResLoader.Allocate();
                    m_ParticleEffects = Instantiate(m_ResLoader.LoadSync("BuildSmokeHammer")) as GameObject;
                    m_ParticleEffects.transform.position = transform.position;
                    StartCoroutine(PlayParticleEffects(PlayParticleEffectsTime));
                    break;
            }
        }

        private IEnumerator PlayParticleEffects(int second)
        {
            yield return new WaitForSeconds(second);
            DestroyImmediate(m_ParticleEffects);
            RefreshStateObj();
            //navObstacleObj?.SetActive(true);

            //ReResLoader.Allocate();

            ShowRoad(true);
            m_ResLoader?.ReleaseRes("BuildSmokeHammer");
        }

        private void OnDestroy()
        {
        }

        public void RefreshStateObj()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_Controller.GetFacilityType());
            if (level < 1 || level > stateObjList.Count)
            {
                Log.e("Level is out of range");
            }
            else
            {
                stateObjList[level - 1].SetActive(true);
            }
        }

        public virtual void SetViewByLevel()
        {
            RefreshStateObj();
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