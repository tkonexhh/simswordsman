using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // Equal to id in table
    public enum PropType
    {
        None = 0,
        Arms = 1,        //ÎäÆ÷
        Armor = 2,        //îø¼×
        RawMaterial = 3,        //Ô­²ÄÁÏ
        Kungfu = 4,        //¹¦·ò
        Herb = 5,        //²ÝÒ©
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
        ///// ÈçÀ´ÉñÕÆ
        ///// </summary>
        //RuLaiShenZhang                   = 1020,
        //JiBenQuanJiao                    = 5,        //»ù±¾È­½Å
    }

    public enum RawMaterial
    {
        None = 0,
        QingRock = 1001,         //ÇàÑÒ             
        WuWood = 1002,           //ÎÚÄ¾             
        CloudRock = 1003,        //ÔÆÑÒ
        SilverWood = 1004,       //ÒøÄ¾    
        Vine = 1005,             //×ÏÌÙ    
        Iron = 1006,             //ºÚÌú    
        SilverLeaf = 1007,       //ÒøÒ¶    
        CloudStone = 1008,       //ÔÆ¾§    
        Meat = 1009,             //Èâ    
        LotusRoot = 1010,        //Á«Åº    
        BearPaw = 1011,          //ÐÜÕÆ    
        Fish = 1012,             //Óã    
        LotusLeaf = 1013,        //ºÉÒ¶    
        Chicken = 1014,          //¼¦    
        SnakeGB = 1015,          //Éßµ¨    
        Honey = 1016,            //·äÍõ½¬    
        Lotus = 1017,            //Á«»¨    
        WellWater = 1018,        //¹Å¾®Ë®    
        DeerHorn = 1019,         //Â¹È×    
        Ganoderma = 1020,        //ÁéÖ¥    
        BeeThorn = 1021,         //·äÕë    
        SnakeTeeth = 1022,       //¶¾ÑÀ       
        /// <summary>
        /// ÒøÅÆÕÐÄ¼Áî
        /// </summary>
        SilverToken = 2001,
        /// <summary>
        /// ½ðÅÆÕÐÄ¼Áî
        /// </summary>
        GoldenToken = 2002,
        Malachite = 3001,       //¿×È¸Ê¯
        Agate = 3002,           //ºìÂêè§
        Crystal = 3003,         //×ÏÁú¾§
        GoldenThread = 3101,    //½ðË¿
        DragonScales = 3102,    //ÁúÁÛ
    }

    public enum ArmorType
    {
        None = 0,
        ZiTenJia = 501,  //×ÏÌÙ¼×
        YinYeJia = 601,        //ÒøÒ¶¼×
        RuanWeiJia = 701,      //Èíâ¬¼×
        MingGuangKai = 702,
    }

    public enum ArmsType
    {
        None = 0,
        ShaZhuDao = 101,//É±Öíµ¶
        DaHuanDao = 201,      //´ó»·µ¶
        HongYingQiang = 202,
        JinGuBang = 203,
        MengGuBaoDao = 204,
        XueDiZi = 205,
        XueDao = 206,
        TuLongDao = 301,      //ÍÀÁúµ¶
        YanYueDao = 302,      //ÙÈÔÂµ¶
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