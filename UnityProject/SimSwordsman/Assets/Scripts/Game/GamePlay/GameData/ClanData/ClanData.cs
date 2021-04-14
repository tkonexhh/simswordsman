using Qarth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameWish.Game
{
    public class ClanData : DataDirtyHandler, IResetHandler
    {
        public string clanName = string.Empty;

        public FacilityDbData ownedFacilityData = new FacilityDbData();
        public CharacterDbData ownedCharacterData = new CharacterDbData();
        public InventoryDbData inventoryData = new InventoryDbData();
        public List<RawMatItemData> rawMatItemDataList = new List<RawMatItemData>();
        public List<WorkItemData> WorkItemDataList = new List<WorkItemData>();
        public List<FoodBuffData> FoodBufferDataList = new List<FoodBuffData>();
        public List<BaiCaoWuData> BaiCaoWuDataList = new List<BaiCaoWuData>();
        public List<ForgeHouseItemData> ForgeHouseItemDataList = new List<ForgeHouseItemData>();
        public List<CollectSystemItemData> CollectSystemItemDataList = new List<CollectSystemItemData>();
        public DeliverData DeliverData = new DeliverData();
        public HeroTrialData heroTrialData = new HeroTrialData();

        public void SetDefaultValue()
        {
            SetDataDirty();

            ownedFacilityData.SetDefaultValue();
        }

        public void OnDataLoadFinish()
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
            EventSystem.S.Send(EventID.OnClanNameChange);
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

        public FacilityDbData GetFacilityDbData()
        {
            return ownedFacilityData;
        }
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

        public void UpgradeFacility(FacilityType facilityType, int deltaLevel/*, int subId = 1*/)
        {
            ownedFacilityData.UpgradeFacility(facilityType, deltaLevel/*, subId*/);

            SetDataDirty();
        }



        #endregion

        #region Character
        public CharacterDbData GetOwnedCharacterData()
        {
            return ownedCharacterData;
        }
        public void AddEquipment(int characterID, CharaceterEquipment characeterEquipment)
        {
            ownedCharacterData.AddEquipment(characterID, characeterEquipment);

            SetDataDirty();
        }
        public void UpGradeEquipment(int characterID, CharaceterEquipment characeterEquipment)
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
            //EventSystem.S.Send(EventID.OnAddCharacter, id, quality);
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

        public void SetCharacterStateDBData(int id, CharacterStateID stateId, FacilityType targetFacilityType, string startTime, int index)
        {
            ownedCharacterData.SetCharacterStateDBData(id, stateId, targetFacilityType, startTime, index);

            SetDataDirty();
        }

        public void SetCharacterTaskDBData(int id, SimGameTask simGameTask)
        {
            ownedCharacterData.SetCharacterTaskDBData(id, simGameTask);

            SetDataDirty();
        }
        public void ClearCharacterTaskDBData(int id, SimGameTask simGameTask)
        {
            ownedCharacterData.ClearCharacterTaskDBData(id, simGameTask);

            SetDataDirty();
        }
        public void SetAtkValue(int id, float atkValue)
        {
            ownedCharacterData.SetAtkValue(id, atkValue);

            SetDataDirty();
        }

        public void SetCharacterStage(CharacterItemDbData item, int stage)
        {
            ownedCharacterData.SetStage(item, stage);

            SetDataDirty();
        }

        public void SetCharacterCollectedObjType(int id, CollectedObjType collectedObjType)
        {
            ownedCharacterData.SetCharacterCollectedObjType(id, collectedObjType);

            SetDataDirty();
        }

        public void RefreshCurExp(CharacterItemDbData item, int deltaExp)
        {
            ownedCharacterData.RefreshCurExp(item, deltaExp);

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

            SetDataDirty();
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

            SetDataDirty();
        }

        public void SetIsHero(int id, bool isHero)
        {
            ownedCharacterData.SetCharacterIsHero(id, isHero);

            SetDataDirty();
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
        public List<HerbItemDbData> GetHerbList()
        {
            return inventoryData.herbDBDataList;

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

            EventSystem.S.Send(EventID.OnGetKungFu, _kungfuItem);

            SetDataDirty();
        }

        public void RemoveKungfu(KungfuItem _kungfuItem, int delta = 1)
        {
            inventoryData.RemoveKungfuItem(_kungfuItem, delta);

            SetDataDirty();
        }

        public void AddHerb(HerbItem herbItem, int delta = 1)
        {
            inventoryData.AddHerbItem(herbItem, delta);

            SetDataDirty();
        }

        public void RemoveHerb(HerbItem herbItem, int delta = 1)
        {
            inventoryData.RemoveHerbItem(herbItem, delta);

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

        // #region Kongfu

        // public void UnlockKongfu(KungfuType kongfuType)
        // {
        //     kongfuData.UnlockKongfu(kongfuType);

        //     SetDataDirty();
        // }

        // #endregion

        #region RawMatItem
        public string GetLastJobFinishedTime(CollectedObjType collectedObjType)
        {
            RawMatItemData item = rawMatItemDataList.FirstOrDefault(i => i.collectObjType == collectedObjType);
            if (item != null)
            {
                return item.lastShowBubbleTime;
            }

            return new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLongTimeString();
        }

        public void SetLastJobFinishedTime(CollectedObjType collectedObjType, DateTime time)
        {
            RawMatItemData item = rawMatItemDataList.FirstOrDefault(i => i.collectObjType == collectedObjType);
            if (item != null)
            {
                item.lastShowBubbleTime = time.ToString();
            }
            else
            {
                rawMatItemDataList.Add(new RawMatItemData(collectedObjType, time.ToString()));
            }

            SetDataDirty();
        }

        public void SetObjCollectedTime(CollectedObjType collectedObjType, int time)
        {
            RawMatItemData item = rawMatItemDataList.FirstOrDefault(i => i.collectObjType == collectedObjType);
            if (item != null)
            {
                item.collectTime = time;
                SetDataDirty();
            }
        }

        public int GetObjCollectedTime(CollectedObjType collectedObjType)
        {
            RawMatItemData item = rawMatItemDataList.FirstOrDefault(i => i.collectObjType == collectedObjType);
            if (item != null)
            {
                return item.collectTime;
            }

            return 0;
        }
        #endregion

        #region work system
        public void SetWorkData(FacilityType ft, int characterID, int totalTime)
        {
            WorkItemData data = WorkItemDataList.FirstOrDefault(x => x.FacilityType == ft);
            if (data != null)
            {
                data.WorkTotalTime = totalTime;
            }
            else
            {
                WorkItemDataList.Add(new WorkItemData(ft, characterID, totalTime));
            }

            SetDataDirty();
        }
        public void RemoveWorkData(FacilityType ft)
        {
            WorkItemData data = WorkItemDataList.FirstOrDefault(x => x.FacilityType == ft);

            if (data != null)
            {
                WorkItemDataList.Remove(data);
            }

            SetDataDirty();
        }
        public void UpdateWorkTime(FacilityType ft, int currentWorkTime)
        {
            WorkItemData data = WorkItemDataList.FirstOrDefault(x => x.FacilityType == ft);

            if (data != null)
            {
                data.CurrentWorkTime = currentWorkTime;
            }

            SetDataDirty();
        }
        public WorkItemData GetWorkItemData(FacilityType ft)
        {
            WorkItemData data = WorkItemDataList.FirstOrDefault(x => x.FacilityType == ft);
            return data;
        }
        #endregion

        #region food buffer system
        public void AddFoodBuffData(int foodBufferID, DateTime startTime, DateTime endTime)
        {
            FoodBuffData data = FoodBufferDataList.FirstOrDefault(x => x.FoodBufferID == foodBufferID);
            if (data != null)
            {
                data.StartTime = startTime;
                data.EndTime = endTime;
            }
            else
            {
                FoodBufferDataList.Add(new FoodBuffData(foodBufferID, startTime, endTime));
            }

            SetDataDirty();
        }
        public void RemoveFoodBuffData(int foodBufferID)
        {
            FoodBuffData data = FoodBufferDataList.FirstOrDefault(x => x.FoodBufferID == foodBufferID);
            if (data != null)
            {
                FoodBufferDataList.Remove(data);
                SetDataDirty();
            }
        }
        public FoodBuffData GetFoodBuffData(int bufferID)
        {
            FoodBuffData data = FoodBufferDataList.FirstOrDefault(x => x.FoodBufferID == bufferID);

            if (data != null && data.IsBufferActive() == false)
            {
                FoodBufferDataList.Remove(data);
                SetDataDirty();
                return null;
            }
            return data;
        }
        public bool IsBuffActiveState(int bufferID)
        {
            FoodBuffData data = FoodBufferDataList.FirstOrDefault(x => x.FoodBufferID == bufferID);

            if (data != null)
            {
                return data.IsBufferActive();
            }
            return false;
        }

        public string GetBuffRemainTime(int bufferID)
        {
            FoodBuffData data = FoodBufferDataList.FirstOrDefault(x => x.FoodBufferID == bufferID);

            if (data != null)
            {
                return data.GetRemainTime();
            }
            return string.Empty;
        }

        public float GetBuffRemainProgress(int bufferID)
        {
            FoodBuffData data = FoodBufferDataList.FirstOrDefault(x => x.FoodBufferID == bufferID);

            if (data != null)
            {
                return data.GetRemainProgress();
            }
            return 0;
        }
        #endregion

        #region bai cao wu system
        public BaiCaoWuData GetBaiCaoWuData(int herbID)
        {
            BaiCaoWuData data = BaiCaoWuDataList.FirstOrDefault(x => x.HerbID == herbID);
            return data;
        }
        public BaiCaoWuData AddBaiCaoWuData(int herbID)
        {
            BaiCaoWuData data = BaiCaoWuDataList.FirstOrDefault(x => x.HerbID == herbID);

            TDHerbConfig herbData = TDHerbConfigTable.GetData(herbID);
            DateTime endTime = DateTime.Now.AddMinutes(herbData.makeTime);

            if (data == null)
            {
                data = new BaiCaoWuData(herbID, DateTime.Now, endTime);
                BaiCaoWuDataList.Add(data);
            }
            else
            {
                data.UpdateData(DateTime.Now, endTime);
            }

            SetDataDirty();

            return data;
        }
        public void RemoveBaiCaoWuData(int herbID)
        {
            BaiCaoWuData data = BaiCaoWuDataList.FirstOrDefault(x => x.HerbID == herbID);

            if (data != null)
            {
                CountDowntMgr.S.StopCountDownItemTest(data.GetCountDownID());

                BaiCaoWuDataList.Remove(data);

                SetDataDirty();
            }
        }
        public void UpdateBaiCaoWuData(int herbID, int reduceTime)
        {
            BaiCaoWuData data = BaiCaoWuDataList.FirstOrDefault(x => x.HerbID == herbID);

            if (data != null)
            {
                data.AlreadyPassTime += reduceTime;

                SetDataDirty();
            }
        }
        public void UpdateBaiCaoWuDataCountDownID(int herbID, int countDownID)
        {
            BaiCaoWuData data = BaiCaoWuDataList.FirstOrDefault(x => x.HerbID == herbID);

            if (data != null)
            {
                data.SetCouontDownID(countDownID);

                SetDataDirty();
            }
        }
        #endregion

        #region forge house system
        public ForgeHouseItemData GetForgeHouseItemData(int forgeHouseID)
        {
            ForgeHouseItemData data = ForgeHouseItemDataList.FirstOrDefault(x => x.ForgeHouseItemID == forgeHouseID);
            return data;
        }
        public ForgeHouseItemData AddForgeHouseItemData(int forgeHouseID)
        {
            ForgeHouseItemData data = ForgeHouseItemDataList.FirstOrDefault(x => x.ForgeHouseItemID == forgeHouseID);

            TDEquipmentConfig equipmentConfig = TDEquipmentConfigTable.GetData(forgeHouseID);
            DateTime endTime = DateTime.Now.AddSeconds(equipmentConfig.forgeTime);

            if (data == null)
            {
                data = new ForgeHouseItemData(forgeHouseID, DateTime.Now, endTime);
                ForgeHouseItemDataList.Add(data);
            }
            else
            {
                data.UpdateData(DateTime.Now, endTime);
            }

            SetDataDirty();

            return data;
        }
        public void UpdateForgeHouseItemData(int forgeHouseID, int reduceTime)
        {
            ForgeHouseItemData data = ForgeHouseItemDataList.FirstOrDefault(x => x.ForgeHouseItemID == forgeHouseID);

            if (data != null)
            {
                data.AlreadyPassTime += reduceTime;

                SetDataDirty();
            }
        }
        public void UpdateForgeHouseItemDataCountDownID(int herbID, int countDownID)
        {
            ForgeHouseItemData data = ForgeHouseItemDataList.FirstOrDefault(x => x.ForgeHouseItemID == herbID);

            if (data != null)
            {
                data.SetCouontDownID(countDownID);

                SetDataDirty();
            }
        }
        public void RemoveForgeHouseItemData(int forgeHouseID)
        {
            ForgeHouseItemData data = ForgeHouseItemDataList.FirstOrDefault(x => x.ForgeHouseItemID == forgeHouseID);

            if (data != null)
            {
                CountDowntMgr.S.StopCountDownItemTest(data.GetCountDownID());

                ForgeHouseItemDataList.Remove(data);

                SetDataDirty();
            }
        }
        #endregion

        #region collect System
        public CollectSystemItemData AddOrUpdateCollectSystemItemData(int collectItemID, DateTime startTime) 
        {
            CollectSystemItemData data = CollectSystemItemDataList.Find(x => x.ID == collectItemID);

            if (data == null)
            {
                data = new CollectSystemItemData(collectItemID, startTime);
                CollectSystemItemDataList.Add(data);
            }
            else {
                data.StartTime = startTime;
            }

            SetDataDirty();

            return data;
        }
        public CollectSystemItemData GetCollectItemData(int collectItemID) 
        {
            CollectSystemItemData data = CollectSystemItemDataList.Find(x => x.ID == collectItemID);
            return data;
        }
        public CollectSystemItemData RemoveCollectSystemItemData(int collectItemID) 
        {
            CollectSystemItemData data = CollectSystemItemDataList.Find(x => x.ID == collectItemID);
            
            if (data != null) 
            {
                data.Clear();

                SetDataDirty();
            }

            return data;
        }
        public int GetCollectItemDataRewardCount(int collectItemID) {
            CollectSystemItemData data = CollectSystemItemDataList.Find(x => x.ID == collectItemID);
            if (data != null) {
                return data.GetRewardCount();
            }
            return 0;
        }
        #endregion


        #region daliver system
        public void RemoveDeliverDataByID(int DeliverID) 
        {
            DeliverData.RemoveDeliverData(DeliverID);
            SetDataDirty();
        }
        public SingleDeliverDetailData AddDeliverData(int DeliverID, DeliverState state, List<DeliverRewadData> rewardDataList,List<int> characterIDList) 
        {
            SingleDeliverDetailData data = DeliverData.AddDeliverData(DeliverID, state, rewardDataList, characterIDList);

            SetDataDirty();

            return data;
        }
        public List<SingleDeliverDetailData> GetAllDaliverData() 
        {
            return DeliverData.DaliverDetailDataList;
        }
        public void SetSpeedUpMultipleByDeliverID(int deliverID,int speedUpMultiple = 2) 
        {
            SingleDeliverDetailData data = GetAllDaliverData().Find(x => x.DeliverID == deliverID);
            if (data != null)
            {
                data.UpdateSpeedUpMultiple(speedUpMultiple);
                data.UpdateSpeedUpMultipleStartTime();
                data.UpdateCountDownSpeedUpMultiple();
                SetDataDirty();
            }
        }
        public bool IsGoOutSide(int deliverID) 
        {
            return DeliverData.IsGoOutSide(deliverID);
        }
        public SingleDeliverDetailData GetDeliverDataByDeliverID(int deliverID) 
        {
            return DeliverData.GetDeliverDataByID(deliverID);
        }
        #endregion

        //#region HeroTrial
        //public void OnHeroTrialStart(int day, int characterId)
        //{
        //    heroTrialData.OnTrialStart(day, characterId);

        //    SetDataDirty();
        //}
        //#endregion

    }
}
