using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // FacilityType should be equal to id in facility table
    public enum FacilityType
    {
        None,
        Lobby                           = 1,        //讲武堂
        LivableRoomEast1                = 2,        //东厢房1
        LivableRoomEast2                = 3,        //东厢房2
        LivableRoomEast3                = 4,        //东厢房3
        LivableRoomEast4                = 5,        //东厢房4
        LivableRoomWest1                = 6,        //西厢房1
        LivableRoomWest2                = 7,        //西厢房2
        LivableRoomWest3                = 8,        //西厢房3
        LivableRoomWest4                = 9,        //西厢房4
        Warehouse                       = 10,       //仓库
        PracticeFieldEast               = 11,       //东练功场
        PracticeFieldWest               = 12,       //西练功场
        KongfuLibrary                   = 13,       //藏经阁
        Kitchen                         = 14,       //伙房
        ForgeHouse                      = 15,       //锻造屋
        Baicaohu                        = 16,       //百草屋
        PatrolRoom                      = 17,       //巡逻房
        //BartizanEast                    = 18,       //东箭塔
        //BartizanWest                    = 19,       //西箭塔
        BulletinBoard                   = 18,       //公告榜
        TotalCount,
    }
	
}