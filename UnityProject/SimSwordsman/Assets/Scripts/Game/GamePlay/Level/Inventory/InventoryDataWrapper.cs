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
                _armorItem.Number += delta;
                m_WarehouseItems.Add(_armorItem);

            }
            m_ClanData.AddArmor(ItemBase.CopySelf(_armorItem), delta);
        }

        public void AddArms(ArmsItem _armsItem, int delta = 1)
        {
            ArmsItem armsItem = (ArmsItem)m_WarehouseItems.Where(i => i.IsHaveItem(_armsItem)).FirstOrDefault();
            if (armsItem != null)
                armsItem.AddEquipNumber(delta);
            else
            {
                _armsItem.Number += delta;
                m_WarehouseItems.Add(_armsItem);
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
                propItem.Number += delta;
                m_WarehouseItems.Add(propItem);
            }
            m_ClanData.AddPropItem(propItem, delta);
        }
        //TODO
        public void RemoveArmor(ArmorItem _armorItem, int delta = 1)
        {
            ArmorItem armorItem = (ArmorItem)m_WarehouseItems.Where(i => i.IsHaveItem(_armorItem)).FirstOrDefault();
            if (armorItem != null && armorItem.ReduceItemNumber(delta))
                m_WarehouseItems.Remove(armorItem);

            EventSystem.S.Send(EventID.OnReduceItems, _armorItem, delta);
            m_ClanData.RemoveArmor(_armorItem, delta);

        }
        public void RemoveArms(ArmsItem _armsItem, int delta = 1)
        {
            ArmsItem armsItem = (ArmsItem)m_WarehouseItems.Where(i => i.IsHaveItem(_armsItem)).FirstOrDefault();
            if (armsItem != null && armsItem.ReduceItemNumber(delta))
                m_WarehouseItems.Remove(armsItem);

            EventSystem.S.Send(EventID.OnReduceItems, _armsItem, delta);
            m_ClanData.RemoveArms(_armsItem, delta);
        }


        public void RemovePropItem(PropItem _propItem, int delta)
        {
            PropItem item = (PropItem)m_WarehouseItems.Where(i => i.IsHaveItem(_propItem)).FirstOrDefault();
            if (item != null && item.ReduceItemNumber(delta))
                m_WarehouseItems.Remove(item);
            EventSystem.S.Send(EventID.OnReduceItems, _propItem, delta);
            m_ClanData.RemovePropItem(_propItem, delta);
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
    }

    public class ArmorItem : ItemBase
    {
        public Armor ArmorID { set; get; }
        public Step ClassID { set; get; }
        public ArmorItem()
        {

        }
        public ArmorItem(ArmorItem armorItem) : base(armorItem)
        {
            ArmorID = armorItem.ArmorID;
            ClassID = armorItem.ClassID;
        }
        public ArmorItem(Armor armor, Step step) 
        {
            PropType = PropType.Armor;
            Number = 0;
            ArmorID = armor;
            ClassID = step;
            RefreshItemInfo();
        }
        public override void RefreshItemInfo()
        {
            Equipment equipment = TDArmorConfigTable.GetEquipmentInfo(ArmorID);
            Name = equipment.Name;
            Desc = equipment.Desc;
            Price = TDArmorConfigTable.GetSellingPrice(ArmorID, ClassID);
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
    }
    public class ArmsItem : ItemBase
    {
        public Arms ArmsID { set; get; }
        public Step ClassID { set; get; }
        public ArmsItem() 
        {
      
        }


        public ArmsItem(ArmsItem armsItem) : base(armsItem)
        {
            ArmsID = armsItem.ArmsID;
            ClassID = armsItem.ClassID;
        }

        public ArmsItem(Arms arms, Step step)
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
            
            Equipment equipment = TDArmsConfigTable.GetEquipmentInfo(ArmsID);
            Name = equipment.Name;
            Desc = equipment.Desc;
            Price = TDArmsConfigTable.GetSellingPrice(ArmsID, ClassID);
        }

        public override int GetSortId()
        {
            return (int)PropType * 100 + (int)ArmsID;
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

    
        public static T CopySelf<T>(T obj)
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
    }
}