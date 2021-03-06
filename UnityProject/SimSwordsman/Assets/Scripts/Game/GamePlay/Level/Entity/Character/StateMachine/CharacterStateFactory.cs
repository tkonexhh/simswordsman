using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class CharacterStateFactory : FSMStateFactory<ICharacterStateHander>
    {
        public CharacterStateFactory(bool alwaysCreate) : base(alwaysCreate)
        {
            InitStateCreator();
        }

        private void InitStateCreator()
        {
            RegisterPlayerState(CharacterStateID.Wander, new CharacterStateWander(CharacterStateID.Wander));
            RegisterPlayerState(CharacterStateID.EnterClan, new CharacterStateEnterClan(CharacterStateID.EnterClan));
            RegisterPlayerState(CharacterStateID.Battle, new CharacterStateBattle(CharacterStateID.Battle));
            RegisterPlayerState(CharacterStateID.Practice, new CharacterStatePractice(CharacterStateID.Practice));
            RegisterPlayerState(CharacterStateID.CollectRes, new CharacterStateCollectRes(CharacterStateID.CollectRes));
            //RegisterPlayerState(CharacterStateID.GoOutsideForTaskBattle, new CharacterStateGoOutsideForTaskBattle(CharacterStateID.GoOutsideForTaskBattle));
            RegisterPlayerState(CharacterStateID.Reading, new CharacterStateReading(CharacterStateID.Reading));
            RegisterPlayerState(CharacterStateID.Working, new CharacterStateWorking(CharacterStateID.Working));
            RegisterPlayerState(CharacterStateID.Deliver, new CharacterStateDeliver(CharacterStateID.Deliver));
        }

        private void RegisterPlayerState(CharacterStateID id, CharacterState state)
        {
            state.stateID = id;
            RegisterState(id, state);
        }

        //public CharacterState GetState(CharacterStateID id)
        //{
        //    CharacterState state = (CharacterState)GetState<CharacterStateID>(id);
        //    return state;
        //}
    }
}
