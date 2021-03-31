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

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();

            m_DoubleRewardBtn.onClick.AddListener(() =>
            {
                AdsManager.S.PlayRewardAD(Define.DoubleGetBossRewardADName, LookRewardADSuccessCallBack);
            });
        }

        private void LookRewardADSuccessCallBack(bool obj)
        {
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
        /// args[1]:�����Ƿ���ʾ˫����ť
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            m_RewardsDataList.Clear();
            m_DoubleRewardBtn.gameObject.SetActive(false);

            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            if (args != null)
            {
                m_RewardsDataList = (List<RewardBase>)args[0];
                InitItems(m_RewardsDataList);

                if (args.Length > 1)
                {
                    bool isShowDoubleRewardBtn = (bool)args[1];
                    m_DoubleRewardBtn.gameObject.SetActive(isShowDoubleRewardBtn);
                }
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
                m_Items[i].Init(this, rewards[i], m_SortingOrder + 10);
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
            //��Ч
            foreach (var item in transform.GetComponentsInChildren<Button>(true))
            {
                item.onClick.AddListener(() => AudioMgr.S.PlaySound(Define.SOUND_UI_BTN));
            }
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                OnBtnCloseEvent?.Invoke();
                HideSelfWithAnim();
            });
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }

        protected override void OnClose()
        {
            base.OnClose();

            if (GuideMgr.S.IsGuideFinish(34) && GuideMgr.S.IsGuideFinish(36) == false)
            //if (GuideMgr.S.IsGuiding(34) && GuideMgr.S.IsGuideFinish(19) == false)
            {
                EventSystem.S.Send(EventID.OnSignInSystem_FinishedTrigger);
            }
        }
    }
}