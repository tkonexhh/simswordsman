using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class DeliverRes : MonoBehaviour
	{
		[SerializeField]
		private Image m_IconSprite;
        [SerializeField]
		private Image m_KungfuName;
		[SerializeField]
		private Text m_Number;
		private DeliverRewadData m_DeliverRewadData;

		public void OnInit(DeliverRewadData item)
		{
			m_DeliverRewadData = item;

            switch (item.RewardType)
            {
                case RewardItemType.Item:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas,TDItemConfigTable.GetIconName(m_DeliverRewadData.RewardID));
                    break;
                case RewardItemType.Armor:
                case RewardItemType.Arms:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.EquipmentAtlas, TDEquipmentConfigTable.GetIconName(m_DeliverRewadData.RewardID));
                    break;
                case RewardItemType.Kongfu:
                    SetKungfuSprite();
                    break;
                case RewardItemType.Medicine:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, TDHerbConfigTable.GetHerbIconNameById(m_DeliverRewadData.RewardID));
                    break;
                case RewardItemType.Food:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas,"Baozi");
                    break;
                case RewardItemType.Coin:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "Coin");
                    break;
                case RewardItemType.Exp_Role:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "PanelCommon_RoleExp");
                    break;
                case RewardItemType.Exp_Kongfu:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "PanelCommon_KungfuExp");
                    break;
            }
            m_Number.text = m_DeliverRewadData.RewardCount.ToString();
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        private void SetKungfuSprite()
        {
            m_KungfuName.gameObject.SetActive(true);
            switch (GetKungfuQuality((KungfuType)m_DeliverRewadData.RewardID))
            {
                case KungfuQuality.Normal:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Introduction");
                    break;
                case KungfuQuality.Master:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Advanced");
                    break;
                case KungfuQuality.Super:
                    m_IconSprite.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Excellent");
                    break;
                default:
                    break;
            }
            m_KungfuName.sprite = SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, TDKongfuConfigTable.GetIconName((KungfuType)m_DeliverRewadData.RewardID));
        }

        void Start()
	    {
	        
	    }
	
	    // Update is called once per frame
	    void Update()
	    {
	        
	    }
	}
	
}