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

        private bool m_IsInited = false;

        private Vector2 m_BattleAreaRightTop = new Vector2(44, 1.5f);
        private Vector2 m_BattleAreaLeftBottom = new Vector2(38, -1.1f);
        private Vector2 m_InitBattleAreaRightTop = new Vector2(44, 1.5f);
        private Vector2 m_InitBattleAreaLeftBottom = new Vector2(38, -1.1f);

        public Vector2 BattleAreaRightTop { get => m_BattleAreaRightTop; }
        public Vector2 BattleAreaLeftBottom { get => m_BattleAreaLeftBottom; }

        public void Init()
        {
            if (m_IsInited)
                return;

            m_IsInited = true;

            m_ResLoader = ResLoader.Allocate("BattleField");

            m_OurCurSlots = new List<Transform>(ourSlots);
            m_EnemyCurSlots = new List<Transform>(enemySlots);

            m_OurSlotsBackup.AddRange(m_OurCurSlots);
            m_EnemySlotsBackup.AddRange(m_EnemyCurSlots);
        }

        public void CalculateBattleArea(float deltaY = 0)
        {
            Vector2 center = new Vector2((m_InitBattleAreaRightTop.x + m_InitBattleAreaLeftBottom.x) / 2, (m_InitBattleAreaRightTop.y + m_InitBattleAreaLeftBottom.y) / 2);
            float height = MainGameMgr.S.MainCamera.battleProperty.size;
            float width = Camera.main.aspect * height;
            m_BattleAreaRightTop = new Vector2(center.x + width - 0.5f, m_InitBattleAreaRightTop.y + deltaY);
            m_BattleAreaLeftBottom = new Vector2(center.x - width + 0.5f, m_InitBattleAreaLeftBottom.y + deltaY);
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
        /// �ı䱳��ͼƬ
        /// </summary>
        /// <param name="clanType"></param>
        public void ChangeBgSpriteRender(ClanType clanType)
        {
            ChangeBGSpriteRender(GetBattleBgName(clanType));
        }

        public void ChangeBgSpriteRenderToHeroTrial()
        {
            ChangeBGSpriteRender("HeroTrialBattleField");
        }

        public void ChangeBgSpriteRenderToTower()
        {
            ChangeBGSpriteRender("BattleField_tower");
        }

        private void ChangeBGSpriteRender(string name)
        {
            Sprite sr = null;

            string spriteName = name;

            if (!string.IsNullOrEmpty(spriteName))
            {
                sr = SpriteLoader.S.GetSpriteByName(spriteName);
            }

            if (sr != null)
            {
                m_BgSpriteRender.sprite = sr;
            }
        }

        public void SetSpriteBgLocalPos(Vector3 localPos)
        {
            m_BgSpriteRender.transform.localPosition = localPos;
        }

        private string GetBattleBgName(ClanType clanType)
        {
            switch (clanType)
            {
                case ClanType.Gaibang: return "BattleField_gaibang";
                case ClanType.Shaolin: return "BattleField_shaolin";
                case ClanType.Wudang: return "BattleField_wudang";
                case ClanType.Emei: return "BattleField_emei";
                case ClanType.Huashan: return "BattleField_huashan";
                case ClanType.Wudu: return "BattleField_wudu";
                case ClanType.Mojiao: return "BattleField_mojiao";
                case ClanType.Xiaoyao: return "BattleField_xiaoyao";
                default: return string.Empty;
            }
        }
    }
}