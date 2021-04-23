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
        None,
        Fish, // 鱼
        Chicken, // 鸡
        Bear, // 熊
        Boar, // 野猪
        Snake, // 蛇
        Deer, // 鹿
        WuWood, // 乌木
        SilverWood, // 银木
        QingRock, // 青岩
        CloudRock, // 云岩
        Vine, // 紫藤
        Iron, // 黑铁
        Ganoderma, // 灵芝
        Well, // 井水
        Wolf,

        ///用于Item的提示
        RoyalJelly,//蜂王浆
        LotusRoot,//莲藕
        Lotus,//莲花
        LotusLeaf,//荷叶

    }
}