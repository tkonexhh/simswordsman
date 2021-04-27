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
        private static KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        private static string GetIconName(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetIconName(kungfuType);
        }
        public static Sprite GetKungfuBg(KungfuType kungfuType)
        {
            switch (GetKungfuQuality(kungfuType))
            {
                case KungfuQuality.Normal:
                    return SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Introduction");
                case KungfuQuality.Master:
                    return SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Advanced");
                case KungfuQuality.Super:
                    return SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, "Excellent");
                default:
                    break;
            }
            Log.e("δ�ҵ����򱳾� = " + kungfuType);
            return null;
        }
        public static Sprite GetKungName(KungfuType kungfuType)
        {
            return SpriteHandler.S.GetSprite(AtlasDefine.MartialArtsAtlas, GetIconName(kungfuType));
        }

        public static Sprite GetKungfuQualitySprite(KungfuType kungfuType)
        {
            switch (GetKungfuQuality(kungfuType))
            {
                case KungfuQuality.Normal:
                    return SpriteHandler.S.GetSprite(AtlasDefine.RewardPanelAtlas, "rewardpanel_kungfu_ordinary");
                case KungfuQuality.Super:
                    return SpriteHandler.S.GetSprite(AtlasDefine.RewardPanelAtlas, "rewardpanel_kungfu_senior");
                case KungfuQuality.Master:
                    return SpriteHandler.S.GetSprite(AtlasDefine.RewardPanelAtlas, "rewardpanel_kungfu_god");
            }
            Log.e("δ�ҵ�����Ʒ�� = " + kungfuType);
            return null;
        }
        public static Sprite GetKungfuQualitySprite(int equipID)
        {
            switch (GetEquipQuality(equipID))
            {
                case EquipQuailty.Primary:
                    return SpriteHandler.S.GetSprite(AtlasDefine.RewardPanelAtlas, "rewardpanel_kungfu_ordinary");
                case EquipQuailty.Intermediate:
                    return SpriteHandler.S.GetSprite(AtlasDefine.RewardPanelAtlas, "rewardpanel_kungfu_senior");
                case EquipQuailty.Senior:
                    return SpriteHandler.S.GetSprite(AtlasDefine.RewardPanelAtlas, "rewardpanel_kungfu_god");
            }
            Log.e("δ�ҵ�����Ʒ�� = " + equipID);
            return null;
        }

        private static EquipQuailty GetEquipQuality(int equipID)
        {
            return TDEquipmentConfigTable.GetEquipmentInfo(equipID).Quality;
        }

        public static bool CheckEnoughDiscipleLevel(LobbyLevelInfo lobbyLevelInfo)
        {
            int discipleLevel = MainGameMgr.S.CharacterMgr.GetLomitLevelDiscipleNumber(lobbyLevelInfo.upgradePreconditions.DiscipleLevel);
            if (discipleLevel >= lobbyLevelInfo.upgradePreconditions.DiscipleNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
