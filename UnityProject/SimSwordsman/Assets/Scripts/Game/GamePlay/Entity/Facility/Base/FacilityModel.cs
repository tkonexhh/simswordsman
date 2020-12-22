using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class FacilityModel : IEntityData
    {
        protected int m_Level = 1;
        private FacilityController m_Controller = null;
        private FacilityItemDbData m_DbItem = null;

        public FacilityModel(FacilityController controller, FacilityItemDbData facilityItemDb)
        {
            m_Controller = controller;
            m_DbItem = facilityItemDb;
        }

        public void Init()
        {
        }
    }

}