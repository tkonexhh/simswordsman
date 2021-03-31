using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class UITipsPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Transform m_ImgBg;
        [SerializeField]
        private Text m_TipName;
        [SerializeField]
        private Text m_Need;
        [SerializeField]
        private Text m_Desc;

        private int TimerID;

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_ImgBg.transform.position = (Vector3)args[0] + new Vector3(0.5f,0.8f, 0);
            m_TipName.text = (string)args[1];
            m_Need.text = (string)args[2];
            m_Desc.text = ((ItemTipsConfig)args[3]).desc;

            Timer.S.Cancel(TimerID);
            TimerID = Timer.S.Post2Really((i) => {
                HideSelfWithAnim();
            }, 2.0f, -1);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }
    }
}