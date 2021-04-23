using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using UnityEditor;

namespace GameWish.Game
{
	public class IOSPrivacePanel : AbstractPanel
	{
		[SerializeField]
		private Button m_AgreeBtn;
		[SerializeField]
		private Button m_DisAgreeBtn;


        protected override void OnUIInit()
        {
            m_AgreeBtn.onClick.AddListener(OnClickAgree);
            m_DisAgreeBtn.onClick.AddListener(OnClickDissAgree);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void OnClickAgree()
        {
            GameDataMgr.S.GetPlayerData().SetIsAgreePrivace();
            CloseSelfPanel();
        }

        private void OnClickDissAgree()
        {
            CloseSelfPanel();
#if UNITY_EDITOR 
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

    }
	
}