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

        public void StartCD()
        {
            Timer.S.Post2Scale(i =>
            {
                GameObjectPoolMgr.S.Recycle(this.gameObject);
            }, time);
        }
    }

}