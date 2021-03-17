using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class RawMatStateMachine : FSMStateMachine<IRawMatStateHander>
    {
        public RawMatStateMachine(IRawMatStateHander mgr) : base(mgr)
        {
            InitStateFactory();
        }

        public RawMatState currentGameplayState
        {
            get
            {
                return base.currentState as RawMatState;
            }
        }

        public RawMatStateID currentStateID
        {
            get
            {
                var state = currentGameplayState;
                if (state == null)
                {
                    return RawMatStateID.None;
                }
                return state.stateID;
            }
        }

        public RawMatState GetState(RawMatStateID id)
        {
            RawMatState state = (RawMatState)stateFactory.GetState<RawMatStateID>(id);
            return state;
        }

        private void InitStateFactory()
        {
            stateFactory = new RawMatStateFactory(false);
        }
    }
}
