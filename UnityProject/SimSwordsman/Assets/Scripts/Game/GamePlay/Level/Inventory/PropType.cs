using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // Equal to id in table
	public enum PropType
	{
        None                             = 0,
        Arms                             = 1,        //ÎäÆ÷
        Armor                            = 2,        //îø¼×
        RawMaterial                      = 3,        //Ô­²ÄÁÏ
        Kungfu                           = 4,        //¹¦·ò
    }

    public enum KungfuType
    {
        None,
        XiangLong18Zhang                 = 1,        //½µÁúÊ®°ËÕÆ
        XiangLong19Zhang                 = 2,
        WuLinMiJi                        = 3,        //ÎäÁÖÃØ¼®
        LingBoWeiBu                      = 3005,        //Áè²¨Î¢²½
        /// <summary>
        /// ÈçÀ´ÉñÕÆ
        /// </summary>
        RuLaiShenZhang                   = 1020,
        JiBenQuanJiao                    = 5,        //»ù±¾È­½Å
    }

    public enum RawMaterial
    { 
        None= 0,
        cyanrock = 1001,         //ÇàÑÒ             
        blackwood = 1002,        //ÎÚÄ¾             
        moirerock = 1003,        //ÔÆÑÒ
        silverwood = 1004,       //ÒøÄ¾    
        wisteria = 1005,         //×ÏÌÙ    
        iron = 1006,             //ºÚÌú    
        silverleaf = 1007,       //ÒøÒ¶    
        moirecrystal = 1008,     //ÔÆ¾§    
        pork = 1009,             //Èâ    
        lotusroot = 1010,        //Á«Åº    
        bearspaw = 1011,         //ÐÜÕÆ    
        fish = 1012,             //Óã    
        lotusleaf = 1013,        //ºÉÒ¶    
        chicken = 1014,          //¼¦    
        snakegall = 1015,        //Éßµ¨    
        royaljelly = 1016,       //·äÍõ½¬    
        lotus = 1017,            //Á«»¨    
        wellwater = 1018,        //¹Å¾®Ë®    
        antier = 1019,           //Â¹È×    
        lingzhi = 1020,          //ÁéÖ¥    
        beeneedle = 1021,        //·äÕë    
        poisonfang = 1022,       //¶¾ÑÀ       
        /// <summary>
        /// ÒøÅÆÕÐÄ¼Áî
        /// </summary>
        SilverOlder = 2001,
        /// <summary>
        /// ½ðÅÆÕÐÄ¼Áî
        /// </summary>
        GoldOlder = 2002,
        malachite = 3001,       //¿×È¸Ê¯
        agate = 3002,           //ºìÂêè§
        charoite = 3003,        //×ÏÁú¾§
        goldenthread = 3004,    //½ðË¿
        DragonScales = 3005,    //ÁúÁÛ

    }

    public enum ArmorType
    {
        None = 0,
        ZiTenJia = 501,  //×ÏÌÙ¼×
        YinYeJia,        //ÒøÒ¶¼×
        RuanWeiJia,      //Èíâ¬¼×
    }

    public enum ArmsType
    {
        None = 0,
        ShaZhuDao = 101,//É±Öíµ¶
        DaHuanDao,      //´ó»·µ¶
        TuLongDao,      //ÍÀÁúµ¶
        YanYueDao,      //ÙÈÔÂµ¶
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