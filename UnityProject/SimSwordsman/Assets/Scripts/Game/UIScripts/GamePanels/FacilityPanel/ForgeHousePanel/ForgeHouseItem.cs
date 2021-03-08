using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ForgeHouseItem : MonoBehaviour, ItemICom
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

        [HideInInspector]
        public int ID;
        [HideInInspector]
        public int UnlockLevel;//解锁等级

        AbstractAnimPanel m_panel;

        string m_StringID = "ForgeHousePanel";

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
            if (!IsUnlock((EquipmentType)ID))
            {
                SetState(0);
            }
            else
            {
                transform.SetAsFirstSibling();

                var tb = TDEquipmentConfigTable.GetData(ID);
                m_ItemIcon.sprite = m_panel.FindSprite(tb.iconName);
                m_NameTxt.text = tb.name;
                m_DescTxt.text = tb.desc;
                var equ = TDEquipmentConfigTable.m_EquipDic[ID];
                float num = 100 * (equ.GetAtkBonusForClassID(1) - 1);
                m_EffecTxt.text = string.Format("功力+<color=#8C343C>{0}%</color>", (int)num);

                if (CountdownSystem.S.IsActive(m_StringID, ID))
                {
                    string dur = CountdownSystem.S.GetCurrentCountdownTime(m_StringID, ID);
                    if (dur != null)
                    {
                        SetState(1);
                        m_DurationTxt.text = dur;
                        m_Progress.fillAmount = CountdownSystem.S.GetCountdowner(m_StringID, ID).GetProgress();
                    }
                    else
                        SetState(2);
                }
                else
                    SetState(2);
            }
        }
        bool IsUnlock(EquipmentType id)
        {
            List<EquipmentType> list = null;
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.ForgeHouse);
            for (int i = 1; i <= level; i++)
            {
                list = TDFacilityForgeHouseTable.GetLevelInfo(i).GetCurEquipmentType();
                if (list.Contains(id))
                    return true;
            }
            return false;
        }
        private void BindAddListenerEvent()
        {
            //音效
            foreach (var item in transform.GetComponentsInChildren<Button>(true))
            {
                item.onClick.AddListener(() => AudioMgr.S.PlaySound(Define.SOUND_UI_BTN));
            }
            m_MakeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                //判断材料
                var list = TDEquipmentConfigTable.MakeNeedItemIDsDic[ID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                {
                    MainGameMgr.S.InventoryMgr.ReduceItems(list);
                    CountdownSystem.S.StartCountdownerWithMin(m_StringID, ID, TDEquipmentConfigTable.GetData(ID).forgeTime);
                }
                else
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            });
            m_CompleteADBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                AdsManager.S.PlayRewardAD("AddFood", LookADSuccessCallBack);

            });
        }
        private void LookADSuccessCallBack(bool obj)
        {
            if (ID > 500)
            {
                List<RewardBase> rewards = new List<RewardBase>();
                rewards.Add(new ArmorReward(ID, 1));
                UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)ID, Step.One), 1);

            }
            else
            {
                List<RewardBase> rewards = new List<RewardBase>();
                rewards.Add(new ArmsReward(ID, 1));
                UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)ID, Step.One), 1);
            }
            SetState(2);
            CountdownSystem.S.Cancel(m_StringID, ID);
        }

        /// <summary>
        /// 0未解锁 1 正在制作 2 未制作
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
                    m_LockConditionTxt.text = string.Format("锻造房升至 <color=#9C4B45>{0}</color>级 后解锁", UnlockLevel);
                    break;
                case 1:
                    UnLock.SetActive(true);
                    Lock.SetActive(false);

                    m_MakingTra.gameObject.SetActive(true);
                    m_DontMakeTra.gameObject.SetActive(false);
                    //设置材料
                    SetMakeNeedRes(TDEquipmentConfigTable.MakeNeedItemIDsDic[ID]);
                    break;
                case 2:
                    UnLock.SetActive(true);
                    Lock.SetActive(false);

                    m_MakingTra.gameObject.SetActive(false);
                    m_DontMakeTra.gameObject.SetActive(true);
                    //设置材料
                    SetMakeNeedRes(TDEquipmentConfigTable.MakeNeedItemIDsDic[ID]);
                    break;
                default:
                    break;
            }
        }
        void SetMakeNeedRes(List<CostItem> infos)
        {
            if (infos.Count == 2)
            {
                m_NeedItem2.gameObject.SetActive(true);
                m_NeedItemCount2Txt.gameObject.SetActive(true);

                m_NeedItem2.sprite = m_panel.FindSprite(TDItemConfigTable.GetData(infos[1].itemId).iconName);
                m_NeedItemCount2Txt.text = infos[1].value.ToString();
            }
            else
            {
                m_NeedItem2.gameObject.SetActive(false);
                m_NeedItemCount2Txt.gameObject.SetActive(false);
            }
            m_NeedItem1.sprite = m_panel.FindSprite(TDItemConfigTable.GetData(infos[0].itemId).iconName);
            m_NeedItemCount1Txt.text = infos[0].value.ToString();
        }
    }
}