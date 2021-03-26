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
        /// δ��ʼ
        /// </summary>
        NotStart,
        /// <summary>
        /// ����ɣ�����ȡ
        /// </summary>
        Unclaimed,
        /// <summary>
        /// ����ִ��
        /// </summary>
        Running,

        /// <summary>
        /// �ѽ���
        /// </summary>
        Finished,
    }
}