using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class HeroTrialStateFactory : FSMStateFactory<IHeroTrialStateHander>
    {
        public HeroTrialStateFactory(bool alwaysCreate) : base(alwaysCreate)
        {
            InitStateCreator();
        }

        private void InitStateCreator()
        {
            RegisterPlayerState(HeroTrialStateID.Idle, new HeroTrialStateIdle(HeroTrialStateID.Idle));

        }

        private void RegisterPlayerState(HeroTrialStateID id, HeroTrialState state)
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
