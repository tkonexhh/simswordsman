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
        //public void AddEquipment(CharacterItem _character, EquipmentItem _equipmentItem, int delta = 1)
        //{
        //    CharacterItemDbData character = characterList.Where(i => i.id == _character.id).FirstOrDefault();
        //    if (character != null)
        //        character.AddEquipmentItem(_equipmentItem);
        //}

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

        public void SetLevel(CharacterItemDbData item, int level)
        {
            item.level = level;
        }

        public void SetStage(CharacterItemDbData item, int stage)
        {
            item.stage = stage;
        }

        public void AddExp(CharacterItemDbData item, int deltaExp)
        {
            item.AddExp(deltaExp);
        }

        public void AddKonfuExp(CharacterItemDbData item, KongfuType kongfuType, int deltaExp)
        {
            CharacterKongfuData data = item.kongfuDatas.FirstOrDefault(i => i.kongfuType == kongfuType);
            if (data != null)
            {
                data.AddExp(deltaExp);
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
        //public List<EquipmentData> characterEquipmentDatas = new List<EquipmentData>();
        public List<CharacterKongfuData> kongfuDatas = new List<CharacterKongfuData>();
        public int curExp;

        public CharacterItemDbData()
        {

        }

        public CharacterItemDbData(CharacterItem item)
        {
            id = item.id;
            name = item.name;
            level = item.level;
            quality = item.quality;
            stage = item.stage;
        }

        public CharacterItemDbData(int id, CharacterQuality quality)
        {
            this.id = id;
            this.level = 1;
            this.quality = quality;
            this.stage = 1;
        }

        //public void AddEquipmentItem(EquipmentItem equipmentItem)
        //{
        //     ReturnEquipment(equipmentItem.PropType);

        //    if (!characterEquipmentDatas.Any(i => i.PropType == equipmentItem.PropType
        //    && i.EquipID == equipmentItem.EquipID && i.ClassID == equipmentItem.ClassID))
        //    {
        //        characterEquipmentDatas.Add(new EquipmentData(equipmentItem));

        //    }
        //}

        /// <summary>
        /// ¹é»¹×°±¸
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

        public void AddExp(int delta)
        {
            curExp += delta;
            curExp = Math.Max(0, curExp);
        }
    }

    [Serializable]
    public class CharacterKongfuData
    {
        public KongfuType kongfuType;
        public int level;
        public int curExp;

        public CharacterKongfuData()
        {

        }

        public void AddExp(int deltaExp)
        {
            curExp += deltaExp;
        }
    }


}