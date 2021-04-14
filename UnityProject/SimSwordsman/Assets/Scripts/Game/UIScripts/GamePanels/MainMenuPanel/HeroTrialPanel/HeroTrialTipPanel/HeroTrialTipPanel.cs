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
        [SerializeField]
        private Button m_BlackBtn;       
        [SerializeField]
        private Text m_ClanType;

        protected override void OnUIInit()
	    {
            base.OnUIInit();

            //m_ClanType.text = MainGameMgr.S.HeroTrialMgr.GetClanType().ToString();

            m_CloseBtn.onClick.AddListener(()=> 
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_BlackBtn.onClick.AddListener(()=> 
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
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