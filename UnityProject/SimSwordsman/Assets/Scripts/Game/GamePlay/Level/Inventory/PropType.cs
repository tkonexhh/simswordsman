using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // Equal to id in table
	public enum PropType
	{
        None                             = 0,
        Arms                             = 1,        //ÎäÆ÷
        Armor                            = 2,        //îø¼×
        RawMaterial                      = 3,        //Ô­²ÄÁÏ
        Kungfu                           = 4,        //Ô­²ÄÁÏ
    }

    public enum KungfuType
    {
        None,
        XiangLong18Zhang                 = 1,        //½µÁúÊ®°ËÕÆ
        XiangLong19Zhang                 = 2,
        WuLinMiJi                        = 3,        //ÎäÁÖÃØ¼®
        LingBoWeiBu                      = 4,        //Áè²¨Î¢²½
        JiBenQuanJiao                    = 5,        //»ù±¾È­½Å
    }

    public enum RawMaterial
    { 
        None                             = 0,
        Malachite                        = 1,        //¿×È¸Ê¯             
        RedAgate                         = 2,        //ºìÂêè§             
        Charoite                         = 3,        //×ÏÁú¾§             
    }

    public enum Armor
    {
        None = 0,
        BrightLightArmor                 = 1,         //Ã÷¹âîø      
        SteelArmour                      = 2,         //¸ÖÌúÕ½¼×      
    }

    public enum Arms
    {
        None                             = 0,
        DragonCarvingKnife               = 1,         //ÍÀÁúµ¶      
        HeavenReliantSword               = 2,         //ÒÐÌì½£      
    }
    public enum Step
    {
        None                             = 0,
        One                              = 1,
        Two                              = 2,
        Three                            = 3,
        Four                             = 4,
        Five                             = 5,
        Six                              = 6,
        Seven                            = 7,
        Eight                            = 8,
        Nine                             = 9,
    }
}