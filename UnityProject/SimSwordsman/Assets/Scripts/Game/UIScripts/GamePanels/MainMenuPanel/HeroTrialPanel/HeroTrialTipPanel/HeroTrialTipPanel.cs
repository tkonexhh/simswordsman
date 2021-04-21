using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameWish.Game
{
   
    public class HeroTrialTipPanel : AbstractAnimPanel
	{

        [SerializeField]
        private Button m_CloseBtn;      
        [SerializeField]
        private Button m_BlackBtn;
        [SerializeField]
        private Button m_GotoBtn;     
        [SerializeField]
        private Image m_ClanImg;
        [SerializeField]
        private Text m_Cont;

        private ClanType m_ClanType;
        protected override void OnUIInit()
	    {
            base.OnUIInit();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_ClanType = MainGameMgr.S.HeroTrialMgr.TrialClan;
            //m_ClanType = MainGameMgr.S.HeroTrialMgr.GetClanType();
            SetContAndSprite();
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
            m_GotoBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            OpenDependPanel(EngineUI.MaskPanel,-1,null);
        }

       

        private void SetContAndSprite()
        {
            switch (m_ClanType)
            {
                case ClanType.None:
                    break;
                case ClanType.Gaibang:
                    m_Cont.text = GetCont("丐帮");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Gaibang");
                    break;
                case ClanType.Shaolin:
                    m_Cont.text = GetCont("少林派");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Shaolin");
                    break;
                case ClanType.Wudang:
                    m_Cont.text = GetCont("武当派");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Wudang");
                    break;
                case ClanType.Emei:
                    m_Cont.text = GetCont("峨眉派");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Emei");
                    break;
                case ClanType.Huashan:
                    m_Cont.text = GetCont("华山派");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Huashan");
                    break;
                case ClanType.Wudu:
                    m_Cont.text = GetCont("五毒");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Wudu");
                    break;
                case ClanType.Mojiao:
                    m_Cont.text = GetCont("魔教");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Mojiao");
                    break;
                case ClanType.Xiaoyao:
                    m_Cont.text = GetCont("逍遥派");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Xiaoyao");
                    break;
              
            }
        }

        private string GetCont(string clanName)
        {
            return "今日前往挑战的是" + CommonUIMethod.GetStrForColor("#405787", clanName) + "试炼,放心，弟子只会变强但不会变秃！";
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