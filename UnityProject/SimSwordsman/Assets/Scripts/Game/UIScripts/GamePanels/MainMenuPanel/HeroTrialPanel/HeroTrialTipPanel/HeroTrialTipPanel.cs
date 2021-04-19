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
                    m_Cont.text = GetCont("ؤ��");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Gaibang");
                    break;
                case ClanType.Shaolin:
                    m_Cont.text = GetCont("������");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Shaolin");
                    break;
                case ClanType.Wudang:
                    m_Cont.text = GetCont("�䵱��");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Wudang");
                    break;
                case ClanType.Emei:
                    m_Cont.text = GetCont("��ü��");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Emei");
                    break;
                case ClanType.Huashan:
                    m_Cont.text = GetCont("��ɽ��");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Huashan");
                    break;
                case ClanType.Wudu:
                    m_Cont.text = GetCont("�嶾");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Wudu");
                    break;
                case ClanType.Mojiao:
                    m_Cont.text = GetCont("ħ��");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Mojiao");
                    break;
                case ClanType.Xiaoyao:
                    m_Cont.text = GetCont("��ң��");
                    m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Big_Xiaoyao");
                    break;
              
            }
        }

        private string GetCont(string clanName)
        {
            return "����ǰ����ս����" + CommonUIMethod.GetStrForColor("#405787", clanName) + "����,���ģ�����ֻ���ǿ�������ͺ��";
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