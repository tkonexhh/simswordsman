using Qarth;
using System;
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
        [SerializeField]
		private Transform m_TitleTra; 
        [SerializeField]
		private GameObject m_ImgFontPre;

        string[] title = new string[] {"ÊÔ", "Á¶", "¹æ", "Ôò" };
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
            foreach (var item in title)
            {
                CreateTitle(item);
            }
        }

        private void CreateTitle(string item)
        {
            ImgFontPre imgFontPre = Instantiate(m_ImgFontPre, m_TitleTra).GetComponent<ImgFontPre>();
            imgFontPre.SetFontCont(item);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }
    }
}