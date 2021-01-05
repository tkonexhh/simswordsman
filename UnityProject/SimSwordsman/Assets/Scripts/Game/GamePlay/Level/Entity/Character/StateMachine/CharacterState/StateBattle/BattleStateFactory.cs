using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class BattleStateFactory : FSMStateFactory<IBattleStateHander>
    {
        public BattleStateFactory(bool alwaysCreate) : base(alwaysCreate)
        {
            InitStateCreator();
        }

        private void InitStateCreator()
        {
            RegisterPlayerState(BattleStateID.Idle, new StateBattleIdle(BattleStateID.Idle));
            RegisterPlayerState(BattleStateID.Attack, new StateBattleAttack(BattleStateID.Attack));
            RegisterPlayerState(BattleStateID.Attacked, new StateBattleAttacked(BattleStateID.Attacked));
            RegisterPlayerState(BattleStateID.Move, new StateBattleMove(BattleStateID.Move));
            RegisterPlayerState(BattleStateID.Dead, new StateBattleDead(BattleStateID.Dead));
            RegisterPlayerState(BattleStateID.Wait, new StateBattleWait(BattleStateID.Wait));
        }

        private void RegisterPlayerState(BattleStateID id, BattleState state)
        {
            state.stateID = id;
            RegisterState(id, state);
        }
    }
}
