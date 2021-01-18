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
        private GameObject m_Flag = null;

        public override FacilityController GenerateContoller()
        {
            return new PracticeFieldController(facilityType, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("PracticeField is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.PracticeFieldPanel, facilityType);
        }

        public Vector3 GetSlotPos(int index)
        {
            index = Mathf.Clamp(index, 0, m_PracticeSlots.Count - 1);
            return m_PracticeSlots[index].position;
        }

        public override void SetViewByState()
        {
            base.SetViewByState();

            m_Flag.SetActive(false);
            switch (m_Controller.GetState())
            {
                case FacilityState.Unlocked:
                    m_Flag.SetActive(true);
                    break;
            }
        }
    }

}