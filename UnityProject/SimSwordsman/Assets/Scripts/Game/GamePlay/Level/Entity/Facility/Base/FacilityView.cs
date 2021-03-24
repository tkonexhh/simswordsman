using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameWish.Game
{
    public class FacilityView : MonoBehaviour, IEntityView, IClickedHandler
    {
        public GameObject stateLockedObj;
        public GameObject stateReadyToUnlockObj;
        public List<GameObject> stateObjList = new List<GameObject>();
        public GameObject navObstacleObj = null;
        public GameObject roadObj = null;
        public GameObject door = null;
        public GameObject tips;
        public Transform pos1 = null;
        public Transform pos2 = null;
        public GameObject m_SubItem = null;

        [SerializeField]
        protected FacilityType facilityType;
        protected FacilityController m_Controller = null;

        private ResLoader m_ResLoader;
        private int PlayParticleEffectsTime = 1;
        private GameObject m_ParticleEffects;

        public FacilityType FacilityType => facilityType;

        private void Awake()
        {
            Init();
        }
        public virtual void Init()
        {

        }

        public virtual Vector3 GetSlotPos(int index)
        {
            return Vector3.zero;
        }

        // Sequence m_TipsAnim;
        public void SetTips(bool active)
        {
            if ((int)facilityType == 20)
                return;
            switch (m_Controller.GetState())
            {
                case FacilityState.Locked:
                    active = false;
                    break;
                case FacilityState.ReadyToUnlock:
                    if (pos1 == null)
                        break;
                    tips.transform.position = pos1.position;
                    break;
                case FacilityState.Unlocked:
                    tips.transform.position = pos2.position;
                    break;
                default:
                    break;
            }
            if (facilityType == FacilityType.Lobby && m_Controller.GetState() == FacilityState.ReadyToUnlock)
                return;
            tips.transform.parent.gameObject.SetActive(active);
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
                    //¥Êµµº”‘ÿΩ¯¿¥
                    if (isFile)
                    {
                        if (m_SubItem != null)
                            m_SubItem.SetActive(true);
                        RefreshStateObj(false);
                        ShowRoad(true);
                        return;
                    }

                    if (facilityType == FacilityType.Lobby || (int)facilityType == 20)
                    {
                        StartCoroutine(PlayConsParticleEffects(0));
                        return;
                    }
                    if (m_SubItem != null)
                        m_SubItem.SetActive(false);
                    CreateEffects();
                    StartCoroutine(PlayConsParticleEffects(PlayParticleEffectsTime));
                    break;
            }
        }

        private void CreatesSpecialEffects()
        {

        }

        private IEnumerator PlayConsParticleEffects(int second)
        {
            yield return new WaitForSeconds(second);
            DestroyImmediate(m_ParticleEffects);
            RefreshStateObj(false);
            ShowRoad(true);
            if (m_SubItem != null)
                m_SubItem.SetActive(true);
            EventSystem.S.Send(EventID.OnRawMaterialChangeEvent);
            m_ResLoader?.ReleaseAllRes();
            //m_ResLoader?.ReleaseRes("BuildSmokeHammer");
        }
        private IEnumerator PlayUpGradeParticleEffects(int second, int level)
        {
            yield return new WaitForSeconds(second);
            DestroyImmediate(m_ParticleEffects);
            stateObjList[level - 1].SetActive(true);
            if (m_SubItem != null)
                m_SubItem.SetActive(true);
            m_ResLoader?.ReleaseAllRes();
            EventSystem.S.Send(EventID.OnRawMaterialChangeEvent);
        }
        private void OnDestroy()
        {
        }
        public void RefreshStateObj(bool isFile, bool isUpgrade = false)
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_Controller.GetFacilityType());
            if (level < 1 || level > stateObjList.Count)
            {
                Log.e("Level is out of range");
            }
            else
            {
                if (!isFile && isUpgrade)
                {
                    if (m_ParticleEffects != null)
                        return;

                    stateObjList.ForEach(i => i.SetActive(false));
                    if (m_SubItem != null)
                        m_SubItem.SetActive(false);
                    SetTips(false);
                    CreateEffects();
                    StartCoroutine(PlayUpGradeParticleEffects(PlayParticleEffectsTime, level));
                }
                else
                    stateObjList[level - 1].SetActive(true);
            }
        }

        private void CreateEffects()
        {
            m_ResLoader = ResLoader.Allocate();
            m_ParticleEffects = Instantiate(m_ResLoader.LoadSync("BuildSmokeHammer")) as GameObject;
            m_ParticleEffects.transform.position = transform.position;
        }

        public virtual void SetViewByLevel(bool isFile = false)
        {
            RefreshStateObj(isFile, true);
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