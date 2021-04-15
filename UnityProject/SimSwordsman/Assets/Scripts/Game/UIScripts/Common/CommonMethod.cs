using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class CommonMethod 
	{
		public static Sprite GetDiscipleSprite(CharacterItem characterItem)
		{
			return SpriteHandler.S.GetSprite(AtlasDefine.CharacterHeadIconsAtlas, "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId);
		}
        public static string GetAppellation(ClanType clanType)
        {
            switch (clanType)
            {
                case ClanType.None:
                    return "";
                case ClanType.Gaibang:
                    return "����";
                case ClanType.Shaolin:
                    return "����";
                case ClanType.Wudang:
                    return "����";
                case ClanType.Emei:
                    return "ʹ��";
                case ClanType.Huashan:
                    return "����";
                case ClanType.Wudu:
                    return "��ʹ";
                case ClanType.Mojiao:
                    return "����";
                case ClanType.Xiaoyao:
                    return "��ͽ";
            }
            return string.Empty;
        }
    }
	
}