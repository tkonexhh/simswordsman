
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameWish.Game;
using Qarth;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class MyDelayCommand : AbstractGuideCommand
    {
        private float DelayTime = 0; //开始延时
        private int m_NextStepId = -1; //下一步ID
        private int m_NextStepTime = 0; //下一步是否需要延迟执行
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
                if (pv.Length > 1)
                {
                    m_NextStepId = Helper.String2Int((string)pv[1]);
                }
                if (pv.Length > 2)
                {
                    m_NextStepTime = Helper.String2Int((string)pv[2]);
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
            UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

            if (m_NextStepTime > 0)
            {
                Timer.S.Post2Scale((x) => {
                    EventSystem.S.Send(EventID.GuideEventTrigger, m_NextStepId);//为下一个引导发送开始
                }, m_NextStepTime);
            }
            else
            {
                EventSystem.S.Send(EventID.GuideEventTrigger, m_NextStepId);//为下一个引导发送开始
            }


          
        }




    }
}

