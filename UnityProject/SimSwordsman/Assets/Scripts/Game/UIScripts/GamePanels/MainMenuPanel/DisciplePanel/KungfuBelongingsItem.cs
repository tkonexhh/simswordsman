using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class KungfuBelongingsItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Plus;
        [SerializeField]
        private GameObject m_Lock;
        [SerializeField]
        private Image m_KungfuBelongingsItemImg;
        [SerializeField]
        private Image m_ItemIcon;
        [SerializeField]
        private Image m_KungfuName;
        [SerializeField]
        private GameObject m_RedPoint;

        private CharacterItem m_CharacterItem = null;
        private CharacterKongfuData m_CharacterKongfuData = null;
        private BgColorType m_BgColorType;
        public void OnInit(CharacterItem characterItem, CharacterKongfuData characterKongfuData, BgColorType bgColorType)
        {
            EventSystem.S.Register(EventID.OnKungfuRedPoint, HandAddlistenerEvent);

            m_CharacterItem = characterItem;
            m_CharacterKongfuData = characterKongfuData;
            m_BgColorType = bgColorType;
            RefreshPanelInro();

            RefreshBgColor(m_BgColorType);
        }
        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EventID.OnKungfuRedPoint, HandAddlistenerEvent);
        }

        private void HandAddlistenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnKungfuRedPoint:
                    if ((int)param[0] == m_CharacterItem.id && m_CharacterKongfuData.Index == ((int)param[1]))
                        m_RedPoint.SetActive((bool)param[2]);
                    break;
                default:
                    break;
            }
        }

        public void RefreshPanelInro()
        {
            switch (m_CharacterKongfuData.KungfuLockState)
            {
                case KungfuLockState.Learned:
                    m_ItemIcon.sprite = CommonMethod.GetKungfuBg(m_CharacterKongfuData.GetKungfuType());
                    m_KungfuName.sprite = CommonMethod.GetKungName(m_CharacterKongfuData.GetKungfuType());
                    m_ItemIcon.gameObject.SetActive(true);
                    m_KungfuName.gameObject.SetActive(true);
                    m_RedPoint.SetActive(false);
                    break;
                case KungfuLockState.NotLearning:
                    m_Plus.SetActive(true);
                    break;
                case KungfuLockState.NotUnlocked:
                    m_Lock.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void RefreshBgColor(BgColorType m_BgColorType)
        {
            switch (m_BgColorType)
            {
                case BgColorType.Black:
                    m_KungfuBelongingsItemImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg21");
                    break;
                case BgColorType.White:
                    m_KungfuBelongingsItemImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg28");
                    break;
            }
        }
    }
}