using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameWish.Game
{
   
    public class HeroTrialTipPanel : AbstractAnimPanel
	{

        [SerializeField]
        private Button m_CloseBtn;

        protected override void OnUIInit()
	    {
            base.OnUIInit();

            m_CloseBtn.onClick.AddListener(()=> 
            {
                HideSelfWithAnim();
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);


            OpenDependPanel(EngineUI.MaskPanel,-1,null);
        }


        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}