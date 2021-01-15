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
        /// <summary>
        /// 进入设施
        /// </summary>
        EnterClan,
        /// <summary>
        /// 漫游
        /// </summary>
        Wander,
        /// <summary>
        /// 战斗
        /// </summary>
        Battle,
        /// <summary>
        /// 训练
        /// </summary>
        Practice,
        /// <summary>
        /// 收集资源
        /// </summary>
        CollectRes,
        /// <summary>
        /// 外出
        /// </summary>
        GoOutsideForTaskBattle,
        /// <summary>
        /// 读经书
        /// </summary>
        Reading,
        /// <summary>
        /// 干活
        /// </summary>
        Working,
        /// <summary>
        /// 巡逻
        /// </summary>
        Patrol,
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
