using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public enum SimGameTaskTriggerType
    {
        Main,
        Common,
        Daily
    }

	public enum SimGameTaskType
	{
        None,
	    Collect,
        Battle,
        Progress,
	}

    public enum CollectedObjType
    {
        Fish,
        Chicken,
        Bear,
        Boar,
        Snake,
        Deer,
        WuWood,
        SilverWood,
        QingRock,
        CloudRock,
        Vine,
        Iron,
        Ganoderma,
        Well,
    }
}