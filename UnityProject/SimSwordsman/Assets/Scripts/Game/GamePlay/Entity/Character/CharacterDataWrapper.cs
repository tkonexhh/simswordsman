using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Qarth;

namespace GameWish.Game
{
    public class CharacterDataWrapper
    {
        public List<CharacterItem> characterList = new List<CharacterItem>();

        private CharacterDbData m_DbData = null;

        public void Wrap(CharacterDbData dbData)
        {
            m_DbData = dbData;

            dbData.characterList.ForEach(i =>
            {
                CharacterItem item = new CharacterItem();
                item.Wrap(i);

                characterList.Add(item);
            });
        }
        /// <summary>
        /// 给弟子增加装备
        /// </summary>
        /// <param name="_character"></param>
        /// <param name="_equipmentItem"></param>
        public void AddEquipment(int chracterID , CharaceterEquipment characeterEquipment)
        {
            //MainGameMgr.S.InventoryMgr.RemoveItem(_equipmentItem, delta);
            //GameDataMgr.S.GetClanData().ownedCharacterData.AddEquipment(_character,_equipmentItem);
            CharacterItem character =characterList.Where(i => i.id == chracterID).FirstOrDefault();
            if (character != null)
            {
                character.AddEquipmentItem(characeterEquipment);
                GameDataMgr.S.GetClanData().AddEquipment(chracterID,characeterEquipment);
                EventSystem.S.Send(EventID.OnSelectedEquipSuccess);
            }
        }
        /// <summary>
        /// 获取装备的加成
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        //public float GetDiscipleEquipBonus(EquipmentItem equipment)
        //{
        //    return TDEquipmentConfigTable.GetBonus(equipment);
        //}

        /// <summary>
        /// 增加弟子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quality"></param>
        public void AddCharacter(int id, CharacterQuality quality)
        {
            CharacterItemDbData itemDbData = new CharacterItemDbData(id, quality);

            CharacterItem item = new CharacterItem();
            item.Wrap(itemDbData);

            characterList.Add(item);

            m_DbData.AddCharacter(itemDbData);
        }

        public void AddCharacter(CharacterItem character)
        {

            CharacterItemDbData itemDbData = new CharacterItemDbData(character);

            CharacterItem item = new CharacterItem();
            item.Wrap(itemDbData);

            characterList.Add(item);

            m_DbData.AddCharacter(itemDbData);
        }

        public void LearnKungfu()
        {
          
        }

        public void RemoveCharacter(int id)
        {
            CharacterItem item = characterList.FirstOrDefault(i => i.id == id);
            if (item != null)
            {
                characterList.Remove(item);
            }

            m_DbData.RemoveCharacter(id);
        }

        public CharacterItem GetCharacterItem(int id)
        {
            CharacterItem item = characterList.FirstOrDefault(i => i.id == id);
            return item;
        }

