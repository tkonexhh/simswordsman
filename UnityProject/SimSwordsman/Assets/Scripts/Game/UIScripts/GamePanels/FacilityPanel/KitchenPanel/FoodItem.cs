using Qarth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class FoodItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private GameObject Lock;
        [SerializeField]
        private GameObject UnLock;

        [SerializeField]
        private Image m_FoodImg;
        [SerializeField]
        private Text m_FoodNameTxt;
        [SerializeField]
        private Text m_FoodContTxt;

        [SerializeField]
        private Text m_LockConditionTxt;

        [SerializeField]
        private Text m_FoodEffecTxt;

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
        private Button m_MakeADBtn;
        [SerializeField]
        private Text m_EffectiveTimeTxt;
        [SerializeField]
        private Text m_ADEffectiveTimeTxt;
        [SerializeField]
        private Text m_DurationTxt;

        [SerializeField]
        private Text m_MakeTypeTxt;

        [SerializeField]
        private Transform m_DontMakeTra;
        [SerializeField]
        private Transform m_MakingTra;
        [SerializeField]
        private Image m_Progress;

        [HideInInspector]
        public int ID;

        AbstractAnimPanel m_panel;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_panel = t as AbstractAnimPanel;
            BindAddListenerEvent();
            Init((int)obj[0]);
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


        void Init(int id)
        {
            ID = id;
            if (!GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Contains(id))//未解锁
            {
                SetState(0);
            }
            else
            {
                transform.SetAsFirstSibling();

                TDFoodConfig tb = TDFoodConfigTable.GetData(id);
                m_FoodImg.sprite = m_panel.FindSprite(tb.spriteName);
                m_FoodNameTxt.text = tb.name;
                m_FoodContTxt.text = tb.desc;
                m_FoodEffecTxt.text = FoodBuffSystem.S.GetEffectDesc(tb);
                m_EffectiveTimeTxt.text = GetTimeDesc(tb.buffTime) + "有效";
                m_ADEffectiveTimeTxt.text = GetTimeDesc(tb.buffTimeAD) + "有效";

                if (FoodBuffSystem.S.IsActive(ID))
                {
                    string dur = FoodBuffSystem.S.GetCurrentCountdown(ID);
                    if (dur != null)
                    {
                        SetState(1);
                        m_DurationTxt.text = dur;
                        m_Progress.fillAmount = FoodBuffSystem.S.GetCountdowner(ID).GetProgress();
                    }
                    else
                        SetState(2);
                }
                else
                    SetState(2);
            }
        }

        string GetTimeDesc(int minute)
        {
            if (minute >= 60)
                return minute / 60.0f + "小时";
            else
                return minute + "分钟";
        }

        public void SetButtonEvent(Action<object> action)
        {
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
                var list = TDFoodConfigTable.MakeNeedItemIDsDic[ID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                {
                    MainGameMgr.S.InventoryMgr.ReduceItems(list);
                    FoodBuffSystem.S.StartBuff(ID);
                }
                else
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            });
            m_MakeADBtn.onClick.AddListener(() => 
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                //判断材料
                var list = TDFoodConfigTable.MakeNeedItemIDsDic[ID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                    AdsManager.S.PlayRewardAD("AddFood", LookADSuccessCallBack);
                else
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            });
        }
        private void LookADSuccessCallBack(bool obj)
        {
            var list = TDFoodConfigTable.MakeNeedItemIDsDic[ID];
            MainGameMgr.S.InventoryMgr.ReduceItems(list);
            FoodBuffSystem.S.StartBuff(ID, true);
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
                    m_LockConditionTxt.text = string.Format("伙房等级达到<color=#384B76>{0}</color>级", GetUnlockLevel(ID));
                    m_FoodNameTxt.text = "未解锁";
                    break;
                case 1:
                    UnLock.SetActive(true);
                    Lock.SetActive(false);
                    m_MakingTra.gameObject.SetActive(true);
                    m_DontMakeTra.gameObject.SetActive(false);
                    break;
                case 2:
                    UnLock.SetActive(true);
                    Lock.SetActive(false);

                    m_MakingTra.gameObject.SetActive(false);
                    m_DontMakeTra.gameObject.SetActive(true);
                    //设置材料
                    SetMakeNeedRes();
                    break;
                default:
                    break;
            }
        }
        int GetUnlockLevel(int id)
        {
            var tb = TDFacilityKitchenTable.dataList;
            foreach (var item in tb)
            {
                if (item.unlockRecipe == id)
                {
                    return item.level;
                }
            }
            return 999;
        }
        void SetMakeNeedRes()
        {
            var infos = TDFoodConfigTable.MakeNeedItemIDsDic[ID];
            if (infos.Count == 2)
            {
                m_NeedItem2.gameObject.SetActive(true);
                m_NeedItemCount2Txt.gameObject.SetActive(true);

                m_NeedItem2.sprite = m_panel.FindSprite(TDItemConfigTable.GetData(infos[1].itemId).iconName);
                m_NeedItemCount2Txt.text = infos[1].value.ToString();
            }
            else
            {
                m_NeedItem1.gameObject.SetActive(false);
                m_NeedItemCount1Txt.gameObject.SetActive(false);
            }
            m_NeedItem1.sprite = m_panel.FindSprite(TDItemConfigTable.GetData(infos[0].itemId).iconName);
            m_NeedItemCount1Txt.text = infos[0].value.ToString();
        }
    }

}

