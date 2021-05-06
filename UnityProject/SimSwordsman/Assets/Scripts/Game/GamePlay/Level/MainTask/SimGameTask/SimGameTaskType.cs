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

    //public enum CollectedObjType
    //{
    //    None,
    //    Fish, // 鱼
    //    Chicken, // 鸡
    //    Bear, // 熊
    //    Boar, // 野猪
    //    Snake, // 蛇
    //    Deer, // 鹿
    //    WuWood, // 乌木
    //    SilverWood, // 银木
    //    QingRock, // 青岩
    //    CloudRock, // 云岩
    //    Vine, // 紫藤
    //    Iron, // 黑铁
    //    Ganoderma, // 灵芝
    //    Well, // 井水
    //    Wolf,

    //    ///用于Item的提示
    //    RoyalJelly,//蜂王浆
    //    LotusRoot,//莲藕
    //    Lotus,//莲花
    //    LotusLeaf,//荷叶

    //}

    public enum CollectedObjType
    {
        None,
        Fish = 1012, // 鱼
        Chicken = 1014, // 鸡
        Bear = 1011, // 熊 
        Boar = 1009, // 野猪 
        Snake = 1015, // 蛇
        Deer = 1019, // 鹿
        WuWood = 1002, // 乌木
        SilverWood = 1004, // 银木
        QingRock = 1001, // 青岩
        CloudRock = 1003, // 云岩
        Vine = 1005, // 紫藤
        Iron = 1006, // 黑铁
        Ganoderma = 1020, // 灵芝
        Well = 1018, // 井水
        Wolf = 1111, //未找到

        ///用于Item的提示
        RoyalJelly = 1016,//蜂王浆
        LotusRoot = 1010,//莲藕
        Lotus = 1017,//莲花
        LotusLeaf = 1013,//荷叶

    }
}