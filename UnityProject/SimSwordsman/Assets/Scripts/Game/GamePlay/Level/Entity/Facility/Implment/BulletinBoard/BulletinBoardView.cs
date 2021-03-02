using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class BulletinBoardView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new BulletinBoardController(FacilityType.BulletinBoard, this);
        }
        ~BulletinBoardView()
        {
            EventSystem.S.UnRegister(EventID.OnSendBulletinBoardFacility, HandAddlistenerEvent);
        }
        public override void Init()
        {
            base.Init();
            EventSystem.S.Register(EventID.OnSendBulletinBoardFacility, HandAddlistenerEvent);

        }

        private void HandAddlistenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSendBulletinBoardFacility:
                    SetBulletinBoardTip((bool)param[0]);
                    break;
            }
        }

        private void SetBulletinBoardTip(bool isActive)
        {
            FacilityController facilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Lobby);
            if (facilityController.GetState() == FacilityState.Unlocked)
                tips.transform.parent.gameObject.SetActive(isActive);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            //Debug.Log("BulletinBoard is clicked");
            UIMgr.S.OpenPanel(UIID.BulletinBoardPanel);
        }


        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            //UIMgr.S.OpenPanel(UIID.BulletinBoard);
        }
    }

}