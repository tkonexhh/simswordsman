using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class TrialClanImg : MonoBehaviour
    {
        [SerializeField]
        private Image m_ClanImg;

        private ClanType m_ClanType;
        public ClanType CurClanType
        {
            get => m_ClanType;
        }
    public void OnInit(int clanType)
    {
        m_ClanType = (ClanType)clanType;
        RefreshClanImg();
    }

    public void SetSelectedClan()
    {
        switch (m_ClanType)
        {
            case ClanType.Gaibang:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Gaibang");
                break;
            case ClanType.Shaolin:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Shaolin");
                break;
            case ClanType.Wudang:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Wudang");
                break;
            case ClanType.Emei:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Emei");
                break;
            case ClanType.Huashan:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Huashan");
                break;
            case ClanType.Wudu:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Wudu");
                break;
            case ClanType.Mojiao:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Mojiao");
                break;
            case ClanType.Xiaoyao:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_YesClan_Xiaoyao");
                break;
        }
    }

    private void RefreshClanImg()
    {
        switch (m_ClanType)
        {
            case ClanType.Gaibang:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Gaibang");
                break;
            case ClanType.Shaolin:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Shaolin");
                break;
            case ClanType.Wudang:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Wudang");
                break;
            case ClanType.Emei:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Emei");
                break;
            case ClanType.Huashan:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Huashan");
                break;
            case ClanType.Wudu:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Wudu");
                break;
            case ClanType.Mojiao:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Mojiao");
                break;
            case ClanType.Xiaoyao:
                m_ClanImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_NotClan_Xiaoyao");
                break;
        }
    }
}
	
}