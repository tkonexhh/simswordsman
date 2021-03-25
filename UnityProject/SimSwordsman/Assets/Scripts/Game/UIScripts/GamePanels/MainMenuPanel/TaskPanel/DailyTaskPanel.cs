using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class DailyTaskPanel : AbstractAnimPanel
    {
        [SerializeField] private Button m_BtnClose;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_BtnClose.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnPanelHideComplete()
        {
            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }
    }

}