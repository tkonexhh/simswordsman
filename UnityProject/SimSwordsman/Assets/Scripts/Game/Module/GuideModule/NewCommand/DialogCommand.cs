using System;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class DialogCommand : AbstractGuideCommand
    {
        string textContentID;//气泡显示的id（Language表）

        public override void SetParam(object[] pv)
        {
            if (pv.Length == 0)
            {
                return;
            }
            try
            {
                textContentID = "";
                for (int i = 0; i < pv.Length; i++)
                {
                    textContentID += (string)pv[i];
                    if (i != (pv.Length - 1))
                    {
                        textContentID += ",";
                    }
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        protected override void OnStart()
        {
            EventSystem.S.Register(EventID.GuideDelayStart, StartCommand);
            Debug.Log("DialogCommand 开始");
        }
        

        protected override void OnFinish(bool forceClean)
        {
            UIMgr.S.ClosePanelAsUIID(UIID.StoryPanel);
            EventSystem.S.UnRegister(EventID.GuideDelayStart, StartCommand);
        }

        private void StartCommand(int key, params object[] param)
        {
            if (param.Length < 0 || guideStep.stepID != (int)param[0])
            {
                Log.e("不满足条件无法开启" + guideStep.stepID);
                return;
            }
            Action action = FinishStep;
            UIMgr.S.OpenTopPanel(UIID.StoryPanel, null, textContentID, action);
        }
    }
}