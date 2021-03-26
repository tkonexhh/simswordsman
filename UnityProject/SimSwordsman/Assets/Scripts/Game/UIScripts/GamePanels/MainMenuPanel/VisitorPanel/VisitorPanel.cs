using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameWish.Game
{
    public class VisitorPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_Title;
        [SerializeField]
        private Image m_Character;
        [SerializeField]
        private Text m_Desc;
        [SerializeField]
        private Image[] m_TitleBgs;
        [SerializeField]
        private Image m_RewardIcon;
        [SerializeField]
        private Text m_RewardName;
        [SerializeField]
        private Text m_RewardNum;

        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_NotAcceptBtn;
        [SerializeField]
        private Button m_CloseBtn;

        Visitor m_visitor;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            if (args != null)
            {
                m_visitor = VisitorSystem.S.CurrentVisitor[(int)args[0]];
                VisitorSystem.S.ShowInPanel(m_visitor);

                TDVisitorConfig tb = TDVisitorConfigTable.GetData(m_visitor.VisitorCfgID);
                m_Title.text = tb.name;
                for (int i = 0; i < m_TitleBgs.Length; i++)
                {
                    if (i < tb.name.Length)
                        m_TitleBgs[i].gameObject.SetActive(true);
                    else
                        m_TitleBgs[i].gameObject.SetActive(false);
                }
                m_Desc.text = tb.desc;
                m_RewardIcon.sprite = FindSprite(m_visitor.Reward.SpriteName());
                m_RewardIcon.SetNativeSize();
                m_RewardName.text = m_visitor.Reward.RewardName();
                m_RewardNum.text = m_visitor.Reward.Count.ToString();
                m_Character.sprite = FindSprite(tb.roleRes);
                m_Character.SetNativeSize();
            }

            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }

        private void LookADSuccessCallBack(bool obj)
        {
            m_visitor.Reward.AcceptReward();
            GameDataMgr.S.GetPlayerData().recordData.AddVisitor();
            UIMgr.S.OpenTopPanel(UIID.RewardPanel, null, new List<RewardBase>() { m_visitor.Reward });
            VisitorSystem.S.Disappear(m_visitor);
            GameDataMgr.S.GetPlayerData().SetNoBroadcastTimes(1);

            HideSelfWithAnim();
        }
        private void BindAddListenerEvent()
        {
            m_AcceptBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.visitor_receive, m_visitor.Reward.KeyID.ToString());

                AdsManager.S.PlayRewardAD("ReceiveVisitor", LookADSuccessCallBack);
            });
            m_NotAcceptBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.visitor_refuse, m_visitor.Reward.KeyID.ToString());

                VisitorSystem.S.Disappear(m_visitor);
                HideSelfWithAnim();
            });
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.visitor_refuse, m_visitor.Reward.KeyID.ToString());

                VisitorSystem.S.Disappear(m_visitor);
                HideSelfWithAnim();
            });
        }



        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}