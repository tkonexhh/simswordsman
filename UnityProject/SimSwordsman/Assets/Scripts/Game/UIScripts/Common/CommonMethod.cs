using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class CommonMethod 
	{
        public static string SplicingTime(double seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }
        public static IEnumerator CountDown(Action action)
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                action?.Invoke();
            }
        }
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

        public static Sprite GetDiscipleHeadPortrait(LoadDiscipleHeadPortrait discipleHeadPortrait,CharacterItem characterItem)
        {
            switch (discipleHeadPortrait)
            {
                case LoadDiscipleHeadPortrait.Head:
                    //return SpriteHandler.S.GetSprite(AtlasDefine.DisicipleHeadIconsAtlas, "Head_"+ characterItem.quality+"_"+ characterItem.headId);
                    return SpriteHandler.S.GetSprite(AtlasDefine.DisicipleHeadIconsAtlas, "head_good_2");
                case LoadDiscipleHeadPortrait.Body:
                    //return SpriteHandler.S.GetSprite(AtlasDefine.DisicipleHeadIconsAtlas, "Body_" + characterItem.quality + "_" + characterItem.bodyId);
                    return SpriteHandler.S.GetSprite(AtlasDefine.DisicipleHeadIconsAtlas, "body_good_2");
            }
            Log.e("δ�ҵ����� = "+ discipleHeadPortrait);
            return null;
        }

    }
}