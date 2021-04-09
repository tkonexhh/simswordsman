using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameWish.Game
{

    [Serializable]
    public class HeroTrialData
    {
        public HeroTrialStateID state = HeroTrialStateID.Idle;
        public int trialStartDay = -1;
        public int characterId = -1;

        public HeroTrialData()
        {

        }

        public void SetDefaultValue()
        {

        }

        public void OnTrialStart(int trialStartDay, int characterId)
        {
            state = HeroTrialStateID.Runing;
            this.trialStartDay = trialStartDay;
            this.characterId = characterId;

            GameDataMgr.S.GetClanData().SetDataDirty();
        }

        public void OnTrialEnd()
        {
            state = HeroTrialStateID.Idle;
            trialStartDay = -1;
            characterId = -1;

            GameDataMgr.S.GetClanData().SetDataDirty();
        }
    }


}