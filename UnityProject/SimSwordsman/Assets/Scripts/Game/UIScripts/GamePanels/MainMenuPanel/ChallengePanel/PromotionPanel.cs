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
        private Text m_CharacterName;
        [SerializeField]
        private Text m_KongfuName;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Image m_CharacterImage;
        [SerializeField]
        private Button m_ExitBtn;

        private CharacterItem m_CharacterItem = null;
        private int stage = 0;
        private CharacterKongfuDBData kungfu = null;
        private AddressableAssetLoader<Sprite> m_CharacterLoader = new AddressableAssetLoader<Sprite>();

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel,-1,null);
            PromotionBase promotionModel = (PromotionBase)args[0];
            m_CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(promotionModel.GetCharacterItem());
            m_CharacterName.text = m_CharacterItem.name;
            switch (promotionModel.GetEventID())
            {
                case EventID.OnCharacterUpgrade:
                    stage = promotionModel.ToSubType<DiscipleRiseStage>().GetStage();
                    m_PromotionTitleImg.sprite = FindSprite("promotionpanel_title2");
                  
                    m_KongfuName.gameObject.SetActive(false);
                    m_Level.text = CommonUIMethod.GetTextNumber(stage) + "¶Î";
                    break;
                case EventID.OnKongfuLibraryUpgrade:
                    kungfu = promotionModel.ToSubType<WugongBreakthrough>().GetWugongBreakthrough();
                    m_PromotionTitleImg.sprite = FindSprite("promotionpanel_title1");

                    m_KongfuName.gameObject.SetActive(true);
                    m_KongfuName.text = kungfu.kongfuType.ToString();
                    m_Level.text = CommonUIMethod.GetTextNumber(kungfu.level) + "²ã";
                    break;
                default:
                    break;
            }

            CharacterQuality quality = m_CharacterItem.quality;
            int headId = m_CharacterItem.headId;
            int bodyId = m_CharacterItem.bodyId;
            string spriteName = quality.ToString().ToLower() + "_" + bodyId + "_" + headId;
            m_CharacterLoader.LoadAssetAsync(spriteName, (sprite)=>
            {
                m_CharacterImage.sprite = sprite;
                m_CharacterImage.SetNativeSize();
            });
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            m_CharacterLoader.Release();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
            PanelPool.S.CurShowPanelIsOver = false;
        }
    }
}