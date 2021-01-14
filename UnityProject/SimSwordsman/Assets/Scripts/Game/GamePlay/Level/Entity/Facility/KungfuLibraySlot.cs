using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KungfuLibraySlot: BaseSlot
    {
        private Vector3 m_SlotPos;
        private CharacterController m_Character;
        private Vector3 vector3;

        private int slotLevel = 1;

        public KungfuLibraySlot(kungfuSoltDBData soltDBData):base(soltDBData)
        {
            //if (PracticeFieldState == SlotState.Practice)
            //    InitTimerUpdate();
        }

        public void Warp(KongfuLibraryLevelInfo kongfuLibrary)
        {
            slotState = SlotState.Free;
            UnlockLevel = kongfuLibrary.level;
        }

        public KungfuLibraySlot(KongfuLibraryLevelInfo item, int index,int unLock):base(item, index, unLock)
        {
            GameDataMgr.S.GetClanData().AddKungfuLibraryData(this);
        }
        public KungfuLibraySlot()
        {
        }
        public KungfuLibraySlot(Vector3 vector3)
        {
            this.vector3 = vector3;
        }

        public bool IsEmpty()
        {
            return m_Character == null;
        }

        public void OnCharacterEnter(CharacterController character)
        {
            m_Character = character;
        }

        public void OnCharacterLeave()
        {
            m_Character = null;
        }

        public Vector3 GetPosition()
        {
            return m_SlotPos;
        }
        public void SetCharacterItem(CharacterItem characterItem, SlotState slotState, FacilityType targetFacility)
        {

            //StartTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(curFacilityType, curLevel);

            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            switch (slotState)
            {
                case SlotState.Free:
                    characterController.SetState(CharacterStateID.Wander);
                    break;
                case SlotState.CopyScriptures:
                    StartTime = DateTime.Now.ToString();
                    CharacterItem = characterItem;
                    characterController.SetState(CharacterStateID.Reading, targetFacility);
                    break;
            }
            base.slotState = slotState;
            GameDataMgr.S.GetClanData().RefresKungfuDBData(this);
        }
     
        public void overAction()
        {
            if (CharacterItem != null)
            {
                AddExperience(CharacterItem);
                TrainingIsOver();
            }
        }
        public void TrainingIsOver()
        {
            SetCharacterItem(CharacterItem, SlotState.Free, FacilityType.None);
            CharacterItem = null;
            StartTime = string.Empty;
            GameDataMgr.S.GetClanData().KungfuTrainingIsOver(this);
            EventSystem.S.Send(EventID.OnKungfuSoltInfo, this);
        }
    }
}