using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public enum CharacterStateID
    {
        None,
        EnterClan,
        Wander,
        Battle,
        Practice,
        CollectRes,
        GoOutside,
    }

    public class CharacterState : FSMState<ICharacterStateHander>
    {
        public CharacterStateID stateID
        {
            get;
            set;
        }

        public CharacterState(CharacterStateID stateEnum)
        {
            stateID = stateEnum;
        }

        public override void Enter(ICharacterStateHander handler)
        {
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
        }
    }
}
