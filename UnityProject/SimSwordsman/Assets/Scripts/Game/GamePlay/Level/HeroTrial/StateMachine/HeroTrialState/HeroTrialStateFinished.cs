using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class HeroTrialStateFinished : HeroTrialState
    {
        private HeroTrialMgr m_HeroTialMgr = null;

        public HeroTrialStateFinished(HeroTrialStateID stateEnum) : base(stateEnum)
        {
            
        }

        public override void Enter(IHeroTrialStateHander handler)
        {
            m_HeroTialMgr = handler.GetHeroTrialMgr();

        }

        public override void Exit(IHeroTrialStateHander handler)
        {

        }

        public override void Execute(IHeroTrialStateHander handler, float dt)
        {

            
        }

    }
}
