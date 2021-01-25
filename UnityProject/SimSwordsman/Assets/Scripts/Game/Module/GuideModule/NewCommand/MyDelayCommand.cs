using System;
using Qarth;

namespace GameWish.Game
{
    public class MyDelayCommand : AbstractGuideCommand
    {
        private float DelayTime = 0; //开始延时

        public override void SetParam(object[] pv)
        {
            if (pv.Length == 0)
            {
                return;
            }

            try
            {
                if (pv.Length > 0)
                {
                    DelayTime = Helper.String2Float((string)pv[0]);
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        protected override void OnStart()
        {
            //根据是否有时间延迟进行 延迟发送
            if (DelayTime > 0)
            {
                UIMgr.S.OpenTopPanel(UIID.GuideMaskPanel, null);
                Timer.S.Post2Scale((x) => {
                    EventSystem.S.Send(EventID.GuideDelayStart, guideStep.stepID);
                    UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

                }, DelayTime);
            }
            else
            {
                EventSystem.S.Send(EventID.GuideDelayStart, guideStep.stepID);
            }
        }

        protected override void OnFinish(bool forceClean)
        {
            //UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);
        }
    }
}