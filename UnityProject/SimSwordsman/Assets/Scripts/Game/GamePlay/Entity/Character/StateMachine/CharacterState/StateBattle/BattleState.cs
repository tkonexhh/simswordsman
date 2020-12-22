using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public enum BattleStateID
    {
        None,
        Idle,
        Attack,
        Attacked,
        Dead,
        Move,
        Wait
    }

    public class BattleState : FSMState<IBattleStateHander>
    {
        public BattleStateID stateID
        {
            get;
            set;
        }

        public BattleState(BattleStateID stateEnum)
        {
            stateID = stateEnum;
        }

        public override void Enter(IBattleStateHander handler)
        {
        }

        public override void Exit(IBattleStateHander handler)
        {
        }

        public override void Execute(IBattleStateHander handler, float dt)
        {
        }
    }
}
