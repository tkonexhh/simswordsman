using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public enum TaskState
    {
        None,
        /// <summary>
        /// 未开始
        /// </summary>
        NotStart,
        /// <summary>
        /// 正在执行
        /// </summary>
        Running,
        /// <summary>
        /// 已完成，待领取
        /// </summary>
        Unclaimed,
        /// <summary>
        /// 已结束
        /// </summary>
        Finished,            
    }
}