        public void AddKungfu(int id, KungfuItem kungfuItem)
        {
            CharacterItem characterItem = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItem!=null)
                characterItem.LearnKungfu(kungfuItem);

        }
        public void AddCharacterLevel(int id, int level)
        {
            CharacterItem characterItem = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItem != null)
                 characterItem.UpgradeLevels(level);
        }
    }

    public class CharacterItem : IComparable
    {
        public int id; // ID
        public int level = 1; // 等级
        public int stage = 1; // 段位
        public int curExp = 0; // 当前经验
        public CharacterQuality quality; // 品质
        public CharacterBehavior behavior; // 品质
        public float atkValue; // 武力值
        public string startTime; // 入门时间
        public string name; // 名字
        public string desc; // 详细信息
        public CharaceterEquipmentData characeterEquipmentData = new CharaceterEquipmentData();
        public List<CharacterKongfu> kongfus = new List<CharacterKongfu>();

        private CharacterStageInfo stageInfo;
        private CharacterQualityConfigInfo qualityInfo;

        private CharacterItemDbData m_ItemDbData = null;

        public CharacterItem(CharacterQuality quality, string decs, string name)
        {
            this.quality = quality;
            this.name = name;
            this.desc = decs;
        }

        public CharacterItem() { }

        public CharacterItem(int id)
        {
            this.id = id;
            level = 1;
            stage = 1;
        }

        public CharacterQualityConfigInfo GetCharacterQualityConfigInfo()
        {
            return qualityInfo;
        }

        public void Wrap(CharacterItemDbData itemDbData)
        {
            m_ItemDbData = itemDbData;

            id = itemDbData.id;
            name = itemDbData.name;
            level = itemDbData.level;
            stage = itemDbData.stage;
            curExp = itemDbData.curExp;
            quality = itemDbData.quality;
            atkValue = TDCharacterStageConfigTable.GetAtk(stage, level);

            itemDbData.kongfuDatas.ForEach(i =>
            {
                CharacterKongfu kongfu = new CharacterKongfu();
                kongfu.Wrap(i);

                kongfus.Add(kongfu);
            });

            characeterEquipmentData.Wrap(itemDbData.characeterDBEquipmentData);

            stageInfo = TDCharacterStageConfigTable.GetStageInfo(stage);
            qualityInfo = TDCharacterQualityConfigTable.GetQualityConfigInfo(quality);
        }

        public void UpgradeLevel()
        {
            int maxLevel = CharacterMgr.GetMaxLevel(quality);
            if (level < maxLevel)
            {
                level += 1;
                GameDataMgr.S.GetClanData().SetCharacterLevel(m_ItemDbData, level);

                int stage = TDCharacterStageConfigTable.GetStage(level);
                if (stage != this.stage)
                {
                    GameDataMgr.S.GetClanData().SetCharacterStage(m_ItemDbData, stage);
                }
            }
        }

        public void UpgradeLevels(int delta)
        {
            int maxLevel = CharacterMgr.GetMaxLevel(quality);
            if (level < maxLevel)
            {
                level += delta;
                GameDataMgr.S.GetClanData().SetCharacterLevel(m_ItemDbData, level);

                int stage = TDCharacterStageConfigTable.GetStage(level);
                if (stage != this.stage)
                {
                    GameDataMgr.S.GetClanData().SetCharacterStage(m_ItemDbData, stage);
                }
            }
        }

        public void LearnKungfu(KungfuItem kungfuItem)
        {
            for (int i = 0; i < qualityInfo.learnKonfuNeedLevelList.Count; i++)
            {
                if (level >= qualityInfo.learnKonfuNeedLevelList[i])
                {
                    if (kongfus.Count > i && !string.IsNullOrEmpty(kongfus[i].name))
                        continue;
                    else
                    {
                        if (CheackLeardKungfu(kungfuItem))
                            return;
                        kongfus.Add(new CharacterKongfu(kungfuItem));
                        GameDataMgr.S.GetClanData().AddKungfu(id, kongfus[i].dbData);
                    }
                }
            }
        }

        private bool CheackLeardKungfu(KungfuItem kungfuItem)
        {
            foreach (var item in kongfus)
            {
                if (item.dbData.kongfuType == kungfuItem.KungfuType)
                    return true;
                continue;
            }
            return false;
        }

        public void AddExp(int deltaExp)
        {
            curExp += deltaExp;

            GameDataMgr.S.GetClanData().AddCharacterExp(m_ItemDbData, deltaExp);
        }

        public void AddKongfuExp(KungfuType kongfuType, int deltaExp)
        {
            GameDataMgr.S.GetClanData().AddCharacterKongfuExp(m_ItemDbData, kongfuType, deltaExp);
        }
        /// <summary>
        /// 获取装备
        /// </summary>
        /// <param name="equipmentItem"></param>
        public void AddEquipmentItem(CharaceterEquipment characeterEquipment)
        {
            characeterEquipmentData.AddEquipment(characeterEquipment);
        }

        /// <summary>
        /// 归还装备
        /// </summary>
        /// <param name="equipType"></param>
        //public EquipmentItem ReturnEquipment(PropType equipType)
        //{
        //    EquipmentItem equipment = characterEquipment.Where(i => i.PropType == equipType).FirstOrDefault();
        //    if (equipment != null)
        //    {
        //        characterEquipment.Remove(equipment);
        //        return equipment;
        //    }
        //    return null;

        //}

        /// <summary>
        /// Get atk enhance ratio of all equipments
        /// </summary>
        /// <returns></returns>
        public float GetEquipmentAtkEnhanceRatio()
        {
            float ratio = 1;
            //foreach (EquipmentItem item in characterEquipment)
            //{
            //    float bonus = TDEquipmentConfigTable.GetBonus(item);
            //    ratio += bonus;
            //}

            return ratio;
        }

        public int CompareTo(object obj)
        {
            int result;
            try
            {
                CharacterItem info = obj as CharacterItem;
                if (id > info.id)
                    result = 1;
                else if (id == info.id)
                    result = 0;
                else
                    result = -1;
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }

    public class CharaceterEquipmentData
    {
        public CharacterArms CharacterArms { set; get; } = new CharacterArms();
        public CharacterArmor CharacterArmor { set; get; } = new CharacterArmor();

        public void AddEquipment(CharaceterEquipment characeterEquipment)
        {
            switch (characeterEquipment.PropType)
            {
                case PropType.Arms:
                    CharacterArms.AddArms((CharacterArms)characeterEquipment);
                    break;
                case PropType.Armor:
                    CharacterArmor.AddArmor((CharacterArmor)characeterEquipment);
                    break;
            }
        }

        public void Wrap(CharaceterDBEquipmentData characeterDBEquipmentData)
        {
            CharacterArms.Wrap(characeterDBEquipmentData.CharacterDBArms);
            CharacterArmor.Wrap(characeterDBEquipmentData.CharacterDBArmor);
        }
    }

    public abstract class CharaceterEquipment
    {
        public PropType PropType { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }
        public int Class { set; get; }
        public float Addition { set; get; }
        public EquipQuailty EquipQuality { set; get; }

        public abstract void Wrap(CharaceterDBEquipment characeterDBEquipment);
        public abstract void RefreshInfo();
    }

    public class CharacterArms : CharaceterEquipment
    {
        public Arms ArmsID {set;get;}
        public CharacterArms() {}
        public CharacterArms(Arms arms)
        {
            ArmsID = arms;
            Class = 1;
            RefreshInfo();
        }


        public void AddArms(CharacterArms characterArms)
        {
            if (ArmsID == characterArms.ArmsID)
                return;

            Class = characterArms.Class;
            ArmsID = characterArms.ArmsID;
            RefreshInfo();
        }

        public override void RefreshInfo()
        {
            Equipment equip = TDArmsConfigTable.GetEquipmentInfo(ArmsID);
            PropType = PropType.Armor;
            Name = equip.Name;
            Desc = equip.Desc;
            Addition = equip.GetBonusForClassID(Class);
            EquipQuality = equip.Quality;
        }

        public override void Wrap(CharaceterDBEquipment characeterDBEquipment)
        {
            CharacterDBArms characterDBArms = (CharacterDBArms)characeterDBEquipment;
            if (characterDBArms.ArmsID!= Arms.None)
            {
                ArmsID = characterDBArms.ArmsID;
                Class = characterDBArms.Class;
                PropType = characterDBArms.PropType;
                RefreshInfo();
            }
        
        }
    }
    public class CharacterArmor : CharaceterEquipment
    {
        public Armor ArmorID { set; get; }

        public CharacterArmor() { }
        public CharacterArmor(Armor armor)
        {
            ArmorID = armor;
            Class = 1;
            RefreshInfo();
        }
        public void AddArmor(CharacterArmor characterArmor)
        {
            if (ArmorID == characterArmor.ArmorID)
                return;
            ArmorID = characterArmor.ArmorID;
            RefreshInfo();
        }

        public override void RefreshInfo()
        {
            Equipment equip = TDArmorConfigTable.GetEquipmentInfo(ArmorID);
            PropType = PropType.Armor;
            Name = equip.Name;
            Desc = equip.Desc;
            Addition = equip.GetBonusForClassID(Class);
            EquipQuality = equip.Quality;
        }

        public override void Wrap(CharaceterDBEquipment characeterDBEquipment)
        {
            CharacterDBArmor characterDBArmor = (CharacterDBArmor)characeterDBEquipment;
            if (characterDBArmor.ArmorID!= Armor.None)
            {
                ArmorID = characterDBArmor.ArmorID;
                Class = characterDBArmor.Class;
                PropType = characterDBArmor.PropType;
                RefreshInfo();
            }
        }
    }


    public class CharacterKongfu
    {
        public CharacterKongfuDBData dbData;
        public string name;
        public string desc;
        public float atkScale = 1f;

        public void Wrap(CharacterKongfuDBData dbData)
        {
            this.dbData = dbData;
            RefeshKungfuInfo();
        }
        public CharacterKongfu()
        {

        }
        public CharacterKongfu(KungfuItem kungfuItem)
        {
            dbData = new CharacterKongfuDBData();
            dbData.kongfuType = kungfuItem.KungfuType;
            dbData.level = 1;
            dbData.curExp = 1;
            name = kungfuItem.Name;
            desc = kungfuItem.Desc;
            atkScale = TDKongfuConfigTable.GetAddition(dbData.kongfuType, dbData.level);
        }

        private void RefeshKungfuInfo()
        {
            KungfuConfigInfo kungfuConfig = TDKongfuConfigTable.GetKungfuConfigInfo(dbData.kongfuType);
            name = kungfuConfig.Name;
            desc = kungfuConfig.Desc;
            atkScale = TDKongfuConfigTable.GetAddition(dbData.kongfuType, dbData.level);
        }
    }

}