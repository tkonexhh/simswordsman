using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
    public class PracticeFieldPanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Image m_Title;

        [Header("Middle")]
        [SerializeField]
        private Text m_BriefIntroductionTxt;
        [SerializeField]
        private Text m_CurLevelText;
        [SerializeField]
        private Image m_PracticeImg;

        [SerializeField]
        private Text m_CurPracticePos;
        [SerializeField]
        private Text m_NextPracticePos;

        [SerializeField]
        private Text m_CurExpValue;
        [SerializeField]
        private Text m_NextExpValue;

        [SerializeField]
        private Text m_UpgradeNeeds;
        [SerializeField]
        private Image m_Res1Img;
        [SerializeField]
        private Text m_Res1Value;
        [SerializeField]
        private Image m_Res2Img;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3Img;
        [SerializeField]
        private Text m_Res3Value;
        [SerializeField]
        private Text m_UpgradeText;
        [SerializeField]
        private Button m_UpgradeBtn; 
        [SerializeField]
        private GameObject m_RedPoint;

        [Header("Bottom")]
        [SerializeField]
        private Transform m_PracticeDiscipleContTra;
        [SerializeField]
        private GameObject m_PracticeDisciplePos;

        private FacilityType m_CurFacilityType;

        private int m_CurLevel;
        private List<CostItem> m_CostItems;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
        private PracticeFieldLevelInfo m_CurPracticeFieldLevelInfo = null;
        private PracticeFieldLevelInfo m_NextPracticeFieldLevelInfo = null;

        private List<PracticeField> m_AllPracticeFieldInfos = null;
        private PracticeFieldController m_CurPracticeFieldController = null;
        private Dictionary<int, GameObject> m_PracticeEntity = new Dictionary<int, GameObject>();


        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (!CommonUIMethod.CheackIsBuild(m_NextPracticeFieldLevelInfo, m_CostItems))
                    return;

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextPracticeFieldLevelInfo.upgradeCoinCost);

                if (isReduceSuccess)
                {
                    AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
                    AddPracticeTime();
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    m_CurPracticeFieldController.RefreshPracticeUnlockInfo(m_CurFacilityType, m_CurLevel);
                    RefreshPanelInfo();
                    HideSelfWithAnim();
                }
            });
        }

        /// <summary>
        /// 升级时增加训练时间
        /// </summary>
        private void AddPracticeTime()
        {
            int curTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(m_CurFacilityType, m_CurLevel);
            int nextTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(m_CurFacilityType, Mathf.Min(m_CurLevel + 1, 6));
            if (nextTime - curTime > 0)
            {
                foreach (var item in m_PracticeEntity.Values)
                {
                    PracticeDisciplePos practice = item.GetComponent<PracticeDisciplePos>();
                    if (practice.GetPracticeFieldState() == SlotState.Practice)
                        practice.IncreaseCountDown(nextTime - curTime);
                }
            }
        }

        private void RefreshResInfo()
        {
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_Res1Value, m_Res1Img, m_Res2Value, m_Res2Img, m_Res3Value, m_Res3Img, m_NextPracticeFieldLevelInfo, this);
        }
        private void HandleAddListenerEvent(int key, object[] param)
        {
            GetPracticeDiscipleForID((PracticeField)param[0]).RefreshPracticeFieldState();
        }

        private PracticeDisciplePos GetPracticeDiscipleForID(PracticeField practiceField)
        {
            if (m_PracticeEntity.ContainsKey(practiceField.Index))
                return m_PracticeEntity[practiceField.Index].GetComponent<PracticeDisciplePos>();
            return null;
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];

            GetInformationForNeed();

            RefreshFixedPanelInfo();
        }

        private void GetInformationForNeed()
        {
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_CurPracticeFieldLevelInfo = (PracticeFieldLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_CurPracticeFieldController = (PracticeFieldController)MainGameMgr.S.FacilityMgr.GetFacilityController(m_CurFacilityType);
            m_AllPracticeFieldInfos = m_CurPracticeFieldController.GetPracticeField();
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_Res1Img.gameObject.SetActive(false);
                m_Res2Img.gameObject.SetActive(false);
                m_Res3Img.gameObject.SetActive(false);
                m_NextExpValue.text = Define.COMMON_DEFAULT_STR;
                m_NextPracticePos.text = Define.COMMON_DEFAULT_STR;
                m_UpgradeNeeds.text = Define.COMMON_DEFAULT_STR;
            }
            else
            {
                m_NextPracticeFieldLevelInfo = (PracticeFieldLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
                m_CostItems = m_NextPracticeFieldLevelInfo.GetUpgradeResCosts();
            }
        }

        private void RefreshFixedPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_NextPracticeFieldLevelInfo, m_CostItems,false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
            switch (m_CurFacilityType)
            {
                case FacilityType.PracticeFieldEast:
                    m_Title.sprite = FindSprite("PracticeFieldEast");
                    break;
                case FacilityType.PracticeFieldWest:
                    m_Title.sprite = FindSprite("PracticeFieldWest");
                    break;
            }
            m_BriefIntroductionTxt.text = m_FacilityConfigInfo.desc;

            RefreshPanelInfo();

            for (int i = 0; i < m_AllPracticeFieldInfos.Count; i++)
            {
                if (m_AllPracticeFieldInfos[i].FacilityType == m_CurFacilityType)
                    CreatePracticeDisciple(m_AllPracticeFieldInfos[i]);
            }
        }

        private void RefreshPanelInfo()
        {
            m_CurLevelText.text = CommonUIMethod.GetGrade(m_CurLevel);
            m_PracticeImg.sprite = FindSprite("PracticeField" + m_CurLevel);
            m_CurPracticePos.text = "练功位:" + CommonUIMethod.GetStrForColor("#365387", m_CurPracticeFieldLevelInfo.GetCurCapacity().ToString());
            m_CurExpValue.text = CommonUIMethod.GetStrForColor("#365387", m_CurPracticeFieldLevelInfo.GetCurExp().ToString() + "经验");
            if (m_NextPracticeFieldLevelInfo != null)
            {
                m_NextPracticePos.text = CommonUIMethod.GetStrForColor("#AD7834", Define.PLUS + (m_NextPracticeFieldLevelInfo.GetNextCapacity ()- m_CurPracticeFieldLevelInfo.GetNextCapacity()).ToString());
                m_NextExpValue.text = CommonUIMethod.GetStrForColor("#365387", Define.PLUS + (m_NextPracticeFieldLevelInfo.GetCurExp()- m_CurPracticeFieldLevelInfo.GetCurExp()).ToString());
                m_UpgradeNeeds.text = "升级需要讲武堂达到" + CommonUIMethod.GetStrForColor("#8C343C", m_NextPracticeFieldLevelInfo.upgradeNeedLobbyLevel.ToString() + "级");
            }
            RefreshResInfo();
        }

        private void CreatePracticeDisciple(PracticeField PracticeField)
        {
            GameObject obj = Instantiate(m_PracticeDisciplePos, m_PracticeDiscipleContTra);

            PracticeDisciplePos itemICom = obj.GetComponent<PracticeDisciplePos>();
            itemICom.OnInit(PracticeField, null, m_CurFacilityType, this);

            if (!m_PracticeEntity.ContainsKey(PracticeField.Index))
                m_PracticeEntity.Add(PracticeField.Index, obj);
        }


        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
            CloseSelfPanel();
        }
    }
}