using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class DeliverView : FacilityView
	{
        public List<Transform> m_FlagPos = new List<Transform>();
        [SerializeField]
        public GameObject m_Flag = null;
        public override FacilityController GenerateContoller()
        {
            return new DeliverController(FacilityType.Deliver, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();
        }

        public override void SetTips(bool active)
        {
            base.SetTips(active);
            if (m_Controller.GetState() == FacilityState.Unlocked)
                m_Flag.SetActive(true);
        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.DeliverPanel);
        }


        public override void SetViewByLevel(bool isFile = false)
        {
            base.SetViewByLevel(isFile);
            m_Flag.SetActive(false);

            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(facilityType);
            int index = Mathf.Clamp(level - 1, 0, m_FlagPos.Count - 1);
            m_Flag.transform.position = m_FlagPos[index].position;
        }
        public override void SetViewByState(bool isFile = false)
        {
            base.SetViewByState(isFile);
        }
    }	
}