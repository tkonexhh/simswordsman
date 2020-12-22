using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class DiscipleDetailsPanel : AbstractAnimPanel
    {
        [Header("µÜ×ÓÊôÐÔ")]
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Text m_LevelValue;
        [SerializeField]
        private Text m_QualityValue;
        [SerializeField]
        private Text m_SkillValue;
        [Header("µÜ×Óîø¼×")]
        [SerializeField]
        private Button m_ArmorBtn;
        [SerializeField]
        private Image m_ArmorImg;
        [SerializeField]
        private Text m_ArmorValue;
        [SerializeField]
        private Text m_ArmorClass;
        [SerializeField]
        private Text m_ArmorAddition;
        [Header("µÜ×ÓÎäÆ÷")]
        [SerializeField]
        private Button m_ArmsBtn;
        [SerializeField]
        private Image m_ArmsImg;
        [SerializeField]
        private Text m_ArmsValue;
        [SerializeField]
        private Text m_ArmsClass;
        [SerializeField]
        private Text m_ArmsAddition;



        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_ExpulsionBtn;

        private CharacterItem m_CurDisciple = null;
        //private EquipmentItem m_CurArmor = null;
        //private EquipmentItem m_CurArms = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }

        private void RefreshPanelInfo()
        {
            m_DiscipleName.text = m_CurDisciple.name;
            m_LevelValue.text = m_CurDisciple.level.ToString();
            m_QualityValue.text = CommonUIMethod.GetStrQualityForChaQua(m_CurDisciple.quality);
            m_SkillValue.text = m_CurDisciple.startTime;

            //foreach (var item in m_CurDisciple.characterEquipment)
            //{
            //    switch (item.PropType)
            //    {
            //        case PropType.Armor:
            //            m_ArmorValue.text = item.Name;
            //            m_ArmorClass.text = CommonUIMethod.GetItemClass(item.ClassID);
            //            m_ArmorAddition.text = CommonUIMethod.GetBonus(MainGameMgr.S.CharacterMgr.GetDiscipleEquipBonus(item));
            //            break;
            //        case PropType.Arms:
            //            m_ArmsValue.text = item.Name;
            //            m_ArmsClass.text = CommonUIMethod.GetItemClass(item.ClassID);
            //            m_ArmsAddition.text = CommonUIMethod.GetBonus(MainGameMgr.S.CharacterMgr.GetDiscipleEquipBonus(item));
            //            break;
            //        default:
            //            break;
            //    }
                
            //}

    

            //m_ArmsValue.text = m_CurDisciple.weapon.name;
            //m_ArmsClass.text = m_CurDisciple.weapon.dbData.level.ToString();
            //m_ArmsAddition.text = m_CurDisciple.weapon.atkScale.ToString();
        }

        private void BindAddListenerEvent()
        {
            m_ArmorBtn.onClick.AddListener(()=> {
                UIMgr.S.OpenPanel(UIID.WearableLearningPanel,PropType.Armor, m_CurDisciple);
                OnPanelHideComplete();
            });
            m_ArmsBtn.onClick.AddListener(()=> {
                UIMgr.S.OpenPanel(UIID.WearableLearningPanel, PropType.Arms, m_CurDisciple);
                OnPanelHideComplete();
            });

            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_ExpulsionBtn.onClick.AddListener(() =>
            {
                MainGameMgr.S.CharacterMgr.RemoveCharacter(m_CurDisciple.id);
                CloseSelfPanel();
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            EventSystem.S.Register(EventID.OnSelectedEquipSuccess,HandleAddListenerEvevt);

            m_CurDisciple = (CharacterItem)args[0];
            RefreshPanelInfo();

            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }

        private void HandleAddListenerEvevt(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedEquipSuccess:
                    RefreshPanelInfo();
                    break;
                default:
                    break;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedEquipSuccess, HandleAddListenerEvevt);

        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);

        }
    }

}