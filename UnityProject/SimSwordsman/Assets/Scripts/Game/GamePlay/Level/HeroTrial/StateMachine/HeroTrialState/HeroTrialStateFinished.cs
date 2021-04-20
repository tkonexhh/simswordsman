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

            if (m_HeroTialMgr.FightGroup == null && m_HeroTialMgr.DbData.characterId != -1)
            {
                var ourCharacter = m_HeroTialMgr.SpawnOurCharacter(m_HeroTialMgr.DbData.characterId);
                m_HeroTialMgr.FightGroup = new FightGroup(1, ourCharacter, null);
            }
        }

        public override void Exit(IHeroTrialStateHander handler)
        {

        }

        public override void Execute(IHeroTrialStateHander handler, float dt)
        {

            
        }

    }
}
