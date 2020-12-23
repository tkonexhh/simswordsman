using System;
using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class ClanData : DataDirtyHandler, IResetHandler
    {
        public string clanName = string.Empty;

        public FacilityDbData ownedFacilityData = new FacilityDbData();
        public CharacterDbData ownedCharacterData = new CharacterDbData();
        public InventoryDbData inventoryData = new InventoryDbData();
        public KongfuData kongfuData = new KongfuData();
        

        public void SetDefaultValue()
        {
            SetDataDirty();

            ownedFacilityData.SetDefaultValue();
        }

        public void Init()
        {
           
        }


        public void OnReset()
        {

        }

        #region ClanName
        /// <summary>
        /// 设置宗派名称
        /// </summary>
        /// <param name="_clanName"></param>
        public void SetClanName(string _clanName)
        {
            clanName = _clanName;
        }
        /// <summary>
        /// 获取宗派名称
        /// </summary>
        /// <returns></returns>
        public string GetClanName()
        {
            return clanName;
        }
        #endregion

        #region Facility

        public List<FacilityItemDbData> GetAllFacility()
        {
            return ownedFacilityData.facilityList;
        }

        public FacilityItemDbData GetFacilityItem(FacilityType facilityType/*, int subId*/)
        {
            return ownedFacilityData.GetFacilityData(facilityType/*, subId*/);
        }

        public int GetFacilityLevel(FacilityType facilityType/*, int subId*/)
        {
            int level = ownedFacilityData.GetFacilityLevel(facilityType/*, subId*/);

            return level;
        }

        public void AddFacility(FacilityType facilityType, int subId, FacilityState facilityState)
        {
            ownedFacilityData.AddFacility(facilityType, subId, facilityState);

            SetDataDirty();
        }

        public void SetFacilityState(FacilityType facilityType, FacilityState facilityState/*, int subId*/)
        {
            ownedFacilityData.SetFacilityState(facilityType, facilityState/*, subId*/);

            SetDataDirty();
        }

        public FacilityItemDbData GetFacilityData(FacilityType facilityType/*, int subId*/)
        {
            FacilityItemDbData facilityItemDbData = ownedFacilityData.GetFacilityData(facilityType/*, subId*/);

            return facilityItemDbData;
        }

        //public void RemoveFacility(FacilityType facilityType)
        //{
        //    ownedFacilityData.RemoveFacility(facilityType);

        //    SetDataDirty();
        //}

        public void UpgradeFacility(FacilityType facilityType, int deltaLevel/*, int subId = 1*/)
        {
            ownedFacilityData.UpgradeFacility(facilityType, deltaLevel/*, subId*/);

            SetDataDirty();
        }

        #endregion

        #region Character
        public void AddCharacter(int id, CharacterQuality quality)
        {
            ownedCharacterData.AddCharacter(id, quality);

            SetDataDirty();
        }

        public void RemoveCharacter(int id)
        {
            ownedCharacterData.RemoveCharacter(id);

            SetDataDirty();
        }

        public void SetCharacterLevel(CharacterItemDbData item, int level)
        {
            ownedCharacterData.SetLevel(item, level);

            SetDataDirty();
        }

        public void SetCharacterStage(CharacterItemDbData item, int stage)
        {
            ownedCharacterData.SetStage(item, stage);

            SetDataDirty();
        }

        public void AddCharacterExp(CharacterItemDbData item, int deltaExp)
        {
            ownedCharacterData.AddExp(item, deltaExp);

            SetDataDirty();
        }

        public void AddCharacterKongfuExp(CharacterItemDbData item, KongfuType kongfuType, int deltaExp)
        {
            ownedCharacterData.AddKonfuExp(item, kongfuType, deltaExp);
        }
        #endregion

        #region Inventory



        public List<ArmsDBData> GetArmsDBDataList()
        {
            return inventoryData.armsDBDataList;
        }
        public List<ArmorDBData> GetArmorDBDataList()
        {
            return inventoryData.armorDBDataList;

        }
        public List<PropItemDbData> GetPropList()
        {
            return inventoryData.propList;

        }
        public void AddArms(ArmsItem _armsItem, int delta = 1)
        {
            inventoryData.AddArms(_armsItem, delta);

            SetDataDirty();
        }

        public void AddArmor(ArmorItem _armorDBData, int delta = 1)
        {
            inventoryData.AddArmor(_armorDBData, delta);

            SetDataDirty();
        }

        public void RemoveArmor(ArmorItem _armorDBData, int delta = 1)
        {
            inventoryData.RemoveArmor(_armorDBData, delta);

            SetDataDirty();
        }

        public void RemoveArms(ArmsItem _armsItem, int delta = 1)
        {
            inventoryData.RemoveArms(_armsItem, delta);

            SetDataDirty();
        }

        public void AddPropItem(PropItem propItem, int delta)
        {
            inventoryData.AddPropItem(propItem, delta);

            SetDataDirty();
        }

        public void RemovePropItem(PropItem _propItem, int delta)
        {
            inventoryData.RemovePropItem(_propItem, delta);

            SetDataDirty();
        }

        #endregion

        #region Kongfu

        public void UnlockKongfu(KongfuType kongfuType)
        {
            kongfuData.UnlockKongfu(kongfuType);

            SetDataDirty();
        }

        #endregion

    }


}