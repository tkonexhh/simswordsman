using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class PracticeFieldView : FacilityView
    {
        [SerializeField]
        private List<Transform> m_PracticeSlots = new List<Transform>();
        [SerializeField]
        private List<Transform> m_FlagPos = new List<Transform>();
        [SerializeField]
        private GameObject m_Flag = null;
        [SerializeField] private GameObject m_ObjFast;



        public override FacilityController GenerateContoller()
        {
            return new PracticeFieldController(facilityType, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();
        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.PracticeFieldPanel, facilityType);
            DataAnalysisMgr.S.CustomEvent(DotDefine.facility_check, facilityType.ToString());
        }

        public override Vector3 GetSlotPos(int index)
        {
            index = Mathf.Clamp(index, 0, m_PracticeSlots.Count - 1);
            return m_PracticeSlots[index].position;
        }

        public override void SetViewByState(bool isFile = false)
        {
            base.SetViewByState(isFile);

            switch (m_Controller.GetState())
            {
                case FacilityState.Unlocked:
                    RefreshFlagPos();
                    break;
            }
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

            RefreshFlagPos();
        }

        public void RefreshFlagPos()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(facilityType);
            int index = Mathf.Clamp(level - 1, 0, m_FlagPos.Count - 1);
            m_Flag.transform.position = m_FlagPos[index].position;
        }
    }

}