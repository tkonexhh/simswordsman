using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class PromotionPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_PromotionTitle;
        [SerializeField]
        private Text m_PromotionInfo;
        [SerializeField]
        private Button m_ExitBtn;
        [SerializeField]
        private Image m_CharacterImage;

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
            PromotionBase promotionModel = (PromotionBase)args[0];
            switch(promotionModel.GetEventID())
            {
                case EventID.OnCharacterUpgrade:
                    m_CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(promotionModel.GetCharacterItem());
                    stage = promotionModel.ToSubType<DiscipleRiseStage>().GetStage();
                    m_PromotionTitle.text = "µÜ×ÓÉý¶Î";
                    m_PromotionInfo.text = m_CharacterItem.name + "ÉýÖÁ" + CommonUIMethod.GetTextNumber(stage) + "¶ÎµÜ×Ó";
                    break;
                case EventID.OnKongfuLibraryUpgrade:
                    m_CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(promotionModel.GetCharacterItem());
                    kungfu = promotionModel.ToSubType<WugongBreakthrough>().GetWugongBreakthrough();
                    m_PromotionTitle.text = "Îä¹¦Í»ÆÆ";
                    m_PromotionInfo.text = m_CharacterItem.name + "µÄ"+ kungfu.kongfuType.ToString()+"ÉýÖÁ" + CommonUIMethod.GetTextNumber(kungfu.level) + "²ã";
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
            });
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            m_CharacterLoader.Release();
            CloseSelfPanel();
            PanelPool.S.CurShowPanelIsOver = false;
        }

    }
	
}