using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using Qarth;
using System.Linq;


namespace GameWish.Game
{
    public class InventoryMgr : MonoBehaviour, IMgr
    {
        private InventoryDataWrapper m_InventoryDataWrapper;

        #region IMgr
        public void OnInit()
        {
            m_InventoryDataWrapper = new InventoryDataWrapper();
            m_InventoryDataWrapper.Init();
        }

        public void OnUpdate()
        {
        }
        public void OnDestroyed()
        {
        }
        #endregion

        #region Public Methods      

        /// <summary>
        /// 根据类型返回所以装备
        /// </summary>
        /// <param name="equipType"></param>
        /// <returns></returns>
        //public List<EquipmentItem> GetAllEquipmentForType(PropType equipType)
        //{
        //    return m_InventoryDataWrapper.GetAllEquipmentForType(equipType);
        //}
        /// <summary>
        /// 获取所有仓库中物品 
        /// </summary>
        /// <returns></returns>
        public List<ItemBase> GetAllInventoryItemList()
        {
            return m_InventoryDataWrapper.WarehouseItems;
        }

        /// <summary>
        /// 得到某个item的当前持有数量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetCurrentCountByItemType(RawMaterial type)
        {
            ItemBase item = GetItemBaseByItemType(type);
            if (item != null)
            {
                return item.Number;
            }
            else
            {
                return 0;
            }
        }

        public List<ItemBase> GetAllEquipmentForType(PropType equipType)
        {
            return m_InventoryDataWrapper.GetAllEquipmentForType(equipType);
        }

        public bool CheckItemInInventory(RawMaterial rawMaterial, int number)
        {
            return m_InventoryDataWrapper.CheckItemInInventory(rawMaterial, number);
        }
        public int GetRawMaterialNumberForID(int rawMaterial)
        {
            return m_InventoryDataWrapper.GetRawMaterialNumberForID((RawMaterial)rawMaterial);
        }

        /// <summary>
        /// 通过item类型得到itembase
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ItemBase GetItemBaseByItemType(RawMaterial type)
        {
            foreach (var item in m_InventoryDataWrapper.WarehouseItems)
            {
                if (item.PropType == PropType.RawMaterial)
                {
                    if ((item as PropItem).PropSubType == type)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取物品的图标名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public string GetIconName(int id)
        {
            return TDItemConfigTable.GetIconName(id);
        }

        public List<HerbItem> GetAllHerbs()
        {
            List<ItemBase> herbList = GetAllEquipmentForType(PropType.Herb);
            List<HerbItem> list = new List<HerbItem>();
            if (herbList != null && herbList.Count > 0)
            {
                herbList.ForEach(i =>
                {
                    list.Add((HerbItem)i);
                });
            }

            return list;
        }
        /// <summary>
        /// 根据ID获取Herb
        /// </summary>
        /// <param name="herbID"></param>
        /// <returns></returns>
        public ItemBase GetHerbForID(int herbID)
        {
            return m_InventoryDataWrapper.GetHerbForID(herbID);
        }

        /// <summary>
        /// 获取所有仓库中装备信息
        /// </summary>
        /// <returns></returns>
        public List<ItemBase> GetAllEquipmentItemList()
        {
            return m_InventoryDataWrapper.WarehouseItems;
        }
        /// <summary>
        /// 获取装备的出售价格
        /// </summary>
        /// <param name="equipmentItem"></param>
        /// <returns></returns>
        //public int GetEquipSellingPrice(EquipmentItem equipmentItem)
        //{
        //    return m_InventoryDataWrapper.GetEquipSellingPrice(equipmentItem);
        //}

        /// <summary>
        /// 从仓库中减少装备
        /// </summary>
        /// <param name="equipmentItem"></param>
        /// <param name="delta"></param>
        public void RemoveItem(ItemBase item, int delta = 1)
        {
            switch (item.PropType)
            {
                case PropType.None:
                    break;
                case PropType.RawMaterial:
                    m_InventoryDataWrapper.RemovePropItem((PropItem)item, delta);
                    break;
                case PropType.Armor:
                    m_InventoryDataWrapper.RemoveArmor((ArmorItem)item, delta);
                    break;
                case PropType.Arms:
                    m_InventoryDataWrapper.RemoveArms((ArmsItem)item, delta);
                    break;
                case PropType.Kungfu:
                    m_InventoryDataWrapper.RemoveKungfu((KungfuItem)item, delta);
                    break;
                case PropType.Herb:
                    m_InventoryDataWrapper.RemoveHerb((HerbItem)item, delta);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 往仓库中添加装备
        /// </summary>
        /// <param name="itemBase"></param>
        /// <param name="delta"></param>
        public void AddItem(ItemBase itemBase, int delta = 1)
        {
            if (itemBase == null || delta == 0)
                return;
            switch (itemBase.PropType)
            {
                case PropType.None:
                    break;
                case PropType.RawMaterial:
                    m_InventoryDataWrapper.AddPropItem((PropItem)itemBase, delta);
                    break;
                case PropType.Armor:
                    m_InventoryDataWrapper.AddArmor((ArmorItem)itemBase, delta);
                    break;
                case PropType.Arms:
                    m_InventoryDataWrapper.AddArms((ArmsItem)itemBase, delta);
                    break;
                case PropType.Kungfu:
                    m_InventoryDataWrapper.AddKungfu((KungfuItem)itemBase, delta);
                    break;
                case PropType.Herb:
                    m_InventoryDataWrapper.AddHerb((HerbItem)itemBase, delta);
                    break;
                default:
                    break;
            }
            EventSystem.S.Send(EventID.OnMainMenuOrDiscipleRedPoint);
        }

        /// <summary>
        /// 获取招募令数量
        /// </summary>
        /// <param name="recruitType"></param>
        public int GetRecruitmentOrderCount(RecruitType recruitType)
        {
            return m_InventoryDataWrapper.GetRecruitmentOrderCount(recruitType);
        }

        /// <summary>
        /// 获取所有招募令的数量
        /// </summary>
        /// <returns></returns>
        public int GetAllRecruitmentOrderCount()
        {
            return GetRecruitmentOrderCount(RecruitType.GoldMedal)+ GetRecruitmentOrderCount(RecruitType.SilverMedal);
        }

        /// <summary>
        /// 判断材料是否足够
        /// </summary>
        /// <returns></returns>
        public bool HaveEnoughItem(List<CostItem> items,bool isShow = true)
        {
            foreach (var item in items)
            {
                if (MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)item.itemId) < item.value)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 减少仓库材料
        /// </summary>
        /// <param name="id"></param>
        public void ReduceItems(List<CostItem> items)
        {
            foreach (var item in items)
            {
                MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)item.itemId), item.value);
            }
        }

        #endregion

        #region Private Methods
        private void RegisterEvents()
        {
            //EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
        }

        private void UnregisterEvents()
        {
            //EventSystem.S.UnRegister(EventID.OnStartUnlockFacility, HandleEvent);
        }



        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {

            }
        }


        #endregion
    }

}