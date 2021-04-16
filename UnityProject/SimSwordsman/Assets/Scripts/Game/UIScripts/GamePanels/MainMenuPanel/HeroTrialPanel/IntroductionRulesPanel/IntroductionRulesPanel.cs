using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class IntroductionRulesPanel : AbstractAnimPanel
	{
		[SerializeField]
		private Button m_CloseBtn;
        [SerializeField]
		private Button m_BlackBtn;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CloseBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_BlackBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }
    }
}