using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ArenaClose : MonoBehaviour
    {
        [SerializeField] private Text m_TxtTime;


        private void Start()
        {
            //或得到下一个起始点时间
            UpdateTime();
        }

        private void Update()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            var nowTime = DateTime.Now;
            var nextTime = MainGameMgr.S.ArenaSystem.GetNextEnterTime();
            var delta = nextTime - nowTime;
            m_TxtTime.text = string.Format("倒计时:     <color=#9A5852>{0:D2}:{1:D2}:{2:D2}</color>", delta.Hours, delta.Minutes, delta.Seconds);
        }
    }

}