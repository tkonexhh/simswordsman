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
        /// ����ִ��
        /// </summary>
        Running,
        /// <summary>
        /// ����ɣ�����ȡ
        /// </summary>
        Unclaimed,
        /// <summary>
        /// �ѽ���
        /// </summary>
        Finished,            
    }
}