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
        [SerializeField]
        private Button m_BlackBtn;
        [SerializeField]
        private Button m_CloseBtn;
        [Header("Top Left")]
        [SerializeField]
        private Transform m_NameTra;       
        [SerializeField]
        private GameObject m_ImgFontPre;   
        [SerializeField]
        private Image m_Post;
        [Header("Top Left Level")]
        [SerializeField]
        private Text m_LevelValue;
        [SerializeField]
        private Slider m_ExpSlider;
        [Header("Top Left Atk")]
        [SerializeField]
        private Text m_SkillValue;
        [Header("Top Left Rank")]
        [SerializeField]
        private Text m_RankValue;
        [Header("Top Right")]
        [SerializeField]
        private Transform m_HeadTra;
        [SerializeField]
        private Image m_GradeImg;
        [SerializeField]
        private Button m_EjectBtn;

        [Header("Middle")]
        [SerializeField]
        private Transform m_KungfuTra;
        [SerializeField]
        private GameObject m_KungfuItem;
        [Header("EqupiTra")]
        [SerializeField]
        private Transform m_EquipTra;
        [SerializeField]
        private GameObject m_EquipItem;

        private CharacterItem m_CurDisciple = null;
        private CharacterController m_CurCharacterController = null;
        private Dictionary<int, CharacterKongfuData> m_Kongfus = null;
        private Dictionary<int, GameObject> m_KongfusGameObject = new Dictionary<int, GameObject>();

        #region ????ʹ?? 
        [SerializeField]
        private Button m_EditerDisciple;
        #endregion
        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
            EventSystem.S.Register(EventID.OnRefreshDisciple, HandleAddListenerEvevt);
            EventSystem.S.Register(EventID.OnSelectedEquipSuccess, HandleAddListenerEvevt);
            EventSystem.S.Register(EventID.OnSelectedKungfuSuccess, HandleAddListenerEvevt);
        }

        private void RefreshPanelInfo()
        {
            m_LevelValue.text = CommonUIMethod.GetGrade(m_CurDisciple.level);
            m_SkillValue.text = CommonUIMethod.GetTenThousandOrMillion((long)m_CurDisciple.atkValue);
            m_RankValue.text = CommonUIMethod.GetPart(m_CurDisciple.stage);

            Transform transform = Instantiate(DiscipleHeadPortraitMgr.S.GetDiscipleHeadPortrait(m_CurDisciple), m_HeadTra).transform;
            transform.localPosition = new Vector3(-1.5f, 70f, 0);
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
            m_ExpSlider.value = (float)m_CurDisciple.curExp / TDCharacterStageConfigTable.GetExpLevelUpNeed(m_CurDisciple);
            switch (m_CurDisciple.quality)
            {
                case CharacterQuality.Normal:
                    m_GradeImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_God");
                    break;
                case CharacterQuality.Good:
                    m_GradeImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Normal");
                    break;
                case CharacterQuality.Perfect:
                    m_GradeImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Perfect");
                    break;
                case CharacterQuality.Hero:
                    m_GradeImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Hero");
                    break;
                default:
                    break;
            }

            SetCharacterBehavior(m_CurDisciple.GetCharacterStateID());

            foreach (var item in m_Kongfus.Values)
            {
                switch (item.KungfuLockState)
                {
                    case KungfuLockState.Learned:
                        CreateKungfu(item.Index, KungfuLockState.Learned, FindSprite(item.GetIconName()), -1, item.CharacterKongfu);
                        break;
                    case KungfuLockState.NotLearning:
                        CreateKungfu(item.Index, KungfuLockState.NotLearning, FindSprite("NotStudy"));
                        break;
                    case KungfuLockState.NotUnlocked:
                        CreateKungfu(item.Index, KungfuLockState.NotUnlocked, FindSprite("Lock1"), MainGameMgr.S.CharacterMgr.GetUnlockConfigInfo(UnlockContent.LearnKongfu, item.Index));
                        break;
                    default:
                        break;
                }
            }
            RefreshArmsInfo();
            RefreshArmorInfo();
        }

        private void RefershIntensifyImg()
        {
            CharacterArms characterArms = m_CurDisciple.characeterEquipmentData.CharacterArms;
            CreateEquip(characterArms, PropType.Arms);
            CharacterArmor characterArmor = m_CurDisciple.characeterEquipmentData.CharacterArmor;
            CreateEquip(characterArmor, PropType.Armor);
        }
        private void RefershIntensifyArmsImg()
        {
            CharacterArms characterArms = m_CurDisciple.characeterEquipmentData.CharacterArms;
            UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume((int)characterArms.ArmsID, characterArms.Class + 1);
            //if (upgrade == null)
            //{
            //    m_IntensifyArmsBtn.gameObject.SetActive(false);
            //    return;
            //}
            //RefrshIntensifyText(m_IntensifyArmsValue, upgrade);
            //m_IntensifyArmsImg.sprite = FindSprite(TDItemConfigTable.GetIconName(upgrade.PropID));
        }

        private void RefershIntensifyArmorImg()
        {
            CharacterArmor characterArmor = m_CurDisciple.characeterEquipmentData.CharacterArmor;
            UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume((int)characterArmor.ArmorID, characterArmor.Class + 1);
            //if (upgrade == null)
            //{
            //    m_IntensifyArmorBtn.gameObject.SetActive(false);
            //    return;
            //}

            //RefrshIntensifyText(m_IntensifyArmorValue, upgrade);

            //m_IntensifyArmorImg.sprite = FindSprite(TDItemConfigTable.GetIconName(upgrade.PropID));
        }

        private void RefrshIntensifyText(Text text, UpgradeCondition upgrade)
        {
            int curNumber = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)upgrade.PropID);
            if (curNumber >= upgrade.Number)
                text.text = upgrade.Number + Define.SLASH + upgrade.Number;
            else
                text.text = CommonUIMethod.GetStrForColor("#8C343C", curNumber.ToString()) + Define.SLASH + upgrade.Number;
        }

        private void RefreshArmsInfo()
        {
            //m_ArmsRedPoint.SetActive(m_CurDisciple.CheckArms());

            //if (m_CurDisciple.characeterEquipmentData.IsArmsUnlock)
            //{
            //    m_ArmsState = EquipBtnState.UnLock;
            //    //????
            //    CharacterArms characterArms = m_CurDisciple.characeterEquipmentData.CharacterArms;
            //    if (characterArms.IsHaveEquip())
            //    {
            //        //??ǰ??װ??
            //        m_ArmsNameValue.text = characterArms.Name;
            //        m_ArmsClassValue.text = CommonUIMethod.GetClass(characterArms.Class);
            //        m_ArmsSkillValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_SKILL) +
            //            CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetStringForTableKey(Define.PLUS) + CommonUIMethod.GetBonus(characterArms.AtkAddition));
            //        m_IntensifyArmsBtn.gameObject.SetActive(true);
            //        m_ArmsLock.gameObject.SetActive(false);
            //        m_ArmsImg.gameObject.SetActive(true);
            //        m_ArmsPlus.gameObject.SetActive(false);
            //        m_ArmsImg.sprite = FindSprite(characterArms.GetIconName());
            //    }
            //    else
            //    {
            //        //??ǰû??װ??
            //        m_ArmsNameValue.text = Define.COMMON_DEFAULT_STR;
            //        m_ArmsClassValue.text = Define.COMMON_DEFAULT_STR;
            //        m_ArmsSkillValue.text = Define.COMMON_DEFAULT_STR;
            //        m_IntensifyArmsBtn.gameObject.SetActive(false);
            //        m_ArmsLock.gameObject.SetActive(false);
            //        m_ArmsImg.gameObject.SetActive(false);
            //        m_ArmsPlus.gameObject.SetActive(true);
            //    }
            //}
            //else
            //{
            //    //δ????
            //    m_ArmsNameValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED);
            //    int unlockLevel = MainGameMgr.S.CharacterMgr.GetUnlockConfigInfo(UnlockContent.EquipWeapon);
            //    m_ArmsClassValue.text = CommonUIMethod.GetStrForColor("#8C343C", unlockLevel.ToString()) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNLOCKED);
            //    m_IntensifyArmsBtn.gameObject.SetActive(false);
            //    m_ArmsState = EquipBtnState.Lock;
            //    m_ArmsLock.gameObject.SetActive(true);
            //    m_ArmsImg.gameObject.SetActive(false);
            //    m_ArmsPlus.gameObject.SetActive(false);
            //}
            //// m_ArmsImg.sprite = FindSprite();
        }
        private void RefreshArmorInfo()
        {
            //m_ArmorRedPoint.SetActive(m_CurDisciple.CheckArmor());
            //if (m_CurDisciple.characeterEquipmentData.IsArmorUnlock)
            //{
            //    m_ArmorState = EquipBtnState.UnLock;
            //    CharacterArmor characterArmor = m_CurDisciple.characeterEquipmentData.CharacterArmor;
            //    if (characterArmor.IsHaveEquip())
            //    {
            //        m_ArmorNameValue.text = characterArmor.Name;
            //        m_ArmorClassValue.text = CommonUIMethod.GetClass(characterArmor.Class);
            //        m_ArmorSkillValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_SKILL) +
            //            CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetStringForTableKey(Define.PLUS) + CommonUIMethod.GetBonus(characterArmor.AtkAddition));
            //        m_IntensifyArmorBtn.gameObject.SetActive(true);
            //        m_ArmorLock.gameObject.SetActive(false);
            //        m_ArmorImg.gameObject.SetActive(true);
            //        m_ArmorPlus.gameObject.SetActive(false);
            //        m_ArmorImg.sprite = FindSprite(characterArmor.GetIconName());
            //    }
            //    else
            //    {
            //        m_ArmorNameValue.text = Define.COMMON_DEFAULT_STR;
            //        m_ArmorClassValue.text = Define.COMMON_DEFAULT_STR;
            //        m_ArmorSkillValue.text = Define.COMMON_DEFAULT_STR;
            //        m_IntensifyArmorBtn.gameObject.SetActive(false);
            //        m_ArmorLock.gameObject.SetActive(false);
            //        m_ArmorImg.gameObject.SetActive(false);
            //        m_ArmorPlus.gameObject.SetActive(true);
            //    }
            //}
            //else
            //{
            //    m_ArmorNameValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED);
            //    int unlockLevel = MainGameMgr.S.CharacterMgr.GetUnlockConfigInfo(UnlockContent.EquipArmor);
            //    m_ArmorClassValue.text = CommonUIMethod.GetStrForColor("#8C343C", unlockLevel.ToString()) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNLOCKED);
            //    m_IntensifyArmorBtn.gameObject.SetActive(false);
            //    m_ArmorState = EquipBtnState.Lock;
            //    m_ArmorLock.gameObject.SetActive(true);
            //    m_ArmorImg.gameObject.SetActive(false);
            //    m_ArmorPlus.gameObject.SetActive(false);
            //}
        }

        private void CreateKungfu(int index, KungfuLockState kungfuLockState, Sprite sprite, int UnLockLevel = -1, CharacterKongfu characterKongfu = null)
        {
            GameObject obj = Instantiate(m_KungfuItem, m_KungfuTra);
            KungfuPanelItem itemICom = obj.GetComponent<KungfuPanelItem>();
            if (!m_KongfusGameObject.ContainsKey(index))
                m_KongfusGameObject.Add(index, obj);
            itemICom.OnInit(kungfuLockState,characterKongfu, m_CurDisciple,  UnLockLevel, index);
        }

        private void CreateEquip(CharaceterEquipment characeterEquipment, PropType prop)
        {
            GameObject obj = Instantiate(m_EquipItem, m_EquipTra);
            EquipmentItem itemICom = obj.GetComponent<EquipmentItem>();
            //if (!m_KongfusGameObject.ContainsKey(index))
            //    m_KongfusGameObject.Add(index, obj);
            itemICom.OnInit(characeterEquipment,m_CurDisciple, prop);

        }

        private string GetEntryTime(int day)
        {
            return CommonUIMethod.GetStrForColor("#86570C", day.ToString()) + CommonUIMethod.GetStrForColor("#382E2E", CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_DAY));
        }

        private void SetCharacterBehavior(CharacterStateID behavior)
        {
            //switch (behavior)
            //{
            //    case CharacterStateID.None:
            //    case CharacterStateID.Wander:
            //    case CharacterStateID.EnterClan:
            //        m_StateBg.sprite = FindSprite("DiscipleDetails_Bg16");
            //        m_StateValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_STATE_FREE);
            //        break;
            //    case CharacterStateID.Practice:
            //        m_StateBg.sprite = FindSprite("DiscipleDetails_Bg14");
            //        m_StateValue.text = "????????";
            //        break;
            //    case CharacterStateID.Working:
            //        m_StateBg.sprite = FindSprite("DiscipleDetails_Bg15");
            //        m_StateValue.text = "????????";
            //        break;
            //    default:
            //        m_StateBg.sprite = FindSprite("DiscipleDetails_Bg13");
            //        m_StateValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_STATE_WORKING);
            //        break;
            //}
        }

        private void BindAddListenerEvent()
        {
            if (PlatformHelper.isTestMode)
            {
                m_EditerDisciple.onClick.AddListener(()=> {
                    MainGameMgr.S.CharacterMgr.AddCharacterLevel(m_CurDisciple.id,20);
                });
            }

            //m_ArmorBtn.onClick.AddListener(() =>
            //{
            //    AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
            //    switch (m_ArmorState)
            //    {
            //        case EquipBtnState.None:
            //            break;
            //        case EquipBtnState.Lock:
            //            FloatMessage.S.ShowMsg("???ӵȼ????㣬??ȥ??????");
            //            break;
            //        case EquipBtnState.UnLock:
            //            UIMgr.S.OpenPanel(UIID.WearableLearningPanel, PropType.Armor, m_CurDisciple);
            //            break;
            //        default:
            //            break;
            //    }
            //});
            //m_ArmsBtn.onClick.AddListener(() =>
            //{
            //    AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
            //    switch (m_ArmsState)
            //    {
            //        case EquipBtnState.None:
            //            break;
            //        case EquipBtnState.Lock:
            //            FloatMessage.S.ShowMsg("???ӵȼ????㣬??ȥ??????");
            //            break;
            //        case EquipBtnState.UnLock:
            //            UIMgr.S.OpenPanel(UIID.WearableLearningPanel, PropType.Arms, m_CurDisciple);
            //            break;
            //        default:
            //            break;
            //    }


            //});
            //m_IntensifyArmorBtn.onClick.AddListener(() =>
            //{
            //    AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

            //    CharacterArmor characterArmor = m_CurDisciple.characeterEquipmentData.CharacterArmor;

            //    UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume((int)characterArmor.ArmorID, characterArmor.Class + 1);

            //    if (upgrade==null)
            //        return;

            //    bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)upgrade.PropID, upgrade.Number);
            //    if (!isHave)
            //    {
            //        FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            //        return;
            //    }

            //    PanelPool.S.AddPromotion(new ArmorEnhancement(m_CurDisciple.id, m_CurDisciple.atkValue, characterArmor));

            //    MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)upgrade.PropID), upgrade.Number);
            //    characterArmor.UpGradeClass(m_CurDisciple.id);
            //    m_CurDisciple.CalculateForceValue();
            //    RefreshArmorInfo();
            //    RefreshSkillValue();
            //    RefershIntensifyArmorImg();

            //    PanelPool.S.DisplayPanel();
            //    EventSystem.S.Send(EventID.OnMainMenuOrDiscipleRedPoint);

            //    DataAnalysisMgr.S.CustomEvent(DotDefine.students_equip_up, characterArmor.ArmorID.ToString() + ";" + characterArmor.Class.ToString());
            //});
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_EjectBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.LogPanel, LogCallBack, "????ʦ??", "ȷ??Ҫ???õ???????ʦ??????", "ȷ??", "??????");
            });
            m_BlackBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            //m_IntensifyArmsBtn.onClick.AddListener(() =>
            //{
            //    AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

            //    CharacterArms characterArms = m_CurDisciple.characeterEquipmentData.CharacterArms;

            //    UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume((int)characterArms.ArmsID, characterArms.Class + 1);

            //    if (upgrade==null)
            //        return;

            //    bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)upgrade.PropID, upgrade.Number);
            //    if (!isHave)
            //    {
            //        FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            //        return;
            //    }

            //    PanelPool.S.AddPromotion(new WeaponEnhancement(m_CurDisciple.id, m_CurDisciple.atkValue, characterArms));

            //    MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)upgrade.PropID), upgrade.Number);
            //    characterArms.UpGradeClass(m_CurDisciple.id);
            //    m_CurDisciple.CalculateForceValue();
            //    RefreshArmsInfo();
            //    RefreshSkillValue();
            //    RefershIntensifyArmsImg();

            //    PanelPool.S.DisplayPanel();
            //    EventSystem.S.Send(EventID.OnMainMenuOrDiscipleRedPoint);
            //    DataAnalysisMgr.S.CustomEvent(DotDefine.students_equip_up, characterArms.ArmsID.ToString()+";"+ characterArms.Class.ToString());
            //});
        }

        private void LogCallBack(AbstractPanel obj)
        {
            LogPanel logPanel =  obj as LogPanel;
            logPanel.OnSuccessBtnEvent += SuccessBtn;
        }

        private void SuccessBtn()
        {
            if (m_CurDisciple.IsFreeState())
            {
                DataAnalysisMgr.S.CustomEvent(DotDefine.students_abandon, m_CurDisciple.quality.ToString()+";"+ m_CurDisciple.level.ToString());

                MainGameMgr.S.CommonTaskMgr.TaskRemoveCharacter(m_CurDisciple.id);
                MainGameMgr.S.CharacterMgr.RemoveCharacter(m_CurDisciple.id);
                EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);
                EventSystem.S.Send(EventID.OnRefreshDisciple, m_CurDisciple.id);
                HideSelfWithAnim();
            }
            else
                FloatMessage.S.ShowMsg("????????æµ??");
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurDisciple = (CharacterItem)args[0];

            GetInformationForNeed();

            BindAddListenerEvent();

            RefreshDiscipleName();

            RefreshPanelInfo();
            RefershIntensifyImg();
            //DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CurDisciple, m_Top, new Vector3(135, -1, 0), new Vector3(0.7f, 0.7f, 1));
            DataAnalysisMgr.S.CustomEvent(DotDefine.students_detail, m_CurDisciple.quality.ToString()+";"+ m_CurDisciple.level.ToString());
        }

        private void RefreshDiscipleName()
        {
            string discipleName = m_CurDisciple.name;
            for (int i = 0; i < discipleName.Length; i++)
            {
                ImgFontPre imgFontPre = Instantiate(m_ImgFontPre, m_NameTra).GetComponent<ImgFontPre>();
                imgFontPre.transform.SetSiblingIndex(i);
                imgFontPre.SetFontCont(discipleName[i].ToString());
            }
            if (m_CurDisciple.quality == CharacterQuality.Hero)
            {
                switch (m_CurDisciple.heroClanType)
                {
                    case ClanType.Gaibang:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post2");
                        break;
                    case ClanType.Shaolin:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post1");
                        break;
                    case ClanType.Wudang:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post8");
                        break;
                    case ClanType.Emei:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post3");
                        break;
                    case ClanType.Huashan:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post5");
                        break;
                    case ClanType.Wudu:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post6");
                        break;
                    case ClanType.Mojiao:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post7");
                        break;
                    case ClanType.Xiaoyao:
                        m_Post.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Post4");
                        break;
                }
            }
            else
            {
                m_Post.gameObject.SetActive(false);
            }
        }

        private void GetInformationForNeed()
        {
            m_CurCharacterController = MainGameMgr.S.CharacterMgr.GetCharacterController(m_CurDisciple.id);
            m_Kongfus = m_CurDisciple.kongfus;
        }

        private void HandleAddListenerEvevt(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedEquipSuccess:
                    RefreshSkillValue();
                    EventSystem.S.Send(EventID.OnMainMenuOrDiscipleRedPoint);
                    break;
                case EventID.OnRefreshDisciple:
                    break;
                case EventID.OnSelectedKungfuSuccess:
                    RefreshPanelKungfuInfo((int)param[0]);
                    RefreshSkillValue();
                    break;
                default:
                    break;
            }
        }

        private void RefreshSkillValue()
        {
            GetInformationForNeed();
            m_SkillValue.text = CommonUIMethod.GetTenThousandOrMillion((long)m_CurDisciple.atkValue);
        }

        private void RefreshPanelKungfuInfo(int index)
        {
            if (m_KongfusGameObject.ContainsKey(index))
            {
                foreach (var item in m_CurDisciple.kongfus.Values)
                {
                    if (item.Index == index)
                    {
                        m_KongfusGameObject[index].GetComponent<KungfuPanelItem>().RefeshKungfuInfo(item, GetSprite(item.CharacterKongfu));
                    }
                }
            }
        }
        private List<Sprite> GetSprite(CharacterKongfu characterKongfu)
        {
            List<Sprite> sprites = new List<Sprite>();
            if (characterKongfu == null)
                return sprites;
            sprites.Add(FindSprite(GetIconName(characterKongfu.dbData.kongfuType)));
            switch (GetKungfuQuality(characterKongfu.dbData.kongfuType))
            {
                case KungfuQuality.Normal:
                    sprites.Add(FindSprite("Introduction"));
                    break;
                case KungfuQuality.Master:
                    sprites.Add(FindSprite("Advanced"));
                    break;
                case KungfuQuality.Super:
                    sprites.Add(FindSprite("Excellent"));
                    break;
                default:
                    break;
            }
            return sprites;
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }

        private string GetIconName(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetIconName(kungfuType);
        }
        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            EventSystem.S.UnRegister(EventID.OnSelectedEquipSuccess, HandleAddListenerEvevt);
            EventSystem.S.UnRegister(EventID.OnRefreshDisciple, HandleAddListenerEvevt);
            EventSystem.S.UnRegister(EventID.OnSelectedKungfuSuccess, HandleAddListenerEvevt);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }
    }
}