using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TowerData : IDataClass, IDailyResetData
    {
        public int coin;
        public int maxLevel;
        public List<int> enemyPoolLst = new List<int>();

        //自身状态保存
        public List<TowerCharacterDB> towerCharacterLst = new List<TowerCharacterDB>();
        private Dictionary<int, TowerCharacterDB> m_TowerCharacterMap = new Dictionary<int, TowerCharacterDB>();
        //Enemy状态保存
        public List<TowerCharacterDB> enemyCharacterLst = new List<TowerCharacterDB>();

        //奖励保存
        public List<int> rewardedLst = new List<int>();

        public override void InitWithEmptyData()
        {
            coin = 0;
            maxLevel = 1;
        }

        public override void OnDataLoadFinish()
        {
            towerCharacterLst.ForEach(c =>
            {
                m_TowerCharacterMap.Add(c.id, c);
            });
        }

        public void ResetDailyData()
        {
            maxLevel = 1;
            enemyPoolLst.Clear();
            for (int i = 0; i < TowerDefine.MAXLEVEL; i++)
            {
                int poolNum = TDTowerEnemyConfigTable.dataList[i].poolNum;
                enemyPoolLst.Add(poolNum);
            }

            KnuthDurstenfeldShuffle(enemyPoolLst);
            rewardedLst.Clear();

            SetDataDirty();
        }

        public int GetEnemyPoolIDByIndex(int index)
        {
            if (index < 0 && index >= enemyPoolLst.Count)
            {
                return 1;
            }

            return enemyPoolLst[index];
        }

        public bool AddCoin(int delta)
        {
            if (delta + coin < 0)
            {
                return false;
            }
            coin += delta;
            coin = Mathf.Clamp(coin, 0, 99999);
            EventSystem.S.Send(EventID.OnRefeshTowerCoin);
            SetDataDirty();
            return true;
        }

        public void AddMaxLevel()
        {
            maxLevel++;
            SetDataDirty();
        }

        private void KnuthDurstenfeldShuffle<T>(List<T> list)
        {
            //随机交换
            int currentIndex;
            T tempValue;
            for (int i = 0; i < list.Count; i++)
            {
                currentIndex = Random.Range(0, list.Count - i);
                tempValue = list[currentIndex];
                list[currentIndex] = list[list.Count - 1 - i];
                list[list.Count - 1 - i] = tempValue;
            }
        }

        #region 爬塔角色
        public void AddTowerCharacter(int id)
        {
            if (HasTowerCharacter(id))
            {
                return;
            }

            TowerCharacterDB character = new TowerCharacterDB();
            character.id = id;
            character.hpRate = 1;
            towerCharacterLst.Add(character);
            m_TowerCharacterMap.Add(character.id, character);
            SetDataDirty();
        }

        public bool HasTowerCharacter(int id)
        {
            return m_TowerCharacterMap.ContainsKey(id);
        }

        public TowerCharacterDB GetTowerCharacterByID(int id)
        {
            TowerCharacterDB character;
            m_TowerCharacterMap.TryGetValue(id, out character);
            return character;
        }

        public void SetHpRate(int id, double rate)
        {
            var data = GetTowerCharacterByID(id);
            if (data == null)
            {
                return;
            }

            data.SetHpRate(rate);
            SetDataDirty();
        }
        #endregion

        #region 敌人数据保存

        public void ClearEnemyDB()
        {
            enemyCharacterLst.Clear();
            SetDataDirty();
        }

        public void SetEnemy(List<EnemyConfig> enemys)
        {
            enemyCharacterLst.Clear();
            enemys.ForEach(e =>
            {
                TowerCharacterDB characterDB = new TowerCharacterDB();
                characterDB.id = e.ConfigId;
                characterDB.hpRate = 1;
                enemyCharacterLst.Add(characterDB);
            });
            SetDataDirty();
        }

        public List<EnemyConfig> GetEnemys()
        {
            List<EnemyConfig> enemyConfigs = new List<EnemyConfig>();
            foreach (var db in enemyCharacterLst)
            {
                EnemyConfig enemy = new EnemyConfig(db.id, 1, 150);
                enemyConfigs.Add(enemy);
            }
            return enemyConfigs;
        }

        public TowerCharacterDB GetEnemyByIndex(int index)
        {
            return enemyCharacterLst[index];
        }

        public void SetEnemyHpRate(int index, double rate)
        {
            if (index < 0 || index >= enemyCharacterLst.Count)
                return;

            enemyCharacterLst[index].hpRate = rate;
            SetDataDirty();
        }
        #endregion


        #region 获得奖励
        public void GetReward(int level)
        {
            rewardedLst.Add(level);
            SetDataDirty();
        }

        public bool HasReward(int level)
        {
            return rewardedLst.Contains(level);
        }
        #endregion
    }


    public class TowerCharacterDB
    {
        public int id;
        public double hpRate;
        public bool revive = false;


        public bool IsDead()
        {
            return hpRate <= 0;
        }

        public void SetHpRate(double rate)
        {
            hpRate = rate;
            hpRate = Mathf.Clamp01((float)hpRate);
        }
    }

}