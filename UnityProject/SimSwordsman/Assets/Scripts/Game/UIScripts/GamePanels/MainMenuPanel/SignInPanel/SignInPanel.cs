using Qarth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class SignInPanel : AbstractAnimPanel
    {
        private Dictionary<int, SignInItem> m_SignItemDic = new Dictionary<int, SignInItem>();
        [SerializeField] private Transform[] m_SignItemTrans;
        [SerializeField] private Button m_BackBtn;//���ذ�ť
        [SerializeField] private Button m_BlackBtn;//���ذ�ť
        [SerializeField] private Button m_AcceptBtn;//���ذ�ť

        //[Header("Image")]
        //[SerializeField] private Image m_DaysShowNum;//??????????
        //[SerializeField] private Image m_Title;
        //[SerializeField] private Image m_DayHint;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            //��Ч
            AudioMgr.S.PlaySound(Define.INTERFACE);
            m_BackBtn.onClick.AddListener(OnClickClose);
            m_BlackBtn.onClick.AddListener(OnClickClose);
            m_AcceptBtn.onClick.AddListener(OnClickAccept);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            InitSignItem();
            SignSystemMgr.S.weekSignState.Check();

            //m_Title.sprite = UIMgrExtend.FindLanguageSprite(TDLanguageTable.Get("SignPanel_Title"));
            //m_DayHint.sprite = UIMgrExtend.FindLanguageSprite(TDLanguageTable.Get("SignPanel_DayItem"));

            foreach (var item in m_SignItemDic)
            {
                item.Value.SignItemCallBack = SignItemCallBack;
            }

            UpdateSignItemStatus();

            //Custom event
            DateTime now = DateTime.Now;
            DateTime firstPlayTime = DateTime.Parse(GameDataMgr.S.GetPlayerData().firstPlayTime);
            TimeSpan timeSpan = now - firstPlayTime;
            int loginDay = timeSpan.Days;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("DayCount", loginDay);

            //DataAnalysisMgr.S.CustomEventDic(Define.CUSTOM_EVENT_OPEN_SIGN_IN, dic);
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

            foreach (var item in m_SignItemDic)
            {
                item.Value.SignItemCallBack = null;
            }
        }

        void OnClickClose()
        {
            AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
            HideSelfWithAnim();
        }

        void OnClickAccept()
        {
            AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
            foreach (var item in m_SignItemDic.Values)
            {
                if (item.Status == SignInStatus.SignEnable)
                {
                    item.ClickSignBtn();
                    return;
                }
            }
        }

        protected override void BeforDestroy()
        {
            base.BeforDestroy();
            ClearSignItem();
        }

        /// <summary>
        /// ����SignItem���Ҽ����ֵ����
        /// </summary>
        private void InitSignItem()
        {
            ClearSignItem();
            if (TDDailySigninTable.dataList.Count != 7)
            {
                Log.e("ǩ�������������");
                return;
            }

            for (int i = 0; i < TDDailySigninTable.dataList.Count; i++)
            {
                TDDailySignin config = TDDailySigninTable.dataList[i];
                RewardBase reward = RewardMgr.S.GetRewardBase(config.reward);
                SignInItem item = new SignInItem(i, m_SignItemTrans[i], reward);
                if (reward.RewardItem != RewardItemType.Kongfu)
                {
                    // Debug.LogError(reward.SpriteName());
                    item.m_IconImage.sprite = FindSprite(reward.SpriteName());
                }
                if (reward.RewardItem != RewardItemType.Coin)
                    item.m_IconImage.SetNativeSize();

                m_SignItemDic.Add(i, item);
            }
            EventSystem.S.Register(EngineEventID.OnSignStateChange, OnSignStateChange);
        }

        private void SignItemCallBack(int id)
        {
            if (SignSystemMgr.S.weekSignState.isSignAble)
            {
                SignSuccess(id);
                //Custom event
                GameDataMgr.S.GetPlayerData().AddSignInCount(1);
                DateTime now = DateTime.Now;
                DateTime firstPlayTime = DateTime.Parse(GameDataMgr.S.GetPlayerData().firstPlayTime);
                TimeSpan timeSpan = now - firstPlayTime;
            }
            UpdateSignItemStatus();
        }

        /// <summary>
        /// ǩ���ɹ�
        /// </summary>
        /// <param name="id"></param>
        private void SignSuccess(int id)
        {
            EventSystem.S.Send(EventID.OnSignSuccess);

            SignSystemMgr.S.weekSignState.Sign();
            SignInItem item = m_SignItemDic[id];

            item.RewardCfg.AcceptReward();
            UIMgr.S.OpenTopPanel(UIID.RewardPanel, null, new List<RewardBase> { item.RewardCfg });
        }

        private void OnSignStateChange(int key, params object[] args)
        {
            UpdateSignItemStatus();
        }

        /// <summary>
        /// ����SignItemDic
        /// </summary>
        private void ClearSignItem()
        {
            foreach (var item in m_SignItemDic)
            {
                item.Value.SignItemCallBack = null;
            }
            m_SignItemDic.Clear();
        }

        /// <summary>
        /// ����ǩ��ͼ��״̬
        /// </summary>
        private void UpdateSignItemStatus()
        {
            int lastSignIndex = SignSystemMgr.S.weekSignState.lastSignIndex;
            int ableSignIndex = SignSystemMgr.S.weekSignState.signAbleIndex;

            if (SignSystemMgr.S.weekSignState.isSignAble)
            {
                foreach (var item in m_SignItemDic)
                {
                    if (item.Key == ableSignIndex)
                    {
                        item.Value.SetSignItemStatus(SignInStatus.SignEnable);
                    }
                    else if (item.Key <= lastSignIndex)
                    {
                        item.Value.SetSignItemStatus(SignInStatus.SignAlready);
                    }
                    else
                    {
                        item.Value.SetSignItemStatus(SignInStatus.SignDisable);
                    }
                }
            }
            else
            {
                foreach (var item in m_SignItemDic)
                {
                    if (item.Key <= lastSignIndex)
                    {
                        item.Value.SetSignItemStatus(SignInStatus.SignAlready);
                    }
                    else
                    {
                        item.Value.SetSignItemStatus(SignInStatus.SignDisable);
                    }
                }
            }
        }

    }
}