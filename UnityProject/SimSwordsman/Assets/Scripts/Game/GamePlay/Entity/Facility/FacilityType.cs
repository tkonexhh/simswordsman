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
        LivableRoomEast                 = 2,        //���᷿
        LivableRoomWest                 = 3,        //���᷿
        Warehouse                       = 4,        //�ֿ�
        PracticeFieldEast               = 5,        //��������
        PracticeFieldWest               = 6,        //��������
        KongfuLibrary                   = 7,        //�ؾ���
        Kitchen                         = 8,        //�﷿
        ForgeHouse                      = 9,        //������
        Baicaohu                        = 10,       //�ٲ���
        PatrolRoom                      = 11,       //Ѳ�߷�
        BartizanEast                    = 12,       //������
        BartizanWest                    = 13,       //������
        BulletinBoard                   = 14,       //�����
        TotalCount,
    }
	
}