using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;
using System;
using System.Reflection;

namespace GameWish.Game
{
    public class InventoryDataWrapper
    {

        private ClanData m_ClanData = null;
        private List<ItemBase> m_WarehouseItems = new List<ItemBase>();

        public List<ItemBase> WarehouseItems { get => m_WarehouseItems; }

        public void Init()
        {
            m_ClanData = GameDataMgr.S.GetClanData();


            m_ClanData.GetPropList().ForEach(i =>
            {
                PropItem item = new PropItem();
                item.Wrap(i);

                m_WarehouseItems.Add(item);
            });

            m_ClanData.GetkungfuDBDataList().ForEach(i =>
            {
                KungfuItem item = new KungfuItem();
                item.Wrap(i);

                m_WarehouseItems.Add(item);
            });

            m_ClanData.GetArmsDBDataList().ForEach(i =>
            {
                ArmsItem item = new ArmsItem();
                item.Wrap(i);

                m_WarehouseItems.Add(item);
            });
            m_ClanData.GetArmorDBDataList().ForEach(i =>
            {
                ArmorItem item = new ArmorItem();
                item.Wrap(i);

                m_WarehouseItems.Add(item);
            });
        }


        #region Private
        private bool CheckInventoryIsFull()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Warehouse);
            WarehouseLevelInfo warehouseNextLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Warehouse, level) as WarehouseLevelInfo;
            int limitReserves = warehouseNextLevelInfo.GetCurReserves();
            if (GetCurReserves() < limitReserves)
                return false;
            return true;
        }
        #endregion
        /// <summary>
        /// 检查RawMaterial类型
        /// </summary>
        /// <param name="rawMaterial"></param>
        /// <returns></returns>
        public bool CheckItemInInventory(RawMaterial rawMaterial,int number)
        {
            bool isGet = false;
            m_WarehouseItems.ForEach(i =>
            {
                if (i.PropType == PropType.RawMaterial)
                {
                    //TODO当某一个物品999的时候
                    PropItem propItem = (PropItem)i;
                    if (propItem.PropSubType == rawMaterial && propItem.Number >= number)
                        isGet =  true;
                }
            });
            return isGet;
        }

        /// <summary>
        /// 根据类型返回所以装备
        /// </summary>
        /// <param name="equipType"></param>
        /// <returns></returns>
        public List<ItemBase> GetAllEquipmentForType(PropType equipType)
        {
            if (m_WarehouseItems.Count > 0)
            {
                List<ItemBase> tempEquip = new List<ItemBase>();
                foreach (var item in m_WarehouseItems)
                    if (item.PropType == equipType)
                        tempEquip.Add((ItemBase)item);
                return tempEquip;
            }
            return null;
        }

        //public int GetEquipSellingPrice(ItemBase itemBase)
        //{
        //    return TDEquipmentConfigTable.GetSellingPrice(itemBase);
        //}

        /// <summary>
        /// 添加装备,朝着仓库添加
        /// </summary>
        /// <param name="equipType"></param>
        /// <param name="equipID"></param>
        /// <param name="classID"></param>
        /// <param name="delta"></param>
        public void AddArmor(ArmorItem _armorItem, int delta = 1)
        {
            ArmorItem armorItem = (ArmorItem)m_WarehouseItems.Where(i => i.IsHaveItem(_armorItem)).FirstOrDefault();
            if (armorItem != null)
                armorItem.AddEquipNumber(delta);
            else
            {
                if (!CheckInventoryIsFull())
                {
                    _armorItem.Number += delta;
                    m_WarehouseItems.Add(_armorItem);
                }
                else
                    return;
            }
            m_ClanData.AddArmor(ItemBase.CopySelf(_armorItem), delta);
        }
        public void AddKungfu(KungfuItem _kungfuItem, int delta = 1)
        {
            KungfuItem kungfuItem = (KungfuItem)m_WarehouseItems.Where(i => i.IsHaveItem(_kungfuItem)).FirstOrDefault();
            if (kungfuItem != null) 
                kungfuItem.AddEquipNumber(delta);
            else
            {
                if (!CheckInventoryIsFull())
                {
                    _kungfuItem.Number += delta;
                    m_WarehouseItems.Add(_kungfuItem);
                }
                else
                    return;
            }
            m_ClanData.AddKungfu(ItemBase.CopySelf(_kungfuItem), delta);
        }

        public void AddHerb(HerbItem herbItem, int delta = 1)
        {
            HerbItem herb = (HerbItem)m_WarehouseItems.Where(i => i.IsHaveItem(herbItem)).FirstOrDefault();
            if (herb != null)
                herb.AddEquipNumber(delta);
            else
            {
                if (!CheckInventoryIsFull())
                {
                    herbItem.Number += delta;
                    m_WarehouseItems.Add(herbItem);
                }
                else
                    return;
            }
            m_ClanData.AddHerb(ItemBase.CopySelf(herbItem), delta);
        }

        public void AddArms(ArmsItem _armsItem, int delta = 1)
        {
            ArmsItem armsItem = (ArmsItem)m_WarehouseItems.Where(i => i.IsHaveItem(_armsItem)).FirstOrDefault();
            if (armsItem != null)
                armsItem.AddEquipNumber(delta);
            else
            {
                if (!CheckInventoryIsFull())
                {
                    _armsItem.Number += delta;
                    m_WarehouseItems.Add(_armsItem);
                }
                else
                    return;
            }
            m_ClanData.AddArms(ItemBase.CopySelf(_armsItem), delta);
        }
        public void AddPropItem(PropItem propItem, int delta=1)
        {
            PropItem item = (PropItem)m_WarehouseItems.Where(i => i.IsHaveItem(propItem)).FirstOrDefault();
            if (item != null)
                item.AddEquipNumber(delta);
            else
            {
                if (!CheckInventoryIsFull())
                {
                    propItem.Number += delta;
                    m_WarehouseItems.Add(propItem);
                }
                else
                    return;
            }
            m_ClanData.AddPropItem(propItem, delta);

            if (propItem.PropSubType == RawMaterial.SilverToken || propItem.PropSubType == RawMaterial.GoldenToken)
                EventSystem.S.Send(EventID.OnRecruitmentOrderIncrease, propItem.PropSubType, delta);
        }


        
        public int GetCurReserves()
        {
            return m_WarehouseItems.Count;
        }
        public void RemoveArmor(ArmorItem _armorItem, int delta = 1)
        {
            ArmorItem armorItem = (ArmorItem)m_WarehouseItems.Where(i => i.IsHaveItem(_armorItem)).FirstOrDefault();
            if (armorItem != null && armorItem.ReduceItemNumber(delta))
                m_WarehouseItems.Remove(armorItem);

            EventSystem.S.Send(EventID.OnReduceItems, _armorItem, delta);
            m_ClanData.RemoveArmor(_armorItem, armorItem.Number);
        }
        /// <summary>
        /// 获取招募令数量
        /// </summary>
        /// <param name="recruitType"></param>
        public int GetRecruitmentOrderCount(RecruitType recruitType)
        {
            int count = 0;
            foreach (var item in m_WarehouseItems)
            {
                if (item.PropType== PropType.RawMaterial)
                {
                    PropItem prop = (PropItem)item;
                    switch (recruitType)
                    {
                        case RecruitType.GoldMedal:
                            if (prop.PropSubType == RawMaterial.GoldenToken)
                                count++;
                            continue;
                        case RecruitType.SilverMedal:
                            if (prop.PropSubType == RawMaterial.SilverToken)
                                count++;
                            continue;
                    }
                }
            }
            return count;
        }
        public void RemoveArms(ArmsItem _armsItem, int delta = 1)
        {
            ArmsItem armsItem = (ArmsItem)m_WarehouseItems.Where(i => i.IsHaveItem(_armsItem)).FirstOrDefault();
            if (armsItem != null && armsItem.ReduceItemNumber(delta))
                m_WarehouseItems.Remove(armsItem);

            EventSystem.S.Send(EventID.OnReduceItems, _armsItem, delta);
            m_ClanData.RemoveArms(_armsItem, armsItem.Number);
        }

        public void RemoveKungfu(KungfuItem _kungfuItem, int delta)
        {
            KungfuItem item = (KungfuItem)m_WarehouseItems.Where(i => i.IsHaveItem(_kungfuItem)).FirstOrDefault();
            if (item != null && item.ReduceItemNumber(delta))
                m_WarehouseItems.Remove(item);
            EventSystem.S.Send(EventID.OnReduceItems, _kungfuItem, delta);
            m_ClanData.RemoveKungfu(_kungfuItem, item.Number);
        }

        public void RemovePropItem(PropItem _propItem, int delta)
        {
            PropItem item = (PropItem)m_WarehouseItems.Where(i => i.IsHaveItem(_propItem)).FirstOrDefault();
            if (item != null && item.ReduceItemNumber(delta))
                m_WarehouseItems.Remove(item);
            EventSystem.S.Send(EventID.OnReduceItems, _propItem, delta);
            m_ClanData.RemovePropItem(_propItem, item.Number);
        }
    }

    public class PropItem : ItemBase
    {
        public RawMaterial PropSubType { set; get; }
        public PropItem()
        {

        }
        public PropItem(PropItem propItem) : base(propItem)
        {
            PropSubType = propItem.PropSubType;
        }
        public PropItem(RawMaterial rawMaterial)
        {
            PropType = PropType.RawMaterial;
            PropSubType = rawMaterial;
            Number = 0;
            RefreshItemInfo();
        }


        public void InitEquipmentItem(PropType equipType)
        {
            //m_ConfigInfo = TDPropConfigTable.GetPropConfigInfo((int)PropType);
            //Name = m_Equipment.Name;
            //Desc = m_Equipment.Desc;
            //Price = m_Equipment.GetSellingPriceForClassID(equipID);
        }

        public override void Wrap<T>(T t)
        {
            PropItemDbData dbItem = t as PropItemDbData;
            PropType = dbItem.PropType;
            PropSubType = dbItem.PropSubType;
            Number = dbItem.Number;

            RefreshItemInfo();
        }

        public void RemoveCount(int delta)
        {
            Number -= delta;
            Number = Mathf.Clamp(Number, 0, Define.MAX_PROP_COUNT);
        }

        public override bool IsHaveItem(ItemBase _itemBase)
        {
            PropItem propItem = _itemBase as PropItem;
            if (propItem != null && PropSubType == propItem.PropSubType )
                return true;
            return false;
        }

        public override void RefreshItemInfo()
        {
            PropConfigInfo configInfo = TDItemConfigTable.GetPropConfigInfo((int)PropSubType);
            Desc = configInfo.desc;
            Name = configInfo.name;
            Price = configInfo.price;
        }

        public override int GetSortId()
        {
            return (int)PropType * 100 + (int)PropSubType;
        }
        public override int GetSubName()
        {
            return (int)PropSubType;
        }
    }

    public class KungfuItem : ItemBase
    {
        public KongfuType KungfuType { set; get; }
        public float AtkScale { set; get; }
        public KungfuItem()
        {

        }
        public override int GetSortId()
        {
            return (int)KungfuType;
        }
        public KungfuItem(KongfuType kungfuType)
        {
            PropType = PropType.Kungfu;
            KungfuType = kungfuType;
            Number = 0;
            RefreshItemInfo();
        }

        public override bool IsHaveItem(ItemBase _itemBase)
        {
            KungfuItem propItem = _itemBase as KungfuItem;
            if (propItem != null && KungfuType == propItem.KungfuType)
                return true;
            return false;
        }

        public override void RefreshItemInfo()
        {
            KungfuConfigInfo configInfo = TDKongfuConfigTable.GetKungfuConfigInfo(KungfuType);
            if (configInfo!=null)
            {
                Desc = configInfo.Desc;
                Name = configInfo.Name;
                Price = configInfo.Price;
            }
            else
                Log.e("KungfuConfigInfo is null,KungfuType is " + KungfuType);
    
        }

        public override void Wrap<T>(T t)
        {
            KungfuItemDbData dBData = t as KungfuItemDbData;
            PropType = dBData.PropType;
            Number = dBData.Number;
            KungfuType = dBData.KungfuType;
            RefreshItemInfo();
        }
        public override int GetSubName()
        {
            return (int)KungfuType;
        }
    }

    public class ArmorItem : ItemBase
    {
        public ArmorType ArmorID { set; get; }
        public Step ClassID { set; get; }
        public ArmorItem()
        {

        }
        public ArmorItem(ArmorItem armorItem) : base(armorItem)
        {
            ArmorID = armorItem.ArmorID;
            ClassID = armorItem.ClassID;
        }
        public ArmorItem(ArmorType armor, Step step) 
        {
            PropType = PropType.Armor;
            Number = 0;
            ArmorID = armor;
            ClassID = step;
            RefreshItemInfo();
        }
        public override void RefreshItemInfo()
        {
            Equipment equipment = TDEquipmentConfigTable.GetEquipmentInfo(ArmorID);
            Name = equipment.Name;
            Desc = equipment.Desc;
            Price = TDEquipmentConfigTable.GetSellingPrice(ArmorID, ClassID);
        }

        public override bool IsHaveItem(ItemBase _itemBase)
        {
            ArmorItem armorItem = _itemBase as ArmorItem;
            if (armorItem!=null && ArmorID == armorItem.ArmorID && ClassID == armorItem.ClassID)
                return true;
            return false;
        }

        public override void Wrap<T>(T t)
        {
            ArmorDBData dBData = t as ArmorDBData;
            PropType = dBData.PropType;
            Number = dBData.Number;
            ArmorID = dBData.ArmorID;
            ClassID = dBData.ClassID;
            RefreshItemInfo();
        }

        public override int GetSortId()
        {
            return (int)PropType * 100 + (int)ArmorID;
        }
        public override int GetSubName()
        {
            return (int)ArmorID;
        }
    }
    public class ArmsItem : ItemBase
    {
        public ArmsType ArmsID { set; get; }
        public Step ClassID { set; get; }
        public ArmsItem() 
        {
      
        }

        public ArmsItem(ArmsItem armsItem) : base(armsItem)
        {
            ArmsID = armsItem.ArmsID;
            ClassID = armsItem.ClassID;
        }

        public ArmsItem(ArmsType arms, Step step)
        {
            ArmsID = arms;
            ClassID = step;
            PropType = PropType.Arms;
            Number = 0;
            RefreshItemInfo();
        }

        public override bool IsHaveItem(ItemBase _itemBase)
        {
            ArmsItem armsItem = _itemBase as ArmsItem;
            if (armsItem != null && ArmsID == armsItem.ArmsID && ClassID == armsItem.ClassID)
                return true;
            return false;
        }

        public override void Wrap<T>(T t)
        {
            ArmsDBData dBData = t as ArmsDBData;
            PropType = dBData.PropType;
            Number = dBData.Number;
            ArmsID = dBData.ArmsID;
            ClassID = dBData.ClassID;
            RefreshItemInfo();
        }

        public override void RefreshItemInfo()
        {
            Equipment equipment = TDEquipmentConfigTable.GetEquipmentInfo(ArmsID);
            Name = equipment.Name;
            Desc = equipment.Desc;
            Price = TDEquipmentConfigTable.GetSellingPrice(ArmsID, ClassID);
        }

        public override int GetSortId()
        {
            return (int)PropType * 100 + (int)ArmsID;
        }
        public override int GetSubName()
        {
            return (int)ArmsID;
        }
    }

    public class HerbItem : ItemBase
    {
        public HerbType HerbID { set; get; }

        public HerbItem()
        {

        }

        public HerbItem(HerbType herbType, int count)
        {
            HerbID = herbType;
            PropType = PropType.Herb;
            Number = count;
            RefreshItemInfo();
        }

        public override bool IsHaveItem(ItemBase item)
        {
            HerbItem herbItem = item as HerbItem;
            if (herbItem != null && HerbID == herbItem.HerbID)
                return true;
            return false;
        }

        public override void Wrap<T>(T t)
        {
            HerbItemDbData dBData = t as HerbItemDbData;
            PropType = dBData.PropType;
            Number = dBData.Number;
            HerbID = dBData.HerbType;
            RefreshItemInfo();
        }

        public override void RefreshItemInfo()
        {
            HerbConfig herbConfig = TDHerbConfigTable.GetHerbForId((int)HerbID);
            Name = herbConfig.Name;
            Desc = herbConfig.Desc;
            Price = herbConfig.Price;
        }

        public override int GetSortId()
        {
            return (int)PropType * 100 + (int)HerbID;
        }
        public override int GetSubName()
        {
            return (int)HerbID;
        }
    }

    public abstract class ItemBase
    {
        public PropType PropType { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }
        public int Number { set; get; }
        public int Price { set; get; }
        public ItemBase()
        {
        }
        public ItemBase(ItemBase item)
        {
            PropType = item.PropType;
            Name = item.Name;
            Desc = item.Desc;
            Number = item.Number;
            Price = item.Price;
        }

        public abstract bool IsHaveItem(ItemBase _itemBase);
        public abstract void RefreshItemInfo();
        public abstract void Wrap<T>(T t);
        public T ToSubType<T>() where T : ItemBase
        {
            return this as T;
        }
        public bool ReduceItemNumber(int nmuber)
        {
            Number -= nmuber;
            if (Number <= 0)
            {
                Number = 1;
                return true;
            }
            return false;
        }
        public void AddEquipNumber(int number)
        {
            Number += number;
            Number = Mathf.Clamp(Number, 0, Define.MAX_PROP_COUNT);
        }

    
        public static T CopySelf<T>(T obj) where T:ItemBase
        {
            object retval = Activator.CreateInstance(typeof(T));
            PropertyInfo[] pis = typeof(T).GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                try { pi.SetValue(retval, pi.GetValue(obj, null), null); }
                catch { }
            }
            return (T)retval;
        }

        public abstract int GetSortId();

        public abstract int GetSubName();
    }
}