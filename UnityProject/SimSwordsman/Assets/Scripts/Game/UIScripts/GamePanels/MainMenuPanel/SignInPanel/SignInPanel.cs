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
        private Action m_GetRewardPanelCallBack;

        [SerializeField] private Color m_BlurMaskColor;
        [SerializeField] private Transform[] m_SignItemTrans;
        [SerializeField] private Button m_BackBtn;//返回按钮

        //[Header("Image")]
        //[SerializeField] private Image m_DaysShowNum;//显示签到天数
        //[SerializeField] private Image m_Title;
        //[SerializeField] private Image m_DayHint;

        [SerializeField] private Button m_AcceptBtn;//返回按钮

        protected override void OnUIInit()
        {
            base.OnUIInit();
            //音效
            foreach (var item in transform.GetComponentsInChildren<Button>(true))
            {
                item.onClick.AddListener(() => AudioMgr.S.PlaySound(Define.SOUND_UI_BTN));
            }
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            InitSignItem();
            SignSystemMgr.S.weekSignState.Check();

            //UIMgr.S.OpenPanel(UIID.BlurMaskPanel);

            m_GetRewardPanelCallBack += GetRewardCallBack;
            //m_Title.sprite = UIMgrExtend.FindLanguageSprite(TDLanguageTable.Get("SignPanel_Title"));
            //m_DayHint.sprite = UIMgrExtend.FindLanguageSprite(TDLanguageTable.Get("SignPanel_DayItem"));

            foreach (var item in m_SignItemDic)
            {
                item.Value.SignItemCallBack = SignItemCallBack;
            }

            m_BackBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                OnBackBtCallBack();
            });
            m_AcceptBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                OnClickAccept();
            });

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

            m_BackBtn.onClick.RemoveListener(OnBackBtCallBack);
            m_AcceptBtn.onClick.RemoveListener(OnClickAccept);

            foreach (var item in m_SignItemDic)
            {
                item.Value.SignItemCallBack = null;
            }
            
            m_GetRewardPanelCallBack -= GetRewardCallBack;
        }

        void OnClickAccept()
        {
            foreach (var item in m_SignItemDic.Values)
            {
                if (item.Status == SignInStatus.SignEnable)
                {
                    item.ClickSignBtn();
                    return;
                }
            }
        }

        private void OnBackBtCallBack()
        {
            HideSelfWithAnim();
            //UIMgrExtend.S.TryCloesPanelWithAnimation(UIID.BlurMaskPanel);
        }

        protected override void BeforDestroy()
        {
            base.BeforDestroy();
            ClearSignItem();
        }

        /// <summary>
        /// 创建SignItem并且加入字典管理
        /// </summary>
        private void InitSignItem()
        {
            ClearSignItem();
            if (TDDailySigninTable.dataList.Count != 7)
            {
                Log.e("签到配表数量错误");
                return;
            }

            for (int i = 0; i < TDDailySigninTable.dataList.Count; i++)
            {
                int id = i;
                TDDailySignin config = TDDailySigninTable.dataList[i];
                RewardBase reward = RewardMgr.S.GetRewardBase(config.reward);
                SignInItem item = new SignInItem(id, m_SignItemTrans[i], reward);
                if (reward.Type != RewardItemType.Kongfu)
                {
                    item.m_IconImage.sprite = FindSprite(reward.SpriteName());
                }
                if (reward.Type != RewardItemType.Coin)
                    item.m_IconImage.SetNativeSize();

                m_SignItemDic.Add(id, item);
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
        /// 签到成功
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

        private void GetRewardCallBack()
        {
            //HideSelfWithAnim();
        }

        private void OnSignStateChange(int key, params object[] args)
        {
            UpdateSignItemStatus();
        }

        /// <summary>
        /// 清理SignItemDic
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
        /// 更新签到图标状态
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