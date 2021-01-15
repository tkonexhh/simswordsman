using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
    public class KongfuLibraryPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Text m_KongfuLibraryNameTxt;
        [SerializeField]
        private Text m_BriefIntroductionTxt;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Text m_UpgradeCostCoinValueTxt;
        [SerializeField]
        private Text m_CurLevelValueTxt;
        [SerializeField]
        private Text m_MartialArtsContTxt;
        [SerializeField]
        private Transform m_MartialArtsContTra;
        [SerializeField]
        private GameObject m_CopyScripturesItem;

        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private KongfuLibraryLevelInfo m_CurKongfuLibraryLevelInfo = null;
        private KongfuLibraryLevelInfo m_NextKongfuLibraryLevelInfo = null;
        private KongfuLibraryController m_CurKongfuLibraryController = null;
        private List<KungfuLibraySlot> m_ReadingSlotList = null;

        private Dictionary<int, GameObject> m_KongfuLibrarySoltInfo = new Dictionary<int, GameObject>();

        protected override void OnUIInit()
        {
            base.OnUIInit();

            EventSystem.S.Register(EventID.OnRefresKungfuSoltInfo, HandleAddListenEvent);

            BindAddListenerEvent();
        }

        private void HandleAddListenEvent(int key, object[] param)
        {
            GetPracticeDiscipleForID((KungfuLibraySlot)param[0]).RefreshPracticeFieldState();
        }
        /// <summary>
        /// 获取具体的坑位
        /// </summary>
        /// <param name="kungfuLibraySlot"></param>
        /// <returns></returns>
        private CopyScripturesItem GetPracticeDiscipleForID(KungfuLibraySlot kungfuLibraySlot)
        {
            if (m_KongfuLibrarySoltInfo.ContainsKey(kungfuLibraySlot.Index))
                return m_KongfuLibrarySoltInfo[kungfuLibraySlot.Index].GetComponent<CopyScripturesItem>();
            return null;
        }

        private void RefreshPanelInfo()
        {
            m_KongfuLibraryNameTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KONGFULIVRARY_NAME);
            m_BriefIntroductionTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KONGFULIVRARY_DESCRIBLE);

            RefreshPanelText();

            for (int i = 0; i < m_ReadingSlotList.Count; i++)
            {
                CreateCopyScripturesItem(m_ReadingSlotList[i]);
            }
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurKongfuLibraryLevelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_NextKongfuLibraryLevelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
            m_CurKongfuLibraryController = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(m_CurFacilityType);
            m_ReadingSlotList = m_CurKongfuLibraryController.GetReadingSlotList();
        }

        private void RefreshPanelText()
        {
            m_CurLevelValueTxt.text = m_CurLevel.ToString();

            if (m_NextKongfuLibraryLevelInfo != null)
            {
                m_UpgradeCostCoinValueTxt.text = m_NextKongfuLibraryLevelInfo.upgradeCoinCost.ToString();
            }

            List<KungfuType> KongfuList = m_CurKongfuLibraryLevelInfo.GetNextLevelUnlockedKongfuList();

            m_MartialArtsContTxt.text = "";
            for (int i = 0; i < KongfuList.Count; i++)
            {
                m_MartialArtsContTxt.text += KongfuList[0].ToString();
            }
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_UpgradeBtn.onClick.AddListener(() =>
            {
                if (m_NextKongfuLibraryLevelInfo == null)
                    return;

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextKongfuLibraryLevelInfo.upgradeCoinCost);

                if (isReduceSuccess)
                {
                    AddPracticeTime();
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    m_CurKongfuLibraryController.RefreshSlotInfo(m_CurLevel);
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
                foreach (var item in m_KongfuLibrarySoltInfo.Values)
                {
                    CopyScripturesItem copyScripturesItem = item.GetComponent<CopyScripturesItem>();
                    if (copyScripturesItem.GetPracticeFieldState() == SlotState.CopyScriptures)
                        copyScripturesItem.IncreaseCountDown(nextTime - curTime);
                }
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

        private void CreateCopyScripturesItem(KungfuLibraySlot kungfuLibraySlot)
        {
            GameObject game = Instantiate(m_CopyScripturesItem, m_MartialArtsContTra);
            ItemICom itemICom = game.GetComponent<ItemICom>();
            itemICom.OnInit(kungfuLibraySlot, null, m_CurFacilityType);
            m_KongfuLibrarySoltInfo.Add(kungfuLibraySlot.Index, game);
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnRefresKungfuSoltInfo, HandleAddListenEvent);
        }
    }

}