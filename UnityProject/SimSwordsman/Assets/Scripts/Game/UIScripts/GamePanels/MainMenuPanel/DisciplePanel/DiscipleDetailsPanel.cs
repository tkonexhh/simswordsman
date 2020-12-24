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
        [Header("Top")]
        [SerializeField]
        private Text m_DiscipleNameValue;
        [SerializeField]
        private Text m_LevelTitle;
        [SerializeField]
        private Text m_LevelValue;
        [SerializeField]
        private Text m_SkillTitle;
        [SerializeField]
        private Text m_SkillValue;
        [SerializeField]
        private Text m_EntryTimeTitle;
        [SerializeField]
        private Text m_EntryTimeValue;
        [SerializeField]
        private Text m_StateValue;
        [SerializeField]
        private Text m_RankTitle;
        [SerializeField]
        private Text m_RankValue;
        [SerializeField]
        private Image m_DiscipleImg;

        [Header("UpperMiddle")]
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Text m_KungfuTitle;
        [SerializeField]
        private Text m_KungfuTra;
        [SerializeField]
        private Text m_KungfuItem;

        [Header("LeftLowerMiddle")]
        [SerializeField]
        private Text m_ArmorTitle;
        [SerializeField]
        private Text m_ArmorNameValue;
        [SerializeField]
        private Text m_ArmorClassValue;
        [SerializeField]
        private Text m_ArmorSkillTitle;
        [SerializeField]
        private Text m_ArmorSkillValue;
        [SerializeField]
        private Image m_ArmorImg;
        [SerializeField]
        private Image m_IntensifyArmorImg;
        [SerializeField]
        private Button m_IntensifyArmorBtn;
        [SerializeField]
        private Text m_IntensifyArmorValue;

        [Header("RightLowerMiddle")]
        [SerializeField]
        private Text m_ArmsTitle;
        [SerializeField]
        private Text m_ArmsNameValue;
        [SerializeField]
        private Text m_ArmsClassValue;
        [SerializeField]
        private Text m_ArmsSkillTitle;
        [SerializeField]
        private Text m_ArmsSkillValue;
        [SerializeField]
        private Image m_ArmsImg;
        [SerializeField]
        private Image m_IntensifyArmsImg;
        [SerializeField]
        private Button m_IntensifyArmsBtn;
        [SerializeField]
        private Text m_IntensifyArmsValue;

        [Header("Bottom")]
        [SerializeField]
        private Button m_PracticeValueBtn;
        [SerializeField]
        private Text m_PracticeValue;
        [SerializeField]
        private Button m_WorkValueBtn;
        [SerializeField]
        private Text m_WorkValue;
        [SerializeField]
        private Button m_EjectValueBtn;
        [SerializeField]
        private Text m_EjectValue;



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
            //m_QualityValue.text = CommonUIMethod.GetStrQualityForChaQua(m_CurDisciple.quality);
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
            //m_ArmorBtn.onClick.AddListener(()=> {
            //    UIMgr.S.OpenPanel(UIID.WearableLearningPanel,PropType.Armor, m_CurDisciple);
            //    OnPanelHideComplete();
            //});
            //m_ArmsBtn.onClick.AddListener(()=> {
            //    UIMgr.S.OpenPanel(UIID.WearableLearningPanel, PropType.Arms, m_CurDisciple);
            //    OnPanelHideComplete();
            //});

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