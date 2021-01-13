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
        [SerializeField]
        private Text m_PracticeFieldName;
        [SerializeField]
        private Text m_BriefIntroductionTxt;
        [SerializeField]
        private Text m_UpgradeCostCoinValueTxt;
        [SerializeField]
        private Text m_CurLevelTxt;
        [SerializeField]
        private Text m_CurTrainingPositionTxt;
        [SerializeField]
        private Text m_NextTrainingPositionTxt;
        [SerializeField]
        private Text m_UpgradeConditionsTxt;
        [SerializeField]
        private Text m_CurUpgradeSpeedTxt;
        [SerializeField]
        private Text m_NextUpgradeSpeedTxt;

        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Button m_CloseBtn;

        [SerializeField]
        private Transform m_PracticeDiscipleContTra;
        [SerializeField]
        private GameObject m_PracticeDisciple;

        private FacilityType m_CurFacilityType;

        private int m_CurLevel;
        private PracticeFieldLevelInfo m_CurPracticeFieldLevelInfo = null;

        private List<PracticeField> m_AllPracticeFieldInfos = null;

        private Dictionary<int, GameObject> m_PracticeEntity = new Dictionary<int, GameObject>();


        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnSelectDisciple, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnDisciplePracticeOver, HandleAddListenerEvent);
            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(long.Parse(m_UpgradeCostCoinValueTxt.text));

                if (isReduceSuccess)
                {
                    AddPracticeTime();
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    MainGameMgr.S.FacilityMgr.RefreshPracticeUnlockInfo(m_CurFacilityType, m_CurLevel);
                    RefreshPanelText();
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
                    PracticeDisciple practice = item.GetComponent<PracticeDisciple>();
                    if (practice.GetPracticeFieldState() == SlotState.Practice)
                        practice.IncreaseCountDown(nextTime - curTime);
                }
            }
        }

        private void CreateCountDown(PracticeField prac)
        {
            CountDownItem countDownMgr = new CountDownItem(prac.FacilityType.ToString() + prac.Index, 100);
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            PracticeDisciple practice = GetPracticeDiscipleForID(prac);
            practice.RefreshPracticeFieldState();
            countDownMgr.OnSecondRefreshEvent = practice.refresAction;
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectDisciple:
                    //CreateCountDown(((PracticeField)param[0]));
                    break;
            }
            GetPracticeDiscipleForID((PracticeField)param[0]).RefreshPracticeFieldState();
        }

        private PracticeDisciple GetPracticeDiscipleForID(PracticeField practiceField)
        {
            if (m_PracticeEntity.ContainsKey(practiceField.Index))
                return m_PracticeEntity[practiceField.Index].GetComponent<PracticeDisciple>();
            return null;
        }



        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];

            GetInformationForNeed();

            RefreshPanelInfo();
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurPracticeFieldLevelInfo = (PracticeFieldLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_AllPracticeFieldInfos = MainGameMgr.S.FacilityMgr.GetPracticeField();
        }

        private void RefreshPanelInfo()
        {
            switch (m_CurFacilityType)
            {
                case FacilityType.PracticeFieldEast:
                    m_PracticeFieldName.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_PRACTICEFIELDEAST_NAME);
                    break;
                case FacilityType.PracticeFieldWest:
                    m_PracticeFieldName.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_PRACTICEFIELDWEST_NAME);
                    break;
            }
            m_BriefIntroductionTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_PRACTICEFIELD_DESCRIBLE);


            RefreshPanelText();

            for (int i = 0; i < m_AllPracticeFieldInfos.Count; i++)
            {
                if (m_AllPracticeFieldInfos[i].FacilityType == m_CurFacilityType)
                    CreatePracticeDisciple(m_AllPracticeFieldInfos[i]);
            }
        }

        private void RefreshPanelText()
        {

            m_UpgradeCostCoinValueTxt.text = m_CurPracticeFieldLevelInfo.upgradeCoinCost.ToString();
            m_CurLevelTxt.text = m_CurLevel.ToString();
            m_CurTrainingPositionTxt.text = m_CurPracticeFieldLevelInfo.GetCurCapacity().ToString();
            m_NextTrainingPositionTxt.text = m_CurPracticeFieldLevelInfo.GetNextCapacity().ToString();
            m_CurUpgradeSpeedTxt.text = m_CurPracticeFieldLevelInfo.GetCurLevelUpSpeed().ToString();
            m_NextUpgradeSpeedTxt.text = m_CurPracticeFieldLevelInfo.GetNextLevelUpSpeed().ToString();
            //m_UpgradeConditionsTxt.text = m_CurPracticeFieldLevelInfo.preconditions

        }

        private void CreatePracticeDisciple(PracticeField PracticeField)
        {
            GameObject obj = Instantiate(m_PracticeDisciple, m_PracticeDiscipleContTra);
            ItemICom itemICom = obj.GetComponent<ItemICom>();
            itemICom.OnInit(PracticeField, null, m_CurFacilityType);

            if (!m_PracticeEntity.ContainsKey(PracticeField.Index))
                m_PracticeEntity.Add(PracticeField.Index, obj);
        }


        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnSelectDisciple, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnDisciplePracticeOver, HandleAddListenerEvent);
            CloseSelfPanel();
        }
    }

}