using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class PracticeFieldController : FacilityController
    {
        public List<PracticeSlot> m_PracticeSlotList = new List<PracticeSlot>();

        private int m_MaxSlotCount = 6;

        public PracticeFieldController(FacilityType facilityType, int subId, FacilityView view) : base(facilityType, subId, view)
        {
            InitPracticeSlotList();
        }

        public PracticeSlot GetIdlePracticeSlot()
        {
            return m_PracticeSlotList.FirstOrDefault(i => i.IsEmpty());
        }

        private void InitPracticeSlotList()
        {
            PracticeFieldView view = (PracticeFieldView)m_View;
            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                PracticeSlot slot = new PracticeSlot(view.GetSlotPos(i));
                m_PracticeSlotList.Add(slot);
            }
        }
    }

}