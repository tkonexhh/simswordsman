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
        Fish, // Óã
        Chicken, // ¼¦
        Bear, // ĞÜ
        Boar, // Ò°Öí
        Snake, // Éß
        Deer, // Â¹
        WuWood, // ÎÚÄ¾
        SilverWood, // ÒøÄ¾
        QingRock, // ÇàÑÒ
        CloudRock, // ÔÆÑÒ
        Vine, // ×ÏÌÙ
        Iron, // ºÚÌú
        Ganoderma, // ÁéÖ¥
        Well, // ¾®Ë®
    }
}