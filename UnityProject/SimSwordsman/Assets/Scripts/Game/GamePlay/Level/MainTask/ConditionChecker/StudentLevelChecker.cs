using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class StudentLevelChecker : ConditionCheckerBase<int>
    {
        public override bool IsFinished()
        {
            int curStudentCount = MainGameMgr.S.CharacterMgr.GetCharacterCount();
            return curStudentCount >= m_TargetValue;
        }
    }

}