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
        private Image m_RewardIcon;
        [SerializeField]
        private Image m_KungName;

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
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_BlackBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_ChallengeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.SendDisciplesPanel, PanelType.Challenge, m_CurChapterConfigInfo, m_LevelConfigInfo);
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

            m_ChallengeRecommendAtkValue.text = m_LevelConfigInfo.recommendAtkValue.ToString();

            foreach (var item in m_LevelConfigInfo.levelRewardList)
            {
                switch (item.RewardItem)
                {
                    case RewardItemType.Item:
                        m_RewardIcon.sprite = FindSprite(TDItemConfigTable.GetIconName((int)item.KeyID));
                        break;
                    case RewardItemType.Armor:
                    case RewardItemType.Arms:
                        m_RewardIcon.sprite = FindSprite(TDEquipmentConfigTable.GetIconName((int)item.KeyID));
                        break;
                    case RewardItemType.Kongfu:
                        SetKungfuSprite(item);
                        break;
                    case RewardItemType.Medicine:
                        m_RewardIcon.sprite = FindSprite(TDHerbConfigTable.GetHerbIconNameById((int)item.KeyID));
                        break;
                    case RewardItemType.Food:
                        m_RewardIcon.sprite = FindSprite("Baozi");
                        break;
                    case RewardItemType.Coin:
                        m_RewardIcon.sprite = FindSprite("Coin");
                        break;

                    case RewardItemType.Exp_Role:
                    case RewardItemType.Exp_Kongfu:
                        break;
                    default:
                        break;
                }
            }
            m_ChallengeRewardValue.text = m_LevelConfigInfo.levelRewardList[0].Count.ToString();
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        private void SetKungfuSprite(RewardBase item)
        {
            m_KungName.gameObject.SetActive(true);
            switch (GetKungfuQuality((KungfuType)item.KeyID))
            {
                case KungfuQuality.Normal:
                    m_RewardIcon.sprite = FindSprite("Introduction");
                    break;
                case KungfuQuality.Super:
                    m_RewardIcon.sprite = FindSprite("Advanced");
                    break;
                case KungfuQuality.Master:
                    m_RewardIcon.sprite = FindSprite("Excellent");
                    break;
                default:
                    break;
            }
            m_KungName.sprite = FindSprite(TDKongfuConfigTable.GetIconName((KungfuType)item.KeyID));
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}