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

        [SerializeField]
        private Button m_MusicBtn;
        [SerializeField]
        private GameObject m_MusicOffObj;
        [SerializeField]
        private GameObject m_MusicOnObj;
        [SerializeField]
        private Button m_SoundBtn;
        [SerializeField]
        private Button m_MessageBtn;
        [SerializeField]
        private Button m_UserBtn;
        [SerializeField]
        private Image m_MessageImg;
        [SerializeField]
        private GameObject m_SoundOffObj;
        [SerializeField]
        private GameObject m_SoundOnObj;
        private bool isMuiscOn;
        private bool isSoundOn;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            m_CloseBtn.onClick.AddListener(OnCloseBtnClickCallBack);
            m_LogOutBtn.onClick.AddListener(OnLogOutBtnClickCallBack);
            m_PrivateBtn.onClick.AddListener(OnPrivateBtnClickCallBack);
            m_UserBtn.onClick.AddListener(OnUserBtnClickCallBack);
            m_MessageBtn.onClick.AddListener(OnMessageBtnClickCallBack);

            m_MusicBtn.onClick.AddListener(() => { UpdateMusic(!isMuiscOn); });
            m_SoundBtn.onClick.AddListener(() => { UpdateSound(!isSoundOn); });
            RefreshMessageBtn(GameDataMgr.S.GetPlayerData().GetMessagePush());
        }

        private void OnMessageBtnClickCallBack()
        {
            GameDataMgr.S.GetPlayerData().SetMessagePush(!GameDataMgr.S.GetPlayerData().GetMessagePush());
            RefreshMessageBtn(GameDataMgr.S.GetPlayerData().GetMessagePush());
        }

        private void RefreshMessageBtn(bool msg)
        {
            if (msg)
            {
                m_MessageImg.sprite = FindSprite("SettingPanel_MessagOn");
            }
            else
            {
                m_MessageImg.sprite = FindSprite("SettingPanel_MessagOff");
            }
        }

        private void OnUserBtnClickCallBack()
        {
            PrivacyMgr.S.JumpToUserAgreement();
        }

        private void UpdateMusic(bool ison)
        {
            isMuiscOn = ison;
            AudioMgr.S.isMusicEnable = ison;
            m_MusicOffObj.SetActive(!ison);
            m_MusicOnObj.SetActive(ison);
        }
        private void UpdateSound(bool ison)
        {
            isSoundOn = ison;
            AudioMgr.S.isSoundEnable = ison;
            m_SoundOffObj.SetActive(!ison);
            m_SoundOnObj.SetActive(ison);
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            UpdateMusic(AudioMgr.S.isMusicEnable);
            UpdateSound(AudioMgr.S.isSoundEnable);

            UpdateUseAccountInfo();
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
            if (lastChapterInfo != null)
            {
                chapterProgressLevel = chapterMgr.GetLevelProgressNumber(lastChapterInfo.chapterId) + 1;
            }
            m_ChallengeProgressText.text = string.Format("{0}-{1}", chapterLevel, chapterProgressLevel);
        }

        private void OnPrivateBtnClickCallBack()
        {
            //https://privacy-policy.modooplay.com/ratel.best.sect/user_agreement.html
            //string url = "https://privacy-policy.modooplay.com/ratel.best.sect/privacy_policy.html";
            //Application.OpenURL(url);
            PrivacyMgr.S.JumpToPrivacyPolicy();
            //PrivacyMgr.S.JumpToUserAgreement();
        }

        private void OnLogOutBtnClickCallBack()
        {
            UIMgr.S.OpenTopPanel(UIID.LogPanel, LogPanelCallBack, "注销", "确认要注销吗？", "确认", "取消");

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

        private void LogOut()
        {
            PlayerPrefs.DeleteAll();
            GameDataMgr.S.Reset();
            // string path = GameDataHandler.s_path;
            // Debug.LogError(path);
            // if (File.Exists(path))
            // {
            //     File.Delete(path);
            // }

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