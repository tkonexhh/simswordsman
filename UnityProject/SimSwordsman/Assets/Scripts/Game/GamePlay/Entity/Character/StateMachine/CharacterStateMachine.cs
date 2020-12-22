using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class CharacterStateMachine : FSMStateMachine<ICharacterStateHander>
    {
        public CharacterStateMachine(ICharacterStateHander mgr) : base(mgr)
        {
            InitStateFactory();
        }

        public CharacterState currentGameplayState
        {
            get
            {
                return base.currentState as CharacterState;
            }
        }

        public CharacterStateID currentStateID
        {
            get
            {
                var state = currentGameplayState;
                if (state == null)
                {
                    return CharacterStateID.None;
                }
                return state.stateID;
            }
        }

        public CharacterState GetState(CharacterStateID id)
        {
            CharacterState state = (CharacterState)stateFactory.GetState<CharacterStateID>(id);
            return state;
        }

        private void InitStateFactory()
        {
            stateFactory = new CharacterStateFactory(false);
        }
    }
}
