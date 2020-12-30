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
        Kungfu                           = 4,        //ԭ����
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
        None                             = 0,
        Malachite                        = 1,        //��ȸʯ             
        RedAgate                         = 2,        //�����             
        Charoite                         = 3,        //������       
        /// <summary>
        /// ������ļ��
        /// </summary>
        SilverToken = 2001,
        /// <summary>
        /// ������ļ��
        /// </summary>
        GoldenToken = 2002,
    }

    public enum Armor
    {
        None = 0,
        BrightLightArmor                 = 1,         //������      
        SteelArmour                      = 2,         //����ս��      
    }

    public enum Arms
    {
        None                             = 0,
        DragonCarvingKnife               = 1,         //������      
        HeavenReliantSword               = 2,         //���콣      
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