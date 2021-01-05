using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class PracticeFieldController : FacilityController
    {
        public List<FacilitySlot> m_PracticeSlotList = new List<FacilitySlot>();

        private int m_MaxSlotCount = 6;

        public PracticeFieldController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            InitPracticeSlotList();
        }

        public FacilitySlot GetIdlePracticeSlot()
        {
            return m_PracticeSlotList.FirstOrDefault(i => i.IsEmpty());
        }

        private void InitPracticeSlotList()
        {
            PracticeFieldView view = (PracticeFieldView)m_View;
            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                FacilitySlot slot = new FacilitySlot(view.GetSlotPos(i));
                m_PracticeSlotList.Add(slot);
            }
        }
    }

}