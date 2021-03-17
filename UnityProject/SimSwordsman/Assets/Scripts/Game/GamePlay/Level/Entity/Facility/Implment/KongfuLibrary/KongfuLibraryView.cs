using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class KongfuLibraryView : FacilityView
    {
        [SerializeField]
        private List<Transform> m_KongfuSlots = new List<Transform>();

        public override FacilityController GenerateContoller()
        {
            return new KongfuLibraryController(FacilityType.KongfuLibrary, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("KongfuLibrary is clicked");
        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.KongfuLibraryPanel, facilityType);
            DataAnalysisMgr.S.CustomEvent(DotDefine.facility_check, facilityType.ToString());
        }

        public override Vector3 GetSlotPos(int index)
        {
            index = Mathf.Clamp(index, 0, m_KongfuSlots.Count - 1);
            return m_KongfuSlots[index].position;
        }
    }

}