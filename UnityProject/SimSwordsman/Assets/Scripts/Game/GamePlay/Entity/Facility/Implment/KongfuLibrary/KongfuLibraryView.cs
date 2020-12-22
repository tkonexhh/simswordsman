using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class KongfuLibraryView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new KongfuLibraryController( FacilityType.KongfuLibrary, 1, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("KongfuLibrary is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.KongfuLibraryPanel,facilityType);
        }
    }

}