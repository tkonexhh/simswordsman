using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class ForgeHouseView : FacilityView
    {
        public List<Transform> m_FlagPos = new List<Transform>();
        [SerializeField]
        public GameObject m_Flag = null;
        public override FacilityController GenerateContoller()
        {
            return new ForgeHouseController( FacilityType.ForgeHouse, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("ForgeHouse is clicked");

        }
        public override void SetTips(bool active)
        {
            base.SetTips(active);
            if (m_Controller.GetState() == FacilityState.Unlocked)
                m_Flag.SetActive(true);
        }
        public override void SetViewByLevel(bool isFile = false)
        {
            base.SetViewByLevel(isFile);
            m_Flag.SetActive(false);

            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(facilityType);
            int index = Mathf.Clamp(level - 1, 0, m_FlagPos.Count - 1);
            m_Flag.transform.position = m_FlagPos[index].position;
        }
        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.ForgeHousePanel,facilityType);
            DataAnalysisMgr.S.CustomEvent(DotDefine.facility_check, facilityType.ToString());
        }
    }

}