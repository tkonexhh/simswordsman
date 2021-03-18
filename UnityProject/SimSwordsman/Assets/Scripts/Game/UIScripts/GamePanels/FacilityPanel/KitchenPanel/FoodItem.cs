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
        public int m_FoodBufferID;

        AbstractAnimPanel m_panel;

        private int m_CountDownTimerID = -1;
        private FoodBuffData m_FoodBufferData = null;

        private float m_FoodBufferProgres = 0;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_panel = t as AbstractAnimPanel;

            BindAddListenerEvent();

            m_FoodBufferID = (int)obj[0];

            Init(m_FoodBufferID);
        }
        /// <summary>
        /// 检测食物buffer数据并开始倒计时
        /// </summary>
        private void CheckFoodBuffDataAndCountDown() 
        {
            m_FoodBufferData = GameDataMgr.S.GetClanData().GetFoodBuffData(m_FoodBufferID);

            Timer.S.Cancel(m_CountDownTimerID);

            if (m_FoodBufferData != null)
            {
                SetState(1);

                m_CountDownTimerID = Timer.S.Post2Really((x) =>
                {
                    m_FoodBufferProgres = m_FoodBufferData.GetRemainProgress();

                    UpdateProgress();

                    if (m_FoodBufferProgres <= 0)
                    {
                        StopEffect();

                        GameDataMgr.S.GetClanData().RemoveFoodBuffData(m_FoodBufferID);

                        Timer.S.Cancel(m_CountDownTimerID);
                    }
                }, 1, -1);

                m_FoodBufferProgres = m_FoodBufferData.GetRemainProgress();

                UpdateProgress();
            }
            else {
                SetState(2);
            }
        }
        public void OnClose()
        {
            Timer.S.Cancel(m_CountDownTimerID);
        }
        private void StopEffect()
        {
            SetState(2);
        }
        private void UpdateProgress()
        {
            if (m_FoodBufferData != null) 
            {
                m_DurationTxt.text = m_FoodBufferData.GetRemainTime();
                m_Progress.fillAmount = m_FoodBufferProgres;
            }
        }
        void Init(int id)
        {
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
                m_FoodEffecTxt.text = GetEffectDesc(tb);
                m_EffectiveTimeTxt.text = GetTimeDesc(tb.buffTime) + "有效";
                m_ADEffectiveTimeTxt.text = GetTimeDesc(tb.buffTimeAD) + "有效";

                CheckFoodBuffDataAndCountDown();
            }
        }
        private string GetEffectDesc(TDFoodConfig tb)
        {
            FoodBuffType type;
            if (Enum.TryParse(tb.buffType, out type))
            {
                switch (type)
                {
                    case FoodBuffType.Food_AddExp:
                        return string.Format("弟子获得经验+<color=#8C343C>{0}%</color>", tb.buffRate);
                    case FoodBuffType.Food_AddRoleExp:
                        return string.Format("弟子获得功夫经验+<color=#8C343C>{0}%</color>", tb.buffRate);
                    case FoodBuffType.Food_AddCoin:
                        return string.Format("获得铜钱+<color=#8C343C>{0}%</color>", tb.buffRate);
                    default:
                        break;
                }
            }
            return null;
        }
        private string GetTimeDesc(int minute)
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
                var list = TDFoodConfigTable.MakeNeedItemIDsDic[m_FoodBufferID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                {
                    MainGameMgr.S.InventoryMgr.ReduceItems(list);

                    AddFoodBufferData(m_FoodBufferID);

                    CheckFoodBuffDataAndCountDown();

                    DataAnalysisMgr.S.CustomEvent(DotDefine.f_cook, m_FoodBufferID.ToString()+ ";KuaiChao");
                }
                else
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            });
            m_MakeADBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                //判断材料
                var list = TDFoodConfigTable.MakeNeedItemIDsDic[m_FoodBufferID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                {
                    DataAnalysisMgr.S.CustomEvent(DotDefine.f_cook, ID.ToString()+ ";JingChao");
                    AdsManager.S.PlayRewardAD("AddFood", LookADSuccessCallBack);
                }
                else
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
            });
        }
        private void AddFoodBufferData(int foodBufferID,bool isLookAD = false) 
        {
            var table = TDFoodConfigTable.GetData(foodBufferID);

            int bufferTime = isLookAD ? table.buffTimeAD : table.buffTime;

            DateTime endTime = DateTime.Now.AddMinutes(bufferTime);

            GameDataMgr.S.GetClanData().AddFoodBuffData(table.id, DateTime.Now, endTime);
        }
        private void LookADSuccessCallBack(bool obj)
        {
            var list = TDFoodConfigTable.MakeNeedItemIDsDic[m_FoodBufferID];

            MainGameMgr.S.InventoryMgr.ReduceItems(list);

            AddFoodBufferData(m_FoodBufferID, true);

            CheckFoodBuffDataAndCountDown();
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
                    m_LockConditionTxt.text = string.Format("伙房等级达到<color=#384B76>{0}</color>级", GetUnlockLevel(m_FoodBufferID));
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
            var infos = TDFoodConfigTable.MakeNeedItemIDsDic[m_FoodBufferID];
            if (infos.Count == 2)
            {
                m_NeedItem2.gameObject.SetActive(true);
                m_NeedItemCount2Txt.gameObject.SetActive(true);

                m_NeedItem2.sprite = m_panel.FindSprite(TDItemConfigTable.GetData(infos[1].itemId).iconName);
                int Cost2 = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)infos[1].itemId);
                if (Cost2 > infos[1].value)
                    m_NeedItemCount2Txt.text = infos[1].value.ToString() + Define.SLASH + infos[1].value.ToString();
                else
                    m_NeedItemCount2Txt.text = CommonUIMethod.GetStrForColor("#A35953", Cost2.ToString()) + Define.SLASH + infos[1].value.ToString();
            }
            else
            {
                m_NeedItem1.gameObject.SetActive(false);
                m_NeedItemCount1Txt.gameObject.SetActive(false);
            }
            m_NeedItem1.sprite = m_panel.FindSprite(TDItemConfigTable.GetData(infos[0].itemId).iconName);
            int Cost1 = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)infos[0].itemId);
            if (Cost1 > infos[0].value)
                m_NeedItemCount1Txt.text = infos[0].value.ToString() + Define.SLASH + infos[0].value.ToString();
            else
                m_NeedItemCount1Txt.text = CommonUIMethod.GetStrForColor("#A35953", Cost1.ToString()) + Define.SLASH + infos[0].value.ToString();
        }
    }
}

