using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class RawMatStateFactory : FSMStateFactory<IRawMatStateHander>
    {
        public RawMatStateFactory(bool alwaysCreate) : base(alwaysCreate)
        {
            InitStateCreator();
        }

        private void InitStateCreator()
        {
            RegisterPlayerState(RawMatStateID.Idle, new RawMatStateIdle(RawMatStateID.Idle));
            RegisterPlayerState(RawMatStateID.Locked, new RawMatStateLocked(RawMatStateID.Locked));
            RegisterPlayerState(RawMatStateID.Working, new RawMatStateWorking(RawMatStateID.Working));
            RegisterPlayerState(RawMatStateID.BubbleShowing, new RawMatStateBubbleShowing(RawMatStateID.BubbleShowing));

        }

        private void RegisterPlayerState(RawMatStateID id, RawMatState state)
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
