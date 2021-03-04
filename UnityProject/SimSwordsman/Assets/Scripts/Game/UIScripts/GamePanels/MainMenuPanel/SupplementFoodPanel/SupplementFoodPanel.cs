using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class SupplementFoodPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_BlackBtn;
        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Text m_Cont;
        [SerializeField]
        private Text m_BaoziNum;
        [SerializeField]
        private Text m_TimesToday;

        private int m_SupplementFood = 50;
        private int m_CurFoodFreshCount;
        private const int _24House = 24;
        private const int FOODFRESHCOUNT = 5;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
            GetInformationForNeed();
            m_Cont.text = "弟子没饭吃，就没力气干活了，看一段广告补充" + m_SupplementFood + "食物吧";
            m_BaoziNum.text = m_SupplementFood.ToString();
            if (m_CurFoodFreshCount <= 0)
            {
                string recordTime = GameDataMgr.S.GetPlayerData().GetFoodRefreshRecordingTime();
                int house = CommonUIMethod.GetDeltaTime(recordTime);
                int count = house / _24House;
                int refreshCount = GameDataMgr.S.GetPlayerData().GetFoodRefreshCount();
                if (count > refreshCount)
                {
                    GameDataMgr.S.GetPlayerData().ResetFoodRefreshTimesToday();
                    GameDataMgr.S.GetPlayerData().SetFoodRefreshCount(count);
                }
            }
            RefreshPanelInfo();
        }

        private void GetInformationForNeed()
        {
            m_CurFoodFreshCount = GameDataMgr.S.GetPlayerData().GetFoodRefreshTimesToday();
        }

        private void RefreshPanelInfo()
        {
            m_TimesToday.text = "今日次数：" + m_CurFoodFreshCount + Define.SLASH + FOODFRESHCOUNT;
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
            m_AcceptBtn.onClick.AddListener(() =>
            {
                if (m_CurFoodFreshCount <= 0)
                {
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "今日食物补充次数已用完。");
                    return;
                }

                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                AdsManager.S.PlayRewardAD("AddFood",LookADSuccessCallBack);
                HideSelfWithAnim();
            });
        }

        private void LookADSuccessCallBack(bool obj)
        {
            GameDataMgr.S.GetPlayerData().SetFoodRefreshTimesToday();
            GameDataMgr.S.GetPlayerData().AddFoodNum(m_SupplementFood);

            GetInformationForNeed();
            RefreshPanelInfo();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }

}