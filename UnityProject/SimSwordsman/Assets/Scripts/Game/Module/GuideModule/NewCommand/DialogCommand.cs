using System;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class DialogCommand : AbstractGuideCommand
    {
        string textContentID;//������ʾ��id��Language��

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
            Debug.Log("DialogCommand ��ʼ");
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
                Log.e("�����������޷�����" + guideStep.stepID);
                return;
            }
            Action action = FinishStep;
            UIMgr.S.OpenTopPanel(UIID.StoryPanel, null, textContentID, action);
        }
    }
}