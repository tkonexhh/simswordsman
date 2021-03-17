using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public enum RawMatStateID
    {
        None,
        NoPop,
        ShowPop,
    }

    public class RawMatState : FSMState<IRawMatStateHander>
    {
        public RawMatStateID stateID
        {
            get;
            set;
        }

        public RawMatState(RawMatStateID stateEnum)
        {
            stateID = stateEnum;
        }

        public override void Enter(IRawMatStateHander handler)
        {
        }

        public override void Exit(IRawMatStateHander handler)
        {
        }

        public override void Execute(IRawMatStateHander handler, float dt)
        {
        }
    }
}
