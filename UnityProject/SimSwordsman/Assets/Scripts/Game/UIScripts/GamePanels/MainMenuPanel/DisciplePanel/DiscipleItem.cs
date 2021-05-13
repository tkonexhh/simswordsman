using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class DiscipleItem : MonoBehaviour
    {
        [Header("背景图片")]
        [SerializeField]
        private Image m_DiscipleItem;       
        [SerializeField]
        private Image m_NotYet;
        [SerializeField]
        private Image m_HeadTra;   
        [SerializeField]
        private Image m_DiscipleNameBg;

        [SerializeField]
        private GameObject m_Frame01;

        [SerializeField]
        private Button m_DiscipleBtn; 
        [SerializeField]
        private Image m_DiscipleQuality;
        [SerializeField]
        private Text m_DiscipleName; 
        [SerializeField]
        private Text m_DiscipleLevel;

        [SerializeField]
        private Transform m_DiscipleHeadTra;  
        [SerializeField]
        private Transform m_BelongingsTra;   
        [SerializeField]
        private GameObject m_EquipBelongingsItem; 
        [SerializeField]
        private GameObject m_KungfuBelongingsItem;

        private CharacterItem m_CharacterItem = null;
        private GameObject m_DiscipleHeadObj =null;
        private BgColorType m_BgColorType;
        private EquipBelongingsItem m_ArmorBelongingsItem = null;
        private EquipBelongingsItem m_ArmsBelongingsItem = null;

        private ResLoader m_Loader;
        private List<KungfuBelongingsItem> m_AllKungfuBelongingsItem = new List<KungfuBelongingsItem>();
        private void Start()
        {
           
        }

        public CharacterItem GetCharacterItem()
        {
            return m_CharacterItem;
        }

        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EventID.OnSelectedKungfuSuccess, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnSelectedEquipSuccess, HandAddListenerEvent);
            m_Loader.ReleaseAllRes();
        }

        public void OnInit(CharacterItem characterItem, BgColorType i)
        {
            BindAddListenerEvent();
            m_Loader = ResLoader.Allocate("BelongingsItem", null);
            if (m_EquipBelongingsItem == null || m_KungfuBelongingsItem == null)
            {
                m_EquipBelongingsItem = (GameObject)m_Loader.LoadSync("EquipBelongingsItem");
                m_KungfuBelongingsItem = (GameObject)m_Loader.LoadSync("KungfuBelongingsItem");
            }
            m_CharacterItem = characterItem;
            m_BgColorType = i;
            RefreshPanelInfo();
        }
        public void RefreshBgColor(BgColorType bgColorType)
        {
            m_BgColorType = bgColorType;
            RefreshBgPhoto();
            if (m_CharacterItem != null)
            {
                for (int i = 0; i < m_AllKungfuBelongingsItem.Count; i++)
                    m_AllKungfuBelongingsItem[i].RefreshBgColor(m_BgColorType);
                m_ArmorBelongingsItem?.RefreshBgColor(m_BgColorType);
                m_ArmorBelongingsItem?.RefreshBgColor(m_BgColorType);
            }
            else
            {
                m_NotYet.gameObject.SetActive(true);
                switch (m_BgColorType)
                {
                    case BgColorType.Black:
                        break;
                    case BgColorType.White:
                        m_NotYet.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg25");
                        break;
                    default:
                        break;
                }
            }
        }

        private void RefreshPanelInfo(bool isRefresh = false)
        {
            RefreshBgPhoto();

            if (m_CharacterItem!=null)
            {
                if (m_DiscipleHeadObj == null)
                {
                    m_DiscipleHeadObj = DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CharacterItem, m_DiscipleHeadTra, new Vector3(0, 0, 0), new Vector3(0.4f, 0.4f, 1));
                }

                m_DiscipleBtn.gameObject.SetActive(true);
                m_Frame01.SetActive(true);

                m_DiscipleName.text = m_CharacterItem.name;
                switch (m_CharacterItem.quality)
                {
                    case CharacterQuality.Normal:
                        m_DiscipleQuality.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Normal");
                        break;
                    case CharacterQuality.Good:
                        m_DiscipleQuality.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Good");
                        break;
                    case CharacterQuality.Perfect:
                        m_DiscipleQuality.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Perfect");
                        break;
                    case CharacterQuality.Hero:
                        m_DiscipleQuality.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Hero");
                        break;
                    default:
                        break;
                }
                SetLevelFontColor();
                SetKungfuBelongingsItem();
                SetEquipBelongingsItem();
            }
            else
            {
                m_NotYet.gameObject.SetActive(true);
                switch (m_BgColorType)
                {
                    case BgColorType.Black:
                        break;
                    case BgColorType.White:
                        m_NotYet.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg25");
                        break;
                    default:
                        break;
                }
              
            }
        }

        private void RefreshBgPhoto()
        {
            switch (m_BgColorType)
            {
                case BgColorType.Black:
                    m_DiscipleItem.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg16");
                    m_HeadTra.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg18");
                    m_DiscipleNameBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg19");
                    break;
                case BgColorType.White:
                    m_DiscipleItem.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg17");
                    m_DiscipleNameBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg26");
                    m_HeadTra.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg27");
                    break;
            }
         
        }

        private void SetEquipBelongingsItem()
        {
            m_ArmsBelongingsItem = Instantiate(m_EquipBelongingsItem, m_BelongingsTra).GetComponent<EquipBelongingsItem>();
            m_ArmsBelongingsItem.OnInit(m_CharacterItem, m_CharacterItem.characeterEquipmentData.IsArmsUnlock
                , m_CharacterItem.characeterEquipmentData.CharacterArms, m_BgColorType, PropType.Arms);

            m_CharacterItem.CheckArms();

            m_ArmorBelongingsItem = Instantiate(m_EquipBelongingsItem, m_BelongingsTra).GetComponent<EquipBelongingsItem>();
            m_ArmorBelongingsItem.OnInit(m_CharacterItem, m_CharacterItem.characeterEquipmentData.IsArmorUnlock
                ,m_CharacterItem.characeterEquipmentData.CharacterArmor, m_BgColorType, PropType.Armor);

            m_CharacterItem.CheckArmor();
        }

        private void SetKungfuBelongingsItem()
        {
            Dictionary<int, CharacterKongfuData> kongfus = m_CharacterItem.kongfus;
            for (int i = 0; i < kongfus.Count; i++)
            {
                KungfuBelongingsItem kungfuBelongingsItem = Instantiate(m_KungfuBelongingsItem, m_BelongingsTra).GetComponent<KungfuBelongingsItem>();
                kungfuBelongingsItem.OnInit(m_CharacterItem, kongfus[i+1], m_BgColorType);
                m_AllKungfuBelongingsItem.Add(kungfuBelongingsItem);
            }
            m_CharacterItem.CheckKungfuRedPoint();
        }

        private void SetLevelFontColor()
        {
            switch (m_CharacterItem.quality)
            {
                case CharacterQuality.Normal:
                    m_DiscipleLevel.text = CommonUIMethod.GetStrForColor("#577891",CommonUIMethod.GetGrade(m_CharacterItem.level));
                    break;
                case CharacterQuality.Good:
                    m_DiscipleLevel.text = CommonUIMethod.GetStrForColor("#70539A", CommonUIMethod.GetGrade(m_CharacterItem.level));
                    break;
                case CharacterQuality.Perfect:
                    m_DiscipleLevel.text = CommonUIMethod.GetStrForColor("#A15A55", CommonUIMethod.GetGrade(m_CharacterItem.level));
                    break;
                case CharacterQuality.Hero:
                    m_DiscipleLevel.text = CommonUIMethod.GetStrForColor("#AE711E", CommonUIMethod.GetGrade(m_CharacterItem.level));
                    break;
            }

        }

        /// <summary>
        /// 获取当前弟子的id
        /// </summary>
        /// <returns></returns>
        public int GetCurDiscipleId()
        {
            if (m_CharacterItem != null)
                return m_CharacterItem.id;
            return -1;
        }

        private void BindAddListenerEvent()
        {
            EventSystem.S.Register(EventID.OnSelectedKungfuSuccess,HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnSelectedEquipSuccess, HandAddListenerEvent);
            m_DiscipleBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.DicipleDetailsPanel, m_CharacterItem);
            });
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedKungfuSuccess:
                    if (m_CharacterItem != null && (int)param[1] == m_CharacterItem.id)
                    {
                        foreach (var item in m_AllKungfuBelongingsItem)
                        {
                            item.RefreshPanelInro();
                        }
                    }
                    break;
                case EventID.OnSelectedEquipSuccess:
                    if (m_CharacterItem != null && (int)param[0] == m_CharacterItem.id)
                    {
                        switch ((PropType)param[1])
                        {
                            case PropType.Arms:
                                m_ArmsBelongingsItem.RefreshPanelInfo();
                                break;
                            case PropType.Armor:
                                m_ArmorBelongingsItem.RefreshPanelInfo();
                                break;
                        }
                    }
                    break;
            }
        }

        public void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }
    }
}