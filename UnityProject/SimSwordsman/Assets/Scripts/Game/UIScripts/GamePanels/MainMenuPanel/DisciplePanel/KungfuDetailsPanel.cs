using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class KungfuDetailsPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Button m_BlackBtn;  
        [SerializeField]
        private Button m_CloseBtn; 
        [SerializeField]
        private Transform m_TopTra;  
        [SerializeField]
        private GameObject m_ImgFontPre;      
        [SerializeField]
        private Text m_BriefIntroduction;    
        [SerializeField]
        private Image m_KungfuBg;   
        [SerializeField]
        private Image m_KungfuName;     
        [SerializeField]
        private Image m_KungfuQuality;  
        [SerializeField]
        private Text m_KungfuOrder;
        [SerializeField]
        private Slider m_KungfuExpSlider;   
        [SerializeField]
        private Text m_CurAddition;  
        [SerializeField]
        private Text m_NextAddition;        
        [SerializeField]
        private Button m_ResetLearnBtn;

        private CharacterKongfu m_CharacterKongfu = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CharacterKongfu = args[0] as CharacterKongfu;

            m_BlackBtn.onClick.AddListener(()=> { 
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            RefreshPanelInfo();
        }

        private void RefreshPanelInfo()
        {
            string kungfuName = m_CharacterKongfu.name;
            for (int i = 0; i < kungfuName.Length; i++)
                CreateKungfuName(kungfuName[i].ToString());

            m_BriefIntroduction.text = m_CharacterKongfu.desc;
            m_KungfuBg.sprite = CommonMethod.GetKungfuBg(m_CharacterKongfu.GetKongfuType());
            m_KungfuName.sprite = CommonMethod.GetKungName(m_CharacterKongfu.GetKongfuType());
            //m_KungfuQuality.sprite
        }

        private void CreateKungfuName(string font)
        {
            ImgFontPre imgFontPre = Instantiate(m_ImgFontPre, m_TopTra).GetComponent<ImgFontPre>();
            imgFontPre.SetFontCont(font);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
	
}