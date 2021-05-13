using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace GameWish.Game
{
    public class RewardPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Transform m_RewardTran;
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private GameObject m_LootFirework;

        [SerializeField]
        private GameObject m_ItemObj;
        [SerializeField]
        private Button m_DoubleRewardBtn;
        public Action OnBtnCloseEvent = null;
        List<RewardPanelItem> m_Items = new List<RewardPanelItem>();

        private List<RewardBase> m_RewardsDataList = new List<RewardBase>();

        private int m_CurrentChallengeLevel = -1;
        private bool m_IsBossLevel = false;
        private bool m_IsLookAD = false;

        private int m_CloseBtnTimerID = -1;
        private int levelID;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();

            m_DoubleRewardBtn.onClick.AddListener(() =>
            {
                DataAnalysisMgr.S.CustomEvent(DotDefine.level_double_reward, levelID);
                AdsManager.S.PlayRewardAD(Define.DoubleGetBossRewardADName, LookRewardADSuccessCallBack);
            });
        }
        public void SetLevelID(int id)
        {
            levelID = id;
        }

        private void LookRewardADSuccessCallBack(bool obj)
        {
            m_IsLookAD = true;   
            
            if (m_RewardsDataList.Count > 0)
            {
                m_RewardsDataList.ForEach(x => x.AcceptReward());

                for (int i = 0; i < m_Items.Count; i++)
                {
                    m_Items[i].UpdateDoubleRewardCount();
                }
            }

            m_DoubleRewardBtn.gameObject.SetActive(false);
        }
        /// <summary>
        /// args[1]:当前挑战的关卡  ，如果不是挑战，则为null
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            try
            {
                m_RewardsDataList.Clear();
                m_DoubleRewardBtn.gameObject.SetActive(false);

                m_CurrentChallengeLevel = -1;

                OpenDependPanel(EngineUI.MaskPanel, -1, null);

                if (args != null)
                {
                    m_RewardsDataList = (List<RewardBase>)args[0];
                    InitItems(m_RewardsDataList);

                    if (args.Length > 1)
                    {
                        m_CurrentChallengeLevel = (int)args[1];
                        m_IsBossLevel = TDLevelConfigTable.IsBossLevel(m_CurrentChallengeLevel);
                        m_DoubleRewardBtn.gameObject.SetActive(m_IsBossLevel);
                    }
                }

                if (m_IsBossLevel)
                {
                    m_CloseBtn.gameObject.SetActive(false);
                    m_CloseBtnTimerID = Timer.S.Post2Really((x) =>
                    {
                        m_CloseBtn.gameObject.SetActive(true);
                        m_CloseBtnTimerID = -1;
                    }, 1, 1);
                }
                else
                {
                    m_CloseBtn.gameObject.SetActive(true);
                }
                Debug.LogError("---m_DoubleRewardBtn = " + m_DoubleRewardBtn);
                Debug.LogError("---m_RewardsDataList = " + m_RewardsDataList);
                Debug.LogError("---args = " + args);
                Debug.LogError("---m_CloseBtn = " + m_CloseBtn);
            }
            catch (Exception e)
            {
                Debug.LogError("---e"+e);
            }

           
        }

        void InitItems(List<RewardBase> rewards)
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                if (i >= 3)
                    break;

                if (m_Items.Count < i + 1)
                {
                    GameObject obj = Instantiate(m_ItemObj, m_RewardTran);
                    RewardPanelItem item = obj.GetComponent<RewardPanelItem>();
                    m_Items.Add(item);
                }
                m_Items[i].gameObject.SetActive(true);
                m_Items[i].Init(this, rewards[i]);
            }


            if (m_Items.Count > rewards.Count)
            {
                for (int i = rewards.Count - 1; i < m_Items.Count - rewards.Count; i++)
                {
                    m_Items[i].gameObject.SetActive(false);
                }
            }

            Instantiate(m_LootFirework, transform).transform.localPosition = Vector3.zero;
        }
        private void BindAddListenerEvent()
        {
            //锟斤拷效
            foreach (var item in transform.GetComponentsInChildren<Button>(true))
            {
                item.onClick.AddListener(() => AudioMgr.S.PlaySound(Define.SOUND_UI_BTN));
            }
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                OnBtnCloseEvent?.Invoke();
                CloseSelfPanel();
                //HideSelfWithAnim();
            });
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            //CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            if (GuideMgr.S.IsGuideFinish(34) && GuideMgr.S.IsGuideFinish(36) == false)
            //if (GuideMgr.S.IsGuiding(34) && GuideMgr.S.IsGuideFinish(19) == false)
            {
                EventSystem.S.Send(EventID.OnSignInSystem_FinishedTrigger);
            }

            if (m_IsBossLevel)
            {
                GameDataMgr.S.GetPlayerData().UpdateIsLookADInLastChallengeBossLevel(m_IsLookAD);
            }   

            if (m_CloseBtnTimerID != -1) 
            {
                Timer.S.Cancel(m_CloseBtnTimerID);
            }

            if (m_CurrentChallengeLevel != -1 && m_IsBossLevel == false && TDLevelConfigTable.IsBossLevel(m_CurrentChallengeLevel - 1)) 
            {
                if (GameDataMgr.S.GetPlayerData().CurrentChallengeLevelIsPlayInterAD())
                {
                    AdsManager.S.PlayInterAD("ChallengePlayInterAD", (x) => { });
                }
            }
        }
    }
}