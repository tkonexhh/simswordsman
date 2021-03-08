using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class CharacterModel : IEntityData
    {
        protected CharacterController m_Controller = null;
        protected int m_Id = 0;
        protected int m_Level = 1;
        protected double m_Hp = 0;
        protected double m_Atk = 1;
        protected float m_MoveSpeed = 1f;
        private CharacterItem m_CharacterItem = null;

        public int Id { get => m_Id; }
        public int Level { get => m_Level; }
        //public float Hp { get => m_Hp; }
        //public float Atk { get => m_Atk; }
        public float MoveSpeed { get => m_MoveSpeed; }

        public CharacterModel(int id, CharacterController character)
        {
            m_Controller = character;
            m_Id = id;

            m_CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(m_Id);

            //SetAtk(GetAtk());
            //SetHp(GetAtk());
        }

        public void Init()
        {
        }

        public void AddHp(double delta)
        {
            m_Hp += delta;
            //m_Hp = Mathf.Max(m_Hp, 0);
            if (m_Hp < 0)
                m_Hp = 0;
        }

        public void SetAtk(float atk)
        {
            m_Atk = atk;
        }

        public void SetDataState(CharacterStateID stateId, FacilityType targetFacilityType)
        {
            m_CharacterItem?.SetCharacterStateData(stateId, targetFacilityType);
        }

        public void SetCurTask(SimGameTask simGameTask)
        {
            m_CharacterItem?.SetCurTask(simGameTask);
        }
        public void ClearCurTask(SimGameTask simGameTask)
        {
            m_CharacterItem?.ClearCurTask(simGameTask);
        }
        public int GetExp()
        {
            return m_CharacterItem.curExp;
        }

        public void AddExp(int deltaExp)
        {
            m_CharacterItem?.AddCharacterExp(deltaExp);
        }

        public void SetHp(double hp)
        {
            m_Hp = hp;
        }

        public double GetHp()
        {
            return m_Hp;
        }

        public double GetAtk()
        {
            return m_Atk;
        }

        public float GetBaseAtkValue()
        {
            
            if (m_Controller.CharacterCamp == CharacterCamp.OurCamp)
            {
                float armorAtkEnhanceRatio = m_CharacterItem.GetArmorAtkEnhanceRatio();
                if (armorAtkEnhanceRatio == 0) // 暂时这么解决，后期优化从存档中解决
                {
                    armorAtkEnhanceRatio = 1;
                }

                float armsAtkEnhanceRatio = m_CharacterItem.GetArmsAtkEnhanceRatio();
                if (armsAtkEnhanceRatio == 0)
                {
                    armsAtkEnhanceRatio = 1;
                }

                Log.i("Character: " + m_Id + " armor atk ratio: " + armorAtkEnhanceRatio + " arms atk ratio: " + armorAtkEnhanceRatio);
                float value = m_CharacterItem.atkValue * armorAtkEnhanceRatio * armorAtkEnhanceRatio;
                return value;
            }
            else
                return 0;
        }

        public int GetCurTaskId()
        {
            return m_CharacterItem.GetCurTaskId();
        }

        public int GetHeadId()
        {
            return m_CharacterItem.headId;
        }

        public int GetExpLevelUpNeed()
        {
            return TDCharacterStageConfigTable.GetExpLevelUpNeed(m_CharacterItem);
        }

        public FacilityType GetTargetFacilityType()
        {
            if (m_CharacterItem == null)
                return FacilityType.None;
            return m_CharacterItem.GetTargetFacilityType();
        }

        public CollectedObjType GetCollectedObjType()
        {
            return m_CharacterItem.collectedObjType;
        }

        public void SetCollectedObjType(CollectedObjType collectedObjType)
        {
            m_CharacterItem.collectedObjType = collectedObjType;
        }

        public void AddKongfuExp(int deltaExp)
        {
            //m_CharacterItem.kongfus.ForEach(i => 
            //{
            //    m_CharacterItem.AddKongfuExp(i.dbData.kongfuType, deltaExp);
            //});
        }

        public int GetKongfuCount()
        {
            return m_CharacterItem.kongfus.Count;
        }

        public List<KungfuType> GetKongfuTypeList()
        {
            return m_CharacterItem.kongfus.Values.Where(i => i.GetKungfuType() != KungfuType.None).Select(i => i.GetKungfuType()).ToList();
        }

        /// <summary>
        /// 某一功夫功夫的权重比例增加经验
        /// </summary>
        /// <returns></returns>
        public void DistributionKungfuExp(int expValue)
        {
            int allWeight = 0;
            foreach (var item in m_CharacterItem.kongfus.Values)
            {
                KungfuWeightConfig kungfuWeight = TDKongfuStageConfigTable.GetKungfuweight(item.GetKungfuLevel());
                if (kungfuWeight != null)
                    allWeight += kungfuWeight.Weight;
            }

            foreach (var item in m_CharacterItem.kongfus.Values)
            {
                KungfuWeightConfig config = TDKongfuStageConfigTable.GetKungfuweight(item.GetKungfuLevel());
                if (config != null && config.Weight != 0)
                {
                    int ratio = config.Weight / allWeight;
                    m_CharacterItem.AddKongfuExp(item, ratio * expValue);
                }
            }
        }

        public bool IsIdle()
        {
            return m_CharacterItem.IsFreeState();
        }

        public bool IsWoman() {
            return m_CharacterItem.IsWoman();
        }
    }
}