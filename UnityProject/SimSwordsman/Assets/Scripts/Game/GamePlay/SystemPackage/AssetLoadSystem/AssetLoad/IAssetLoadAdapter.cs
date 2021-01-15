using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameWish.Game
{
	public interface IAssetLoadAdapter
	{
        void InstantiateAsync(string name, Action<GameObject> callback);
        void LoadAssetAsync(string name, Action<GameObject> callback);
        void Release();
    }
	
}