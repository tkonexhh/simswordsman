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
        private Image m_StateBg;
        [SerializeField]
        private Text m_StateValue;
        [SerializeField]
        private Text m_RankTitle;
        [SerializeField]
        private Text m_RankValue;
        [SerializeField]
        private Image m_GradeImg;
        [SerializeField]
        private Image m_DiscipleImg;

        [Header("UpperMiddle")]
        [SerializeField]
        private Text m_KungfuTitle;
        [SerializeField]
        private Transform m_KungfuTra;
        [SerializeField]
        private GameObject m_KungfuItem;

        [Header("LeftLowerMiddle")]
        [SerializeField]
        private Text m_ArmorTitle;
        [SerializeField]
        private Text m_ArmorNameValue;
        [SerializeField]
        private Text m_ArmorClassValue;
        [SerializeField]
        private Text m_ArmorSkillValue;
        [SerializeField]
        private Image m_ArmorImg;
        [SerializeField]
        private Image m_ArmorLock;
        [SerializeField]
        private Image m_ArmorPlus;
        [SerializeField]
        private Button m_ArmorBtn;
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
        private Text m_ArmsSkillValue;
        [SerializeField]
        private Image m_ArmsImg;
        [SerializeField]
        private Image m_ArmsLock;
        [SerializeField]
        private Image m_ArmsPlus;
        [SerializeField]
        private Button m_ArmsBtn;
        [SerializeField]
        private Button m_IntensifyArmsBtn;
        [SerializeField]
        private Text m_IntensifyArmsValue;

        [Header("Bottom")]
        [SerializeField]
        //private Button m_PracticeValueBtn;
        //[SerializeField]
        //private Text m_PracticeValue;
        //[SerializeField]
        //private Button m_WorkValueBtn;
        //[SerializeField]
        //private Text m_WorkValue;
        //[SerializeField]
        private Button m_EjectValueBtn;
        [SerializeField]
        private Text m_EjectValue;
        [SerializeField]
        private Button m_CloseBtn;
        private CharacterItem m_CurDisciple = null;
        private CharacterController m_CurCharacterController = null;
        private Dictionary<int, CharacterKongfuData> m_Kongfus = null;
        private Dictionary<int, GameObject> m_KongfusGameObject = new Dictionary<int, GameObject>();

        //private EquipmentItem m_CurArmor = null;
        //private EquipmentItem m_CurArms = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnRefreshDisciple, HandleAddListenerEvevt);
            EventSystem.S.Register(EventID.OnSelectedEquipSuccess, HandleAddListenerEvevt);
            EventSystem.S.Register(EventID.OnSelectedKungfuSuccess, HandleAddListenerEvevt);

            InitPanelTitleInfo();

            BindAddListenerEvent();
        }

        /// <summary>
        /// 初始化面板的固定信息
        /// </summary>
        private void InitPanelTitleInfo()
        {
            m_LevelTitle.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_LEVEL);
            m_SkillTitle.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_SKILL);
            m_EntryTimeTitle.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_ENTRYTIME);
            m_RankTitle.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_RANK);
            m_KungfuTitle.text = CommonUIMethod.GetStringForTableKey(Define.KUNGFU_TITLE);
            m_ArmorTitle.text = CommonUIMethod.GetStringForTableKey(Define.ARMOR_TITLE);
            m_ArmsTitle.text = CommonUIMethod.GetStringForTableKey(Define.ARMS_TITLE);
            //m_PracticeValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_PRACTICE);
            //m_WorkValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_WORK);
            m_EjectValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_EJECT);
            m_IntensifyArmorValue.text = CommonUIMethod.GetStringForTableKey(Define.EQUIP_INTENSIFY);
            m_IntensifyArmsValue.text = CommonUIMethod.GetStringForTableKey(Define.EQUIP_INTENSIFY);

        }

        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }

        public void LoadClanPrefabs(string prefabsName)
        {
            AddressableAssetLoader<Sprite> loader = new AddressableAssetLoader<Sprite>();
            loader.LoadAssetAsync(prefabsName, (obj) =>
            {
                //Debug.Log(obj);
                m_DiscipleImg.sprite = obj;
            });
        }

        private void RefreshPanelInfo()
        {
            m_DiscipleNameValue.text = m_CurDisciple.name;
            m_LevelValue.text = CommonUIMethod.GetGrade(m_CurDisciple.level);
            m_SkillValue.text = m_CurDisciple.atkValue.ToString();
            m_EntryTimeValue.text = GetEntryTime(m_CurDisciple.GetEntryTime());
            m_RankValue.text = CommonUIMethod.GetPart(m_CurDisciple.stage);
            switch (m_CurDisciple.quality)
            {
                case CharacterQuality.Normal:
                    m_GradeImg.sprite = FindSprite("DiscipleDetails_Bg18");
                    break;
                case CharacterQuality.Good:
                    m_GradeImg.sprite = FindSprite("DiscipleDetails_Bg19");
                    break;
                case CharacterQuality.Perfect:
                    m_GradeImg.sprite = FindSprite("DiscipleDetails_Bg17");
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

        private void RefreshArmsInfo()
        {
            if (m_CurDisciple.characeterEquipmentData.IsArmorUnlock)
            {
                m_ArmsBtn.enabled = true;
                //解锁
                CharacterArms characterArms = m_CurDisciple.characeterEquipmentData.CharacterArms;
                if (characterArms.IsHaveEquip())
                {
                    //当前有装备
                    m_ArmsNameValue.text = characterArms.Name;
                    m_ArmsClassValue.text = CommonUIMethod.GetClass(characterArms.Class);
                    m_ArmsSkillValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_SKILL) +
                        CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetStringForTableKey(Define.PLUS) + CommonUIMethod.GetBonus(characterArms.AtkAddition));
                    m_IntensifyArmsBtn.gameObject.SetActive(true);
                    m_ArmsLock.gameObject.SetActive(false);
                    m_ArmsImg.gameObject.SetActive(true);
                    m_ArmsPlus.gameObject.SetActive(false);
                    m_ArmsImg.sprite = FindSprite(characterArms.GetIconName());
                }
                else
                {
                    //当前没有装备
                    m_ArmsNameValue.text = Define.COMMON_DEFAULT_STR;
                    m_ArmsClassValue.text = Define.COMMON_DEFAULT_STR;
                    m_ArmsSkillValue.text = Define.COMMON_DEFAULT_STR;
                    m_IntensifyArmsBtn.gameObject.SetActive(false);
                    m_ArmsLock.gameObject.SetActive(false);
                    m_ArmsImg.gameObject.SetActive(false);
                    m_ArmsPlus.gameObject.SetActive(true);
                }
            }
            else
            {
                //未解锁
                m_ArmsNameValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED);
                int unlockLevel = MainGameMgr.S.CharacterMgr.GetUnlockConfigInfo(UnlockContent.EquipWeapon);
                m_ArmsClassValue.text = CommonUIMethod.GetStrForColor("#8C343C", unlockLevel.ToString()) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNLOCKED);
                m_IntensifyArmsBtn.gameObject.SetActive(false);
                m_ArmsBtn.enabled = false;
                m_ArmsLock.gameObject.SetActive(true);
                m_ArmsImg.gameObject.SetActive(false);
                m_ArmsPlus.gameObject.SetActive(false);
            }
            // m_ArmsImg.sprite = FindSprite();
        }
        private void RefreshArmorInfo()
        {
            if (m_CurDisciple.characeterEquipmentData.IsArmorUnlock)
            {
                m_ArmorBtn.enabled = true;
                CharacterArmor characterArmor = m_CurDisciple.characeterEquipmentData.CharacterArmor;
                if (characterArmor.IsHaveEquip())
                {
                    m_ArmorNameValue.text = characterArmor.Name;
                    m_ArmorClassValue.text = CommonUIMethod.GetClass(characterArmor.Class);
                    m_ArmorSkillValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_TITLE_SKILL) +
                        CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetStringForTableKey(Define.PLUS) + CommonUIMethod.GetBonus(characterArmor.AtkAddition));
                    m_IntensifyArmorBtn.gameObject.SetActive(true);
                    m_ArmorLock.gameObject.SetActive(false);
                    m_ArmorImg.gameObject.SetActive(true);
                    m_ArmorPlus.gameObject.SetActive(false);
                    m_ArmorImg.sprite = FindSprite(characterArmor.GetIconName());
                }
                else
                {
                    m_ArmorNameValue.text = Define.COMMON_DEFAULT_STR;
                    m_ArmorClassValue.text = Define.COMMON_DEFAULT_STR;
                    m_ArmorSkillValue.text = Define.COMMON_DEFAULT_STR;
                    m_IntensifyArmorBtn.gameObject.SetActive(false);
                    m_ArmorLock.gameObject.SetActive(false);
                    m_ArmorImg.gameObject.SetActive(false);
                    m_ArmorPlus.gameObject.SetActive(true);
                }
            }
            else
            {
                m_ArmorNameValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED);
                int unlockLevel = MainGameMgr.S.CharacterMgr.GetUnlockConfigInfo(UnlockContent.EquipArmor);
                m_ArmorClassValue.text = CommonUIMethod.GetStrForColor("#8C343C", unlockLevel.ToString()) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNLOCKED);
                m_IntensifyArmorBtn.gameObject.SetActive(false);
                m_ArmorBtn.enabled = false;
                m_ArmorLock.gameObject.SetActive(true);
                m_ArmorImg.gameObject.SetActive(false);
                m_ArmorPlus.gameObject.SetActive(false);
            }
        }

        //素材名称 和 KongfuType名称保持一致
        private Sprite GetKungfuSprite(CharacterKongfu characterKongfu)
        {
            return FindSprite(characterKongfu.dbData.kongfuType.ToString());
        }

        private void CreateKungfu(int index, KungfuLockState kungfuLockState, Sprite sprite, int UnLockLevel = -1, CharacterKongfu characterKongfu = null)
        {
            GameObject obj = Instantiate(m_KungfuItem, m_KungfuTra);
            ItemICom itemICom = obj.GetComponent<ItemICom>();
            if (!m_KongfusGameObject.ContainsKey(index))
                m_KongfusGameObject.Add(index, obj);

            
            List<Sprite> sprites = GetSprite(characterKongfu);
            sprites.Add(sprite);
            itemICom.OnInit(characterKongfu, null, kungfuLockState, sprites, UnLockLevel, m_CurDisciple, index);
        }

        private string GetEntryTime(int day)
        {
            return CommonUIMethod.GetStrForColor("#86570C", day.ToString()) + CommonUIMethod.GetStrForColor("#382E2E", CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_DAY));
        }

        private void SetCharacterBehavior(CharacterStateID behavior)
        {
            switch (behavior)
            {
                case CharacterStateID.None:
                case CharacterStateID.Wander:
                case CharacterStateID.EnterClan:
                    m_StateBg.sprite = FindSprite("DiscipleDetails_Bg16");
                    m_StateValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_STATE_FREE);
                    break;
                case CharacterStateID.Practice:
                    m_StateBg.sprite = FindSprite("DiscipleDetails_Bg14");
                    m_StateValue.text = "正在练功";
                    break;
                case CharacterStateID.Working:
                    m_StateBg.sprite = FindSprite("DiscipleDetails_Bg15");
                    m_StateValue.text = "正在任务";
                    break;
                default:
                    m_StateBg.sprite = FindSprite("DiscipleDetails_Bg13");
                    m_StateValue.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_STATE_WORKING);
                    break;
            }
        }

        private void BindAddListenerEvent()
        {
            m_ArmorBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.WearableLearningPanel, PropType.Armor, m_CurDisciple);
            });
            m_ArmsBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.WearableLearningPanel, PropType.Arms, m_CurDisciple);
            });
            m_IntensifyArmorBtn.onClick.AddListener(() =>
            {
                CharacterArmor characterArmor = m_CurDisciple.characeterEquipmentData.CharacterArmor;

                UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume((int)characterArmor.ArmorID, characterArmor.Class + 1);
                  
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)upgrade.PropID, upgrade.Number);
                if (!isHave)   
                {
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
                    return;
                }

                MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)upgrade.PropID), upgrade.Number);
                characterArmor.UpGradeClass(m_CurDisciple.id);
                RefreshArmorInfo();
            });
            m_EjectValueBtn.onClick.AddListener(() =>
            {
                MainGameMgr.S.CharacterMgr.RemoveCharacter(m_CurDisciple.id);
                OnPanelHideComplete();
            });
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_IntensifyArmsBtn.onClick.AddListener(() =>
            {
                CharacterArms characterArms = m_CurDisciple.characeterEquipmentData.CharacterArms;

                UpgradeCondition upgrade = TDEquipmentConfigTable.GetEquipUpGradeConsume((int)characterArms.ArmsID, characterArms.Class + 1);

                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)upgrade.PropID, upgrade.Number);
                if (!isHave)
                {
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
                    return;
                }

                MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)upgrade.PropID), upgrade.Number);
                characterArms.UpGradeClass(m_CurDisciple.id);
                RefreshArmsInfo();
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurDisciple = (CharacterItem)args[0];

            GetInformationForNeed();
            RefreshPanelInfo();
            LoadClanPrefabs(GetLoadDiscipleName(m_CurDisciple));
        }

        private void GetInformationForNeed()
        {
            m_CurCharacterController = MainGameMgr.S.CharacterMgr.GetCharacterController(m_CurDisciple.id);
            m_Kongfus = m_CurDisciple.kongfus;
        }

        private void HandleAddListenerEvevt(int key, object[] param)
        {
            GetInformationForNeed();
            switch ((EventID)key)
            {
                case EventID.OnSelectedEquipSuccess:
                    RefreshArmsInfo();
                    RefreshArmorInfo();
                    break;
                case EventID.OnRefreshDisciple:
                    break;
                case EventID.OnSelectedKungfuSuccess:
                    RefreshPanelKungfuInfo((int)param[0]);
                    break;
                default:
                    break;
            }
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
            if (characterKongfu==null)
                return sprites;
            sprites.Add(FindSprite(GetIconName(characterKongfu.dbData.kongfuType)));
            switch (GetKungfuQuality(characterKongfu.dbData.kongfuType))
            {
                case KungfuQuality.Normal:
                    sprites.Add(FindSprite("Introduction"));
                    break;
                case KungfuQuality.Super:
                    sprites.Add(FindSprite("Advanced"));
                    break;
                case KungfuQuality.Master:
                    sprites.Add(FindSprite("Excellent"));
                    break;
                default:
                    break;
            }
            return sprites;
        }
        private KungfuQuality GetKungfuQuality(KongfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }

        private string GetIconName(KongfuType kungfuType)
        {
            return TDKongfuConfigTable.GetIconName(kungfuType);
        }
        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedEquipSuccess, HandleAddListenerEvevt);
            EventSystem.S.UnRegister(EventID.OnRefreshDisciple, HandleAddListenerEvevt);
            EventSystem.S.UnRegister(EventID.OnSelectedKungfuSuccess, HandleAddListenerEvevt);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}