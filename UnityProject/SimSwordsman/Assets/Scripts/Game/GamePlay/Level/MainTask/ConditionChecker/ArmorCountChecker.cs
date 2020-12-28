using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
	public class ArmorCountChecker : ConditionCheckerBase<int>
    {
        public override bool IsFinished()
        {
            int curStudentCount = MainGameMgr.S.CharacterMgr.GetCharacterCount();
            return curStudentCount >= m_TargetValue;
        }
    }

}