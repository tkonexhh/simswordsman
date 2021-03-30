using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public enum SimGameTaskTriggerType
    {
        Main,
        Common,
        Daily
    }

	public enum SimGameTaskType
	{
        None,
	    Collect,
        Battle,
        Progress,
	}

    public enum CollectedObjType
    {
        None,
        Fish, // ��
        Chicken, // ��
        Bear, // ��
        Boar, // Ұ��
        Snake, // ��
        Deer, // ¹
        WuWood, // ��ľ
        SilverWood, // ��ľ
        QingRock, // ����
        CloudRock, // ����
        Vine, // ����
        Iron, // ����
        Ganoderma, // ��֥
        Well, // ��ˮ
        Wolf,

        ///����Item����ʾ
        RoyalJelly,//������
        LotusRoot,//��ź
        Lotus,//����
        LotusLeaf,//��Ҷ

    }
}