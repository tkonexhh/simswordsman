using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
	public class BattleField : MonoBehaviour
	{
        public List<Transform> ourSlots = new List<Transform>();
        public List<Transform> enemySlots = new List<Transform>();

        private List<Transform> m_OurCurSlots;
        private List<Transform> m_EnemyCurSlots;

        private List<Transform> m_OurSlotsBackup = new List<Transform>();
        private List<Transform> m_EnemySlotsBackup = new List<Transform>();
        [SerializeField]
        private SpriteRenderer m_BgSpriteRender;

        private ResLoader m_ResLoader;

        public void Init()
        {
            m_ResLoader = ResLoader.Allocate("BattleField");

            m_OurCurSlots = new List<Transform>(ourSlots);
            m_EnemyCurSlots = new List<Transform>(enemySlots);

            m_OurSlotsBackup.AddRange(m_OurCurSlots);
            m_EnemySlotsBackup.AddRange(m_EnemyCurSlots);
        }

        public Vector3 GetOurCharacterPos()
        {
            int random = Random.Range(0, m_OurCurSlots.Count);
            Vector3 pos = m_OurCurSlots[random].position;
            m_OurCurSlots.RemoveAt(random);

            return pos;
        }

        public Vector3 GetEnemyCharacterPos()
        {
            int random = Random.Range(0, m_EnemyCurSlots.Count);
            Vector3 pos = m_EnemyCurSlots[random].position;
            m_EnemyCurSlots.RemoveAt(random);

            return pos;
        }

        public void OnBattleEnd()
        {
            m_OurCurSlots.Clear();
            m_OurCurSlots.AddRange(m_OurSlotsBackup);

            m_EnemyCurSlots.Clear();
            m_EnemyCurSlots.AddRange(m_EnemySlotsBackup);
        }

        /// <summary>
        /// ¸Ä±ä±³¾°Í¼Æ¬
        /// </summary>
        /// <param name="clanType"></param>
        public void ChangeBgSpriteRender(ClanType clanType) 
        {
            Sprite sr = null;

            string spriteName = string.Empty;

            switch (clanType)
            {
                case ClanType.Gaibang:
                    spriteName = "BattleField_gaibang";
                    break;
                case ClanType.Shaolin:
                    spriteName = "BattleField_shaolin";
                    break;
                case ClanType.Wudang:
                    spriteName = "BattleField_wudang";
                    break;
                case ClanType.Emei:
                    spriteName = "BattleField_emei";
                    break;
                case ClanType.Huashan:
                    spriteName = "BattleField_huashan";
                    break;
                case ClanType.Wudu:
                    spriteName = "BattleField_wudu";
                    break;
                case ClanType.Mojiao:
                    spriteName = "BattleField_mojiao";
                    break;
                case ClanType.Xiaoyao:
                    spriteName = "BattleField_xiaoyao";
                    break;
            }

            if (string.IsNullOrEmpty(spriteName) == false) 
            {
                sr = SpriteLoader.S.GetSpriteByName(spriteName);
            }            
            
            if (sr != null) 
            {
                m_BgSpriteRender.sprite = sr;
            }
        }
    }	
}