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

        [Header("Bottom")]
        [SerializeField]
        private Transform m_MartialArtsContTra;
        [SerializeField]
        private GameObject m_CopyScripturesItem;

        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private List<CostItem> m_CostItems;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
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

        private bool CheackIsBuild()
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (m_NextKongfuLibraryLevelInfo.GetUpgradeCondition() <= lobbyLevel && CheckPropIsEnough())
                return true;
            return false;
        }
        private bool CheckPropIsEnough()
        {
            for (int i = 0; i < m_CostItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)m_CostItems[i].itemId, m_CostItems[i].value);
                if (!isHave)
                    return false;
            }

            return GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_NextKongfuLibraryLevelInfo.upgradeCoinCost);
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
            RefreshFixedInfo();

       
            RefreshPanelText();

            for (int i = 0; i < m_ReadingSlotList.Count; i++)
            {
                CreateCopyScripturesItem(m_ReadingSlotList[i]);
            }
        }

        private string GetKungfuStr(List<KongfuType> kungfuTypes)
        {
            string str = string.Empty;
            str += TDKongfuConfigTable.GetKungfuConfigInfo(kungfuTypes[0]).Name;
            for (int i = 1; i < kungfuTypes.Count; i++)
            {
                str += TDKongfuConfigTable.GetKungfuConfigInfo(kungfuTypes[i]).Name;
            }
            return str; 
         }


        private void RefreshFixedInfo()
        {
            m_NextLevelUnlockText.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NEXTLEVELUNLOCK);
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_CurKongfuLibraryLevelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_NextKongfuLibraryLevelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
            m_CurKongfuLibraryController = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(m_CurFacilityType);
            m_ReadingSlotList = m_CurKongfuLibraryController.GetReadingSlotList();
            m_CostItems = m_NextKongfuLibraryLevelInfo.GetUpgradeResCosts();
        }

        private void RefreshPanelText()
        {
            RefreshResInfo();
            m_IconImage.sprite = FindSprite("KongfuLibrary");
            m_BriefIntroductionTxt.text = m_FacilityConfigInfo.desc;
            m_UpgradeCondition.text = CommonUIMethod.GetUpgradeCondition(m_NextKongfuLibraryLevelInfo.upgradeNeedLobbyLevel);
            m_LevelValue.text = CommonUIMethod.GetGrade(m_CurKongfuLibraryLevelInfo.level);
            m_MartialArtsContTxt.text = GetKungfuStr(m_CurKongfuLibraryLevelInfo.GetCurLevelUnlockedKongfuList());
            if (m_NextKongfuLibraryLevelInfo != null)
            {
                //m_UpgradeCostCoinValueTxt.text = m_NextKongfuLibraryLevelInfo.upgradeCoinCost.ToString();
            }

            List<KongfuType> KongfuList = m_CurKongfuLibraryLevelInfo.GetNextLevelUnlockedKongfuList();

            m_MartialArtsContTxt.text = "";
            for (int i = 0; i < KongfuList.Count; i++)
            {
                m_MartialArtsContTxt.text += KongfuList[0].ToString();
            }
        }
        private void RefreshResInfo()
        {
            if (m_CostItems.Count == 1)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite("QingRock");
                m_Res2Value.text = m_NextKongfuLibraryLevelInfo.upgradeCoinCost.ToString();
                m_Res2Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite("QingRock");
                m_Res2Value.text = m_CostItems[1].value.ToString();
                m_Res2Img.sprite = FindSprite("silverWood");
                m_Res3Value.text = m_NextKongfuLibraryLevelInfo.upgradeCoinCost.ToString();
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_UpgradeBtn.onClick.AddListener(() =>
            {
                if (!CheackIsBuild())
                {
                    FloatMessage.S.ShowMsg("未达到升级条件");
                    return;
                }
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