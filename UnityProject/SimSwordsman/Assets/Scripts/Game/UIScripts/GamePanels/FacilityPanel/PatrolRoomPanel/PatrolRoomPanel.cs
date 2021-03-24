using Qarth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
    public class PatrolRoomPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Text m_PatrolRoomName;
        [SerializeField]
        private Text m_BriefIntroductionTxt;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Text m_UpgradeCostCoinValueTxt;
        [SerializeField]
        private Text m_CurLevelValueTxt;
        [SerializeField]
        private Text m_NextLevelUnlock;
        [SerializeField]
        private Transform m_PatrolRoomContTra;
        [SerializeField]
        private GameObject m_PatrolRoomItem;

        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private FacilityConfigInfo m_PatrolRoomInfo = null;
        private PatrolRoomInfo m_CurPatrolRoomLevelInfo = null;
        private PatrolRoomInfo m_NextPatrolRoomLevelInfo = null;
        private PatrolRoomController m_CurPatrolRoomController = null;
        private List<PatrolRoomSlot> m_PatrolRoomSlotList = null;

        private Dictionary<int, GameObject> m_PatrolRoomSlotInfo = new Dictionary<int, GameObject>();

        protected override void OnUIInit()
        {
            base.OnUIInit();

            EventSystem.S.Register(EventID.OnRefresPatrolSoltInfo, HandleAddListenEvent);

            BindAddListenerEvent();
        }

        private void HandleAddListenEvent(int key, object[] param)
        {
            GetPracticeDiscipleForID((PatrolRoomSlot)param[0]).RefreshPracticeFieldState();
        }
        /// <summary>
        /// 获取具体的坑位
        /// </summary>
        /// <param name="patrolRoomSlot"></param>
        /// <returns></returns>
        private PatrolRoomItem GetPracticeDiscipleForID(PatrolRoomSlot patrolRoomSlot)
        {
            if (m_PatrolRoomSlotInfo.ContainsKey(patrolRoomSlot.Index))
                return m_PatrolRoomSlotInfo[patrolRoomSlot.Index].GetComponent<PatrolRoomItem>();
            return null;
        }

        private void RefreshPanelInfo()
        {
            m_PatrolRoomName.text = m_PatrolRoomInfo.name;
            m_BriefIntroductionTxt.text = m_PatrolRoomInfo.desc;

            RefreshPanelText();

            for (int i = 0; i < m_PatrolRoomSlotList.Count; i++)
            {
                CreateCopyScripturesItem(m_PatrolRoomSlotList[i]);
            }
        }

        private void GetInformationForNeed()
        {
            m_PatrolRoomInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurPatrolRoomLevelInfo = (PatrolRoomInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_NextPatrolRoomLevelInfo = (PatrolRoomInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
            m_CurPatrolRoomController = (PatrolRoomController)MainGameMgr.S.FacilityMgr.GetFacilityController(m_CurFacilityType);
            m_PatrolRoomSlotList = m_CurPatrolRoomController.GetReadingSlotList();
        }

        private void RefreshPanelText()
        {
            m_CurLevelValueTxt.text = m_CurLevel.ToString();

            if (m_NextPatrolRoomLevelInfo != null)
            {
                m_NextLevelUnlock.text = m_NextPatrolRoomLevelInfo.UnlockTower;

                m_UpgradeCostCoinValueTxt.text = m_NextPatrolRoomLevelInfo.upgradeCoinCost.ToString();
            }

            //List<KungfuType> KongfuList = m_CurPatrolRoomLevelInfo.GetNextLevelUnlockedKongfuList();

            //m_MartialArtsContTxt.text = "";
            //for (int i = 0; i < KongfuList.Count; i++)
            //{
            //    m_MartialArtsContTxt.text += KongfuList[0].ToString();
            //}
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_UpgradeBtn.onClick.AddListener(() =>
            {
                if (m_NextPatrolRoomLevelInfo == null)
                    return;

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextPatrolRoomLevelInfo.upgradeCoinCost);

                if (isReduceSuccess)
                {
                    AddPracticeTime();
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    m_CurPatrolRoomController.RefreshSlotInfo(m_CurLevel);
                    RefreshPanelText();
                }
            });
        }

        private void AddPracticeTime()
        {
            int curTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(m_CurFacilityType, m_CurLevel);
            int nextTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(m_CurFacilityType, Mathf.Min(m_CurLevel + 1, 6));
            if (nextTime - curTime > 0)
            {
                // foreach (var item in m_PatrolRoomSlotInfo.Values)
                // {
                //     CopyScripturesItem copyScripturesItem = item.GetComponent<CopyScripturesItem>();
                //     if (copyScripturesItem.GetPracticeFieldState() == SlotState.Busy)
                //         copyScripturesItem.IncreaseCountDown(nextTime - curTime);
                // }
            }
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();

            RefreshPanelInfo();
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        private void CreateCopyScripturesItem(PatrolRoomSlot patrolRoomSlot)
        {
            GameObject game = Instantiate(m_PatrolRoomItem, m_PatrolRoomContTra);
            ItemICom itemICom = game.GetComponent<ItemICom>();
            itemICom.OnInit(patrolRoomSlot, null, m_CurFacilityType);
            m_PatrolRoomSlotInfo.Add(patrolRoomSlot.Index, game);
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnRefresPatrolSoltInfo, HandleAddListenEvent);
        }
    }

}