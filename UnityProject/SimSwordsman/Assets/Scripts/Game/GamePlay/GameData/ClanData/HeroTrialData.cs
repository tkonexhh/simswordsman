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
        public ClanType clanType = ClanType.None;
        public string trialStartTime;
        public int characterId = -1;
        
        public HeroTrialData()
        {

        }

        public void SetDefaultValue()
        {

        }

        public void OnTrialStart(DateTime trialStartDay, int characterId, ClanType clanType)
        {
            state = HeroTrialStateID.Runing;
            this.trialStartTime = trialStartDay.ToString();
            this.characterId = characterId;
            this.clanType = clanType;

            GameDataMgr.S.GetClanData().SetDataDirty();
        }

        public void OnTrialTimeOver()
        {
            state = HeroTrialStateID.Finished;

            GameDataMgr.S.GetClanData().SetDataDirty();
        }

        public void Reset()
        {
            state = HeroTrialStateID.Idle;
            //trialStartTime = string.Empty;
            characterId = -1;
            clanType = ClanType.None;

            GameDataMgr.S.GetClanData().SetDataDirty();
        }

        public DateTime GetStartTime()
        {
            if (string.IsNullOrEmpty(trialStartTime))
            {
                return new DateTime(1970,1,1);
            }

            DateTime dateTime = DateTime.Parse(trialStartTime);
            return dateTime;
        }
    }


}