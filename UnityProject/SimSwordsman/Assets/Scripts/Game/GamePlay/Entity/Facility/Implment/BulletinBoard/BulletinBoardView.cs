using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class BulletinBoardView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new BulletinBoardController( FacilityType.BulletinBoard, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("BulletinBoard is clicked");

        }        

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            //UIMgr.S.OpenPanel(UIID.BulletinBoard);
        }
    }

}