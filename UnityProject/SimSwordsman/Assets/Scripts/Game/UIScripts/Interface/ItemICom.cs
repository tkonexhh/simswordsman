using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public interface ItemICom
    {
        // Start is called before the first frame update
        void OnInit<T>(T t, Action action = null, params object[] obj);
        void SetButtonEvent(Action<object> action);
    }

}