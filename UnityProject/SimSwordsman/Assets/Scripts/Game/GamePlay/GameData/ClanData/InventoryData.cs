using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameWish.Game
{
    [Serializable]
    public class InventoryDbData
    {
        public List<PropItemDbData> propList = new List<PropItemDbData>();
        public List<ArmsDBData> armsDBDataList = new List<ArmsDBData>();
        public List<ArmorDBData> armorDBDataList = new List<ArmorDBData>();


        public InventoryDbData()
        {

        }

        #region Equipment
        public void AddArms(ArmsItem _armsItem, int delta = 1)
        {
            ArmsDBData armsItemDBData = armsDBDataList.Where(i => i.IsHaveItem(_armsItem)).FirstOrDefault();
            if (armsItemDBData != null)
                armsItemDBData.AddEquipNumber(delta);
            else
            {
                ArmsDBData newArmsDBData = new ArmsDBData(_armsItem);
                armsDBDataList.Add(newArmsDBData);
            }

        }

        public void AddArmor(ArmorItem _armorDBData, int delta = 1)
        {
            ArmorDBData armorDBData = armorDBDataList.Where(i => i.IsHaveItem(_armorDBData)).FirstOrDefault();
            if (armorDBData != null)
                armorDBData.AddEquipNumber(delta);
            else
            {
                ArmorDBData newArmorDBData = new ArmorDBData(_armorDBData);
                armorDBDataList.Add(newArmorDBData);
            }
        }

        public void RemoveArmor(ArmorItem _armorDBData, int delta = 1)
        {
            ArmorDBData armorDBData = armorDBDataList.Where(i => i.IsHaveItem(_armorDBData)).FirstOrDefault();
            if (armorDBData != null && armorDBData.ReduceItemNumber(delta))
                armorDBDataList.Remove(armorDBData);
        }

        public void RemoveArms(ArmsItem _armsItem, int delta = 1)
        {
            ArmsDBData armsDBData = armsDBDataList.Where(i => i.IsHaveItem(_armsItem)).FirstOrDefault();
            if (armsDBData != null && armsDBData.ReduceItemNumber(delta))
                armsDBDataList.Remove(armsDBData);
        }
        #endregion
        public PropItemDbData GetItem(RawMaterial propSubType)
        {
            return propList.FirstOrDefault(i => i.PropSubType == propSubType);
        }

        public void AddPropItem(PropItem _propItem, int delta)
        {
            PropItemDbData propItemDbData = propList.FirstOrDefault(i => i.IsHaveItem(_propItem));

            if (propItemDbData != null)
                propItemDbData.AddEquipNumber(delta);
            else
            {
                PropItemDbData newPropItemDbData = new PropItemDbData(_propItem);
                propList.Add(newPropItemDbData);
            }
        }

        public void RemovePropItem(PropItem _propItem, int delta)
        {
            PropItemDbData propItemDbData = propList.Where(i => i.IsHaveItem(_propItem)).FirstOrDefault();
            if (propItemDbData != null && propItemDbData.ReduceItemNumber(delta))
                propList.Remove(propItemDbData);
        }
    }
    [Serializable]
    public abstract class ItemDBData
    {
        public PropType PropType { set; get; }
        public int Number { set; get; }
        public abstract bool IsHaveItem(ItemBase _itemBase);
        public ItemDBData() { }
        public ItemDBData(ItemBase itemBase)
        {
            PropType = itemBase.PropType;
            Number = itemBase.Number;
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
        }

    }
    [Serializable]
    public class ArmsDBData : ItemDBData
    {
        public Arms ArmsID { set; get; }
        public Step ClassID { set; get; }
        public ArmsDBData() { }

        public ArmsDBData(ArmsItem armsItem) : base(armsItem)
        {
            ArmsID = armsItem.ArmsID;
            ClassID = armsItem.ClassID;
        }
        public override bool IsHaveItem(ItemBase _itemBase)
        {
            ArmsItem armsItem = _itemBase as ArmsItem;
            if (ArmsID == armsItem.ArmsID && ClassID == armsItem.ClassID)
                return true;
            return false;
        }
    }
    [Serializable]
    public class ArmorDBData : ItemDBData
    {
        public Armor ArmorID { set; get; }
        public Step ClassID { set; get; }
        public ArmorDBData() { }

        public ArmorDBData(ArmorItem armorItem) : base(armorItem)
        {
            ArmorID = armorItem.ArmorID;
            ClassID = armorItem.ClassID;
        }

        public override bool IsHaveItem(ItemBase _itemBase)
        {
            ArmorItem armorItem = _itemBase as ArmorItem;
            if (ArmorID == armorItem.ArmorID && ClassID == armorItem.ClassID)
                return true;
            return false;
        }
    }

    [Serializable]
    public class PropItemDbData:ItemDBData
    {
        public PropItemDbData() { }
        public RawMaterial PropSubType { set; get; }
  

        public PropItemDbData(PropItem _propItem)
        {
            PropType = _propItem.PropType;
            PropSubType = _propItem.PropSubType;
            Number = _propItem.Number;
        }

        public void AddCount(int delta)
        {
            Number += delta;

            Number = Mathf.Clamp(Number, 0, Define.MAX_PROP_COUNT);
        }

        public void RemoveCount(int delta)
        {
            Number -= delta;

            Number = Mathf.Clamp(Number, 0, Define.MAX_PROP_COUNT);
        }

        public override bool IsHaveItem(ItemBase _itemBase)
        {
            return true;
        }
    }
}