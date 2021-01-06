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
        private Sprite Progress_Over;
        [SerializeField]
        private Sprite Progress_Not_Over;

        [SerializeField]
        private GameObject Lock;
        [SerializeField]
        private GameObject UnLock;

        [SerializeField]
        private Image m_FoodImg;
        //[SerializeField]
        //private Image m_LockFoodImg;
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
        private Transform m_DontMakeTra;
        [SerializeField]
        private Transform m_MakingTra;
        [SerializeField]
        private Image[] Progress;

        [HideInInspector]
        public int ID;
        

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            BindAddListenerEvent();
            Init((int)obj[0]);
        }
        
        public void StartEffect(string dur)
        {
            SetState(1);
            m_DurationTxt.text = dur;
        }
        public void StopEffect()
        {
            SetState(2);
        }
        public void Countdown(string dur)
        {
            m_DurationTxt.text = dur;
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
                TDFoodConfig tb = TDFoodConfigTable.GetData(id);
                m_FoodImg.sprite = Resources.Load<Sprite>("Sprites/Facility/Kitchen/" + tb.spriteName);
                m_FoodNameTxt.text = tb.name;
                m_FoodContTxt.text = tb.desc;
                m_FoodEffecTxt.text = BuffSystem.S.GetEffectDesc(tb);
                m_EffectiveTimeTxt.text = GetTimeDesc(tb.buffTime) + "有效";
                m_ADEffectiveTimeTxt.text = GetTimeDesc(tb.buffTimeAD) + "有效";

                if (BuffSystem.S.IsActive(ID))
                {
                    string dur = BuffSystem.S.GetCurrentCountdown(ID);
                    if (dur != null)
                    {
                        SetState(1);
                        m_DurationTxt.text = dur;
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
            m_MakeBtn.onClick.AddListener(() => 
            {
                //判断材料
                var list = TDFoodConfigTable.FoodItemMakeNeedResInfoDis[ID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                {
                    MainGameMgr.S.InventoryMgr.ReduceItems(list);
                    BuffSystem.S.StartBuff(ID);
                }
            });
            m_MakeADBtn.onClick.AddListener(() => 
            {
                //判断材料
                MainGameMgr.S.InventoryMgr.ReduceItems(TDFoodConfigTable.FoodItemMakeNeedResInfoDis[ID]);
                UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "这里应该显示广告");
                BuffSystem.S.StartBuff(ID, true);
            });
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
                    m_LockConditionTxt.text = string.Format("主城等级达到<color=#384B76>{0}</color>级", TDFoodConfigTable.GetData(ID).unlockLevel);
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
        void SetMakeNeedRes()
        {
            var infos = TDFoodConfigTable.FoodItemMakeNeedResInfoDis[ID];
            if (infos.Count == 2)
            {
                m_NeedItem2.gameObject.SetActive(true);
                m_NeedItemCount2Txt.gameObject.SetActive(true);

                m_NeedItem2.sprite = Resources.Load<Sprite>("Sprites/ItemIcon/" + TDItemConfigTable.GetData(infos[0].itemId).iconName);
                m_NeedItemCount2Txt.text = infos[0].value.ToString();
            }
            else
            {
                m_NeedItem1.gameObject.SetActive(false);
                m_NeedItemCount1Txt.gameObject.SetActive(false);
            }
            m_NeedItem1.sprite = Resources.Load<Sprite>("Sprites/ItemIcon/" + TDItemConfigTable.GetData(infos[1].itemId).iconName);
            m_NeedItemCount1Txt.text = infos[1].value.ToString();
        }
    }

}

