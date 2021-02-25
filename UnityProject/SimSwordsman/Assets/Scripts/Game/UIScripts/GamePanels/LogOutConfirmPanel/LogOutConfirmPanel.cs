using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using UnityEngine.UI;
using System.IO;

namespace GameWish.Game
{
	public class LogOutConfirmPanel : AbstractAnimPanel
	{
		[SerializeField]
		private Button m_ConfirmBtn;
		[SerializeField]
		private Button m_CancelBtn;
		protected override void OnUIInit()
		{
			base.OnUIInit();

			m_ConfirmBtn.onClick.AddListener(OnConfirmBtnClick);

			m_CancelBtn.onClick.AddListener(OnCancelBtnClick);
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
        private void OnCancelBtnClick()
        {
            HideSelfWithAnim();
        }

        private void OnConfirmBtnClick()
        {
            PlayerPrefs.DeleteAll();

            string path = GameDataHandler.s_path;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            PlayerPrefs.SetInt(Define.LogoutKey, 1);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }	
}