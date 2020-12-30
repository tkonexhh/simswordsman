using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // Equal to id in table
	public enum PropType
	{
        None                             = 0,
        Arms                             = 1,        //武器
        Armor                            = 2,        //铠甲
        RawMaterial                      = 3,        //原材料
        Kungfu                           = 4,        //原材料
    }

    public enum KungfuType
    {
        None,
        XiangLong18Zhang                 = 1,        //降龙十八掌
        XiangLong19Zhang                 = 2,
        WuLinMiJi                        = 3,        //武林秘籍
        LingBoWeiBu                      = 1024,        //凌波微步
        /// <summary>
        /// 如来神掌
        /// </summary>
        RuLaiShenZhang                   = 1020,
        JiBenQuanJiao                    = 5,        //基本拳脚
    }

    public enum RawMaterial
    { 
        None                             = 0,
        Malachite                        = 1,        //孔雀石             
        RedAgate                         = 2,        //红玛瑙             
        Charoite                         = 3,        //紫龙晶       
        /// <summary>
        /// 银牌招募令
        /// </summary>
        SilverToken = 2001,
        /// <summary>
        /// 金牌招募令
        /// </summary>
        GoldenToken = 2002,
    }

    public enum Armor
    {
        None = 0,
        BrightLightArmor                 = 1,         //明光铠      
        SteelArmour                      = 2,         //钢铁战甲      
    }

    public enum Arms
    {
        None                             = 0,
        DragonCarvingKnife               = 1,         //屠龙刀      
        HeavenReliantSword               = 2,         //倚天剑      
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