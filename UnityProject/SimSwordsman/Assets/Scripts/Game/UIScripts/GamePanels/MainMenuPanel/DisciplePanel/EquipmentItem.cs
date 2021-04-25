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
        /// 已学习
        /// </summary>
        Learned,
        /// <summary>
        /// 未学习
        /// </summary>
        NotLearning,
        /// <summary>
        /// 未解锁
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
        private Button m_EquipBtn;
        [SerializeField]
        private Text m_ClassValue;
        [SerializeField]
        private Text m_Addition;
        [SerializeField]
        private Text m_RestrictionsValue;
        [SerializeField]
        private Button m_ReplaceBtn;
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

            CheckEquipRedPoint();

            RefreshEquipInfo();

            m_ReplaceBtn.onClick.AddListener(() => {
                UIMgr.S.OpenPanel(UIID.WearableLearningPanel, m_PropType, m_CurDisciple);
            });

            m_EquipBtn.onClick.AddListener(()=>{

                switch (m_EquipState)
                {
                    case EquipState.Learned:
                        UIMgr.S.OpenPanel(UIID.EquipDetailsPanel, m_CharaceterEquipment, m_CurDisciple, m_PropType);
                        break;
                    case EquipState.NotLearning:
                        UIMgr.S.OpenPanel(UIID.WearableLearningPanel, m_PropType, m_CurDisciple);
                        break;
                    case EquipState.NotUnlocked:
                        FloatMessage.S.ShowMsg("弟子等级不足，先去升级吧");
                        break;
                    default:
                        break;
                }
            });
        }

        private void CheckEquipRedPoint()
        {
            switch (m_PropType)
            {
                case PropType.Arms:
                    m_RedPoint.SetActive(m_CurDisciple.CheckArms());
                    break;
                case PropType.Armor:
                    m_RedPoint.SetActive(m_CurDisciple.CheckArmor());
                    break;
            }
        }
        private void HandleAddListenerEvevt(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedEquipSuccess:
                    RefreshEquipInfo();
                    CheckEquipRedPoint();
                    EventSystem.S.Send(EventID.OnMainMenuOrDiscipleRedPoint);
                    break;
                case EventID.OnEquipRedPoint:
                    if ((int)param[0] == m_CurDisciple.id && m_PropType == ((PropType)param[1]))
                        m_RedPoint.SetActive((bool)param[2]);
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
                //解锁
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
                    m_ReplaceBtn.gameObject.SetActive(true);
                    //当前有装备
                    m_EquipName.text = m_CharaceterEquipment.Name;
                    m_ClassValue.text = CommonUIMethod.GetClass(m_CharaceterEquipment.Class);
                    m_RestrictionsValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_SKILL) +
                        CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetStringForTableKey(Define.PLUS) + CommonUIMethod.GetBonus(m_CharaceterEquipment.AtkAddition));
                    m_Addition.text = Define.COMMON_DEFAULT_STR;
                    m_LearnBg.sprite  = SpriteHandler.S.GetSprite(AtlasDefine.EquipmentAtlas, m_CharaceterEquipment.GetIconName());
                    m_NotLearnBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Bg34");
                    m_LearnBg.gameObject.SetActive(true);
                    m_EquipState = EquipState.Learned;
                }
                else
                {
                    //当前没有装备
                    m_EquipName.text = "未装备";
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
                //未解锁
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