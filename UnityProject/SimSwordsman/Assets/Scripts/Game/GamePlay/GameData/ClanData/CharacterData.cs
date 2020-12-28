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

        public void AddKonfuExp(CharacterItemDbData item, KungfuType kongfuType, int deltaExp)
        {
            CharacterKongfuDBData data = item.kongfuDatas.FirstOrDefault(i => i.kongfuType == kongfuType);
            if (data != null)
            {
                data.AddExp(deltaExp);
            }
        }

        public void AddKungfu(int id, CharacterKongfuDBData characterKongfu)
        {
            CharacterItemDbData characterItemDb = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItemDb != null)
                characterItemDb.LearnKungfu(characterKongfu);
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
        public CharaceterDBEquipmentData characeterDBEquipmentData = new CharaceterDBEquipmentData();
        public List<CharacterKongfuDBData> kongfuDatas = new List<CharacterKongfuDBData>();
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

        public void LearnKungfu(CharacterKongfuDBData characterKongfu)
        {
            kongfuDatas.Add(characterKongfu);
        }

        public void AddEquipmentItem(CharaceterEquipment characeterEquipment)
        {
            characeterDBEquipmentData.AddEquipment(characeterEquipment);
        }

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
    public class CharacterKongfuDBData
    {
        public KungfuType kongfuType;
        public int level;
        public int curExp;

        public CharacterKongfuDBData()
        {

        }
        public void AddExp(int deltaExp)
        {
            curExp += deltaExp;
        }
    }
    [Serializable]
    public class CharaceterDBEquipmentData
    {
        public CharacterDBArms CharacterDBArms { set; get; } = new CharacterDBArms();
        public CharacterDBArmor CharacterDBArmor { set; get; } = new CharacterDBArmor();

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
    }

    public class CharaceterDBEquipment
    {
        public PropType PropType { set; get; }
        public int Class { set; get; }
        public CharaceterDBEquipment() { }

    }

    public class CharacterDBArms : CharaceterDBEquipment
    {
        public Arms ArmsID { set; get; }
        public CharacterDBArms() { }

        public void AddArms(CharacterArms arms)
        {
            PropType = arms.PropType;
            Class = arms.Class;
            ArmsID = arms.ArmsID; ;
        }
    }
    public class CharacterDBArmor : CharaceterDBEquipment
    {
        public Armor ArmorID { set; get; }
        public CharacterDBArmor() { }

        public void AddArmor(CharacterArmor armor)
        {
            PropType = armor.PropType;
            Class = armor.Class;
            ArmorID = armor.ArmorID; ;
        }
    }
}