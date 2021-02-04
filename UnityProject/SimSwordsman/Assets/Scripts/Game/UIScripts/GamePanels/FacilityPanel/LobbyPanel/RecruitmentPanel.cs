using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using System;
using Random = UnityEngine.Random;

namespace GameWish.Game
{
    public class RecruitmentPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Image m_DisciplePhoto;
        [SerializeField]
        private Button m_CloseBtn;

        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Text m_DiscipleQuality;
        [SerializeField]
        private Text m_DiscipleDescribe;

        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_RefuseBtn;

        private RecruitType m_RecruitType;
        private CharacterItem m_CharacterItem;

        private ClickType m_CurrentClickType = ClickType.None; 

        protected override void OnUIInit()
        {
            base.OnUIInit();
            

            BindAddListenerEvent();
        
        }

        private void InitPanelInfo()
        {
            m_DiscipleName.text = m_CharacterItem.name;
            m_DiscipleDescribe.text = m_CharacterItem.desc;
            m_DiscipleQuality.text = GetQualiteStr(m_CharacterItem.quality);

            LoadClanPrefabs(GetLoadDiscipleName(m_CharacterItem));
        }
        public void LoadClanPrefabs(string prefabsName)
        {
            AddressableAssetLoader<Sprite> loader = new AddressableAssetLoader<Sprite>();
            loader.LoadAssetAsync(prefabsName, (obj) =>
            {
                //Debug.Log(obj);
                m_DisciplePhoto.sprite = obj;
                m_DisciplePhoto.SetNativeSize();
            });
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }

        private string GetQualiteStr(CharacterQuality quality)
        {
            switch (quality)
            {
                case CharacterQuality.Normal:
                    return "平凡弟子";
                case CharacterQuality.Good:
                    return "精英弟子";
                case CharacterQuality.Perfect:
                    return "天才弟子";
                default:
                    return string.Empty;
            }
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_AcceptBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                switch (m_CurrentClickType)
                {
                    case ClickType.Free:
                        break;
                    case ClickType.RecruitmentOrder:
                        MainGameMgr.S.RecruitDisciplerMgr.SetCurRecruitCount(m_RecruitType);
                        break;
                    case ClickType.LookAdvertisement:
                        MainGameMgr.S.RecruitDisciplerMgr.SetAdvertisementCount(m_RecruitType);
                        break;
                    default:
                        break;
                }
                EventSystem.S.Send(EventID.OnRefreshRecruitmentOrder, m_RecruitType);

                MainGameMgr.S.RecruitDisciplerMgr.RemoveCharacterList(m_RecruitType,m_CharacterItem);
                MainGameMgr.S.CharacterMgr.AddCharacter(m_CharacterItem);
                MainGameMgr.S.CharacterMgr.SpawnCharacterController(m_CharacterItem);
                EventSystem.S.Send(EventID.OnRefreshPanelInfo, m_RecruitType, m_CurrentClickType);

                //引导
                if (m_RecruitType == RecruitType.GoldMedal && !GameDataMgr.S.GetPlayerData().firstGoldRecruit)
                {
                    GameDataMgr.S.GetPlayerData().firstGoldRecruit = true;
                    EventSystem.S.Send(EventID.OnGuideSecondGetCharacter);
                }
                if (m_RecruitType == RecruitType.SilverMedal && !GameDataMgr.S.GetPlayerData().firstSilverRecruit)
                {
                    GameDataMgr.S.GetPlayerData().firstSilverRecruit = true;
                    EventSystem.S.Send(EventID.OnGuideFirstGetCharacter);
                }
                OnPanelHideComplete();
            });
            m_RefuseBtn.onClick.AddListener(HideSelfWithAnim);

        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            RecruitType recruitType = (RecruitType)args[0];
            m_CurrentClickType = (ClickType)args[1];
            switch (recruitType)
            {
                case RecruitType.GoldMedal:
                    m_CharacterItem = MainGameMgr.S.RecruitDisciplerMgr.GetRecruitForRecruitType(RecruitType.GoldMedal);
                    if (m_CharacterItem != null)
                    {
                        m_RecruitType = RecruitType.GoldMedal;
                        InitPanelInfo();
                    }     
                    break;
                case RecruitType.SilverMedal:
                    m_CharacterItem = MainGameMgr.S.RecruitDisciplerMgr.GetRecruitForRecruitType(RecruitType.SilverMedal);
                    if (m_CharacterItem != null)
                    {
                        m_RecruitType = RecruitType.SilverMedal;
                        InitPanelInfo();
                    }
                    break;
                default:
                    break;
            }
            //MainGameMgr.S.CharacterMgr.AddCharacter(m_CharacterItem);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}