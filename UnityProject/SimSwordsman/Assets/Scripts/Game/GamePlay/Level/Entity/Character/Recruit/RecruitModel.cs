using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    /// <summary>
    /// ��ļ����
    /// </summary>
    public enum RecruitType
    {
        GoldMedal,                  //������ļ��
        SilverMedal,                //������ļ��
    }

    /// <summary>
    /// ��ļģ��
    /// </summary>
    public class RecruitModel
    {
        private RecruitType m_RecruitType;
        private bool m_isFirstRecruit = true;
        private int m_RecruitCount = 0;
        private List<CharacterItem> m_CharacterModels = new List<CharacterItem>();

        public int advertisementCount = 0;

        public int goldMedalGood = 7;
        public int goldMedalPerfect = 3;

        public int silverMedalNormal = 13;
        public int silverMedalGood = 6;
        public int silverMedalPerfect = 1;

        public RecruitModel(RecruitType recruitType, RecruitData recruitData)
        {
            m_RecruitType = recruitType;
            switch (m_RecruitType)
            {
                case RecruitType.GoldMedal:
                    m_isFirstRecruit = recruitData.goldIsFirst;
                    m_RecruitCount = recruitData.goldRecruitCount;
                    advertisementCount = recruitData.goldAdvertisementCount;
                    break;
                case RecruitType.SilverMedal:
                    m_isFirstRecruit = recruitData.silverIsFirst;
                    m_RecruitCount = recruitData.silverRecruitCount;
                    advertisementCount = recruitData.silverAdvertisementCount;
                    break;
                default:
                    break;
            }
            goldMedalGood = recruitData.goldMedalGood;
            goldMedalPerfect = recruitData.goldMedalPerfect;
            silverMedalNormal = recruitData.silverMedalNormal;
            silverMedalGood = recruitData.silverMedalGood;
            silverMedalPerfect = recruitData.silverMedalPerfect;
            ResetDefault();
        }
        /// <summary>
        /// ����Ĭ��ֵ
        /// </summary>
        private void SetDefaultValue()
        {
            goldMedalGood = 7;
            goldMedalPerfect = 3;

            silverMedalNormal = 13;
            silverMedalGood = 6;
            silverMedalPerfect = 1;

            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }

        public void SetAdvertisementCount(int delta = 1)
        {
            advertisementCount -= delta;
            if (advertisementCount < 0)
                advertisementCount = 0;

            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }
        public int GetAdvertisementCount()
        {
            return advertisementCount;
        }

        public void ResetAdvertisementCount(RecruitType recruitType)
        {
            switch (recruitType)
            {
                case RecruitType.GoldMedal:
                    advertisementCount = 1;
                    break;
                case RecruitType.SilverMedal:
                    advertisementCount = 1;
                    break;
                default:
                    break;
            }
            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }

        /// <summary>
        /// ���õ�ǰ��ļ���Ϊ���ǵ�һ��
        /// </summary>
        public void SetIsFirstRecruit()
        {
            m_isFirstRecruit = false;
            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }

        /// <summary>
        /// ���ص�ǰ��ļ����
        /// </summary>
        /// <returns></returns>
        public RecruitType GetCurRecruitType()
        {
            return m_RecruitType;
        }
        /// <summary>
        /// �����Ƿ��ǵ�һ��
        /// </summary>
        /// <returns></returns>
        public bool GetmIsFirstRecruit()
        {
            return m_isFirstRecruit;
        }
        /// <summary>
        /// ��ȡ��ǰ������ļ����
        /// </summary>
        /// <returns></returns>
        public int GetCurRecruitCount()
        {
            return m_RecruitCount;
        }

        /// <summary>
        /// ���õ�ǰ��ļ����
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
        /// ������ļ���� ��Ϊ��ļ�����
        /// </summary>
        public void ReduceRecruitCount(int count)
        {
            m_RecruitCount -= count;
            if (m_RecruitCount < 0)
            {
                m_RecruitCount = 0;
            }

            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }


        /// <summary>
        /// ������ļ����
        /// </summary>
        /// <param name="delta"></param>
        public void IncreaseCurRecruitCount(int delta)
        {
            m_RecruitCount = Mathf.Min(m_RecruitCount + delta, Define.MAX_PROP_COUNT);
            GameDataMgr.S.GetPlayerData().IncreaseCurRecruitCount(m_RecruitType, delta);
        }
        /// <summary>
        /// ���õ�ǰ��ļ����
        /// </summary>
        public void ResetCurRecruitCount()
        {
            m_RecruitCount = 3;
            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }

        /// <summary>
        /// �Ƴ���Ӧ�ѱ���ļ�ĵ���
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
                            goldMedalGood--;
                            break;
                        case CharacterQuality.Perfect:
                            goldMedalPerfect--;
                            break;
                    }
                }
                else
                {
                    switch (item.quality)
                    {
                        case CharacterQuality.Normal:
                            silverMedalNormal--;
                            break;
                        case CharacterQuality.Good:
                            silverMedalGood--;
                            break;
                        case CharacterQuality.Perfect:
                            silverMedalPerfect--;
                            break;
                    }
                }
            }
            MainGameMgr.S.RecruitDisciplerMgr.RefreshRecruitData();
        }
        /// <summary>
        /// �����ȡ��ǰ�����еĵ���
        /// </summary>
        /// <returns></returns>
        public CharacterItem GetCurContCharacter()
        {
            if (m_isFirstRecruit)
            {
                SetIsFirstRecruit();
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
        /// ��ʼ�� Ĭ��ֵ
        /// </summary>
        private void ResetDefault()
        {
            switch (m_RecruitType)
            {
                case RecruitType.GoldMedal:
                    for (int i = 0; i < goldMedalGood; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Good));
                    for (int i = 0; i < goldMedalPerfect; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Perfect));
                    break;
                case RecruitType.SilverMedal:
                    for (int i = 0; i < silverMedalNormal; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Normal));
                    for (int i = 0; i < silverMedalGood; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Good));
                    for (int i = 0; i < silverMedalPerfect; i++)
                        m_CharacterModels.Add(GetMoreCharacterInfo(CharacterQuality.Perfect));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// �ӱ������ݻ�ȡ�������
        /// </summary>
        /// <param name="characterQuality"></param>
        /// <returns></returns>
        private CharacterItem GetMoreCharacterInfo(CharacterQuality characterQuality)
        {
            string desc = TDCharacterConfigTable.GetRandomDesc(characterQuality);
            string name = TDCharacterNameTable.GetRandomName();

            if (!MainGameMgr.S.CharacterMgr.CheckForDuplicateNames(name))
            {

                int bodyId = Random.Range(1, CharacterDefine.Max_Body);
                int headId = Random.Range(1, CharacterDefine.Max_Head);
                CharacterItem characterItem = new CharacterItem(characterQuality, desc, name, bodyId, headId);
                return characterItem;
            }
            else
                return GetMoreCharacterInfo(characterQuality);
        }

    }
}
