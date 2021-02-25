using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

namespace GameWish.Game
{
	public class UserAccountPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_SecNameText;
        [SerializeField]
        private Text m_LobbyLevelText;
        [SerializeField]
        private Text m_DiscipleCountText;
        [SerializeField]
        private Text m_ChallengeProgressText;
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
            UpdateUseAccountInfo();
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }
        private void UpdateUseAccountInfo() 
        {
            m_SecNameText.text = GameDataMgr.S.GetClanData().GetClanName();

            m_LobbyLevelText.text = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel().ToString();

            int discipleCount = MainGameMgr.S.CharacterMgr.GetAllCharacterList().Count;
            m_DiscipleCountText.text = discipleCount.ToString();

            ChapterMgr chapterMgr = MainGameMgr.S.ChapterMgr;
            List<ChapterConfigInfo> chapterInfoList = MainGameMgr.S.ChapterMgr.GetAllChapterInfo().Where(x => chapterMgr.JudgeChapterIsUnlock(x.chapterId)).ToList();            
            int chapterLevel = chapterInfoList.Count;
            int chapterProgressLevel = 1;
            ChapterConfigInfo lastChapterInfo = chapterInfoList.Last();
            if (lastChapterInfo != null) {
                chapterProgressLevel = chapterMgr.GetLevelProgressNumber(lastChapterInfo.chapterId) + 1;
            }
            m_ChallengeProgressText.text = string.Format("{0}-{1}", chapterLevel, chapterProgressLevel);
        }

        private void OnPrivateBtnClickCallBack()
        {
            //https://privacy-policy.modooplay.com/ratel.best.sect/user_agreement.html
            string url = "https://privacy-policy.modooplay.com/ratel.best.sect/privacy_policy.html";
            Application.OpenURL(url);
        }

        private void OnLogOutBtnClickCallBack()
        {
            UIMgr.S.OpenTopPanel(UIID.LogPanel, LogPanelCallBack,"注销","确认要注销吗？","确认","取消");

            HideSelfWithAnim();
        }

        private void LogPanelCallBack(AbstractPanel obj)
        {
            LogPanel logPanel = obj as LogPanel;
            if (logPanel != null) 
            {
                logPanel.OnSuccessBtnEvent = LogOut;
            }
        }

        private void LogOut() {
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

        private void OnCloseBtnClickCallBack()
        {
            HideSelfWithAnim();
        }
    }	
}