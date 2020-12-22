using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    /// <summary>
    /// 招募类型
    /// </summary>
    public enum RecruitType
    {
        GoldMedal,                  //金牌招募令
        SilverMedal,                //银牌招募令
    }

    /// <summary>
    /// 招募模型
    /// </summary>
    public class RecruitModel
    {
        private RecruitType m_RecruitType;
        public bool m_isFirstRecruit = true;
        private int m_RecruitCount = 3;
        public List<CharacterItem> m_CharacterModels = new List<CharacterItem>();

        public int GoldMedalGood = 7;
        public int GoldMedalPerfect = 3;

        public int SilverMedalNormal = 13;
        public int SilverMedalGood = 6;
        public int SilverMedalPerfect = 1;

        public RecruitModel(RecruitType recruitType, RecruitData recruitData)
        {
            m_RecruitType = recruitType;
            switch (m_RecruitType)
            {
                case RecruitType.GoldMedal:
                    m_isFirstRecruit = recruitData.goldIsFirst;
                    break;
                case RecruitType.SilverMedal:
                    m_isFirstRecruit = recruitData.silverIsFirst;
                    break;
                default:
                    break;
            }
            GoldMedalGood = recruitData.goldMedalGood;
            GoldMedalPerfect = recruitData.goldMedalPerfect;
            SilverMedalNormal = recruitData.silverMedalNormal;
            SilverMedalGood = recruitData.silverMedalGood;
            SilverMedalPerfect = recruitData.silverMedalPerfect;
            ResetDefault();

        }
        /// <summary>
        /// 设置默认值
        /// </summary>
        private void SetDefaultValue()
        {
            GoldMedalGood = 7;
            GoldMedalPerfect = 3;

            SilverMedalNormal = 13;
            SilverMedalGood = 6;
            SilverMedalPerfect = 1;

            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }

        /// <summary>
        /// 设置当前招募类别为不是第一次
        /// </summary>
        public void SetIsFirstRecruit()
        {
            m_isFirstRecruit = false;
            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }

        /// <summary>
        /// 返回当前招募类型
        /// </summary>
        /// <returns></returns>
        public RecruitType GetCurRecruitType()
        {
            return m_RecruitType;
        }
        /// <summary>
        /// 返回是否是第一次
        /// </summary>
        /// <returns></returns>
        public bool GetmIsFirstRecruit()
        {
            return m_isFirstRecruit;
        }
        /// <summary>
        /// 获取当前类型招募次数
        /// </summary>
        /// <returns></returns>
        public int GetCurRecruitCount()
        {
            return m_RecruitCount;
        }

        /// <summary>
        /// 设置当前招募次数
        /// </summary>
        public void SetCurRecruitCount()
        {
            m_RecruitCount--;

            if (m_RecruitCount < 0)
            {
                m_RecruitCount = 0;
            }

            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }
        /// <summary>
        /// 重置当前招募次数
        /// </summary>
        public void ResetCurRecruitCount()
        {
            m_RecruitCount = 3;
            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }

        /// <summary>
        /// 移除相应已被招募的弟子
        /// </summary>
        /// <param name="item"></param>
        public void RemoveCharacterList(CharacterItem item)
        {
            if (m_CharacterModels.Contains(item))
            {
                m_CharacterModels.Remove(item);
                if (m_RecruitType == RecruitType.GoldMedal)
                {
                    switch (item.quality)
                    {
                        case CharacterQuality.Good:
                            GoldMedalGood--;
                            break;
                        case CharacterQuality.Perfect:
                            GoldMedalPerfect--;
                            break;
                    }
                }
                else
                {
                    switch (item.quality)
                    {
                        case CharacterQuality.Normal:
                            SilverMedalNormal--;
                            break;
                        case CharacterQuality.Good:
                            SilverMedalGood--;
                            break;
                        case CharacterQuality.Perfect:
                            SilverMedalPerfect--;
                            break;
                    }
                }
            }
            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }
        /// <summary>
        /// 随机获取当前容器中的弟子
        /// </summary>
        /// <returns></returns>
        public CharacterItem GetCurContCharacter()
        {
            if (m_isFirstRecruit)
            {
                switch (m_RecruitType)
                {
                    case RecruitType.GoldMedal:
                        return SetCurReturnCharacterId(GetMoreCharacterInfo(CharacterQuality.Good));
                    case RecruitType.SilverMedal:
                        return SetCurReturnCharacterId(GetMoreCharacterInfo(CharacterQuality.Normal));
                }
            }

            if (m_CharacterModels.Count > 0)
            {
                //m_CharacterModels.OrderBy(u => Guid.NewGuid()).First();
                //int r = new System.Random().Next(m_CharacterModels.Count);
                int pos = UnityEngine.Random.Range(0, m_CharacterModels.Count);
                return SetCurReturnCharacterId(m_CharacterModels[pos]);
            }
            else
            {
                SetDefaultValue();
                ResetDefault();
                return GetCurContCharacter();
            }
        }

        private CharacterItem SetCurReturnCharacterId(CharacterItem item)
        {
            item.id = MainGameMgr.S.CharacterMgr.GetMaxCharacterId();
            return item;
        }

        /// <summary>
        /// 初始化 默认值
        /// </summary>
        private void ResetDefault()
        {
            switch (m_RecruitType)
            {
                case RecruitType.GoldMedal:
                    for (int i = 0; i < GoldMedalGood; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Good));
                    for (int i = 0; i < GoldMedalPerfect; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Perfect));
                    break;
                case RecruitType.SilverMedal:
                    for (int i = 0; i < SilverMedalNormal; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Normal));
                    for (int i = 0; i < SilverMedalGood; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Good));
                    for (int i = 0; i < SilverMedalPerfect; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Perfect));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 从表中数据获取更多弟子
        /// </summary>
        /// <param name="characterQuality"></param>
        /// <returns></returns>
        private CharacterItem GetMoreCharacterInfo(CharacterQuality characterQuality)
        {
            string desc = TDCharacterConfigTable.GetRandomDesc(characterQuality);
            string name = TDCharacterNameTable.GetRandomName();
            
            if (!MainGameMgr.S.CharacterMgr.CheckForDuplicateNames(name))
            {
                CharacterItem characterItem = new CharacterItem(characterQuality, desc, name);
                return characterItem;
            }
            else
                return GetMoreCharacterInfo(characterQuality);
        }
    }
}
