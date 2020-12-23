using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class WarehousePanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_WarehouseName;
        [SerializeField]
        private Text m_BriefIntroduction;
        [SerializeField]
        private Text m_UpgradeCostCoinValueText;
        [SerializeField]
        private Text m_CurLevelValue;
        [SerializeField]
        private Text m_CurReservesValue;
        [SerializeField]
        private Text m_NextReservesValue;
        [SerializeField]
        private Text m_UpgradeConditionsValue;

        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Button m_ClsoeBtn;

        [SerializeField]
        private Transform m_GoodsTrans;

        [SerializeField]
        private GameObject m_WarehouseItem;

        private WarehouseLevelInfo m_WarehouseNextLevelInfo = null;
        private WarehouseLevelInfo m_WarehouseCurLevelInfo = null;
        private List<ItemBase> m_InventoryItems = null;
        private int m_CurLevel = -1;

        private List<WarehouseItem> m_CurItemList = new List<WarehouseItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            GetInformationForNeed();
            BindAddListenerEvent();


            RefreshPanelInfo();

            //仓库测试代码
            //MainGameMgr.S.InventoryMgr.AddPropItem(new PropItem (), 5);
            //MainGameMgr.S.InventoryMgr.RemovePropItem(PropType.Wood, 10);

            RefreshCreateGoods();
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Warehouse);

            m_InventoryItems = MainGameMgr.S.InventoryMgr.GetAllInventoryItemList();

            m_WarehouseNextLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Warehouse, (m_CurLevel + 1)) as WarehouseLevelInfo;

            m_WarehouseCurLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Warehouse, m_CurLevel) as WarehouseLevelInfo;

        }

        private void RefreshPanelInfo()
        {
            m_WarehouseName.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_WAREHOUSE_NAME);
            m_BriefIntroduction.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_WAREHOUSE_DESCRIBE);
            //m_UpgradeCostCoinValueText.text = m_WarehouseCurLevelInfo.upgradeCost.ToString();

            m_CurLevelValue.text = m_CurLevel.ToString();
            //m_UpgradeCostCoinValueText.text = m_WarehouseLevelInfo.rewards.facilityRewards
            m_CurReservesValue.text = m_WarehouseCurLevelInfo.reserves.ToString();

            m_NextReservesValue.text = m_WarehouseNextLevelInfo.reserves.ToString();

            foreach (var item in m_WarehouseNextLevelInfo.preconditions.facilityConditions)
            {
                m_UpgradeConditionsValue.text += item.facilityType.ToString();
                m_UpgradeConditionsValue.text += "_";
                m_UpgradeConditionsValue.text += item.preditionType.ToString();
                m_UpgradeConditionsValue.text += "_";
                m_UpgradeConditionsValue.text += item.value.ToString();
                m_UpgradeConditionsValue.text += ";";
            }
        }

        private void RefreshCreateGoods()
        {
            int quantityDifference = m_WarehouseCurLevelInfo.reserves - m_CurItemList.Count;
            for (int i = 0; i < quantityDifference; i++)
            {
                m_CurItemList.Add(CreateWarehouseItem());
            }

            if (m_InventoryItems != null)
            {
                for (int i = 0; i < m_InventoryItems.Count; i++)
                {
                    m_CurItemList[i].AddItemToWarehouse(m_InventoryItems[i]);
                    // m_CurItemList.Add(CreateGoods(m_InventoryItems[i], m_CurItemBgList[i]));
                }
            }

            //m_CurItemList.Sort();
            RefeshSort(m_CurItemList);
        }

        private void ReduceItemGameObject(ItemBase itemBase, int delta)
        {
            for (int i = 0; i < m_CurItemList.Count; i++)
            {
                if (m_CurItemList[i].IsHaveItem && m_CurItemList[i].IsSameItemBase(itemBase))
                {
                    m_CurItemList[i].RemoveItemCount(delta);
                    //m_CurItemList.Sort();

                    RefeshSort(m_CurItemList);
                }
            }
        }
        //TODO  先装备 后 物品排序 插入排序
        public void RefeshSort(List<WarehouseItem> warehouseItems)
        {



            for (int i = 0; i < m_CurItemList.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (CompareSize(m_CurItemList[j].CurItemBase, m_CurItemList[i].CurItemBase))
                    {
                        ItemBase tempItem = m_CurItemList[j].CurItemBase;
                        m_CurItemList[j].AddItemToWarehouse(m_CurItemList[i].CurItemBase);
                        m_CurItemList[i].AddItemToWarehouse(tempItem);
                    }
                }
            }
        }

        /// <summary>
        /// 比较大小  
        /// </summary>
        /// <param name="_last"></param>
        /// <param name="_next"></param>
        /// <returns></returns>
        private bool CompareSize(ItemBase _last, ItemBase _next)
        {
            if (_next == null)
                return false;

            if (_last == null)
                return true;

            int targetItem = _next.GetSortId();
            int curItem = _last.GetSortId();

            if (targetItem> curItem)
                return false;
            else
                return true;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            RegisterEvevnts();
        }

        protected override void OnClose()
        {
            base.OnClose();
            UnRegisterEvevnts();
        }

        private void RegisterEvevnts()
        {
            EventSystem.S.Register(EventID.OnReduceItems, HandleEvevnt);
        }

        private void UnRegisterEvevnts()
        {
            EventSystem.S.UnRegister(EventID.OnReduceItems, HandleEvevnt);
        }

        private void HandleEvevnt(int key, object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnReduceItems:
                    ReduceItemGameObject((ItemBase)param[0], (int)param[1]);
                    break;
                default:
                    break;
            }
        }

        private void BindAddListenerEvent()
        {
            m_ClsoeBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                bool isReducceSuccess = GameDataMgr.S.GetGameData().playerInfoData.ReduceCoinNum(double.Parse(m_UpgradeCostCoinValueText.text));
                if (isReducceSuccess)
                {
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, FacilityType.Warehouse, 1, 1);
                    GetInformationForNeed();
                    RefreshPanelInfo();
                    RefreshCreateGoods();
                }
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        private WarehouseItem CreateWarehouseItem()
        {
            return Instantiate(m_WarehouseItem, m_GoodsTrans).GetComponent<WarehouseItem>();
        }
    }
}