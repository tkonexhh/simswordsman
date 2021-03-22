using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class FacilityCDController : FacilityController
    {
        public List<CDBaseSlot> m_SlotList = new List<CDBaseSlot>();

        public FacilityCDController(FacilityType facilityType, FacilityView view) : base(facilityType, view)
        {
            EventSystem.S.Register(EventID.DeleteDisciple, DeleteDisciple);
        }

        ~FacilityCDController()
        {
            EventSystem.S.UnRegister(EventID.DeleteDisciple, DeleteDisciple);
        }

        private void DeleteDisciple(int key, object[] param)
        {
            foreach (var item in m_SlotList)
                if (item.IsHaveSameCharacterItem((int)param[0]))
                    item.CDIsOver();
        }

        protected void Refesh()
        {
            RefreshExclamatoryMark(CheckSlotInfo());
        }

        private bool CheckSlotInfo()
        {
            foreach (var item in m_SlotList)
            {
                if (item.IsFree() && m_FacilityType == item.FacilityType)
                {
                    return true;
                }
            }
            return false;
        }

        public List<CDBaseSlot> GetSlotList()
        {
            return m_SlotList;
        }

        protected override bool CheckSubFunc()
        {
            if (m_FacilityState != FacilityState.Unlocked)
                return false;
            return CheckSlotInfo();
        }
    }

}