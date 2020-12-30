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
        [SerializeField]
        private Text m_ItemDetailsTitle;
        [SerializeField]
        private Text m_NumberValue;
        [SerializeField]
        private Text m_ItemDescribe;
        [SerializeField]
        private Text m_UnitPriceValue;
        [SerializeField]
        private Text m_CountCont;

        [SerializeField]
        private Button m_SaleBtn;
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_IncreaseBtn;
        [SerializeField]
        private Button m_ReduceBtn;
        [SerializeField]
        private Button m_MostBtn;
        [SerializeField]
        private Button m_LeastBtn;
        [SerializeField]
        private EventTrigger m_IncreaseEventTri;
        [SerializeField]
        private EventTrigger m_ReduceEventTri;

        private ItemBase m_CurInventoryItem = null;

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
            m_SaleBtn.onClick.AddListener(() =>
            {
                if (m_CurInventoryItem != null)
                {
                    MainGameMgr.S.InventoryMgr.RemoveItem(m_CurInventoryItem, m_CurInventoryItem.Number);

                    //TODO m_CurInventoryItem.Number会变为负数
                    GameDataMgr.S.GetPlayerData().AddCoinNum(m_CurInventoryItem.Price * int.Parse(m_NumberValue.text));

                    CloseSelfPanel();
                }
            });
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_IncreaseBtn.onClick.AddListener(() => {

            });
            m_ReduceBtn.onClick.AddListener(() => { });
            m_MostBtn.onClick.AddListener(() => { });
            m_LeastBtn.onClick.AddListener(() => { });
            m_IncreaseEventTri.triggers = new List<EventTrigger.Entry>();
            m_ReduceEventTri.triggers = new List<EventTrigger.Entry>();

            EventTrigger.Entry pointerDownEvent = new EventTrigger.Entry();
            pointerDownEvent.eventID = EventTriggerType.PointerDown;
            pointerDownEvent.callback.AddListener(OnPointerDown);

            EventTrigger.Entry pointerUpEvent = new EventTrigger.Entry();
            pointerUpEvent.eventID = EventTriggerType.PointerUp;
            pointerUpEvent.callback.AddListener(OnPointerUp);


            m_ReduceEventTri.triggers.Add(pointerDownEvent);
            m_IncreaseEventTri.triggers.Add(pointerDownEvent);
            m_ReduceEventTri.triggers.Add(pointerUpEvent);
            m_IncreaseEventTri.triggers.Add(pointerUpEvent);

        }

        private Coroutine m_StartCount;
        private Coroutine m_IncreaseCount;
        private Coroutine m_ReduceCount;

        private void OnPointerDown(BaseEventData arg0)
        {
            Debug.LogError("按下");
            if (arg0.selectedObject!=null)
            {
                if (arg0.selectedObject.name.Equals("IncreaseBtn"))
                    m_BtnState = BtnState.Increase;
                else if(arg0.selectedObject.name.Equals("ReduceBtn"))
                    m_BtnState = BtnState.Reduce;

                m_StartCount = StartCoroutine(TimingBtn(0.5f, m_BtnState, OnCountBtnEvent));
            }
        
        }
        private IEnumerator TimingBtn(float time, BtnState btnState = BtnState.None, Action<BtnState> action = null)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke(btnState);
        }

        private void OnPointerUp(BaseEventData arg0)
        {
            Debug.LogError("起来");
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
        
        }
        private void OnCountBtnEvent(BtnState btnState)
        {
            switch (btnState)
            {
                case BtnState.Increase:
                    m_IncreaseCount = StartCoroutine(TimingBtn(0.2f, BtnState.None, ContinuousCounting));
                    break;
                case BtnState.Reduce:
                    m_ReduceCount = StartCoroutine(TimingBtn(0.2f, BtnState.None, (e) => {
                        m_CountCont.text = (int.Parse(m_CountCont.text) - 1).ToString();
                    }));
                    break;  
                default:
                    break;
            }
        }


        private void ContinuousCounting(BtnState btnState)
        {
            string[] strs = m_CountCont.text.Split('/');
            m_CountCont.text = (int.Parse(strs[0]) + 1).ToString() + Define.SLASH + m_CurInventoryItem.Number.ToString();
            OnCountBtnEvent(m_BtnState);
        }




        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurInventoryItem = args[0] as ItemBase;
            InitPanelInfo();
        }

        private void InitPanelInfo()
        {
            if (m_CurInventoryItem != null)
            {
                m_ItemDetailsTitle.text = m_CurInventoryItem.Name.ToString();
                m_NumberValue.text = m_CurInventoryItem.Number.ToString();
                m_CountCont.text = m_SelectedNumber.ToString()+ Define.SLASH + m_CurInventoryItem.Number.ToString();
                m_ItemDescribe.text = m_CurInventoryItem.Desc.ToString();
                m_UnitPriceValue.text = m_CurInventoryItem.Price.ToString();
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