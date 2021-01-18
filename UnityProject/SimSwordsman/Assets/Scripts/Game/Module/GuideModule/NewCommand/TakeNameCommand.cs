using System;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TakeNameCommand : AbstractGuideCommand
    {
        public override void SetParam(object[] pv)
        {

        }

        protected override void OnStart()
        {
            EventSystem.S.Register(EventID.GuideDelayStart, StartCommand);
            Debug.Log("TakeNameCommand ¿ªÊ¼");
        }
        

        protected override void OnFinish(bool forceClean)
        {
            UIMgr.S.ClosePanelAsUIID(UIID.StoryPanel);
            EventSystem.S.UnRegister(EventID.GuideDelayStart, StartCommand);
        }

        private void StartCommand(int key, params object[] param)
        {
            Action action = FinishStep;
            UIMgr.S.OpenTopPanel(UIID.SectNamePanel, null, action);
        }
    }
}