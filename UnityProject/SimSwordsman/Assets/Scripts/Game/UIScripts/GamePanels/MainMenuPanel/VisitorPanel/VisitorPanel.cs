using Qarth;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class VisitorPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_Title;
        [SerializeField]
        private Text m_Desc;
        [SerializeField]
        private Image m_Pic;
        [SerializeField]
        private Image m_RewardIcon;
        [SerializeField]
        private Text m_RewardDesc;

        [SerializeField]
		private Button m_AcceptBtn;
		[SerializeField]
		private Button m_NotAcceptBtn;

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
                m_visitor.Disappear();
                VisitorSystem.S.CheckVisitorList();

                TDVisitorConfig tb = TDVisitorConfigTable.GetData(m_visitor.VisitorCfgID);
                m_Title.text = tb.name;
                m_Desc.text = tb.desc;
                m_RewardIcon.sprite = m_visitor.Reward.GetSprite();
                m_RewardDesc.text = m_visitor.Reward.RewardName() + " ×" + m_visitor.Reward.Count;
            }

            OpenDependPanel(EngineUI.MaskPanel,-1,null);
        }
        
        
        private void BindAddListenerEvent()
        {
            m_AcceptBtn.onClick.AddListener(()=> 
            {
                FloatMessage.S.ShowMsg("应当看广告");

                m_visitor.Reward.AcceptReward();
                VisitorSystem.S.StartAppearVisitorCountdown();

                HideSelfWithAnim();
            });
            m_NotAcceptBtn.onClick.AddListener(() =>
            {
                VisitorSystem.S.StartAppearVisitorCountdown();
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