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
        public PracticeFieldData ownedPracticeFieldData = new PracticeFieldData();
        public KungfuLibraryData ownedKungfuLibraryData = new KungfuLibraryData();
        public PatrolRoomData ownedPatrolRoomData = new PatrolRoomData();
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


        #region Practice
        public List<PracticeSoltDBData> GetPracticeFieldData()
        {
            return ownedPracticeFieldData.GetPracticeFieldData();
        }

        public void AddPracticeFieldData(PracticeField practiceField)
        {
            ownedPracticeFieldData.AddPracticeFieldData(practiceField);

            SetDataDirty();
        }
        public void RefresPracticeDBData(PracticeField practiceField)
        {
            ownedPracticeFieldData.RefresDBData(practiceField);

            SetDataDirty();
        }
        public void PraceTrainingIsOver(PracticeField practiceField)
        {
            ownedPracticeFieldData.TrainingIsOver(practiceField);

            SetDataDirty();
        }
        #endregion

        #region KungfuLibrary
        public List<KungfuSoltDBData> GetKungfuLibraryData()
        {
            return ownedKungfuLibraryData.GetKungfuLibrayData();
        }
        public void AddKungfuLibraryData(KungfuLibraySlot kungfuLibraySlot)
        {
            ownedKungfuLibraryData.AddKungfuLibrayData(kungfuLibraySlot);

            SetDataDirty();
        }
        public void RefresKungfuDBData(KungfuLibraySlot kungfuLibraySlot)
        {
            ownedKungfuLibraryData.RefresDBData(kungfuLibraySlot);

            SetDataDirty();
        }
        public void KungfuTrainingIsOver(KungfuLibraySlot kungfuLibraySlot)
        {
            ownedKungfuLibraryData.TrainingIsOver(kungfuLibraySlot);

            SetDataDirty();
        }
        #endregion
        #region PatrolRoom
        public List<PatrolRoomSoltDBData> GetPatrolRoomData()
        {
            return ownedPatrolRoomData.GetPatrolRoomData();
        }
        public void AddPatrolRoomData(PatrolRoomSlot patrolRoomSlot)
        {
            ownedPatrolRoomData.AddPatrolRoomData(patrolRoomSlot);

            SetDataDirty();
        }
        public void RefresPatrolRoomDBData(PatrolRoomSlot patrolRoomSlot)
        {
            ownedPatrolRoomData.RefresDBData(patrolRoomSlot);

            SetDataDirty();
        }
        public void PatrolRoomTrainingIsOver(PatrolRoomSlot patrolRoomSlot)
        {
            ownedPatrolRoomData.TrainingIsOver(patrolRoomSlot);

            SetDataDirty();
        }
        #endregion




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

        public bool IsLocked(FacilityType facilityType)
        {
            return ownedFacilityData.IsLocked(facilityType);
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

        public void AddEquipment(int characterID,CharaceterEquipment characeterEquipment)
        {
            ownedCharacterData.AddEquipment(characterID, characeterEquipment);

            SetDataDirty();
        }

        public void UnlockEquip(int characterID, UnlockContent unlockContent)
        {
            ownedCharacterData.UnlockEquip(characterID, unlockContent);

            SetDataDirty();
        }

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

        public void SetCharacterStateDBData(int id, CharacterStateID stateId, FacilityType targetFacilityType)
        {
            ownedCharacterData.SetCharacterStateDBData(id, stateId, targetFacilityType);

            SetDataDirty();
        }

        public void SetCharacterTaskDBData(int id, SimGameTask simGameTask)
        {
            ownedCharacterData.SetCharacterTaskDBData(id, simGameTask);

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
        public void AddKungfu(int id, CharacterKongfuData characterKongfu)
        {
            ownedCharacterData.AddKungfu(id, characterKongfu);

            SetDataDirty();
        }

        public void AddCharacterKongfuExp(int id, CharacterKongfuData kongfuType, int deltaExp)
        {
            ownedCharacterData.AddKonfuExp(id, kongfuType, deltaExp);
        }

        public List<CharacterItemDbData> GetAllCharacterList()
        {
            return ownedCharacterData.characterList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kongfuType"></param>
        /// <param name="deltaExp"></param>
        public void AddCharacterKongfuLevel(int id, CharacterKongfuData kongfuType, int deltaLevel)
        {
            ownedCharacterData.AddCharacterKongfuLevel(id, kongfuType, deltaLevel);
        }
        #endregion

        #region Inventory

        public List<KungfuItemDbData> GetkungfuDBDataList()
        {
            return inventoryData.kungfuList;
        }
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
        public void AddKungfu(KungfuItem _kungfuItem, int delta = 1)
        {
            inventoryData.AddKungfuItem(_kungfuItem, delta);

            SetDataDirty();
        }

        public void RemoveKungfu(KungfuItem _kungfuItem, int delta = 1)
        {
            inventoryData.RemoveKungfuItem(_kungfuItem, delta);

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

        public void RemovePropItem(PropItem _propItem, int number)
        {
            inventoryData.RefreshPropItem(_propItem, number);

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