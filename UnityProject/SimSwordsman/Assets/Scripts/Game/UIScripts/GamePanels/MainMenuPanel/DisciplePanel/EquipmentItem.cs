using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum EquipState
    {
        /// <summary>
        /// ��ѧϰ
        /// </summary>
        Learned,
        /// <summary>
        /// δѧϰ
        /// </summary>
        NotLearning,
        /// <summary>
        /// δ����
        /// </summary>
        NotUnlocked,
    }

    public class EquipmentItem : MonoBehaviour
    {
        [SerializeField]
        private Image m_NotLearnBg;
        [SerializeField]
        private Image m_LearnBg;
        [SerializeField]
        private Text m_EquipName;
        [SerializeField]
        private Button m_KungfuBtn;
        [SerializeField]
        private Text m_ClassValue;
        [SerializeField]
        private Text m_Addition;
        [SerializeField]
        private Text m_RestrictionsValue;
        [SerializeField]
        private GameObject m_RedPoint;

        private CharacterItem m_CurDisciple = null;
        private CharaceterEquipment m_CharaceterEquipment;
        private UpgradeCondition m_Upgrade;
        private PropType m_PropType;
        private EquipState m_EquipState;
        public void OnInit(CharaceterEquipment characeterEquipment, CharacterItem disciple, PropType prop)
        {
            EventSystem.S.Register(EventID.OnSelectedEquipSuccess, HandleAddListenerEvevt);

            m_CharaceterEquipment = characeterEquipment;
            m_CurDisciple = disciple;
            m_PropType = prop;
   
            RefreshEquipInfo();

            m_KungfuBtn.onClick.AddListener(()=>{

                switch (m_EquipState)
                {
                    case EquipState.Learned:
                        break;
                    case EquipState.NotLearning:
                        UIMgr.S.OpenPanel(UIID.WearableLearningPanel, m_PropType, m_CurDisciple);
                        break;
                    case EquipState.NotUnlocked:
                        FloatMessage.S.ShowMsg("���ӵȼ����㣬��ȥ������");
                        break;
                    default:
                        break;
                }
            });
        }
        private void HandleAddListenerEvevt(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedEquipSuccess:
                    RefreshEquipInfo();
                    EventSystem.S.Send(EventID.OnMainMenuOrDiscipleRedPoint);
                    break;
                default:
                    break;
            }
        }
        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EventID.OnSelectedEquipSuccess, HandleAddListenerEvevt);

        }
        private void RefreshEquipInfo()
        {
            //m_RedPoint.SetActive(m_CurDisciple.CheckArms());
            bool isUnlock = false;
            switch (m_PropType)
            {
                case PropType.Arms:
                    isUnlock = m_CurDisciple.characeterEquipmentData.IsArmsUnlock;
                    break;
                case PropType.Armor:
                    isUnlock = m_CurDisciple.characeterEquipmentData.IsArmorUnlock;
                    break;
            }
            if (isUnlock)
            {
                //����
                if (m_CharaceterEquipment.IsHaveEquip())
                {
                    //switch (m_PropType)
                    //{
                    //    case PropType.Arms:
                    //        m_Upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume(m_CharaceterEquipment.GetSubID(), m_CharaceterEquipment.Class + 1);
                    //        break;
                    //    case PropType.Armor:
                    //        m_Upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume(m_CharaceterEquipment.GetSubID(), m_CharaceterEquipment.Class + 1);
                    //        break;
                    //}

                    //��ǰ��װ��
                    m_EquipName.text = m_CharaceterEquipment.Name;
                    m_ClassValue.text = CommonUIMethod.GetClass(m_CharaceterEquipment.Class);
                    m_RestrictionsValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_SKILL) +
                        CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetStringForTableKey(Define.PLUS) + CommonUIMethod.GetBonus(m_CharaceterEquipment.AtkAddition));
                    m_Addition.text = Define.COMMON_DEFAULT_STR;
                    m_LearnBg.sprite  = SpriteHandler.S.GetSprite(AtlasDefine.EquipmentAtlas, m_CharaceterEquipment.GetIconName());
                    m_NotLearnBg.gameObject.SetActive(false);
                    m_LearnBg.gameObject.SetActive(true);
                    m_EquipState = EquipState.Learned;
                }
                else
                {
                    //��ǰû��װ��
                    m_EquipName.text = "δװ��";
                    m_ClassValue.text = Define.COMMON_DEFAULT_STR;
                    m_Addition.text = Define.COMMON_DEFAULT_STR;
                    m_RestrictionsValue.text = Define.COMMON_DEFAULT_STR;
                    m_NotLearnBg.sprite= SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Plus");
                    m_NotLearnBg.gameObject.SetActive(true);
                    m_LearnBg.gameObject.SetActive(false);
                    m_EquipState = EquipState.NotLearning;
                }
            }
            else
            {
                int unlockLevel = 0;
                switch (m_PropType)
                {

                    case PropType.Arms:
                        unlockLevel = MainGameMgr.S.CharacterMgr.GetUnlockConfigInfo(UnlockContent.EquipWeapon);
                        break;
                    case PropType.Armor:
                        unlockLevel = MainGameMgr.S.CharacterMgr.GetUnlockConfigInfo(UnlockContent.EquipArmor);
                        break;
                }
                //δ����
                m_EquipName.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED);
                m_ClassValue.text = Define.COMMON_DEFAULT_STR;
                m_RestrictionsValue.text = Define.COMMON_DEFAULT_STR;
                m_Addition.text = CommonUIMethod.GetStrForColor("#8C343C", unlockLevel.ToString()) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNLOCKED); 
                m_NotLearnBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Lock");
                m_NotLearnBg.gameObject.SetActive(true);
                m_LearnBg.gameObject.SetActive(false);
                m_EquipState = EquipState.NotUnlocked;
            }
            // m_ArmsImg.sprite = FindSprite();
        }

        private void HandAddlistenerEvent(int key, object[] param)
        {
          
        }

    }

}