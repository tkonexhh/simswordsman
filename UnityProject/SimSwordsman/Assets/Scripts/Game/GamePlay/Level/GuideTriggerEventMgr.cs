using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class GuideTriggerEventMgr : TSingleton<GuideTriggerEventMgr>
	{
	    public void Init()
        {
            EventSystem.S.Register(EventID.OnFirstGetCharacter, StartGuide_Task);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, UnlockFacility);
        }


        private void StartGuide_Task(int key, object[] param)
        {
            //EventSystem.S.Send(EventID.InGuideProgress, false);
            Timer.S.Post2Really(x => 
            {
                EventSystem.S.Send(EventID.OnGuideDialog4);
            }, 1f);
        }
        private void UnlockFacility(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            switch (type)
            {
                case FacilityType.KongfuLibrary:
                    break;
                case FacilityType.ForgeHouse:
                    break;
                case FacilityType.Baicaohu:
                    break;
                case FacilityType.PracticeFieldEast:
                    break;
                case FacilityType.PracticeFieldWest:
                    break;
            }
        }
    }
	
}