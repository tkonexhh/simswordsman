using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class HeroTrialStateMachine : FSMStateMachine<IHeroTrialStateHander>
    {
        public HeroTrialStateMachine(IHeroTrialStateHander mgr) : base(mgr)
        {
            InitStateFactory();
        }

        public HeroTrialState currentGameplayState
        {
            get
            {
                return base.currentState as HeroTrialState;
            }
        }

        public HeroTrialStateID currentStateID
        {
            get
            {
                var state = currentGameplayState;
                if (state == null)
                {
                    return HeroTrialStateID.None;
                }
                return state.stateID;
            }
        }

        public HeroTrialState GetState(HeroTrialStateID id)
        {
            HeroTrialState state = (HeroTrialState)stateFactory.GetState<HeroTrialStateID>(id);
            return state;
        }

        private void InitStateFactory()
        {
            stateFactory = new HeroTrialStateFactory(false);
        }
    }
}
