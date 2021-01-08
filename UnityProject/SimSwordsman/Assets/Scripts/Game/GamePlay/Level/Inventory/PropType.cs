using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // Equal to id in table
	public enum PropType
	{
        None                             = 0,
        Arms                             = 1,        //����
        Armor                            = 2,        //����
        RawMaterial                      = 3,        //ԭ����
        Kungfu                           = 4,        //����
    }

    public enum KungfuType
    {
        None,
        XiangLong18Zhang                 = 1,        //����ʮ����
        XiangLong19Zhang                 = 2,
        WuLinMiJi                        = 3,        //�����ؼ�
        LingBoWeiBu                      = 1024,        //�貨΢��
        /// <summary>
        /// ��������
        /// </summary>
        RuLaiShenZhang                   = 1020,
        JiBenQuanJiao                    = 5,        //����ȭ��
    }

    public enum RawMaterial
    { 
        None= 0,
        QingRock = 1001,         //����             
        WuWood = 1002,           //��ľ             
        CloudRock = 1003,        //����
        SilverWood = 1004,       //��ľ    
        Vine = 1005,             //����    
        Iron = 1006,             //����    
        SilverLeaf = 1007,       //��Ҷ    
        CloudStone = 1008,       //�ƾ�    
        Meat = 1009,             //��    
        LotusRoot = 1010,        //��ź    
        BearPaw = 1011,          //����    
        Fish = 1012,             //��    
        LotusLeaf = 1013,        //��Ҷ    
        Chicken = 1014,          //��    
        SnakeGB = 1015,          //�ߵ�    
        Honey = 1016,            //������    
        Lotus = 1017,            //����    
        WellWater = 1018,        //�ž�ˮ    
        DeerHorn = 1019,         //¹��    
        Ganoderma = 1020,        //��֥    
        BeeThorn = 1021,         //����    
        SnakeTeeth = 1022,       //����       
        /// <summary>
        /// ������ļ��
        /// </summary>
        SilverToken = 2001,
        /// <summary>
        /// ������ļ��
        /// </summary>
        GoldenToken = 2002,
        Malachite = 3001,       //��ȸʯ
        Agate = 3002,           //�����
        Crystal = 3003,         //������
        GoldenThread = 3004,    //��˿
        DragonScales = 3005,    //����

    }

    public enum ArmorType
    {
        None = 0,
        ZiTenJia = 501,  //���ټ�
        YinYeJia,        //��Ҷ��
        RuanWeiJia,      //��⬼�
    }

    public enum ArmsType
    {
        None = 0,
        ShaZhuDao = 101,//ɱ��
        DaHuanDao,      //�󻷵�
        TuLongDao,      //������
        YanYueDao,      //���µ�
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