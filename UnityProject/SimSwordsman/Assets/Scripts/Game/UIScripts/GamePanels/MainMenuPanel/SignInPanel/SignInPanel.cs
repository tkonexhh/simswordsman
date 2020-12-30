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
        private Sprite[] m_NumberSprite = null;
        private Action m_GetRewardPanelCallBack;

        [SerializeField] private Color m_BlurMaskColor;
        [SerializeField] private Transform[] m_SignItemTrans;
        [SerializeField] private Button m_BackBtn;//返回按钮

        //[Header("Image")]
        //[SerializeField] private Image m_DaysShowNum;//显示签到天数
        //[SerializeField] private Image m_Title;
        //[SerializeField] private Image m_DayHint;

        [SerializeField] private Button m_AcceptBtn;//返回按钮

        //protected override void OnUIInit()
        //{
        //    base.OnUIInit();
        //}

        //protected override void OnOpen()
        //{
        //    base.OnOpen();
        //}

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen();

            InitSignItem();
            SignSystem.S.weekSignState.Check();

            //UIMgr.S.OpenPanel(UIID.BlurMaskPanel);

            m_GetRewardPanelCallBack += GetRewardCallBack;
            //m_Title.sprite = UIMgrExtend.FindLanguageSprite(TDLanguageTable.Get("SignPanel_Title"));
            //m_DayHint.sprite = UIMgrExtend.FindLanguageSprite(TDLanguageTable.Get("SignPanel_DayItem"));

            foreach (var item in m_SignItemDic)
            {
                item.Value.SignItemCallBack = SignItemCallBack;
            }

            m_BackBtn.onClick.AddListener(OnBackBtCallBack);
            m_AcceptBtn.onClick.AddListener(OnClickAccept);

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
                //Debug.LogError(config.rewardParam);
                //string spriteName = "SignPanel_" + item.SignConfig.RewardParameter;
                item.SetIconSprite(reward.GetSprite());
                m_SignItemDic.Add(id, item);
            }

            EventSystem.S.Register(EngineEventID.OnSignStateChange, OnSignStateChange);
            UpdateSignItemStatus();
        }

        private void SignItemCallBack(int id)
        {
            if (SignSystem.S.weekSignState.isSignAble)
            {
                SignSuccess(id);
                //GameplayMgr.S.CheckIsFirstSign();
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

            SignSystem.S.weekSignState.Sign();
            SignInItem item = m_SignItemDic[id];

            item.RewardCfg.AcceptReward();
            UIMgr.S.OpenTopPanel(UIID.GetRewardPanel, null, m_GetRewardPanelCallBack, item.RewardCfg);
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
            int lastSignIndex = SignSystem.S.weekSignState.lastSignIndex;
            int ableSignIndex = SignSystem.S.weekSignState.signAbleIndex;


            if (SignSystem.S.weekSignState.isSignAble)
            {
                //SetSignDayNum(ableSignIndex);
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
                //SetSignDayNum(lastSignIndex);
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

        /// <summary>
        /// 设置Title中显示的签到天数
        /// </summary>
        /// <param name="index"></param>
        //private void SetSignDayNum(int index)
        //{
        //    string numName = "SignPanel_Num" + (index + 1);
        //    string spriteName = TDLanguageTable.Get(numName);

        //    if (m_NumberSprite == null)
        //    {
        //        Sprite[] sprites1 = Resources.LoadAll<Sprite>("UI/Language/SignPanel_Num");
        //        Sprite[] sprites2 = Resources.LoadAll<Sprite>("UI/Language/SignPanel_Num_TW");
        //        m_NumberSprite = sprites1.ToArray().Union(sprites2).ToArray();
        //    }
        //    Sprite sprite = null;
        //    foreach (Sprite s in m_NumberSprite)
        //    {
        //        if (s.name == spriteName)
        //        {
        //            sprite = s;
        //            break;
        //        }
        //    }
        //    m_DaysShowNum.sprite = sprite;
        //}
    }
	
}