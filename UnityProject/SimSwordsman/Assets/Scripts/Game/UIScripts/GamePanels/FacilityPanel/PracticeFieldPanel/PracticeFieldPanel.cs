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

        [Header("Bottom")]
        [SerializeField]
        private Transform m_PracticeDiscipleContTra;
        [SerializeField]
        private GameObject m_PracticeDisciple;

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
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                if (!CheackIsBuild())
                {
                    FloatMessage.S.ShowMsg("未达到升级条件");
                    return;
                }

                if (m_NextPracticeFieldLevelInfo == null)
                    return;
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextPracticeFieldLevelInfo.upgradeCoinCost);

                if (isReduceSuccess)
                {
                    AddPracticeTime();
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    m_CurPracticeFieldController.RefreshPracticeUnlockInfo(m_CurFacilityType, m_CurLevel);
                    RefreshPanelInfo();
                }
            });
        }

        private bool CheackIsBuild()
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (m_NextPracticeFieldLevelInfo.GetUpgradeCondition() <= lobbyLevel && CheckPropIsEnough())
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

            return GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_NextPracticeFieldLevelInfo.upgradeCoinCost);
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

        private void RefreshResInfo()
        {
            if (m_CostItems.Count == 1)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite("QingRock");
                m_Res2Value.text = m_NextPracticeFieldLevelInfo.upgradeCoinCost.ToString();
                m_Res2Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {

                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite("QingRock");
                m_Res2Value.text = m_CostItems[1].value.ToString();
                m_Res2Img.sprite = FindSprite("silverWood");
                m_Res3Value.text = m_NextPracticeFieldLevelInfo.upgradeCoinCost.ToString();
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
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

            RefreshFixedPanelInfo();
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_CurPracticeFieldLevelInfo = (PracticeFieldLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_NextPracticeFieldLevelInfo = (PracticeFieldLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
            m_CurPracticeFieldController = (PracticeFieldController)MainGameMgr.S.FacilityMgr.GetFacilityController(m_CurFacilityType);
            m_AllPracticeFieldInfos = m_CurPracticeFieldController.GetPracticeField();
            m_CostItems = m_NextPracticeFieldLevelInfo.GetUpgradeResCosts();
        }

        private void RefreshFixedPanelInfo()
        {
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
            m_CurLevelText.text = m_CurLevel.ToString()+"级";
            m_CurPracticePos.text = "练功位:" + CommonUIMethod.GetStrForColor("#365387", m_CurPracticeFieldLevelInfo.GetCurCapacity().ToString());
            m_NextPracticePos.text = CommonUIMethod.GetStrForColor("#AD7834", Define.PLUS + m_CurPracticeFieldLevelInfo.GetNextCapacity().ToString());
            m_CurExpValue.text = CommonUIMethod.GetStrForColor("#365387", m_CurPracticeFieldLevelInfo.GetCurExp().ToString()+"经验");
            if (m_NextPracticeFieldLevelInfo!=null)
            {
                m_NextExpValue.text = CommonUIMethod.GetStrForColor("#365387", Define.PLUS + m_NextPracticeFieldLevelInfo.GetCurExp().ToString());
            }
            else
            {
                m_NextExpValue.text = Define.COMMON_DEFAULT_STR;
            }
            m_UpgradeNeeds.text = "升级需要讲武堂达到" + CommonUIMethod.GetStrForColor("#8C343C", m_NextPracticeFieldLevelInfo.upgradeNeedLobbyLevel.ToString()+"级");
            RefreshResInfo();

            //m_NextTrainingPositionTxt.text = m_CurPracticeFieldLevelInfo.GetNextCapacity().ToString();
            //m_CurUpgradeSpeedTxt.text = m_CurPracticeFieldLevelInfo.GetCurLevelUpSpeed().ToString();
            //m_NextUpgradeSpeedTxt.text = m_CurPracticeFieldLevelInfo.GetNextLevelUpSpeed().ToString();
            //m_UpgradeConditionsTxt.text = m_CurPracticeFieldLevelInfo.preconditions

        }

        private void CreatePracticeDisciple(PracticeField PracticeField)
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(FindSprite("Lock"));
        

            GameObject obj = Instantiate(m_PracticeDisciple, m_PracticeDiscipleContTra);

            ItemICom itemICom = obj.GetComponent<ItemICom>();
            itemICom.OnInit(PracticeField, null, m_CurFacilityType, sprites);

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