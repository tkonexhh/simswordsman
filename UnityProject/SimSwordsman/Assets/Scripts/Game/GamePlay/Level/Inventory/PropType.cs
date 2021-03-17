using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // Equal to id in table
    public enum PropType
    {
        None = 0,
        Arms = 1,        //����
        Armor = 2,        //����
        RawMaterial = 3,        //ԭ����
        Kungfu = 4,        //����
        Herb = 5,        //��ҩ
    }

    public enum KungfuType
    {
        None,
        TaiZuChangQuan = 1001,
        PaoDingJieNiu = 1002,
        FengShenTui = 1003,
        YanHuiJianFa = 1004,
        QingMangZhi = 1005,
        FeiYunDu = 1006,
        WuLangBaGuaGun = 1007,
        LianHuaZhang = 1008,
        KuiHuaDianXueShou = 1009,
        SaoYeTui = 1010,
        ZuiQuan = 1011,
        SuiYuQuan = 2001,
        FoShanWuYingJiao = 2002,
        TanZhiShenGong = 2003,
        LuanPiFengJianFa = 2004,
        LieYanDaoFa = 2005,
        YuJianShu = 2006,
        ShiErLuTanTui = 2007,
        KaiBeiShou = 2008,
        RuLaiShenZhang = 3001,
        XiangLongZhang = 3002,
        LiuMaiShenJian = 3003,
        DuGuJiuJian = 3004,
        LingBoWeiBu = 3005,
        BaHuangLiuHe = 3006,
        TianCanJiao = 3007,
        TianShanZheMeiShou = 3008,
        YiJinJing = 3009,
        Attack = 9901,
        ///// <summary>
        ///// ��������
        ///// </summary>
        //RuLaiShenZhang                   = 1020,
        //JiBenQuanJiao                    = 5,        //����ȭ��
    }

    public enum RawMaterial
    {
        None = 0,
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
        GoldenThread = 3101,    //��˿
        DragonScales = 3102,    //����
    }

    public enum ArmorType
    {
        None = 0,
        ZiTenJia = 501,  //���ټ�
        YinYeJia = 601,        //��Ҷ��
        RuanWeiJia = 701,      //��⬼�
        MingGuangKai = 702,
    }

    public enum ArmsType
    {
        None = 0,
        ShaZhuDao = 101,//ɱ��
        DaHuanDao = 201,      //�󻷵�
        HongYingQiang = 202,
        JinGuBang = 203,
        MengGuBaoDao = 204,
        XueDiZi = 205,
        XueDao = 206,
        TuLongDao = 301,      //������
        YanYueDao = 302,      //���µ�
        DaGouBang = 303,
        JinSheJian = 304,
        XuanTieBiShou = 305,
        YiTianJian = 306,

    }
    public enum Step
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
    }
}