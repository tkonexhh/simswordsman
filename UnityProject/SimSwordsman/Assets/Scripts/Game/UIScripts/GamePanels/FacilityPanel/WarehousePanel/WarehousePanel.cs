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
        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;

        [Header("Middle")]
        [SerializeField]
        private Text m_BriefIntroduction;
        [SerializeField]
        private Image m_WarehouseFontBg;
        [SerializeField]
        private Image m_WarehouseImgae;
        [SerializeField]
        private Text m_WarehouseLevel;
        [SerializeField]
        private Text m_CurReservesValue; 
        [SerializeField]
        private Text m_NextReservesValue;
        [SerializeField]
        private Image m_NextReservesIcon;
        [SerializeField]
        private Text m_UpgradeCondition;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Text m_UpgradeText;
        [SerializeField]
        private GameObject m_RedPoint;
        [Header("Bottom")]
        [SerializeField]
        private Transform m_GoodsTrans;
        [SerializeField]
        private GameObject m_WarehouseItem;

        [Header("Res")]
        [SerializeField]
        private Transform m_UpgradeResItemTra;
        [SerializeField]
        private GameObject m_UpgradeResItem;

        private const int m_NotUnlockMaxCapacity = 10;

        private List<CostItem> m_CostItems;
        private WarehouseLevelInfo m_WarehouseNextLevelInfo = null;
        private WarehouseLevelInfo m_WarehouseCurLevelInfo = null;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
        private List<ItemBase> m_InventoryItems = null;
        private WarehouseController m_FacilityController = null;
        private int m_CurLevel = -1;

        private List<WarehouseItem> m_CurItemList = new List<WarehouseItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();

            EventSystem.S.Register(EventID.RefreshWarehouseRes,HandAddListenerEvent);

            GetInformationForNeed();
            BindAddListenerEvent();


            RefreshPanelInfo();

            RefreshCreateGoods();
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.RefreshWarehouseRes:
                    GetInformationForNeed();
                    RefreshCreateGoods();
                    break;
                default:
                    break;
            }
        }
        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Warehouse);
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(FacilityType.Warehouse);
            m_InventoryItems = MainGameMgr.S.InventoryMgr.GetAllInventoryItemList();
            m_FacilityConfigInfo =  MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(FacilityType.Warehouse);
            m_WarehouseCurLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Warehouse, m_CurLevel) as WarehouseLevelInfo;
            m_FacilityController = (WarehouseController)MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Warehouse);

            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_NextReservesIcon.gameObject.SetActive(false);
                m_NextReservesValue.text = Define.COMMON_DEFAULT_STR;
                m_UpgradeCondition.text = Define.COMMON_DEFAULT_STR; ;
            }
            else
            {
                m_WarehouseNextLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Warehouse, (m_CurLevel + 1)) as WarehouseLevelInfo;
                m_CostItems = m_WarehouseNextLevelInfo?.GetUpgradeResCosts();
            }
        }

        private void RefreshPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_WarehouseNextLevelInfo, m_CostItems, false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);

            m_BriefIntroduction.text = m_FacilityConfigInfo.desc;
            m_WarehouseLevel.text = CommonUIMethod.GetGrade(m_WarehouseCurLevelInfo.level);
            m_CurReservesValue.text = "当前储量:" + m_WarehouseCurLevelInfo.GetCurReserves() + "格";
            if (m_WarehouseNextLevelInfo!=null)
            {
                m_NextReservesValue.text = "下一级储量:" + m_WarehouseNextLevelInfo.GetCurReserves() + "格";
                m_UpgradeCondition.text = "升级需要讲武堂达到" + m_WarehouseNextLevelInfo.upgradeNeedLobbyLevel + "级";
            }
            m_WarehouseImgae.sprite = FindSprite("Warehouse" + m_CurLevel);

            RefreshResInfo();
        }
      
        private void RefreshResInfo()
        {
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_UpgradeResItemTra, m_UpgradeResItem, m_WarehouseNextLevelInfo);
        }
       
        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }
     

        private void RefreshCreateGoods()
        {
            if (m_FacilityController.GetState() != FacilityState.Unlocked)
            {
                m_WarehouseFontBg.gameObject.SetActive(false);
                m_WarehouseImgae.sprite = FindSprite("NotUnlockWarehouse");
                m_CurReservesValue.text = "当前储量:" + m_NotUnlockMaxCapacity + "格";
                m_NextReservesValue.text = "下一级储量:" + m_WarehouseCurLevelInfo.GetCurReserves() + "格";
                m_UpgradeText.text = "建造";
                m_UpgradeCondition.text = "建造需要讲武堂达到" + m_WarehouseNextLevelInfo.upgradeNeedLobbyLevel + "级";

                for (int i = 0; i < m_NotUnlockMaxCapacity; i++)
                {
                    m_CurItemList.Add(CreateWarehouseItem());
                }
            }
            else
            {
                int quantityDifference = m_WarehouseCurLevelInfo.GetCurReserves() - m_CurItemList.Count;
                for (int i = 0; i < quantityDifference; i++)
                {
                    m_CurItemList.Add(CreateWarehouseItem());
                }
            }

            if (m_InventoryItems != null)
            {
                foreach (var item in m_CurItemList)
                {
                    item.ItemReset();
                }

                for (int i = 0; i < m_InventoryItems.Count; i++)
                {
                    if (m_CurItemList.Count>i)
                        m_CurItemList[i].AddItemToWarehouse(m_InventoryItems[i], GetItemSprite(m_InventoryItems[i]));
                }
            }

            //m_CurItemList.Sort();
            RefeshSort(m_CurItemList);
        }
        private Sprite GetItemSprite(ItemBase itemBase)
        {
            if (itemBase == null)
                return null;
            switch (itemBase.PropType)
            {
                case PropType.None:
                    break;
                case PropType.Arms:
                    return FindSprite(TDEquipmentConfigTable.GetIconName(itemBase.GetSubName()));
                case PropType.Armor:
                    return FindSprite(TDEquipmentConfigTable.GetIconName(itemBase.GetSubName()));
                case PropType.RawMaterial:
                    return FindSprite(GetIconName(itemBase.GetSubName()));
                case PropType.Kungfu:
                    return FindSprite(TDKongfuConfigTable.GetIconName((KungfuType)itemBase.GetSubName()));
                case PropType.Herb:
                    return FindSprite(GetHerbIconName(itemBase.GetSubName()));
                default:
                    break;
            }
            return null;
        }
        private string GetHerbIconName(int herbType)
        {
            return TDHerbConfigTable.GetHerbIconNameById(herbType);
        }
        private void ReduceItemGameObject(ItemBase itemBase, int delta)
        {
            for (int i = 0; i < m_CurItemList.Count; i++)
            {
                if (m_CurItemList[i].IsHaveItem && m_CurItemList[i].IsSameItemBase(itemBase))
                {
                    m_CurItemList[i].RefreshNumber(delta);
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
                        m_CurItemList[j].AddItemToWarehouse(m_CurItemList[i].CurItemBase, GetItemSprite(m_CurItemList[i].CurItemBase));
                        m_CurItemList[i].AddItemToWarehouse(tempItem, GetItemSprite(tempItem));
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
            EventSystem.S.UnRegister(EventID.RefreshWarehouseRes, HandAddListenerEvent);
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
                    //ReduceItemGameObject((ItemBase)param[0], (int)param[1]);
                    break;
                default:
                    break;
            }
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                #region  新手引导没有时候暂用
                //if (m_FacilityController.GetState() != FacilityState.Unlocked)
                //{
                //    EventSystem.S.Send(EventID.OnStartUnlockFacility, FacilityType.Warehouse);
                //    HideSelfWithAnim();
                //    return;
                //}
                #endregion
                if (!CommonUIMethod.CheackIsBuild(m_WarehouseNextLevelInfo, m_CostItems))
                    return;
                bool isReducceSuccess = GameDataMgr.S.GetGameData().playerInfoData.ReduceCoinNum(m_WarehouseNextLevelInfo.upgradeCoinCost);
                if (isReducceSuccess)
                {
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, FacilityType.Warehouse, 1, 1);
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    GetInformationForNeed();
                    RefreshPanelInfo();
                    RefreshCreateGoods();
                    DataAnalysisMgr.S.CustomEvent(DotDefine.facility_upgrade, FacilityType.Warehouse.ToString() + ";" + m_CurLevel);
                    HideSelfWithAnim();
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
            WarehouseItem warehouse = Instantiate(m_WarehouseItem, m_GoodsTrans).GetComponent<WarehouseItem>();
            warehouse.OnInit(this);
            return warehouse;
        }
    }
}