using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // FacilityType should be equal to id in facility table
    public enum FacilityType
    {
        None,
        Lobby                           = 1,        //������
        LivableRoomEast1                = 2,        //���᷿1
        LivableRoomEast2                = 3,        //���᷿2
        LivableRoomEast3                = 4,        //���᷿3
        LivableRoomEast4                = 5,        //���᷿4
        LivableRoomWest1                = 6,        //���᷿1
        LivableRoomWest2                = 7,        //���᷿2
        LivableRoomWest3                = 8,        //���᷿3
        LivableRoomWest4                = 9,        //���᷿4
        Warehouse                       = 10,       //�ֿ�
        PracticeFieldEast               = 11,       //��������
        PracticeFieldWest               = 12,       //��������
        KongfuLibrary                   = 13,       //�ؾ���
        Kitchen                         = 14,       //�﷿
        ForgeHouse                      = 15,       //������
        Baicaohu                        = 16,       //�ٲ���
        PatrolRoom                      = 17,       //Ѳ�߷�
        //BartizanEast                    = 18,       //������
        //BartizanWest                    = 19,       //������
        BulletinBoard                   = 18,       //�����
        TotalCount,
    }
	
}