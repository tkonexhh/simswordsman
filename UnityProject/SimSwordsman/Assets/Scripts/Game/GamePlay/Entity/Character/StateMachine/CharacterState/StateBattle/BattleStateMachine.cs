using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class BattleStateMachine : FSMStateMachine<IBattleStateHander>
    {
        public BattleStateMachine(IBattleStateHander mgr) : base(mgr)
        {
            InitStateFactory();
        }

        public BattleState currentGameplayState
        {
            get
            {
                return base.currentState as BattleState;
            }
        }

        public BattleStateID currentStateID
        {
            get
            {
                var state = currentGameplayState;
                if (state == null)
                {
                    return BattleStateID.None;
                }
                return state.stateID;
            }
        }

        private void InitStateFactory()
        {
            stateFactory = new BattleStateFactory(false);
        }
    }
}
