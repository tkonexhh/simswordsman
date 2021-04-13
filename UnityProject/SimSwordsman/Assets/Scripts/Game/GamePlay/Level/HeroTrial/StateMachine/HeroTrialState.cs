using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public enum HeroTrialStateID
    {
        None,
        Idle,
        SelectingCharacter,
        Runing,
        Finished,
    }

    public class HeroTrialState : FSMState<IHeroTrialStateHander>
    {
        public HeroTrialStateID stateID
        {
            get;
            set;
        }

        public HeroTrialState(HeroTrialStateID stateEnum)
        {
            stateID = stateEnum;
        }

        public override void Enter(IHeroTrialStateHander handler)
        {
        }

        public override void Exit(IHeroTrialStateHander handler)
        {
        }

        public override void Execute(IHeroTrialStateHander handler, float dt)
        {
        }
    }
}
