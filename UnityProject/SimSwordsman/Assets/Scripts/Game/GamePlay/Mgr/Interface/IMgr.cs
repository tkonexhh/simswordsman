using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public interface IMgr
    {
        void OnInit();
        void OnUpdate();
        void OnDestroyed();
    }

}