using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class EquipDetailsPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_BlackBtn;
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Transform m_TopTra;
        [SerializeField]
        private GameObject m_ImgFontPre;
        [SerializeField]
        private Text m_BriefIntroduction;
        [SerializeField]
        private Image m_EquipItem;
        [SerializeField]
        private Image m_EquipQuality;
        [SerializeField]
        private Text m_EquipOrder;
        [SerializeField]
        private Text m_CurAddition;
        [SerializeField]
        private Text m_NextAddition;
        [SerializeField]
        private Image m_MaterialImg;
        [SerializeField]
        private Text m_MaterialNumber;
        [SerializeField]
        private Button m_StrengthenBtn;
        [SerializeField]
        private GameObject m_Two;      
        [SerializeField]
        private GameObject m_MaterialObj;

        private CharacterItem m_CurDisciple = null;
        private CharaceterEquipment m_CharaceterEquipment = null;
        private PropType m_PropType;

        private List<GameObject> m_FontList = new List<GameObject>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CharaceterEquipment = args[0] as CharaceterEquipment;
            m_CurDisciple = args[1] as CharacterItem;
            m_PropType = (PropType)args[2];

            m_BlackBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_StrengthenBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume(m_CharaceterEquipment.GetSubID(), m_CharaceterEquipment.Class + 1);

                if (upgrade == null)
                    return;

                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)upgrade.PropID, upgrade.Number);
                if (!isHave)
                {
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
                    return;
                }
                switch (m_PropType)
                {
                    case PropType.Arms:
                        PanelPool.S.AddPromotion(new WeaponEnhancement(m_CurDisciple.id, m_CurDisciple.atkValue, (CharacterArms)m_CharaceterEquipment));
                        break;
                    case PropType.Armor:
                        PanelPool.S.AddPromotion(new ArmorEnhancement(m_CurDisciple.id, m_CurDisciple.atkValue, (CharacterArmor)m_CharaceterEquipment));
                        break;
                }

                MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)upgrade.PropID), upgrade.Number);
                m_CharaceterEquipment.UpGradeClass(m_CurDisciple.id);
                m_CurDisciple.CalculateForceValue();
                EventSystem.S.Send(EventID.OnRefreshEquipInfo, m_PropType);
                EventSystem.S.Send(EventID.OnSelectedEquipSuccess, m_CurDisciple.id, m_PropType);
                RefershIntensifyImg();

                PanelPool.S.DisplayPanel();

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_equip_up, m_CharaceterEquipment.GetSubID() + ";" + m_CharaceterEquipment.Class.ToString());

                RefreshPanelInfo();
            });
            RefreshPanelInfo();
        }
        private void RefrshIntensifyText(Text text, UpgradeCondition upgrade)
        {
            int curNumber = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)upgrade.PropID);
            if (curNumber >= upgrade.Number)
                text.text = upgrade.Number + Define.SLASH + upgrade.Number;
            else
                text.text = CommonUIMethod.GetStrForColor("#8C343C", curNumber.ToString()) + Define.SLASH + upgrade.Number;
        }
        private void RefershIntensifyImg()
        {
            UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume(m_CharaceterEquipment.GetSubID(), m_CharaceterEquipment.Class + 1);

            if (upgrade == null)
            {
                m_StrengthenBtn.gameObject.SetActive(false);
                return;
            }

            RefrshIntensifyText(m_MaterialNumber, upgrade);

            m_MaterialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, TDItemConfigTable.GetIconName(upgrade.PropID));
        }

        private void RefreshPanelInfo()
        {
            if (m_FontList.Count==0)
            {
                string equipName = m_CharaceterEquipment.Name;
                for (int i = 0; i < equipName.Length; i++)
                    CreateKungfuName(equipName[i].ToString());
            }
            m_BriefIntroduction.text = m_CharaceterEquipment.Desc;
            m_EquipItem.sprite = SpriteHandler.S.GetSprite(AtlasDefine.EquipmentAtlas, m_CharaceterEquipment.GetIconName());
            m_EquipQuality.sprite = CommonMethod.GetKungfuQualitySprite(m_CharaceterEquipment.GetSubID());
            m_EquipOrder.text = "µÚ" + m_CharaceterEquipment.Class + "½×";
            m_CurAddition.text = (m_CharaceterEquipment.AtkAddition * 100) + Define.PERCENT;
            if (m_CharaceterEquipment.Class == 9)
            {
                m_Two.SetActive(false);
                m_MaterialObj.SetActive(false);
            }
            else
                m_NextAddition.text = (TDEquipmentConfigTable.GetEquipmentInfo(m_CharaceterEquipment.GetSubID()).GetAtkBonusForClassID(m_CharaceterEquipment.Class + 1) * 100) + Define.PERCENT;
            RefershIntensifyImg();
        }
        private void CreateKungfuName(string font)
        {
            ImgFontPre imgFontPre = Instantiate(m_ImgFontPre, m_TopTra).GetComponent<ImgFontPre>();
            imgFontPre.SetFontCont(font);
            m_FontList.Add(imgFontPre.gameObject);
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }
    }
}