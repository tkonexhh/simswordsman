using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class CharacterStatePractice : CharacterStateCD
    {
        protected override CharacterStateID targetState => CharacterStateID.Practice;
        protected override string animName => "practice";

        public CharacterStatePractice(CharacterStateID stateEnum) : base(stateEnum) { }

        protected override BaseSlot GetTargetSlot()
        {
            FacilityType facilityType = m_Controller.CharacterModel.GetTargetFacilityType();
            if (facilityType != FacilityType.PracticeFieldEast && facilityType != FacilityType.PracticeFieldWest)
            {
                Log.e("facilityType not right: " + facilityType.ToString());
                return null;
            }

            PracticeFieldController practiceFieldController = MainGameMgr.S.FacilityMgr.GetFacilityController(facilityType) as PracticeFieldController;
            // return practiceFieldController.GetIdlePracticeSlot(facilityType);
            return practiceFieldController.GetSlotByIndex(m_Controller.CharacterModel.CharacterItem.GetTargetFacilityIndex());
        }

        protected override void OnCDOver()
        {
            //添加经验值
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_Slot.FacilityType);
            int exp = GetExpValue(m_Slot.FacilityType, level);
            m_Controller.AddExp(exp);
        }


        /// <summary>
        /// 根据等级获取练功经验
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetExpValue(FacilityType facilityType, int level)
        {
            return TDFacilityPracticeFieldTable.GetExpValue(facilityType, level);
        }
    }
}
