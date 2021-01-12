using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        public void Init()
        {
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
    }
	
}