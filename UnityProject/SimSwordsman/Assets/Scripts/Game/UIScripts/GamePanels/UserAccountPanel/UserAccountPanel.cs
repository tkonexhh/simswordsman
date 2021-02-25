using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using UnityEngine.UI;
using System;
using System.IO;

namespace GameWish.Game
{
	public class UserAccountPanel : AbstractAnimPanel
	{
		[SerializeField]
		private Button m_CloseBtn;
		[SerializeField]
		private Button m_LogOutBtn;
		[SerializeField]
		private Button m_PrivateBtn;

        protected override void OnUIInit()
        {
            base.OnUIInit();

			m_CloseBtn.onClick.AddListener(OnCloseBtnClickCallBack);
            m_LogOutBtn.onClick.AddListener(OnLogOutBtnClickCallBack);
            m_PrivateBtn.onClick.AddListener(OnPrivateBtnClickCallBack);
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }
        private void OnPrivateBtnClickCallBack()
        {
            //https://privacy-policy.modooplay.com/ratel.best.sect/user_agreement.html
            string url = "https://privacy-policy.modooplay.com/ratel.best.sect/privacy_policy.html";
            Application.OpenURL(url);
        }

        private void OnLogOutBtnClickCallBack()
        {
            UIMgr.S.OpenTopPanel(UIID.LogOutConfirmPanel, null);

            HideSelfWithAnim();
        }

        private void OnCloseBtnClickCallBack()
        {
            HideSelfWithAnim();
        }
    }	
}