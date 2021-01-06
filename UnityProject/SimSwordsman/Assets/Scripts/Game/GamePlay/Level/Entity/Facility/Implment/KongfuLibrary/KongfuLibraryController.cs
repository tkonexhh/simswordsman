using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class KongfuLibraryController : FacilityController
    {
        public List<FacilitySlot> m_ReadingSlotList = new List<FacilitySlot>();

        private int m_MaxSlotCount = 6;

        public KongfuLibraryController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            InitPracticeSlotList();
        }

        public FacilitySlot GetIdlePracticeSlot()
        {
            return m_ReadingSlotList.FirstOrDefault(i => i.IsEmpty());
        }

        private void InitPracticeSlotList()
        {
            KongfuLibraryView view = (KongfuLibraryView)m_View;
            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                FacilitySlot slot = new FacilitySlot(view.GetSlotPos(i));
                m_ReadingSlotList.Add(slot);
            }
        }
    }

}