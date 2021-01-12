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
        LingBoWeiBu                      = 3005,        //�貨΢��
        /// <summary>
        /// ��������
        /// </summary>
        RuLaiShenZhang                   = 1020,
        JiBenQuanJiao                    = 5,        //����ȭ��
    }

    public enum RawMaterial
    { 
        None= 0,
        cyanrock = 1001,         //����             
        blackwood = 1002,        //��ľ             
        moirerock = 1003,        //����
        silverwood = 1004,       //��ľ    
        wisteria = 1005,         //����    
        iron = 1006,             //����    
        silverleaf = 1007,       //��Ҷ    
        moirecrystal = 1008,     //�ƾ�    
        pork = 1009,             //��    
        lotusroot = 1010,        //��ź    
        bearspaw = 1011,         //����    
        fish = 1012,             //��    
        lotusleaf = 1013,        //��Ҷ    
        chicken = 1014,          //��    
        snakegall = 1015,        //�ߵ�    
        royaljelly = 1016,       //������    
        lotus = 1017,            //����    
        wellwater = 1018,        //�ž�ˮ    
        antier = 1019,           //¹��    
        lingzhi = 1020,          //��֥    
        beeneedle = 1021,        //����    
        poisonfang = 1022,       //����       
        /// <summary>
        /// ������ļ��
        /// </summary>
        SilverOlder = 2001,
        /// <summary>
        /// ������ļ��
        /// </summary>
        GoldOlder = 2002,
        malachite = 3001,       //��ȸʯ
        agate = 3002,           //�����
        charoite = 3003,        //������
        goldenthread = 3004,    //��˿
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