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

        [HideInInspector]
        public int ID;
        [HideInInspector]
        public int UnlockLevel;//解锁等级 

        AbstractAnimPanel m_panel;

        string m_StringID = "BaicaohuPanel";

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
            if (!IsUnlock((MedicinalPowderType)ID))
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
        bool IsUnlock(MedicinalPowderType id)
        {
            List<MedicinalPowderType> list = null;
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Baicaohu);
            for (int i = 1; i <= level; i++)
            {
                list = TDFacilityBaicaohuTable.GetLevelInfo(i).GetCurMedicinalPowderType();
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
                //判断材料
                var list = TDHerbConfigTable.MakeNeedItemIDsDic[ID];
                if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                {
                    MainGameMgr.S.InventoryMgr.ReduceItems(list);
                    CountdownSystem.S.StartCountdownerWithMin(m_StringID, ID, TDHerbConfigTable.GetData(ID).makeTime);
                }
                else
                    UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "材料不足！");
            });
            m_CompleteADBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "这里应该显示广告");

                CountdownSystem.S.Cancel(m_StringID, ID);
                //MainGameMgr.S.MedicinalPowderMgr.AddHerb(ID, 1);
                MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)ID, 1));
                SetState(2);
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