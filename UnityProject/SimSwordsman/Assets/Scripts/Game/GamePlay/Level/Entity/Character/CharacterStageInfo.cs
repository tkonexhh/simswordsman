using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class CharacterStageInfoItem
	{
        public int Stage { set; get; }
        public CharacterQuality CharacterQuality { set; get; }
        public int FromLevel { set; get; }
        public int ToLevel { set; get; }
        public float BaseAtk { set; get; }
        public int GrowAtk { set; get; }
        public UnlockContentConfigInfo UnlockContentInfo { set; get; }
        public int StartExp { set; get; }
        public int GrowExp { set; get; }
        public CharacterStageInfoItem(TDCharacterStageConfig tdData)
        {
            Stage = tdData.stage;
            CharacterQuality = EnumUtil.ConvertStringToEnum<CharacterQuality>(tdData.quality);
            FromLevel = tdData.fromLevel;
            ToLevel = tdData.toLevel;
            BaseAtk = float.Parse(tdData.baseAtk);
            GrowAtk = tdData.growAtk;
            UnlockContentInfo = new UnlockContentConfigInfo(tdData.unlockContent);
            StartExp = tdData.startExp;
            GrowExp = tdData.growExp;
        }

        public bool IsHaveUnlockContentInfo(UnlockContent unlockContent, int count)
        {
            if (UnlockContentInfo.UnlockContent == unlockContent && UnlockContentInfo.Count == count)
                return true;
            return false;
        }

    }

    public class UnlockContentConfigInfo
    {
        public UnlockContent UnlockContent { set; get; }
        public int Count { set; get; }
        public UnlockContentConfigInfo(string unlockContent)
        {
            string[] unlockContentStr = unlockContent.Split('|');
            if (unlockContentStr.Length == 2)
                Count = int.Parse(unlockContentStr[1]);
            UnlockContent = EnumUtil.ConvertStringToEnum<UnlockContent>(unlockContentStr[0]);
        }
    }

    public class CharacterStageInfo
    {
        public Dictionary<int, CharacterStageInfoItem> m_CharacterStageInfoItems = new Dictionary<int, CharacterStageInfoItem>();

        public Dictionary<int, CharacterStageInfoItem> GetCharacterStageInfoItems()
        {
            return m_CharacterStageInfoItems;
        }

        public CharacterStageInfo(TDCharacterStageConfig tdData)
        {
            AddCharacterStageInfo(tdData);
        }
          

        public void AddCharacterStageInfo(TDCharacterStageConfig tdData)
        {
            if (!m_CharacterStageInfoItems.ContainsKey(tdData.stage))
                m_CharacterStageInfoItems.Add(tdData.stage, new CharacterStageInfoItem(tdData));
        }

        public CharacterStageInfoItem GetCharacterStageInfoItem(int stage)
        {
            if (m_CharacterStageInfoItems.ContainsKey(stage))
                return m_CharacterStageInfoItems[stage];
            return null;
        }
        public CharacterStageInfoItem GetUnlockContent(int level)
        {
            foreach (var item in m_CharacterStageInfoItems.Values)
                if (level>= item.FromLevel && level<=item.ToLevel)
                    return item;
            return null;
        }

     

    }
}