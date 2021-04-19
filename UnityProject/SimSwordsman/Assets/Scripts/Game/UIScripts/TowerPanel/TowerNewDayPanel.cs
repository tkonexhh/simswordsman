using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerNewDayPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnClose2;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(OnClickClose);
            m_BtnClose2.onClick.AddListener(OnClickClose);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
        }

        protected override void OnClose()
        {
            CloseDependPanel(EngineUI.MaskPanel);
        }

        private void OnClickClose()
        {
            CloseSelfPanel();
        }
    }

}