using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{


    public class CharacterModel : IEntityData
    {
        protected CharacterController m_Controller = null;
        protected int m_Id = 0;
        protected int m_Level = 1;
        protected float m_Hp = 10;
        protected float m_Atk = 1;
        protected float m_MoveSpeed = 1f;
        private CharacterItem m_CharacterItem = null;

        public int Id { get => m_Id; }
        public int Level { get => m_Level;}
        public float Hp { get => m_Hp; }
        public float Atk { get => m_Atk; }
        public float MoveSpeed { get => m_MoveSpeed; }

        public CharacterModel(int id, CharacterController character)
        {
            m_Controller = character;
            m_Id = id;

            if (character.CharacterCamp == CharacterCamp.OurCamp)
            {
                m_CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(id);
                m_Atk = m_CharacterItem.atkValue * m_CharacterItem.GetEquipmentAtkEnhanceRatio();
                m_Hp = m_Atk;
            }
        }

        public void Init()
        {
        }

        public void AddHp(float delta)
        {
            m_Hp += delta;
            m_Hp = Mathf.Max(m_Hp, 0);
        }

        public void SetAtk(int atk)
        {
            m_Atk = atk;
            m_Hp = m_Atk;
        }

        public void SetDataState(CharacterStateID stateId)
        {
            m_CharacterItem?.SetCharacterStateData(stateId);
        }

        public void SetCurTask(SimGameTask simGameTask)
        {
            m_CharacterItem?.SetCurTask(simGameTask);
        }

        public int GetExp()
        {
            return m_CharacterItem.curExp;
        }

        public void AddExp(int deltaExp)
        {
            m_CharacterItem?.AddExp(deltaExp);
        }

        public int GetCurTaskId()
        {
            return m_CharacterItem.GetCurTaskId();
        }

        public int GetExpLevelUpNeed()
        {
            return TDCharacterStageConfigTable.GetExpLevelUpNeed(m_CharacterItem);
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
    }

}