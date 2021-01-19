using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EventTrigger = UnityEngine.EventSystems.EventTrigger;

namespace GameWish.Game
{
    public enum BtnState
    {
        None,
        Increase,
        Reduce,
    }
    public class ItemDetailsPanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Image[] m_NameList;
        [SerializeField]
        private Button m_CloseBtn;

        [Header("Middle")]
        [SerializeField]
        private Text m_BriefIntroduction;
        [SerializeField]
        private Image m_ItemIcon;
        [SerializeField]
        private Text m_UnitPrice;
        [SerializeField]
        private Text m_Have;
        [SerializeField]
        private Text m_SellNumber;
        [SerializeField]
        private Text m_AllPrice;
        [SerializeField]
        private Button m_IncreaseBtn;
        [SerializeField]
        private Button m_ReduceBtn;

        [Header("Bottom")]
        [SerializeField]
        private Button m_SellBtn;

        [SerializeField]
        private EventTrigger m_IncreaseEventTri;
        [SerializeField]
        private EventTrigger m_ReduceEventTri;

        private ItemBase m_CurInventoryItem = null;
        private bool m_IsStart = false;
        private int m_SelectedNumber = 0;

        private BtnState m_BtnState;
        protected override void OnUIInit()
        {
            base.OnUIInit();

            GetInformationForNeed();

            BindAddListenerEvevnt();
        }

        private void GetInformationForNeed()
        {
        }

        private void BindAddListenerEvevnt()
        {
            m_SellBtn.onClick.AddListener(() =>
            {
                if (m_CurInventoryItem != null)
                {
                    MainGameMgr.S.InventoryMgr.RemoveItem(m_CurInventoryItem, m_SelectedNumber);

                    GameDataMgr.S.GetPlayerData().AddCoinNum(m_CurInventoryItem.Price * m_SelectedNumber);

                    CloseSelfPanel();
                }
            });
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_IncreaseBtn.onClick.AddListener(() => {
                Debug.LogError("点击");
                RefreshIncreaseSellNumber();
            });
            m_ReduceBtn.onClick.AddListener(() => {
                RefresReduceSellNumber();
            });

            m_IncreaseEventTri.triggers = new List<EventTrigger.Entry>();
            m_ReduceEventTri.triggers = new List<EventTrigger.Entry>();

            EventTrigger.Entry pointerDownEvent = new EventTrigger.Entry();
            pointerDownEvent.eventID = EventTriggerType.PointerDown;
            pointerDownEvent.callback.AddListener(OnPointerDown);

            EventTrigger.Entry pointerUpEvent = new EventTrigger.Entry();
            pointerUpEvent.eventID = EventTriggerType.PointerUp;
            pointerUpEvent.callback.AddListener(OnPointerUp);

            m_ReduceEventTri.triggers.Add(pointerDownEvent);
            m_ReduceEventTri.triggers.Add(pointerUpEvent);

            m_IncreaseEventTri.triggers.Add(pointerDownEvent);
            m_IncreaseEventTri.triggers.Add(pointerUpEvent);
        }

        private Coroutine m_StartCount;
        private Coroutine m_IncreaseCount;
        private Coroutine m_ReduceCount;

        private void OnPointerDown(BaseEventData arg0)
        {
            Debug.LogError("按下");
            if (arg0.selectedObject!=null)
                m_StartCount = StartCoroutine(TimingBtn(0.5f, m_BtnState, OnCountBtnEvent, arg0.selectedObject));
        }
        private IEnumerator TimingBtn(float time, BtnState btnState = BtnState.None, Action<BtnState> action = null,GameObject obj = null)
        {
            yield return new WaitForSeconds(time);
            if (obj!=null)
            {
                if (obj.name.Equals("IncreaseBtn"))
                    m_BtnState = BtnState.Increase;
                else if (obj.name.Equals("ReduceBtn"))
                    m_BtnState = BtnState.Reduce;
            }
            m_IsStart = true;
            action?.Invoke(m_BtnState);
        }

        private void OnPointerUp(BaseEventData arg0)
        {
            Debug.LogError("起来");
            if (!m_IsStart)
            {
                StopCoroutine(m_StartCount);
                return;
            }
            switch (m_BtnState)
            {
                case BtnState.None:
                    break;
                case BtnState.Increase:
                    StopCoroutine(m_IncreaseCount);
                    break;
                case BtnState.Reduce:
                    StopCoroutine(m_ReduceCount);
                    break;
                default:
                    break;
            }
            m_IsStart = false;
        }
        private void OnCountBtnEvent(BtnState btnState)
        {
            switch (btnState)
            {
                case BtnState.Increase:
                    m_IncreaseCount = StartCoroutine(TimingBtn(0.2f, BtnState.None, (e)=> {
                        RefreshIncreaseSellNumber();
                        OnCountBtnEvent(m_BtnState);
                    }));
                    break;
                case BtnState.Reduce:
                    m_ReduceCount = StartCoroutine(TimingBtn(0.2f, BtnState.None, (e) => {
                        RefresReduceSellNumber();
                        OnCountBtnEvent(m_BtnState);
                    }));
                    break;  
                default:
                    break;
            }
        }

        private void RefresReduceSellNumber()
        {
            m_SelectedNumber--;
            if (m_SelectedNumber <= 0)
            {
                m_SelectedNumber = 0;
                m_SellNumber.text = m_SelectedNumber + Define.SLASH + m_CurInventoryItem.Number.ToString();
                return;
            }
            m_SellNumber.text = m_SelectedNumber.ToString() + Define.SLASH + m_CurInventoryItem.Number.ToString();
            m_AllPrice.text = (m_CurInventoryItem.Price * m_SelectedNumber).ToString();
        }

        private void RefreshIncreaseSellNumber()
        {
            m_SelectedNumber++;
            int hava = int.Parse(m_Have.text);
            if (m_SelectedNumber >= hava)
            {
                m_SelectedNumber = hava;
                m_SellNumber.text = m_SelectedNumber.ToString() + Define.SLASH + m_CurInventoryItem.Number.ToString();
                return;
            }
            m_SellNumber.text = m_SelectedNumber.ToString() + Define.SLASH + m_CurInventoryItem.Number.ToString();
            m_AllPrice.text = (m_CurInventoryItem.Price * m_SelectedNumber).ToString();
        }


        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurInventoryItem = args[0] as ItemBase;
            RefreshPanelInfo();
        }

        private void RefreshPanelInfo()
        {
            if (m_CurInventoryItem != null)
            {
                GenerateItemName();
                m_Have.text = m_CurInventoryItem.Number.ToString();
                m_SellNumber.text = m_SelectedNumber.ToString() + Define.SLASH + m_CurInventoryItem.Number.ToString();
                m_BriefIntroduction.text = m_CurInventoryItem.Desc.ToString();
                m_UnitPrice.text = m_CurInventoryItem.Price.ToString();
            }
        }
        /// <summary>
        /// 生成物品名称
        /// </summary>
        private void GenerateItemName()
        {
            foreach (var item in m_NameList)
            {
                item.GetComponentInChildren<Text>().text = Define.COMMON_DEFAULT_STR;
                item.gameObject.SetActive(false);
            }
            string strName = m_CurInventoryItem.Name.ToString();
            int maxLength = Mathf.Min(strName.Length,8);
            for (int i = 0; i < maxLength; i++)
            {
                m_NameList[i].GetComponentInChildren<Text>().text = strName[i].ToString();
                m_NameList[i].gameObject.SetActive(true);
            }
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}