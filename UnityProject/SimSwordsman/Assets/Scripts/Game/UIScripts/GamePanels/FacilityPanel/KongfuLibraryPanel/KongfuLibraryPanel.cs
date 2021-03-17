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
        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;

        [Header("Middle")]
        [SerializeField]
        private Text m_BriefIntroductionTxt;
        [SerializeField]
        private Image m_IconImage;
        [SerializeField]
        private Text m_LevelValue;
        [SerializeField]
        private RectTransform m_UnlockBg;
        [SerializeField]
        private Text m_NextLevelUnlockText;
        [SerializeField]
        private Text m_MartialArtsContTxt;
        [SerializeField]
        private Text m_UpgradeCondition;
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
        private Button m_UpgradeBtn;
        [SerializeField]
        private Text m_UpgradeText;
        [SerializeField]
        private GameObject m_RedPoint;
        [Header("Bottom")]
        [SerializeField]
        private Transform m_MartialArtsContTra;
        [SerializeField]
        private GameObject m_CopyScripturesItem;


        private FacilityType m_CurFacilityType = FacilityType.None;

        private const int NextKungfuFontSize = 0;
        private const int KungfuFontWight = 218;
        private int m_CurLevel;
        private List<CostItem> m_CostItems;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
        private KongfuLibraryLevelInfo m_CurKongfuLibraryLevelInfo = null;
        private KongfuLibraryLevelInfo m_NextFacilityLevelInfo = null;
        private KongfuLibraryController m_CurKongfuLibraryController = null;
        private List<KungfuLibraySlot> m_ReadingSlotList = null;

        private Dictionary<int, GameObject> m_KongfuLibrarySoltInfo = new Dictionary<int, GameObject>();

        public static bool isOpened = false;

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
            if (CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems, false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);

            RefreshFixedInfo();

            RefreshPanelText();

            for (int i = 0; i < m_ReadingSlotList.Count; i++)
            {
                CreateCopyScripturesItem(m_ReadingSlotList[i]);
            }
        }

        /// <summary>
        /// 动态变化字体背景
        /// </summary>
        /// <param name="kungfuTypes"></param>
        private void SetNextKungfuStr(List<KungfuType> kungfuTypes)
        {
            m_UnlockBg.sizeDelta = new Vector2(KungfuFontWight, 0);
            m_MartialArtsContTxt.rectTransform.sizeDelta = new Vector2(KungfuFontWight, 0);
            string str = string.Empty;
            Vector2 deltaData = Vector2.zero;
            str += TDKongfuConfigTable.GetKungfuConfigInfo(kungfuTypes[0]).Name;
            deltaData += new Vector2(0, NextKungfuFontSize);
            for (int i = 1; i < kungfuTypes.Count; i++)
            {
                str += "\n";
                str += TDKongfuConfigTable.GetKungfuConfigInfo(kungfuTypes[i]).Name;
                deltaData += new Vector2(0, NextKungfuFontSize);
            }
            m_MartialArtsContTxt.rectTransform.sizeDelta += deltaData;
            deltaData += new Vector2(0, NextKungfuFontSize);
            m_UnlockBg.sizeDelta += deltaData;
            m_MartialArtsContTxt.text = str;
            m_MartialArtsContTxt.text = m_MartialArtsContTxt.text.Replace("\\n", "\n");
        }


        private void RefreshFixedInfo()
        {
            m_NextLevelUnlockText.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NEXTLEVELUNLOCK);
        }

        private void GetInformationForNeed()
        {
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_CurKongfuLibraryLevelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_CurKongfuLibraryController = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(m_CurFacilityType);
            m_ReadingSlotList = m_CurKongfuLibraryController.GetReadingSlotList();
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_Res1Img.gameObject.SetActive(false);
                m_Res2Img.gameObject.SetActive(false);
                m_Res3Img.gameObject.SetActive(false);
                m_UpgradeCondition.text = Define.COMMON_DEFAULT_STR;
                m_MartialArtsContTxt.text = Define.COMMON_DEFAULT_STR;
            }
            else
            {
                m_NextFacilityLevelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
                m_CostItems = m_NextFacilityLevelInfo.GetUpgradeResCosts();
            }
        }

        private void RefreshPanelText()
        {
            if (m_NextFacilityLevelInfo != null)
            {
                SetNextKungfuStr(m_NextFacilityLevelInfo.GetCurLevelUnlockedKongfuList());
                m_UpgradeCondition.text = CommonUIMethod.GetUpgradeCondition(m_NextFacilityLevelInfo.upgradeNeedLobbyLevel);
            }
            RefreshResInfo();
            m_IconImage.sprite = FindSprite("KongfuLibrary" + m_CurLevel);
            m_BriefIntroductionTxt.text = m_FacilityConfigInfo.desc;
            m_LevelValue.text = CommonUIMethod.GetGrade(m_CurKongfuLibraryLevelInfo.level);


            List<KungfuType> KongfuList = m_CurKongfuLibraryLevelInfo.GetNextLevelUnlockedKongfuList();
        }
        private void RefreshResInfo()
        {
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_Res1Value, m_Res1Img, m_Res2Value, m_Res2Img, m_Res3Value, m_Res3Img, m_NextFacilityLevelInfo, this);
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_UpgradeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (!CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems))
                    return;
                if (m_NextFacilityLevelInfo == null)
                    return;

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);

                if (isReduceSuccess)
                {
                    AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);

                    AddPracticeTime();
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    m_CurKongfuLibraryController.RefreshSlotInfo(m_CurLevel);
                    RefreshPanelText();
                    DataAnalysisMgr.S.CustomEvent(DotDefine.facility_upgrade, m_CurFacilityType.ToString() + ";" + m_CurLevel);
                    HideSelfWithAnim();
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
            isOpened = true;

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
            CopyScripturesItem itemICom = game.GetComponent<CopyScripturesItem>();
            itemICom.OnInit(kungfuLibraySlot, null, m_CurFacilityType, this);
            m_KongfuLibrarySoltInfo.Add(kungfuLibraySlot.Index, game);
        }

        protected override void OnClose()
        {
            base.OnClose();

            isOpened = false;

            EventSystem.S.UnRegister(EventID.OnRefresKungfuSoltInfo, HandleAddListenEvent);
        }
    }

}