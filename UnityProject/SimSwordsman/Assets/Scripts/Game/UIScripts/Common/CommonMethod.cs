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
                    return "长老";
                case ClanType.Shaolin:
                    return "主持";
                case ClanType.Wudang:
                    return "护法";
                case ClanType.Emei:
                    return "使者";
                case ClanType.Huashan:
                    return "首座";
                case ClanType.Wudu:
                    return "毒使";
                case ClanType.Mojiao:
                    return "舵主";
                case ClanType.Xiaoyao:
                    return "首徒";
            }
            return string.Empty;
        }
    }
	
}