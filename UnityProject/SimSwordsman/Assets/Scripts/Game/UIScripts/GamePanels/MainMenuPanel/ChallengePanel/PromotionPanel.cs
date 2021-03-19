using Qarth;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PromotionPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Image m_PromotionTitleImg;
        [SerializeField]
        private Text m_Cont;
        [SerializeField]
        private Text m_KongfuName;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Text m_Skill;
        [SerializeField]
        private Image m_CharacterImage;
        [SerializeField]
        private Button m_ExitBtn;

        private const float ExitShowTime = 0.5f;

        private CharacterItem m_CharacterItem = null;
        private int stage = 0;
        private CharacterKongfuDBData kungfu = null;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            PromotionBase promotionModel = (PromotionBase)args[0];
            m_CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(promotionModel.GetCharacterItem());
            //m_CharacterName.text = m_CharacterItem.name;
            switch (promotionModel.GetEventID())
            {
                case UpgradePanelType.WeaponEnhancement:
                    break;
                case UpgradePanelType.ArmorEnhancement:
                    break;
                case UpgradePanelType.EquipAmrs:
                    break;
                case UpgradePanelType.EquipAmror:
                    break;
                case UpgradePanelType.LearnMartialArts:
                    break;
                case UpgradePanelType.DiscipleAscendingSection:
                    AudioMgr.S.PlaySound(Define.CLEVELUP);
                    DiscipleRiseStage discipleRiseStage = promotionModel.ToSubType<DiscipleRiseStage>();
                    m_PromotionTitleImg.sprite = FindSprite("PromotionPanel_DiscipleAscendingSection");
                    m_Cont.text = m_CharacterItem.name+"����"+ CommonUIMethod.GetTextNumber(discipleRiseStage.GetStage()) + "�ε���";
                    CommonUIMethod.TextFlipUpEffect(m_Skill, discipleRiseStage.GetPreAtk(), m_CharacterItem.atkValue);
                    break;
                case UpgradePanelType.BreakthroughMartialArts:
                    kungfu = promotionModel.ToSubType<WugongBreakthrough>().GetWugongBreakthrough();
                    m_PromotionTitleImg.sprite = FindSprite("promotionpanel_title1");
                    //m_KongfuName.gameObject.SetActive(true);
                    m_KongfuName.text = TDKongfuConfigTable.GetData((int)kungfu.kongfuType).kongfuName;
                    m_Level.text = CommonUIMethod.GetTextNumber(kungfu.level) + "��";
                    break;
                default:
                    break;
            }

            CharacterQuality quality = m_CharacterItem.quality;
            int headId = m_CharacterItem.headId;
            int bodyId = m_CharacterItem.bodyId;
            string spriteName = quality.ToString().ToLower() + "_" + bodyId + "_" + headId;
            m_CharacterImage.sprite = FindSprite(spriteName);
            m_CharacterImage.SetNativeSize();

            Timer.S.Post2Really((i)=> {
                m_ExitBtn.gameObject.SetActive(true);
            }, ExitShowTime);
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
            PanelPool.S.CurShowPanelIsOver = false;
        }
    }
}