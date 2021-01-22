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
        public List<KungfuItemDbData> kungfuList = new List<KungfuItemDbData>();
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
            if (armorDBData != null && armorDBData.RefreshNumber(delta))
                armorDBDataList.Remove(armorDBData);
        }

        public void RemoveArms(ArmsItem _armsItem, int delta = 1)
        {
            ArmsDBData armsDBData = armsDBDataList.Where(i => i.IsHaveItem(_armsItem)).FirstOrDefault();
            if (armsDBData != null && armsDBData.RefreshNumber(delta))
                armsDBDataList.Remove(armsDBData);
        }
        #endregion
        public PropItemDbData GetItem(RawMaterial propSubType)
        {
            return propList.FirstOrDefault(i => i.PropSubType == propSubType);
        }
        public void AddKungfuItem(KungfuItem _kungfuItem, int delta)
        {
            PropItemDbData propItemDbData = propList.FirstOrDefault(i => i.IsHaveItem(_kungfuItem));

            if (propItemDbData != null)
                propItemDbData.AddEquipNumber(delta);
            else
            {
                KungfuItemDbData newKungfuItemDbData = new KungfuItemDbData(_kungfuItem);
                kungfuList.Add(newKungfuItemDbData);
            }
        }

        public void RemoveKungfuItem(KungfuItem _kungfuItem, int delta)
        {
            KungfuItemDbData kungfutemDbData = kungfuList.Where(i => i.IsHaveItem(_kungfuItem)).FirstOrDefault();
            if (kungfutemDbData != null && kungfutemDbData.RefreshNumber(delta))
                kungfuList.Remove(kungfutemDbData);
        }

        public void AddPropItem(PropItem _propItem, int delta)
        {
            PropItemDbData propItemDbData = propList.Where(i => i.IsHaveItem(_propItem)).FirstOrDefault();

            if (propItemDbData != null)
                propItemDbData.AddEquipNumber(delta);
            else
            {
                PropItemDbData newPropItemDbData = new PropItemDbData(_propItem);
                propList.Add(newPropItemDbData);
            }
        }

        public void RefreshPropItem(PropItem _propItem, int number)
        {
            PropItemDbData propItemDbData = propList.Where(i => i.IsHaveItem(_propItem)).FirstOrDefault();
            if (propItemDbData != null && propItemDbData.RefreshNumber(number))
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
        public bool RefreshNumber(int nmuber)
        {
            Number = nmuber;
            if (Number <= 0)
            {
                Number = 0;
                return true;
            }
            return false;
        }
        public void AddEquipNumber(int number)
        {
            Number += number;
            Number = Mathf.Min(Number,Define.MAX_PROP_COUNT);
        }

    }
    [Serializable]
    public class ArmsDBData : ItemDBData
    {
        public ArmsType ArmsID { set; get; }
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
        public ArmorType ArmorID { set; get; }
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
            PropItem propItem = _itemBase as PropItem;
            if (PropType == propItem.PropType && PropSubType == propItem.PropSubType)
                return true;
            return false;
        }
    }

    [Serializable]
    public class KungfuItemDbData : ItemDBData
    {
        public KungfuItemDbData() { }
        public KongfuType KungfuType { set; get; }


        public KungfuItemDbData(KungfuItem _kungfuItem)
        {
            PropType = _kungfuItem.PropType;
            KungfuType = _kungfuItem.KungfuType;
            Number = _kungfuItem.Number;
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
            KungfuItem kungfuItem = _itemBase as KungfuItem;
            if (PropType == kungfuItem.PropType && KungfuType == kungfuItem.KungfuType)
                return true;
            return false;
        }
    }
}