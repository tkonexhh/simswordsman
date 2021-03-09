using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    [DisallowMultipleComponent]
    public class ParticleAutoRecycle : MonoBehaviour
    {
        public float time = 5;
        private float m_Timer = 0;
        private bool m_Start;

        public void StartCD()
        {
            m_Timer = 0;
            m_Start = true;
        }

        private void Update()
        {
            if (!m_Start)
                return;

            m_Timer += Time.deltaTime;
            if (m_Timer >= time)
            {
                m_Start = false;
                GameObjectPoolMgr.S.Recycle(this.gameObject);
            }
        }
    }

}