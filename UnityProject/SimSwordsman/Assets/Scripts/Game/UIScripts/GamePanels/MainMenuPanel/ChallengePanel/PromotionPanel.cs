using Qarth;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PromotionPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Image m_PromotionTitleImg;

        [Header("Status One")]
        [SerializeField]
        private Text m_Cont;

        [Header("Status Two")]
        [SerializeField]
        private GameObject m_InfoPar;
        [SerializeField]
        private Text m_InfoParName;
        [SerializeField]
        private Image m_InfoParIcon;
        [SerializeField]
        private Image m_KungfuNameImg;
        [SerializeField]
        private Text m_KungfuName;
        [SerializeField]
        private Text m_Paragraph;

        [Header("Status Three")]
        [SerializeField]
        private GameObject m_LearnMartialArts;
        [SerializeField]
        private Text m_DiscipleLearn;
        [SerializeField]
        private Image m_KungfuBgImg;
        [SerializeField]
        private Image KungfuNameImg;
        [SerializeField]
        private Text KungfuName;

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
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        private string GetKungfuName(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).Name;
        }
        private void SetKungfuSprite(KungfuItem item, Image image, Image kungfuName)
        {
            kungfuName.gameObject.SetActive(true);
            switch (GetKungfuQuality(item.KungfuType))
            {
                case KungfuQuality.Normal:
                    image.sprite = FindSprite("Introduction");
                    break;
                case KungfuQuality.Super:
                    image.sprite = FindSprite("Advanced");
                    break;
                case KungfuQuality.Master:
                    image.sprite = FindSprite("Excellent");
                    break;
                default:
                    break;
            }
            kungfuName.sprite = FindSprite(TDKongfuConfigTable.GetIconName(item.KungfuType));
        }
        private void SetKungfuSprite(CharacterKongfuDBData item, Image image, Image kungfuName)
        {
            kungfuName.gameObject.SetActive(true);
            switch (GetKungfuQuality(item.kongfuType))
            {
                case KungfuQuality.Normal:
                    image.sprite = FindSprite("Introduction");
                    break;
                case KungfuQuality.Super:
                    image.sprite = FindSprite("Advanced");
                    break;
                case KungfuQuality.Master:
                    image.sprite = FindSprite("Excellent");
                    break;
                default:
                    break;
            }
            kungfuName.sprite = FindSprite(TDKongfuConfigTable.GetIconName(item.kongfuType));
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
                    m_PromotionTitleImg.sprite = FindSprite("PromotionPanel_LearnMartialArts");
                    m_DiscipleLearn.text = m_CharacterItem.name + "学会了";
                    LearnMartialArts learnMartialArts = promotionModel.ToSubType<LearnMartialArts>();
                    SetKungfuSprite(learnMartialArts.GetKungfuItem(), m_KungfuBgImg, KungfuNameImg);
                    KungfuName.text = CommonUIMethod.GetStrForColor("#4C6AA5", TDKongfuConfigTable.GetKungfuConfigInfo(learnMartialArts.GetKungfuItem().KungfuType).Name);
                    SetDifferetState(UpgradePanelType.LearnMartialArts);
                    break;
                case UpgradePanelType.DiscipleAscendingSection:
                    AudioMgr.S.PlaySound(Define.CLEVELUP);
                    DiscipleRiseStage discipleRiseStage = promotionModel.ToSubType<DiscipleRiseStage>();
                    m_PromotionTitleImg.sprite = FindSprite("PromotionPanel_DiscipleAscendingSection");
                    SetDifferetState(UpgradePanelType.DiscipleAscendingSection);
                    m_Cont.text = m_CharacterItem.name + "升至" + CommonUIMethod.GetTextNumber(discipleRiseStage.GetStage()) + "段弟子";
                    break;
                case UpgradePanelType.BreakthroughMartialArts:
                    SetDifferetState(UpgradePanelType.BreakthroughMartialArts);
                    WugongBreakthrough wugongBreakthrough = promotionModel.ToSubType<WugongBreakthrough>();
                    kungfu = wugongBreakthrough.GetWugongBreakthrough();
                    m_PromotionTitleImg.sprite = FindSprite("PromotionPanel_BreakthroughMartialArts");
                    m_InfoParName.text = m_CharacterItem.name + "的";
                    SetKungfuSprite(kungfu, m_InfoParIcon, m_KungfuNameImg);
                    m_KungfuName.text = CommonUIMethod.GetStrForColor("#4C6AA5", GetKungfuName(kungfu.kongfuType));
                    m_Paragraph.text = "升至" + CommonUIMethod.GetPart(kungfu.level);
                    break;
                default:
                    break;
            }
            CommonUIMethod.TextFlipUpEffect(m_Skill, promotionModel.GetPreAtk(), m_CharacterItem.atkValue);
            CharacterQuality quality = m_CharacterItem.quality;
            int headId = m_CharacterItem.headId;
            int bodyId = m_CharacterItem.bodyId;
            string spriteName = quality.ToString().ToLower() + "_" + bodyId + "_" + headId;
            m_CharacterImage.sprite = FindSprite(spriteName);
            m_CharacterImage.SetNativeSize();

            Timer.S.Post2Really((i) =>
            {
                m_ExitBtn.gameObject.SetActive(true);
            }, ExitShowTime);
        }

        private void SetDifferetState(UpgradePanelType showState)
        {
            switch (showState)
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
                    m_LearnMartialArts.SetActive(true);
                    break;
                case UpgradePanelType.DiscipleAscendingSection:
                    m_Cont.gameObject.SetActive(true);
                    break;
                case UpgradePanelType.BreakthroughMartialArts:
                    m_InfoPar.SetActive(true);
                    break;
                default:
                    break;
            }
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