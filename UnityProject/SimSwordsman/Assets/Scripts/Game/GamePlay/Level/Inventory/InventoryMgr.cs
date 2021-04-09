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
        /// �������ͷ�������װ��
        /// </summary>
        /// <param name="equipType"></param>
        /// <returns></returns>
        //public List<EquipmentItem> GetAllEquipmentForType(PropType equipType)
        //{
        //    return m_InventoryDataWrapper.GetAllEquipmentForType(equipType);
        //}
        /// <summary>
        /// ��ȡ���вֿ�����Ʒ 
        /// </summary>
        /// <returns></returns>
        public List<ItemBase> GetAllInventoryItemList()
        {
            return m_InventoryDataWrapper.WarehouseItems;
        }

        /// <summary>
        /// �õ�ĳ��item�ĵ�ǰ��������
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
        /// ͨ��item���͵õ�itembase
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
        /// ��ȡ��Ʒ��ͼ������
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
        /// ����ID��ȡHerb
        /// </summary>
        /// <param name="herbID"></param>
        /// <returns></returns>
        public ItemBase GetHerbForID(int herbID)
        {
            return m_InventoryDataWrapper.GetHerbForID(herbID);
        }

        /// <summary>
        /// ��ȡ���вֿ���װ����Ϣ
        /// </summary>
        /// <returns></returns>
        public List<ItemBase> GetAllEquipmentItemList()
        {
            return m_InventoryDataWrapper.WarehouseItems;
        }
        /// <summary>
        /// ��ȡװ���ĳ��ۼ۸�
        /// </summary>
        /// <param name="equipmentItem"></param>
        /// <returns></returns>
        //public int GetEquipSellingPrice(EquipmentItem equipmentItem)
        //{
        //    return m_InventoryDataWrapper.GetEquipSellingPrice(equipmentItem);
        //}

        /// <summary>
        /// �Ӳֿ��м���װ��
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
        /// ���ֿ�������װ��
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

        public int GetItemCount(ItemBase item)
        {
            if (item == null)
                return 0;
            switch (item.PropType)
            {
                case PropType.None:
                    break;
                case PropType.Armor:
                    {
                        var data = GameDataMgr.S.GetClanData().inventoryData.armorDBDataList.Find(x => x.ArmorID == (item as ArmorItem).ArmorID);
                        if (data == null)
                            return 0;

                        return data.Number;
                    }
                case PropType.Arms:
                    {
                        var data = GameDataMgr.S.GetClanData().inventoryData.armsDBDataList.Find(x => x.ArmsID == (item as ArmsItem).ArmsID);
                        if (data == null)
                            return 0;

                        return data.Number;
                    }
                case PropType.RawMaterial:
                    {
                        return m_InventoryDataWrapper.GetRawMaterialNumberForID((item as PropItem).PropSubType);
                    }

                case PropType.Kungfu:
                    {
                        var data = GameDataMgr.S.GetClanData().inventoryData.kungfuList.Find(x => x.KungfuType == (item as KungfuItem).KungfuType);
                        if (data == null)
                            return 0;

                        return data.Number;
                    }
            }

            return 0;
        }

        /// <summary>
        /// ��ȡ��ļ������
        /// </summary>
        /// <param name="recruitType"></param>
        public int GetRecruitmentOrderCount(RecruitType recruitType)
        {
            return m_InventoryDataWrapper.GetRecruitmentOrderCount(recruitType);
        }

        /// <summary>
        /// ��ȡ������ļ�������
        /// </summary>
        /// <returns></returns>
        public int GetAllRecruitmentOrderCount()
        {
            return GetRecruitmentOrderCount(RecruitType.GoldMedal) + GetRecruitmentOrderCount(RecruitType.SilverMedal);
        }

        /// <summary>
        /// �жϲ����Ƿ��㹻
        /// </summary>
        /// <returns></returns>
        public bool HaveEnoughItem(List<CostItem> items, bool isShow = true)
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
        /// ���ٲֿ����
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