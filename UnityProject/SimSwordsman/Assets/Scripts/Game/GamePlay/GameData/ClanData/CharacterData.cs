using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameWish.Game
{

    [Serializable]
    public class CharacterDbData
    {
        public List<CharacterItemDbData> characterList = new List<CharacterItemDbData>();

        public CharacterDbData()
        {

        }

        public void AddEquipment(int characterID, CharaceterEquipment characeterEquipment)
        {
            CharacterItemDbData character = characterList.Where(i => i.id == characterID).FirstOrDefault();
            if (character != null)
                character.AddEquipmentItem(characeterEquipment);
        }
        public void UpGradeEquipment(int characterID, CharaceterEquipment characeterEquipment)
        {
            CharacterItemDbData character = characterList.Where(i => i.id == characterID).FirstOrDefault();
            if (character != null)
                character.UpGradeEquipment(characeterEquipment);
        }

        public void AddCharacter(int id, CharacterQuality quality)
        {
            bool isOwned = characterList.Any(i => i.id == id);
            if (!isOwned)
            {
                characterList.Add(new CharacterItemDbData(id, quality));
            }
        }

        public void AddCharacter(CharacterItemDbData item)
        {
            bool isOwned = characterList.Any(i => i.id == item.id);
            if (!isOwned)
            {
                characterList.Add(item);
            }
        }

        public void RemoveCharacter(int id)
        {
            characterList = characterList.Except(characterList.Where(i => i.id == id)).ToList();
        }

        public bool IsHaveCharacterLevelGreaterNumber(int number)
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                CharacterItemDbData data = characterList[i];
                if (data != null && data.level >= number)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsEquipmentArms() {
            return characterList.Find(x => x.IsEquipmentArms()) != null;
        }
        public bool IsStudyKungFu() {
            return characterList.Find(x => x.IsStudyKungFu()) != null;
        }

        public int GetCharacterCount()
        {
            return characterList.Count;
        }
        public void SetLevel(CharacterItemDbData item, int level)
        {
            item.level = level;
        }

        public void SetStage(CharacterItemDbData item, int stage)
        {
            item.stage = stage;
        }

        public void RefreshCurExp(CharacterItemDbData item, int deltaExp)
        {
            item.RefreshCurExp(deltaExp);
        }
        /// <summary>
        /// 增加功夫经验
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kongfuType"></param>
        /// <param name="deltaExp"></param>
        public void AddKonfuExp(int id, CharacterKongfuData kongfuType, int deltaExp)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                CharacterKongfuDBData characterKongfuDBData = characterItemDb.kongfuDatas.Where(i => i.kongfuType == kongfuType.GetKungfuType()).FirstOrDefault();
                characterKongfuDBData.AddExp(deltaExp);
            }
        }
        /// <summary>
        ///  增加功夫等级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kongfuType"></param>
        /// <param name="deltaLevel"></param>
        public void AddCharacterKongfuLevel(int id, CharacterKongfuData kongfuType, int deltaLevel)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                CharacterKongfuDBData characterKongfuDBData = characterItemDb.kongfuDatas.Where(i => i.kongfuType == kongfuType.GetKungfuType()).FirstOrDefault();
                characterKongfuDBData.AddLevel(deltaLevel);
            }
        }


        public void AddKungfu(int id, CharacterKongfuData characterKongfu)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
                characterItemDb.LearnKungfu(characterKongfu);
        }

        public void UnlockEquip(int id, UnlockContent unlockContent)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                switch (unlockContent)
                {
                    case UnlockContent.EquipWeapon:
                        characterItemDb.characeterDBEquipmentData.IsDBArmsUnlock = true;
                        break;
                    case UnlockContent.EquipArmor:
                        characterItemDb.characeterDBEquipmentData.IsDBArmorUnlock = true;
                        break;
                }
            }
        }

        public void SetCharacterStateDBData(int id, CharacterStateID characterStateData, FacilityType targetFacilityType, string startTime, int index, bool isFindPathToTargetPos = true)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                characterItemDb.SetCharacterStateDBData(characterStateData, targetFacilityType, startTime, index,isFindPathToTargetPos);
            }
        }

        public void SetCharacterTaskDBData(int id, SimGameTask task)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                int taskId = task == null ? -1 : task.TaskId;
                characterItemDb.SetTask(taskId);
            }
        }
        public void ClearCharacterTaskDBData(int id, SimGameTask task)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                int taskId = task == null ? -1 : task.TaskId;
                characterItemDb.ClearTask(-1);
            }
        }

        public void SetAtkValue(int id, float atkValue)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                characterItemDb.SetAtkValue(atkValue);
            }
        }

        public void SetCharacterCollectedObjType(int id, CollectedObjType collectedObjType)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
            {
                characterItemDb.SetCollectObjType(collectedObjType);
            }
        }
    }

    [Serializable]
    public class CharacterItemDbData
    {
        public int id;
        public int level;
        public int stage;
        public CharacterQuality quality;
        public int atkValue;
        public string startTime;
        public string name;
        public int taskId = -1;
        public CharacterStateID characterStateId = CharacterStateID.Wander; //当前状态
        public CharacterFacilityData facilityData = new CharacterFacilityData();
        public CharaceterDBEquipmentData characeterDBEquipmentData = new CharaceterDBEquipmentData();
        public List<CharacterKongfuDBData> kongfuDatas = new List<CharacterKongfuDBData>();
        public int curExp;
        public int bodyId = 1; // Which body used
        public int headId = 1; // Which head used
        public CollectedObjType collectedObjType = CollectedObjType.None;
        /// <summary>
        /// 是否寻路到目标地点，
        /// true:正常寻路到目标地点
        /// false:不寻路，做其他操作
        /// </summary>
        private bool m_IsFindPathToTargetPos = true;

        public void SetAtkValue(float _atkValue)
        {
            atkValue = (int)_atkValue;
        }
        public CharacterItemDbData()
        {

        }
        public void SetCharacterStateDBData(CharacterStateID stateId, FacilityType targetFacilityType, string startTime, int id,bool isFindPathToTargetPos = true)
        {
            characterStateId = stateId;
            this.facilityData.targetFacility = targetFacilityType;
            this.facilityData.startTime = startTime;
            this.facilityData.id = id;
            m_IsFindPathToTargetPos = isFindPathToTargetPos;
        }
        public bool IsFindPathToTargetPos() 
        {
            return m_IsFindPathToTargetPos;
        }
        public CharacterItemDbData(CharacterItem item)
        {
            this.startTime = DateTime.Now.ToString();
            id = item.id;
            name = item.name;
            level = item.level;
            quality = item.quality;
            stage = item.stage;
            bodyId = item.bodyId;
            headId = item.headId;
        }
        public CharacterItemDbData(int id, CharacterQuality quality)
        {
            this.id = id;
            this.level = 1;
            this.quality = quality;
            this.stage = 1;
        }
        public void LearnKungfu(CharacterKongfuData characterKongfu)
        {
            kongfuDatas.Add(new CharacterKongfuDBData(characterKongfu));
        }
        public void AddEquipmentItem(CharaceterEquipment characeterEquipment)
        {
            characeterDBEquipmentData.AddEquipment(characeterEquipment);
        }
        public void UpGradeEquipment(CharaceterEquipment characeterEquipment)
        {
            characeterDBEquipmentData.AddEquipment(characeterEquipment);
        }
        public void SetTask(int taskId)
        {
            this.taskId = taskId;
        }
        public void ClearTask(int taskId)
        {
            this.taskId = taskId;
        }
        /// <summary>
        /// 归还装备
        /// </summary>
        /// <param name="equipType"></param>
        //public EquipmentData ReturnEquipment(PropType equipType)
        //{
        //    EquipmentData equipment = characterEquipmentDatas.Where(i => i.PropType == equipType).FirstOrDefault();
        //    if (equipment != null)
        //    {
        //        characterEquipmentDatas.Remove(equipment);
        //        return equipment;
        //    }
        //    return null;
        //}
        public void RefreshCurExp(int delta)
        {
            curExp = delta;
            //curExp = Math.Max(0, curExp);
        }
        public void SetCollectObjType(CollectedObjType collectedObjType)
        {
            this.collectedObjType = collectedObjType;
        }

        public bool IsEquipmentArms() {
            return characeterDBEquipmentData.CharacterDBArms.IsEquipmentArms();
        }
        public bool IsStudyKungFu() 
        {
            return kongfuDatas.Find(x => x.IsStudyKungFu()) != null;
        }
    }

    [Serializable]
    public class CharacterFacilityData
    {
        public FacilityType targetFacility = FacilityType.None;
        public string startTime = string.Empty;
        public int id;
    }

    [Serializable]
    public class CharacterKongfuDBData
    {
        public int index;
        public KungfuLockState kungfuLockState;
        public KungfuType kongfuType;
        public int level;
        public int curExp;
        public CharacterKongfuDBData()
        {

        }
        public CharacterKongfuDBData(int index, KungfuLockState kungfuLockState, KungfuType kongfuType, int level, int curExp)
        {
            this.index = index;
            this.kungfuLockState = kungfuLockState;
            this.kongfuType = kongfuType;
            this.level = level;
            this.curExp = curExp;
        }
        public CharacterKongfuDBData(CharacterKongfuData characterKongfu)
        {
            index = characterKongfu.Index;
            kungfuLockState = characterKongfu.KungfuLockState;
            if (characterKongfu.KungfuLockState == KungfuLockState.Learned)
            {
                kongfuType = characterKongfu.CharacterKongfu.dbData.kongfuType;
                level = characterKongfu.CharacterKongfu.dbData.level;
                curExp = characterKongfu.CharacterKongfu.dbData.curExp;
            }
        }
        public void AddExp(int deltaExp)
        {
            curExp += deltaExp;
        }
        public void AddLevel(int deltaLevel)
        {
            level = deltaLevel;
        }

        public bool IsStudyKungFu() {
            return kongfuType != KungfuType.None;
        }
    }

    [Serializable]
    public class CharaceterDBEquipmentData
    {
        public CharacterDBArms CharacterDBArms { set; get; } = new CharacterDBArms();

        public bool IsDBArmsUnlock { set; get; } = false;
        public CharacterDBArmor CharacterDBArmor { set; get; } = new CharacterDBArmor();
        public bool IsDBArmorUnlock { set; get; } = false;

        public CharaceterDBEquipmentData() { }

        public void AddEquipment(CharaceterEquipment characeterEquipment)
        {
            switch (characeterEquipment.PropType)
            {
                case PropType.Arms:
                    CharacterDBArms.AddArms((CharacterArms)characeterEquipment);
                    break;
                case PropType.Armor:
                    CharacterDBArmor.AddArmor((CharacterArmor)characeterEquipment);
                    break;
            }
        }

        public void UpGradeEquipment(CharaceterEquipment characeterEquipment)
        {
            switch (characeterEquipment.PropType)
            {
                case PropType.Arms:
                    CharacterDBArms.UpGradeArms();
                    break;
                case PropType.Armor:
                    CharacterDBArmor.UpGradeArmor();
                    break;
            }
        }
    }

    [Serializable]
    public class CharaceterDBEquipment
    {
        public PropType PropType { set; get; }
        public int Class { set; get; }
        public CharaceterDBEquipment() { }

    }

    [Serializable]
    public class CharacterDBArms : CharaceterDBEquipment
    {
        public ArmsType ArmsID { set; get; }
        public CharacterDBArms() { }

        public void AddArms(CharacterArms arms)
        {
            PropType = arms.PropType;
            Class = arms.Class;
            ArmsID = arms.ArmsID; ;
        }
        public void UpGradeArms()
        {
            Class = Mathf.Min(CharaceterEquipment.MaxLevel, Class + 1);
        }
        public bool IsEquipmentArms() {
            return ArmsID != ArmsType.None;
        }
    }

    [Serializable]
    public class CharacterDBArmor : CharaceterDBEquipment
    {
        public ArmorType ArmorID { set; get; }
        public CharacterDBArmor() { }

        public void AddArmor(CharacterArmor armor)
        {
            PropType = armor.PropType;
            Class = armor.Class;
            ArmorID = armor.ArmorID; ;
        }
        public void UpGradeArmor()
        {
            Class = Mathf.Min(CharaceterEquipment.MaxLevel, Class + 1);
        }

        public bool IsEquipmentArmor() {
            return ArmorID != ArmorType.None;
        }
    }

    //[Serializable]
    //public class CharacterTaskDBData
    //{
    //    public int taskId;
    //    public SimGameTaskType taskType = SimGameTaskType.None;
    //    public int subType = -1;
    //    public string startTime;

    //    public CharacterTaskDBData() { }

    //    public CharacterTaskDBData(int taskId, SimGameTaskType taskType, int subType, string startTime)
    //    {
    //        this.taskId = taskId;
    //        this.taskType = taskType;
    //        this.subType = subType;
    //        this.startTime = startTime;
    //    }
    //}
}