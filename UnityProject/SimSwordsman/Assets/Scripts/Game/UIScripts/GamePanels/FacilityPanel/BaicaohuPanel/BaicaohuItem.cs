using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class BaicaohuItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private GameObject Lock;
        [SerializeField]
        private GameObject UnLock;

        [SerializeField]
        private Image m_ItemIcon;
        [SerializeField]
        private Text m_NameTxt;
        [SerializeField]
        private Text m_DescTxt;
        [SerializeField]
        private Text m_EffecTxt;

        [SerializeField]
        private Image m_NeedItem1;
        [SerializeField]
        private Image m_NeedItem2;
        [SerializeField]
        private Text m_NeedItemCount1Txt;
        [SerializeField]
        private Text m_NeedItemCount2Txt;

        [SerializeField]
        private Button m_MakeBtn;
        [SerializeField]
        private Button m_CompleteADBtn;
        [SerializeField]
        private Text m_LockConditionTxt;
        [SerializeField]
        private Text m_DurationTxt;
        [SerializeField]
        private Transform m_DontMakeTra;
        [SerializeField]
        private Transform m_MakingTra;

        [SerializeField]
        private Image m_Progress;
        [Header("Res")]
        [SerializeField]
        private Transform m_UpgradeResItemTra;
        [SerializeField]
        private GameObject m_UpgradeResItem;
        [HideInInspector]
        public int ID;
        [HideInInspector]
        public int UnlockLevel;//解锁等级 

        private List<UpgradeResItem> m_UpgradeResItemList = new List<UpgradeResItem>();

        AbstractAnimPanel m_panel;

        string m_StringID = "BaicaohuPanel";

        private BaiCaoWuData m_BaiCaoWuData = null;

        private CountDownItemTest m_CountDownItem = null;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_panel = t as AbstractAnimPanel;
            BindAddListenerEvent();
            Init();
        }

        public void SetButtonEvent(Action<object> action)
        {
        }

        public void StartEffect(float progress, string dur)
        {
            SetState(1);
            m_DurationTxt.text = dur;
            m_Progress.fillAmount = progress;
        }
        public void StopEffect()
        {
            SetState(2);
        }
        public void Countdown(float progress, string dur)
        {
            m_DurationTxt.text = dur;
            m_Progress.fillAmount = progress;
        }


        void Init()
        {
            if (!IsUnlock((HerbType)ID))
            {
                SetState(0);
            }
            else
            {
                transform.SetAsFirstSibling();

                var tb = TDHerbConfigTable.GetData(ID);
                m_ItemIcon.sprite = m_panel.FindSprite(tb.icon);
                m_NameTxt.text = tb.name;
                m_DescTxt.text = tb.desc;
                m_EffecTxt.text = tb.effectDesc;

                m_BaiCaoWuData = GameDataMgr.S.GetClanData().GetBaiCaoWuData(ID);
                if (m_BaiCaoWuData == null)
                {
                    SetState(2);
                }
                else
                {
                    SetState(1);
                    UpdateProgress();
                }
            }
        }

        private void UpdateProgress()
        {
            m_CountDownItem = CountDowntMgr.S.GetCountDownItemByID(m_BaiCaoWuData.GetCountDownID());

            if (m_CountDownItem != null)
            {
                m_CountDownItem.RegisterUpdateCallBack(OnUpdateCountDownCallBack);
                m_CountDownItem.RegisterEndCallBack(OnEndCountDownCallBack);
            }
            OnUpdateCountDownCallBack(0);
        }

        private void OnEndCountDownCallBack(int remainTime)
        {
            SetState(2);
        }

        private void OnUpdateCountDownCallBack(int remainTime)
        {
            Countdown(m_BaiCaoWuData.GetProgress(), m_BaiCaoWuData.GetRemainTimeStr());
        }

        public void OnClose()
        {
            if (m_CountDownItem != null)
            {
                m_CountDownItem.UnRegisterUpdateCallBack(OnUpdateCountDownCallBack);
                m_CountDownItem.UnRegisterEndCallBack(OnEndCountDownCallBack);
            }
        }
        bool IsUnlock(HerbType id)
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Baicaohu);
            for (int i = 1; i <= level; i++)
                if (id == TDFacilityBaicaohuTable.GetLevelInfo(i).GetCurMedicinalPowderType())
                    return true;
            return false;
        }
        private void BindAddListenerEvent()
        {
            //音效
            m_MakeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                //判断材料
                var list = TDHerbConfigTable.MakeNeedItemIDsDic[ID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                {
                    DataAnalysisMgr.S.CustomEvent(DotDefine.f_make_medicine, ID.ToString());

                    MainGameMgr.S.InventoryMgr.ReduceItems(list);

                    m_BaiCaoWuData = BaiCaoWuSystemMgr.S.AddBaiCaoWuItemData(ID);
                    GameDataMgr.S.GetPlayerData().recordData.AddMedicine();
                    SetState(1);
                    UpdateProgress();
                }
                else
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            });
            m_CompleteADBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                AdsManager.S.PlayRewardAD("FinishNow", LookADSuccessCallBack);
            });
        }
        private void LookADSuccessCallBack(bool obj)
        {
            BaiCaoWuSystemMgr.S.ImmediatelyCompleteBaiCaoWuCountDown(ID);

            List<RewardBase> rewards = new List<RewardBase>();
            rewards.Add(new MedicineReward(ID, 1));
            UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)ID, 1));
            SetState(2);
        }

        /// <summary>
        /// 0未解锁 1 正在炒制 2 未炒制
        /// </summary>
        /// <param name="status"></param>
        void SetState(int status)
        {
            switch (status)
            {
                case 0:
                    UnLock.SetActive(false);
                    Lock.SetActive(true);
                    //解锁条件
                    m_LockConditionTxt.text = string.Format("百草屋升至 <color=#9C4B45>{0}</color>级 后解锁", UnlockLevel);
                    break;
                case 1:
                    UnLock.SetActive(true);
                    Lock.SetActive(false);

                    m_MakingTra.gameObject.SetActive(true);
                    m_DontMakeTra.gameObject.SetActive(false);
                    //设置材料
                    SetMakeNeedRes(TDHerbConfigTable.MakeNeedItemIDsDic[ID]);
                    break;
                case 2:
                    UnLock.SetActive(true);
                    Lock.SetActive(false);

                    m_MakingTra.gameObject.SetActive(false);
                    m_DontMakeTra.gameObject.SetActive(true);
                    //设置材料
                    SetMakeNeedRes(TDHerbConfigTable.MakeNeedItemIDsDic[ID]);
                    break;
                default:
                    break;
            }
        }

        void SetMakeNeedRes(List<CostItem> infos)
        {
            for (int i = 0; i < m_UpgradeResItemList.Count; i++)
                DestroyImmediate(m_UpgradeResItemList[i].gameObject);
            m_UpgradeResItemList.Clear();
            CommonUIMethod.RefreshUpgradeResInfo(infos, m_UpgradeResItemTra, m_UpgradeResItem,null, m_UpgradeResItemList);
        }
    }
}