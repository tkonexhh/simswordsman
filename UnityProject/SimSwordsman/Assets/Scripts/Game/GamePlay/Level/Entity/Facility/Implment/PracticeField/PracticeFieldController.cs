using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class PracticeFieldController : FacilityController
    {
        public List<BaseSlot> m_PracticeSlotList = new List<BaseSlot>();

        private int m_MaxSlotCount = 6;

        public PracticeFieldController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            InitPracticeSlotList();
        }

        public BaseSlot GetIdlePracticeSlot()
        {
            return m_PracticeSlotList.FirstOrDefault(i => i.IsEmpty());
        }

        private void InitPracticeSlotList()
        {
            PracticeFieldView view = (PracticeFieldView)m_View;
            for (int i = 0; i < m_MaxSlotCount; i++)
            {
                BaseSlot slot = new BaseSlot(view.GetSlotPos(i));
                m_PracticeSlotList.Add(slot);
            }
        }
    }
}