using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class GetDisciplePanel : AbstractAnimPanel
    {
        [SerializeField]
        private Image m_DiscipleImg;
        [SerializeField]
        private Image m_DiscipleGrade;
        [SerializeField]
        private Text m_CharacterName;
        [SerializeField]
        private Button m_BlackBtn;
        [SerializeField]
        private GameObject m_NewSkillHeroUnlock;
        [SerializeField]
        private Button m_WeChatShareBtn;

        private CharacterItem m_CharacterItem;
        private ClickType m_CurrentClickType = ClickType.None;
        private RecruitType m_RecruitType;
        private AddressableAssetLoader<Sprite> m_Loader;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_BlackBtn.onClick.AddListener(() =>
            {
                EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_WeChatShareBtn.onClick.AddListener(()=> 
            {
                DataAnalysisMgr.S.CustomEvent(DotDefine.Click_WeChatShare_Btn);
                WeChatShareMgr.S.Share(WeChatTex.PrefectCharacter);
            });
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CharacterItem = args[0] as CharacterItem;

            m_WeChatShareBtn.gameObject.SetActive(false);

            m_DiscipleImg.enabled = true;
            m_DiscipleImg.sprite = FindSprite(GetLoadDiscipleName(m_CharacterItem));
            m_DiscipleImg.SetNativeSize();

            m_CurrentClickType = (ClickType)args[1];
            m_RecruitType = (RecruitType)args[2];
            switch (m_CharacterItem.quality)
            {
                case CharacterQuality.Normal:
                    m_DiscipleGrade.sprite = FindSprite("LobbyPanel_Grade_Civilian");
                    break;
                case CharacterQuality.Good:
                    m_DiscipleGrade.sprite = FindSprite("LobbyPanel_Grade_Elite");
                    break;
                case CharacterQuality.Perfect:
                    m_DiscipleGrade.sprite = FindSprite("LobbyPanel_Grade_Genius");

                    m_WeChatShareBtn.gameObject.SetActive(true);

                    DataAnalysisMgr.S.CustomEvent(DotDefine.Open_WeChatShare_Panel);
                    break;
                default:
                    break;
            }
            m_CharacterName.text = m_CharacterItem.name;
            m_NewSkillHeroUnlock.SetActive(true);
            //Instantiate(m_NewSkillHeroUnlock, m_DiscipleImg.transform).transform.localPosition = Vector3.zero;
            RecruitDisciple();
        }

        private void RecruitDisciple()
        {
            switch (m_CurrentClickType)
            {
                case ClickType.Free:
                    break;
                case ClickType.RecruitmentOrder:
                    if (m_RecruitType == RecruitType.SilverMedal)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem(RawMaterial.SilverToken));
                    else
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem(RawMaterial.GoldenToken));
                    MainGameMgr.S.RecruitDisciplerMgr.SetCurRecruitCount(m_RecruitType);
                    break;
                case ClickType.LookAdvertisement:
                    MainGameMgr.S.RecruitDisciplerMgr.SetAdvertisementCount(m_RecruitType);
                    break;
                default:
                    break;
            }
            //EventSystem.S.Send(EventID.OnRefreshRecruitmentOrder, m_RecruitType);
            MainGameMgr.S.RecruitDisciplerMgr.RemoveCharacterList(m_RecruitType, m_CharacterItem);
            MainGameMgr.S.CharacterMgr.AddCharacter(m_CharacterItem);
            MainGameMgr.S.CharacterMgr.SpawnCharacterController(m_CharacterItem);
            EventSystem.S.Send(EventID.OnRefreshPanelInfo, m_RecruitType, m_CurrentClickType);
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            m_Loader?.Release();

            EventSystem.S.Send(EventID.OnAddCharacterPanelClosed);

            //Òýµ¼
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

            if (GuideMgr.S.IsGuideFinish(33) && GuideMgr.S.IsGuideFinish(35) == false)
            {
                EventSystem.S.Send(EventID.OnRecruitmentSystem_FinishedTrigger);
            }

            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }
    }
}