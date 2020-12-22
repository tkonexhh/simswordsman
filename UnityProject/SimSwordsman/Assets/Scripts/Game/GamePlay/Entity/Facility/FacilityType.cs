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
        LivableRoomEast                 = 2,        //东厢房
        LivableRoomWest                 = 3,        //西厢房
        Warehouse                       = 4,        //仓库
        PracticeFieldEast               = 5,        //东练功场
        PracticeFieldWest               = 6,        //西练功场
        KongfuLibrary                   = 7,        //藏经阁
        Kitchen                         = 8,        //伙房
        ForgeHouse                      = 9,        //锻造屋
        Baicaohu                        = 10,       //百草屋
        PatrolRoom                      = 11,       //巡逻房
        BartizanEast                    = 12,       //东箭塔
        BartizanWest                    = 13,       //西箭塔
        BulletinBoard                   = 14,       //公告榜
        TotalCount,
    }
	
}