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
        private Transform m_RewardTra;
        [SerializeField]
        private GameObject m_Reward;
        //[SerializeField]
        //private Image m_RewardIcon;
        //[SerializeField]
        //private Image m_KungName;

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
            try
            {
                m_CurChapterConfigInfo = args[0] as ChapterConfigInfo;
                m_LevelConfigInfo = (LevelConfigInfo)args[1];
                RefreshPanelInfo();

                //RandomAccess(m_LevelConfigInfo.enemyHeadIcon);
                m_ChallengePhoto.enabled = true;
                m_ChallengePhoto.sprite = FindSprite("enemy_icon_" + m_LevelConfigInfo.enemyHeadIcon);
            }
            catch (Exception e)
            {
                Log.e("IdentifyChallengesPanel error: " + e.Message + " " + e.StackTrace);
            }
        }

        private void RefreshPanelInfo()
        {
            m_ChallengeTitle.text = CommonUIMethod.GetChallengeTitle(m_CurChapterConfigInfo, m_LevelConfigInfo.level);
            m_ChallengeCont.text = m_LevelConfigInfo.desc;

            for (int i = 0; i < m_LevelConfigInfo.levelRewardList.Count; i++)
            {
                switch (m_LevelConfigInfo.levelRewardList[i].RewardItem)
                {
                    case RewardItemType.Item:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(TaskRewardType.Item, m_LevelConfigInfo.levelRewardList[i],false);
                        break;
                    case RewardItemType.Armor:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(TaskRewardType.Armor, m_LevelConfigInfo.levelRewardList[i], false);
                        break;
                    case RewardItemType.Arms:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(TaskRewardType.Arms, m_LevelConfigInfo.levelRewardList[i], false);
                        break;
                    case RewardItemType.Kongfu:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(TaskRewardType.Kongfu, m_LevelConfigInfo.levelRewardList[i], false);
                        break;
                    case RewardItemType.Medicine:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(TaskRewardType.Medicine, m_LevelConfigInfo.levelRewardList[i], false);
                        break;
                    case RewardItemType.Food:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(TaskRewardType.Food, m_LevelConfigInfo.levelRewardList[i], false);
                        break;
                    case RewardItemType.TowerCoin:
                        break;
                    case RewardItemType.Exp_Kongfu:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(RewardItemType.Exp_Kongfu, m_LevelConfigInfo.levelRewardList[i].Count);
                        break;
                    case RewardItemType.Exp_Role:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(RewardItemType.Exp_Role, m_LevelConfigInfo.levelRewardList[i].Count);
                        break;
                    case RewardItemType.Coin:
                        Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(TaskRewardType.Coin, m_LevelConfigInfo.levelRewardList[i], false);
                        break;
                    default:
                        break;
                }
            }

            m_ChallengeRecommendAtkValue.text = CommonUIMethod.GetTenThousandOrMillion(m_LevelConfigInfo.recommendAtkValue);
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
    }
}