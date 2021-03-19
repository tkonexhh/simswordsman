using Qarth;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum ShowState
    {
        StateDiscipleAscendingSection,
        StateBreakthroughMartialArts,
        State3,
    }

    public class PromotionPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Image m_PromotionTitleImg;


        [SerializeField]
        private Text m_Cont;

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
            return TDKongfuConfigTable.GetIconName(kungfuType);
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
                    break;
                case UpgradePanelType.DiscipleAscendingSection:
                    AudioMgr.S.PlaySound(Define.CLEVELUP);
                    DiscipleRiseStage discipleRiseStage = promotionModel.ToSubType<DiscipleRiseStage>();
                    m_PromotionTitleImg.sprite = FindSprite("PromotionPanel_DiscipleAscendingSection");
                    SetDifferetState(ShowState.StateDiscipleAscendingSection);
                    m_Cont.text = m_CharacterItem.name+"����"+ CommonUIMethod.GetTextNumber(discipleRiseStage.GetStage()) + "�ε���";
                    CommonUIMethod.TextFlipUpEffect(m_Skill, discipleRiseStage.GetPreAtk(), m_CharacterItem.atkValue);
                    break;
                case UpgradePanelType.BreakthroughMartialArts:
                    SetDifferetState(ShowState.StateBreakthroughMartialArts);
                    kungfu = promotionModel.ToSubType<WugongBreakthrough>().GetWugongBreakthrough();
                    m_PromotionTitleImg.sprite = FindSprite("PromotionPanel_BreakthroughMartialArts");
                    m_InfoParName.text = m_CharacterItem.name + "��";
                    SetKungfuSprite(kungfu, m_InfoParIcon, m_KungfuNameImg);
                    m_KungfuName.text = CommonUIMethod.GetStrForColor("#4C6AA5", GetKungfuName(kungfu.kongfuType));
                    m_Paragraph.text = "����" + CommonUIMethod.GetPart(kungfu.level);
                    break;
                default:
                    break;
            }
            //[SerializeField]
            //private GameObject m_InfoPar;
            //[SerializeField]
            //private Text m_InfoParName;
            //[SerializeField]
            //private Image m_InfoParIcon;
            //[SerializeField]
            //private Image m_KungfuNameImg;
            //[SerializeField]
            //private Text m_KungfuName;
            //[SerializeField]
            //private Text m_Paragraph;

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

        private void SetDifferetState(ShowState showState)
        {
            switch (showState)
            {
                case ShowState.StateDiscipleAscendingSection:
                    m_Cont.gameObject.SetActive(true);
                    m_InfoPar.SetActive(false);
                    break;
                case ShowState.StateBreakthroughMartialArts:
                    m_Cont.gameObject.SetActive(false);
                    m_InfoPar.SetActive(true);
                    break;
                case ShowState.State3:
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