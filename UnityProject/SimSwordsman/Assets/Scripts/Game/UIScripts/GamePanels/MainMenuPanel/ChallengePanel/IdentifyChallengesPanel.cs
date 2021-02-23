using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class IdentifyChallengesPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_ChallengeTitle;
        [SerializeField]
        private Text m_ChallengeCont;
        [SerializeField]
        private Text m_ChallengeRewardValue;
        [SerializeField]
        private Text m_ChallengeRecommendAtkText;
        [SerializeField]
        private Text m_ChallengeRecommendAtkValue;
        [SerializeField]
        private Image m_ChallengePhoto;

        [SerializeField]
        private Button m_ChallengeBtn;
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_BlackBtn;
        private AddressableAssetLoader<Sprite> m_Loader;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_BlackBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_ChallengeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.SendDisciplesPanel,PanelType.Challenge, m_CurChapterConfigInfo, m_LevelConfigInfo);
                CloseSelfPanel();
                CloseDependPanel(EngineUI.MaskPanel);
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurChapterConfigInfo = args[0] as ChapterConfigInfo;
            m_LevelConfigInfo = (LevelConfigInfo)args[1];
            RefreshPanelInfo();

            RandomAccess(m_LevelConfigInfo.enemyHeadIcon);
            m_ChallengePhoto.enabled = true;
            m_ChallengePhoto.sprite = FindSprite("enemy_icon_" + m_LevelConfigInfo.enemyHeadIcon);
        }

        private void RandomAccess(string iconName)
        {
            iconName.Split(';');
        }
        public void LoadClanPrefabs(string prefabsName)
        {
            m_Loader = new AddressableAssetLoader<Sprite>();
            m_Loader.LoadAssetAsync(prefabsName, (obj) =>
            {
                //Debug.Log(obj);
                m_ChallengePhoto.enabled = true;
                m_ChallengePhoto.sprite = obj;
            });
        }
        private void RefreshPanelInfo()
        {
            m_ChallengeTitle.text = CommonUIMethod.GetChallengeTitle(m_CurChapterConfigInfo, m_LevelConfigInfo.level);
            m_ChallengeCont.text = m_LevelConfigInfo.desc;
            m_ChallengeRewardValue.text = m_LevelConfigInfo.levelRewardList[0].GetRewardValue().ToString();
            m_ChallengeRecommendAtkValue.text = m_LevelConfigInfo.recommendAtkValue.ToString();
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}