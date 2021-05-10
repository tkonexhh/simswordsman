using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameWish.Game
{
    public class VisitorPanel : AbstractAnimPanel
    {

        [SerializeField] private Image m_Character;
        [SerializeField] private Text m_Desc;
        [SerializeField] private ImgFontPre[] m_TitleBgs;
        [SerializeField] private Image m_RewardIcon;
        [SerializeField] private Text m_RewardName;
        [SerializeField] private Text m_RewardNum;
        [SerializeField] private Text m_TxtCurNum;

        [SerializeField] private Button m_AcceptBtn;
        [SerializeField] private Button m_NotAcceptBtn;
        [SerializeField] private Button m_CloseBtn;

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

                for (int i = 0; i < m_TitleBgs.Length; i++)
                {
                    if (i < tb.name.Length)
                    {
                        m_TitleBgs[i].gameObject.SetActive(true);
                        m_TitleBgs[i].SetFontCont(tb.name[i].ToString());
                    }
                    else
                        m_TitleBgs[i].gameObject.SetActive(false);
                }
                m_Desc.text = tb.desc;
                m_RewardIcon.sprite = FindSprite(m_visitor.Reward.SpriteName());
                m_RewardIcon.SetNativeSize();
                m_RewardName.text = m_visitor.Reward.RewardName();
                m_RewardNum.text = m_visitor.Reward.Count.ToString();
                long targetNum = RewardMgr.S.GetOwnRewardCount(m_visitor.Reward);
                m_TxtCurNum.text = CommonUIMethod.GetTenThousandOrMillion(targetNum);
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
        }

        protected override void OnClose()
        {
            base.OnClose();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}