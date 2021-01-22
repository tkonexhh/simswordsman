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
        private Button m_ClsoeBtn;

        [Header("Middle")]
        [SerializeField]
        private Text m_BriefIntroduction;
        [SerializeField]
        private Image m_WarehouseImgae;
        [SerializeField]
        private Text m_WarehouseLevel;
        [SerializeField]
        private Text m_CurReservesValue; 
        [SerializeField]
        private Text m_NextReservesValue;
        [SerializeField]
        private Text m_UpgradeCondition;
        [SerializeField]
        private Image m_Res1Img;
        [SerializeField]
        private Text m_Res1Value;
        [SerializeField]
        private Image m_Res2Img;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3Img;
        [SerializeField]
        private Text m_Res3Value;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Text m_UpgradeText;

        [Header("Bottom")]

        [SerializeField]
        private Transform m_GoodsTrans;
        [SerializeField]
        private GameObject m_WarehouseItem;


        private List<CostItem> m_CostItems;
        private WarehouseLevelInfo m_WarehouseNextLevelInfo = null;
        private WarehouseLevelInfo m_WarehouseCurLevelInfo = null;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
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

            m_FacilityConfigInfo =  MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(FacilityType.Warehouse);

            m_WarehouseNextLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Warehouse, (m_CurLevel + 1)) as WarehouseLevelInfo;

            m_WarehouseCurLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Warehouse, m_CurLevel) as WarehouseLevelInfo;
            m_CostItems = m_WarehouseNextLevelInfo.GetUpgradeResCosts();
        }

        private void RefreshPanelInfo()
        {
            m_BriefIntroduction.text = m_FacilityConfigInfo.desc;
            m_WarehouseLevel.text = CommonUIMethod.GetGrade(m_WarehouseCurLevelInfo.level);
            m_CurReservesValue.text = "当前储量:" + m_WarehouseCurLevelInfo.GetCurReserves() + "格";
            m_NextReservesValue.text = "下一级储量:" + m_WarehouseNextLevelInfo.GetCurReserves() + "格";
            m_UpgradeCondition.text = "升级需要讲武堂达到" + m_WarehouseNextLevelInfo.upgradeNeedLobbyLevel + "级";
            
            RefreshResInfo();
        }

        private bool CheackIsBuild()
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (m_WarehouseNextLevelInfo.GetUpgradeCondition() <= lobbyLevel && CheckPropIsEnough())
                return true;
            return false;
        }
        private bool CheckPropIsEnough()
        {
            for (int i = 0; i < m_CostItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)m_CostItems[i].itemId, m_CostItems[i].value);
                if (!isHave)
                    return false;
            }

            return GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_WarehouseNextLevelInfo.upgradeCoinCost);
        }

        private void RefreshResInfo()
        {
            if (m_CostItems.Count == 1)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_WarehouseNextLevelInfo.upgradeCoinCost.ToString();
                m_Res2Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {

                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_CostItems[1].value.ToString();
                m_Res2Img.sprite = FindSprite(GetIconName(m_CostItems[1].itemId));
                m_Res3Value.text = m_WarehouseNextLevelInfo.upgradeCoinCost.ToString();
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
        }
        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }

        private void RefreshCreateGoods()
        {
            int quantityDifference = m_WarehouseCurLevelInfo.GetCurReserves() - m_CurItemList.Count;
            for (int i = 0; i < quantityDifference; i++)
            {
                m_CurItemList.Add(CreateWarehouseItem());
            }

            if (m_InventoryItems != null)
            {
                for (int i = 0; i < m_InventoryItems.Count; i++)
                {
                    m_CurItemList[i].AddItemToWarehouse(m_InventoryItems[i], GetItemSprite(m_InventoryItems[i]));
                    // m_CurItemList.Add(CreateGoods(m_InventoryItems[i], m_CurItemBgList[i]));
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
                    break;
                case PropType.Armor:
                    break;
                case PropType.RawMaterial:
                    return FindSprite(GetIconName(itemBase.GetSubName()));
                case PropType.Kungfu:
                    break;
                default:
                    break;
            }
            return null;
        }

        private void ReduceItemGameObject(ItemBase itemBase, int delta)
        {
            for (int i = 0; i < m_CurItemList.Count; i++)
            {
                if (m_CurItemList[i].IsHaveItem && m_CurItemList[i].IsSameItemBase(itemBase))
                {
                    m_CurItemList[i].RefreshNumber();
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
                if (!CheackIsBuild())
                {
                    FloatMessage.S.ShowMsg("未达到升级条件");
                    return;
                }
                if (m_WarehouseNextLevelInfo == null)
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