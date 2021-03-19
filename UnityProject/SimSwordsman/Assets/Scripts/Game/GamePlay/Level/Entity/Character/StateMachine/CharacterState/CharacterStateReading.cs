using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateReading : CharacterStateCD
    {
        protected override CharacterStateID targetState => CharacterStateID.Reading;
        protected override string animName => "write";

        public CharacterStateReading(CharacterStateID stateEnum) : base(stateEnum) { }


        protected override BaseSlot GetTargetSlot()
        {
            KongfuLibraryController kongFuController = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.KongfuLibrary);
            return kongFuController.GetIdlePracticeSlot();
        }

        protected override void OnCDOver()
        {

        }
    }
}
