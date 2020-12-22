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
                default:
                    break;
            }
        }

        public void AddItem(ItemBase itemBase, int delta = 1)
        {
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
                default:
                    break;
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