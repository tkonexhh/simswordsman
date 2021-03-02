using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class DialogWithCircleMaskCommand : AbstractGuideCommand
	{
        string textContentID;//������ʾ��id��Language��
        Vector3 targetPos;//Ŀ����λ��

        public override void SetParam(object[] pv)
        {
            if (pv.Length == 0)
            {
                return;
            }
            try
            {
                Transform tar = GameplayMgr.S.transform.Find((string)pv[0]);
                targetPos = tar.position;
                Camera.main.transform.position = new Vector3(targetPos.x, targetPos.y, Camera.main.transform.position.z);

                textContentID = "";
                for (int i = 1; i < pv.Length; i++)
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
            Debug.Log("DialogWithCircleMaskCommand ��ʼ");
        }


        protected override void OnFinish(bool forceClean)
        {
            UIMgr.S.ClosePanelAsUIID(UIID.StoryWithCircleMaskPanel);
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
            UIMgr.S.OpenTopPanel(UIID.StoryWithCircleMaskPanel, null, targetPos, textContentID, action);
        }
    }	
}