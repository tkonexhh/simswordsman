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
        public void AddEquipment(int chracterID, CharaceterEquipment characeterEquipment)
        {
            //MainGameMgr.S.InventoryMgr.RemoveItem(_equipmentItem, delta);
            //GameDataMgr.S.GetClanData().ownedCharacterData.AddEquipment(_character,_equipmentItem);
            CharacterItem character = characterList.Where(i => i.id == chracterID).FirstOrDefault();
            if (character != null)
            {
                character.AddEquipmentItem(characeterEquipment);
                GameDataMgr.S.GetClanData().AddEquipment(chracterID, characeterEquipment);
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
                EventSystem.S.Send(EventID.OnDiscipleReduce, id);
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
            if (characterItem != null)
                characterItem.LearnKungfu(kungfuItem);

        }
        public void AddCharacterLevel(int id, int level)
        {
            CharacterItem characterItem = characterList.Where(i => i.id == id).FirstOrDefault();
            if (characterItem != null)
                characterItem.UpgradeLevels(level);
        }
    }

    public class CharacterKongfuData
    {
        public int Index { set; get; }
        public KungfuLockState KungfuLockState { set; get; } = KungfuLockState.NotUnlocked;
        public CharacterKongfu CharacterKongfu { set; get; } 

        public CharacterKongfuData(int index)
        {
            Index = index;
        }

        public CharacterKongfuData(){}

        internal void Wrap(CharacterKongfuDBData i)
        {
            Index = i.index;
            KungfuLockState = i.kungfuLockState;
            if (KungfuLockState== KungfuLockState.Learned&&CharacterKongfu == null)
            {
                 CharacterKongfu = new CharacterKongfu();
                CharacterKongfu.Wrap(i);
            }
        }
    }

    //public class CharacterStateData
    //{
    //    public CharacterStateID characterState = CharacterStateID.Wander; // 弟子行为
    //    public CollectedObjType collectObjType;

    //    public CharacterStateData() { }
    //    public CharacterStateData(CharacterStateID stateId, CollectedObjType collectObjType = CollectedObjType.None)
    //    {
    //        this.characterState = stateId;
    //        this.collectObjType = collectObjType;
    //    }
    //}

    public class CharacterItem : IComparable
    {
        public const int MaxKungfuNumber = 4;

        public int id; // ID
        public int level = 1; // 等级
        public int stage = 1; // 段位
        public int curExp = 0; // 当前经验
        public string startTime; // 入门时间
        public CharacterQuality quality; // 品质
        public CharacterBehavior behavior; // 行为
        public CharacterStateID characterStateId = CharacterStateID.None; // 行为
        public float atkValue; // 武力值
        public string name; // 名字
        public string desc; // 详细信息
        public CharaceterEquipmentData characeterEquipmentData = new CharaceterEquipmentData();
        public Dictionary<int, CharacterKongfuData> kongfus = new Dictionary<int, CharacterKongfuData>();

        private CharacterStageInfoItem stageInfo;


        private CharacterItemDbData m_ItemDbData = null;

        public CharacterItem(CharacterQuality quality, string decs, string name)
        {
            this.quality = quality;
            this.name = name;
            this.desc = decs;
        }

        public CharacterItem()
        {
            for (int i = 0; i < MaxKungfuNumber; i++)
                kongfus.Add(i + 1, new CharacterKongfuData(i + 1));
        }

        public CharacterItem(int id)
        {
            this.id = id;
            level = 1;
            stage = 1;
        }

        /// <summary>
        /// 设置人物的状态
        /// </summary>
        /// <param name="stateId"></param>
        public void SetCharacterStateData(CharacterStateID stateId)
        {
            this.characterStateId = stateId;
            GameDataMgr.S.GetClanData().SetCharacterStateDBData(id, stateId);
        }

        public void SetCurTask(SimGameTask task)
        {
            GameDataMgr.S.GetClanData().SetCharacterTaskDBData(id, task);
        }

        public void Wrap(CharacterItemDbData itemDbData)
        {
            m_ItemDbData = itemDbData;
            startTime = itemDbData.startTime;
            id = itemDbData.id;
            name = itemDbData.name;
            level = itemDbData.level;
            stage = itemDbData.stage;
            curExp = itemDbData.curExp;
            quality = itemDbData.quality;
            characterStateId = itemDbData.characterStateId;

            atkValue = TDCharacterStageConfigTable.GetAtk(quality, stage, level);

            itemDbData.kongfuDatas.ForEach(i =>
            {
                CharacterKongfuData kongfu = new CharacterKongfuData();
                kongfu.Wrap(i);
                kongfus[i.index] = kongfu;
            });

            characeterEquipmentData.Wrap(itemDbData.characeterDBEquipmentData);

            stageInfo = TDCharacterStageConfigTable.GetStageInfo(quality, stage);
        }

        public bool IsFreeState()
        {
            if (characterStateId == CharacterStateID.Wander || characterStateId == CharacterStateID.EnterClan)
                return true;
            return false;
        }

        public void UpgradeLevels(int delta)
        {
            if (level < Define.CHARACTER_MAX_LEVEL)
            {
                level += delta;
                GameDataMgr.S.GetClanData().SetCharacterLevel(m_ItemDbData, level);

                int priviewStage = stage;
                this.stage = TDCharacterStageConfigTable.GetStage(quality, level);
                if (priviewStage != this.stage)
                {
                    GameDataMgr.S.GetClanData().SetCharacterStage(m_ItemDbData, stage);
                    int delte = stage - priviewStage;

                    for (int i = 1; i <= delte; i++)
                    {
                        UnlockContentConfigInfo unlockContentConfigInfo = TDCharacterStageConfigTable.GetUnlockForStage(quality, priviewStage + i);
                        switch (unlockContentConfigInfo.UnlockContent)
                        {
                            case UnlockContent.None:
                                break;
                            case UnlockContent.LearnKongfu:
                                kongfus[unlockContentConfigInfo.Count].KungfuLockState = KungfuLockState.NotLearning;
                                GameDataMgr.S.GetClanData().AddKungfu(id, kongfus[unlockContentConfigInfo.Count]);
                                break;
                            case UnlockContent.EquipArmor:
                                characeterEquipmentData.IsArmorUnlock = true;
                                GameDataMgr.S.GetClanData().UnlockEquip(id, UnlockContent.EquipArmor);
                                break;
                            case UnlockContent.EquipWeapon:
                                characeterEquipmentData.IsArmsUnlock = true;
                                GameDataMgr.S.GetClanData().UnlockEquip(id, UnlockContent.EquipWeapon);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public int GetEntryTime()
        {
            //startTime = "2020/12/27 16:22:50";
            DateTime dateTime ;
            DateTime.TryParse(startTime, out dateTime);
            if (dateTime!=null)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                return (int)timeSpan.TotalDays;
            }
            return 1;
        }

        public void LearnKungfu(KungfuItem kungfuItem)
        {
            foreach (var item in kongfus.Values)
            {
                if(item.KungfuLockState == KungfuLockState.Learned)
                {
                    if (item.CharacterKongfu.IsHaveKungfu(kungfuItem))
                        break;
                }
                if (item.KungfuLockState == KungfuLockState.NotLearning)
                {
                    item.CharacterKongfu = new CharacterKongfu(kungfuItem);
                    item.KungfuLockState = KungfuLockState.Learned;
                    GameDataMgr.S.GetClanData().AddKungfu(id, item);
                    break;
                }
            }
        }

   

        /// <summary>
        /// 增加经验
        /// </summary>
        /// <param name="deltaExp"></param>
        public void AddExp(int deltaExp)
        {
            curExp += deltaExp;

            while (true)
            {
                int upExp = MainGameMgr.S.CharacterMgr.GetExpLevelUpNeed(this);
                if (curExp > upExp)
                {
                    UpgradeLevels(1);
                    curExp -= upExp;
                }
                else
                    break;
            }
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

        public int GetCurTaskId()
        {
            return m_ItemDbData.taskId;
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
        public bool IsArmsUnlock { set; get; } = false;
        public CharacterArmor CharacterArmor { set; get; } = new CharacterArmor();
        public bool IsArmorUnlock { set; get; } = false;

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
            IsArmsUnlock = characeterDBEquipmentData.IsDBArmsUnlock;
            IsArmorUnlock = characeterDBEquipmentData.IsDBArmorUnlock;
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
        public abstract bool IsHaveEquip();
    }

    public class CharacterArms : CharaceterEquipment
    {
        public Arms ArmsID { set; get; }
        public CharacterArms() { }
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
            PropType = PropType.Arms;
            Name = equip.Name;
            Desc = equip.Desc;
            Addition = equip.GetBonusForClassID(Class);
            EquipQuality = equip.Quality;
        }

        public override void Wrap(CharaceterDBEquipment characeterDBEquipment)
        {
            CharacterDBArms characterDBArms = (CharacterDBArms)characeterDBEquipment;
            if (characterDBArms.ArmsID != Arms.None)
            {
                ArmsID = characterDBArms.ArmsID;
                Class = characterDBArms.Class;
                PropType = characterDBArms.PropType;
                RefreshInfo();
            }

        }

        public override bool IsHaveEquip()
        {
            if (ArmsID == Arms.None)
                return false;
            return true;
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
            if (characterDBArmor.ArmorID != Armor.None)
            {
                ArmorID = characterDBArmor.ArmorID;
                Class = characterDBArmor.Class;
                PropType = characterDBArmor.PropType;
                RefreshInfo();
            }
        }

        public override bool IsHaveEquip()
        {
            if (ArmorID == Armor.None)
                return false;
            return true;
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
            AddCharacterKongfu(kungfuItem);
        }

        public void AddCharacterKongfu(KungfuItem kungfuItem)
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

        public bool IsHaveKungfu(KungfuItem kungfuItem)
        {
            if (dbData.kongfuType == kungfuItem.KungfuType)
                return true;
            return false;
        }
    }

}