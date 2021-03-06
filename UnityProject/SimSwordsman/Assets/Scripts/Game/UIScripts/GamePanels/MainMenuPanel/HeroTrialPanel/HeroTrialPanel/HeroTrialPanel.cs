using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace GameWish.Game
{

    public class HeroTrialPanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_QuestionMarkBtn; 
        [SerializeField]
        private Image m_TrialImg;

        [Header("Middle")]
        [SerializeField]
        private Button m_SelectCharacterBtn;
        [SerializeField]
        private Button m_FinishTrialBtn;
        [SerializeField]
        private Transform m_TrialClanTra;
        [SerializeField]
        private GameObject m_TrialClanImg;

        [Header("Bottom")]

        [Header("Left")]
        //[SerializeField]
        //private Image m_LeftIcon;   
        [SerializeField]
        private Image m_LeftQualityImg;  
        [SerializeField]
        private Image m_LeftLine;   
        [SerializeField]
        private Text m_LeftName;
        [SerializeField]
        private Transform m_LeftBg;

        [Header("Right")]
        //[SerializeField]
        //private Image m_RightIcon;   
        [SerializeField]
        private Image m_RightQualityImg;  
        [SerializeField]
        private Image m_RightLine;     
        [SerializeField]
        private Transform m_RightBg;   
        [SerializeField]
        private Text m_RightName;

        [Header("Middle")]
        [SerializeField]
        private Text m_CountDownNumber;
        [SerializeField]
        private Slider m_CountDownSlider;

        private CharacterItem m_TrialDisciple;
        private ClanType m_ClanType;
        private HeroTrialStateID m_HeroTrialStateID;
        private List<TrialClanImg> m_TrialClanImgList = new List<TrialClanImg>();

        private List<GameObject> m_HeadList = new List<GameObject>();

        private bool m_TrialComplete = false;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnRefreshTrialPanel, HandAdListenerEvent);
            EventSystem.S.Register(EventID.OnCountDownRefresh, HandAdListenerEvent);
            EventSystem.S.Register(EventID.OnTrialTimeOver, HandAdListenerEvent);

            PlayerPrefs.SetInt(Define.IsOpenHeroTrialPanel, 1);
        }

        private void GetInfomationForNeed()
        {
            m_TrialDisciple = MainGameMgr.S.CharacterMgr.GetCharacterItem(MainGameMgr.S.HeroTrialMgr.TrialDiscipleID);
            m_ClanType = MainGameMgr.S.HeroTrialMgr.TrialClan;
            m_HeroTrialStateID = MainGameMgr.S.HeroTrialMgr.CurState;
        }

        private void HandAdListenerEvent(int key, object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnRefreshTrialPanel:
                    GetInfomationForNeed();
                    RefreshDiscipleInfo();
                    break;
                case (int)EventID.OnCountDownRefresh:
                    RefreshProgress((double)param[0]);
                    break;   
                case (int)EventID.OnEnableFinishBtn:
                    m_TrialComplete = true;
                    m_FinishTrialBtn.gameObject.SetActive(true);
                    break;
                case (int)EventID.OnTrialTimeOver:
                    m_FinishTrialBtn.gameObject.SetActive(true);
                    break;
            }
        }

        private void RefreshProgress(double second)
        {
            if (second<=0)
            {
                second = 0;
            }
            m_CountDownSlider.value = (float)second / (float)MainGameMgr.S.HeroTrialMgr.TrialTotalTime;
            m_CountDownNumber.text = CommonMethod.SplicingTime(second);
        }

        public void RefreshPanelInfo()
        {
            List<HeroTrialConfig>  heroTrialConfigs =  TDHeroTrialConfigTable.GetHeroTrialConfigList();
            foreach (var item in heroTrialConfigs)
            {
                CreateNotClan(item.clanType);
            }
          
            SetTrialImgAndYesClan();
            m_TrialClanImgList.ForEach(i=> {
                if (i.CurClanType == m_ClanType)
                    i.SetSelectedClan();
            });

            double leftTime = MainGameMgr.S.HeroTrialMgr.GetLeftTime();
            if (leftTime>0)
            {
                m_CountDownSlider.value = (float)leftTime / (float)MainGameMgr.S.HeroTrialMgr.TrialTotalTime;
                m_CountDownNumber.text = CommonMethod.SplicingTime(leftTime);
            }
        }

        private void CreateNotClan(ClanType i)
        {
            TrialClanImg trialClanImg = Instantiate(m_TrialClanImg, m_TrialClanTra).GetComponent<TrialClanImg>();
            trialClanImg.OnInit(i);
            m_TrialClanImgList.Add(trialClanImg);
        }

        private void SetTrialImgAndYesClan()
        {
            switch (m_ClanType)
            {
                case ClanType.Gaibang:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Gaibang");
                    break;
                case ClanType.Shaolin:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Shaolin");
                    break;
                case ClanType.Wudang:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Wudang");
                    break;
                case ClanType.Emei:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Emei");
                    break;
                case ClanType.Huashan:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Huashan");
                    break;
                case ClanType.Wudu:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Wudu");
                    break;
                case ClanType.Mojiao:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Mojiao");
                    break;
                case ClanType.Xiaoyao:
                    m_TrialImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Font_Xiaoyao");
                    break;
            }
        }

        private void BindAddListenerEvent()
        {
            m_QuestionMarkBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.IntroductionRulesPanel);
            });

            m_CloseBtn.onClick.AddListener(() =>
            {
                MainGameMgr.S.HeroTrialMgr.OnExitHeroTrial();

                HideSelfWithAnim();
            });

            m_SelectCharacterBtn.onClick.AddListener(() =>
            {
                bool isTrialReady = MainGameMgr.S.HeroTrialMgr.CheckIsTrialReady();
                if (!isTrialReady)
                {
                    FloatMessage.S.ShowMsg("??????????????");
                    return;
                }

                UIMgr.S.OpenPanel(UIID.HeroTrialChooseDisciplePanel, PanelType.HeroTrial);
            });

            m_FinishTrialBtn.onClick.AddListener(() =>
            {
                MainGameMgr.S.HeroTrialMgr.FinishTrial();
                MainGameMgr.S.HeroTrialMgr.Reset();
                MainGameMgr.S.HeroTrialMgr.OnExitHeroTrial();

                float atk = m_TrialDisciple.atkValue;
                m_TrialDisciple.CalculateForceValue();
                PanelPool.S.AddPromotion(new HeroTrial(m_TrialDisciple.id, m_ClanType, atk));
                PanelPool.S.DisplayPanel();
                HideSelfWithAnim();
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            BindAddListenerEvent();
            GetInfomationForNeed();
            RefreshPanelInfo();
            RefreshDiscipleInfo();

        }

        private void RefreshDiscipleInfo()
        {
            if (m_HeadList.Count>=0)
            {
                for (int i = 0; i < m_HeadList.Count; i++)
                {
                    DestroyImmediate(m_HeadList[i]);
                }
                m_HeadList.Clear();
            }
            if (m_TrialDisciple!=null)
            {
                m_HeadList.Add(DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_TrialDisciple, m_LeftBg, new Vector3(5f, 3f, 0), new Vector3(0.4f, 0.4f, 1)));
                //DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_TrialDisciple, m_RightBg, new Vector3(5f, 3f, 0), new Vector3(0.4f, 0.4f, 1));
                m_HeadList.Add(DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_TrialDisciple, m_RightBg, new Vector3(5f, 3f, 0), new Vector3(0.4f, 0.4f, 1)));
            }

            if (m_TrialDisciple == null)
                return;

            //m_LeftIcon.sprite = CommonMethod.GetDiscipleSprite(m_TrialDisciple);
            //m_RightIcon.sprite = CommonMethod.GetDiscipleSprite(m_TrialDisciple);
            m_LeftName.text = m_TrialDisciple.name;
            m_RightName.text = m_TrialDisciple.name;
            m_LeftLine.sprite = GetLine(m_TrialDisciple.quality);
            m_LeftQualityImg.sprite = GetQualityImg(m_TrialDisciple.quality); 
            m_RightLine.sprite = GetLine(CharacterQuality.Hero);
            m_RightQualityImg.sprite = GetQualityImg(CharacterQuality.Hero);
            if (m_HeroTrialStateID == HeroTrialStateID.Idle)
                m_SelectCharacterBtn.gameObject.SetActive(true);
            else
                m_SelectCharacterBtn.gameObject.SetActive(false);

            if (m_HeroTrialStateID == HeroTrialStateID.Finished)
                m_FinishTrialBtn.gameObject.SetActive(true);
            else
                m_FinishTrialBtn.gameObject.SetActive(false);
        }

        private Sprite GetQualityImg(CharacterQuality quality)
        {
            switch (quality)
            {
                case CharacterQuality.Normal:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Quality_Normal");
                case CharacterQuality.Good:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Quality_God");
                case CharacterQuality.Perfect:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Quality_Perfect");
                case CharacterQuality.Hero:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Quality_Hero");

            }
            return null;
        }

        private Sprite GetLine(CharacterQuality quality)
        {
            switch (quality)
            {
                case CharacterQuality.Normal:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Line_Normal");
                case CharacterQuality.Good:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Line_Good");
                case CharacterQuality.Perfect:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Line_Perfect");
                case CharacterQuality.Hero:
                    return SpriteHandler.S.GetSprite(AtlasDefine.HeroTrialPanelAtlas, "HeroTrialPanel_Line_Hero");

            }
            return null;
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            UIMgr.S.OpenPanel(UIID.MainMenuPanel);

            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnRefreshTrialPanel, HandAdListenerEvent);
            EventSystem.S.UnRegister(EventID.OnCountDownRefresh, HandAdListenerEvent);
            EventSystem.S.UnRegister(EventID.OnTrialTimeOver, HandAdListenerEvent);
            //CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}