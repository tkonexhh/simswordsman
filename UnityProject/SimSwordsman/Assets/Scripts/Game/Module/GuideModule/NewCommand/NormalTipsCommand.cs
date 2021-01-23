using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class NormalTipsCommand : AbstractGuideCommand
    {
        private string m_TipsKey;
        private Vector3 m_Offset;
        bool m_Flip = false;

        public override void SetParam(object[] pv)
        {
            if (pv.Length == 0)
            {
                Log.w("PlayAudioCommand Init With Invalid Param.");
                return;
            }

            m_TipsKey = (string)pv[0];
            if (pv.Length > 1)
            {
                m_Offset = Helper.String2Vector3((string)pv[1], '|');
            }
            if (pv.Length > 2)
            {
                m_Flip = Helper.String2Bool((string)pv[2]);
            }
        }

        protected override void OnStart()
        {
            EventSystem.S.Register(EventID.OnGuidePanelOpen, StartCommand);

        }
        protected override void OnFinish(bool forceClean)
        {
            EventSystem.S.UnRegister(EventID.OnGuidePanelOpen, StartCommand);
        }

        private void StartCommand(int key, params object[] param)
        {
            if (UIMgr.S.FindPanel(UIID.MyGuidePanel) && (int)param[0] == guideStep.stepID)
            {
                MyGuidePanel guidepanel = UIMgr.S.FindPanel(UIID.MyGuidePanel) as MyGuidePanel;
                if(guidepanel != null)
                {
                    guidepanel.LocateMyGuideTips(TDLanguageTable.Get(m_TipsKey), m_Offset, m_Flip);
                }

            }


        }

    }
}
