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
        public List<TowerLevelConfigDB> towerLevelConfigs = new List<TowerLevelConfigDB>();

        //自身状态保存
        public List<TowerCharacterDB> towerCharacterLst = new List<TowerCharacterDB>();
        private Dictionary<int, TowerCharacterDB> m_TowerCharacterMap = new Dictionary<int, TowerCharacterDB>();
        //Enemy状态保存
        public List<TowerCharacterDB> enemyCharacterLst = new List<TowerCharacterDB>();

        //商店信息
        public List<TowerShopItemDB> shopInfoLst = new List<TowerShopItemDB>();

        //已经复活的关卡保存
        //public List<int> revivedLvlLst = new List<int>();

        public override void InitWithEmptyData()
        {
            coin = 0;
            maxLevel = 1;
        }

        public override void OnDataLoadFinish()
        {
            towerCharacterLst.ForEach(c =>
            {
                if (!m_TowerCharacterMap.ContainsKey(c.id))
                    m_TowerCharacterMap.Add(c.id, c);
            });

            if (shopInfoLst.Count == 0)
                RandomShopData();

            SetDataDirty();
        }

        public void ResetDailyData()
        {
            maxLevel = 1;
            towerLevelConfigs.Clear();
            towerCharacterLst.Clear();
            m_TowerCharacterMap.Clear();
            enemyCharacterLst.Clear();
            // InitLevelConfig();
            RandomShopData();
            SetDataDirty();
        }

        public void Init()
        {
            if (towerLevelConfigs.Count == 0)
                InitLevelConfig();
        }

        private void InitLevelConfig()
        {

            towerLevelConfigs.Clear();

            var enemyPoolLst = new List<TowerLevelEnemyDB>();
            for (int i = 0; i < TowerDefine.MAXLEVEL; i++)
            {
                var config = TDTowerEnemyConfigTable.dataList[i];
                int poolNum = config.poolNum;
                TowerLevelEnemyDB levelEnemyDB = new TowerLevelEnemyDB();
                levelEnemyDB.id = poolNum;
                levelEnemyDB.enemyIDLst.Add(int.Parse(config.boss));
                //随机添加四个敌人
                for (int e = 0; e < 4; e++)
                {
                    levelEnemyDB.enemyIDLst.Add(config.enemyIDs[RandomHelper.Range(0, config.enemyIDs.Count)]);
                }
                enemyPoolLst.Add(levelEnemyDB);
            }
            KnuthDurstenfeldShuffle(enemyPoolLst);

            // Debug.LogError(GameDataMgr.S.GameDataHandler);
            int lobbyLevel = GameDataMgr.S.GetClanData().GetFacilityData(FacilityType.Lobby).level;
            // Debug.LogError(lobbyLevel);
            for (int i = 0; i < TowerDefine.MAXLEVEL; i++)
            {
                TowerLevelConfigDB levelConfig = new TowerLevelConfigDB();
                levelConfig.level = i + 1;
                levelConfig.enemyConfig = enemyPoolLst[i];

                //设置奖励
                var towerConfig = TDTowerConfigTable.GetData(levelConfig.level);
                if (towerConfig != null && towerConfig.fcoinNum != 0)
                {
                    levelConfig.reward = "TowerCoin|" + towerConfig.fcoinNum;
                }
                else
                {
                    var rewardConfig = TDTowerRewardConfigTable.GetData(lobbyLevel);
                    if (rewardConfig != null)
                    {
                        levelConfig.reward = rewardConfig.GetRandomRewardStr();
                    }
                }
                // Debug.LogError(levelConfig.reward);
                towerLevelConfigs.Add(levelConfig);
            }
            DataRecord.S.SetBool(TowerDefine.SAVEKEY_NEWDAYSHOW, false);
            SetDataDirty();
        }

        public TowerLevelConfigDB GetLevelConfigByIndex(int index)
        {
            if (index < 0 && index >= towerLevelConfigs.Count)
            {
                return towerLevelConfigs[0];
            }

            return towerLevelConfigs[index];
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

        public List<EnemyConfig> GetEnemys(float baseAtk)
        {
            List<EnemyConfig> enemyConfigs = new List<EnemyConfig>();
            for (int i = 0; i < enemyCharacterLst.Count; i++)
            {
                EnemyConfig enemy = new EnemyConfig(enemyCharacterLst[i].id, 1, (int)(i == 0 ? baseAtk * 1.5f : baseAtk));
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

        #region 商店相关
        public TowerShopItemDB GetShopDataByIndex(int index)
        {
            if (index < 0 || index >= shopInfoLst.Count)
                return null;

            return shopInfoLst[index];
        }

        public void RandomShopData()
        {
            shopInfoLst.Clear();
            List<TowerShopItemInfo> itemInfos = TDTowerShopTable.GetRandomShopItem();
            for (int i = 0; i < itemInfos.Count; i++)
            {
                TowerShopItemDB shopItemDB = new TowerShopItemDB();
                shopItemDB.id = itemInfos[i].id;
                shopItemDB.buyed = false;
                shopInfoLst.Add(shopItemDB);
            }
            EventSystem.S.Send(EventID.OnRefeshTowerShop);
            SetDataDirty();
        }

        public void BuyShopData(int index)
        {
            if (index < 0 || index >= shopInfoLst.Count)
                return;

            shopInfoLst[index].buyed = true;
            SetDataDirty();
        }
        #endregion

        #region 复活相关
        public void ReviveTowerCharacter(int id)
        {
            var data = GetTowerCharacterByID(id);
            if (data == null)
            {
                return;
            }

            if (data.revive)
            {
                Log.w("Already Revive Tower Character");
                return;
            }
            data.hpRate = 1;
            data.revive = true;
            SetDataDirty();
        }

        // public void LevelRevived(int level)
        // {
        //     if (revivedLvlLst.Contains(level))
        //     {
        //         Log.w("Already Cointains Tower Revive Level");
        //         return;
        //     }

        //     revivedLvlLst.Add(level);
        //     SetDataDirty();
        // }

        // public bool HasLevelRevived(int level)
        // {
        //     return revivedLvlLst.Contains(level);
        // }
        #endregion
    }

    public class TowerLevelConfigDB
    {
        public int level;
        public TowerLevelEnemyDB enemyConfig;
        public string reward;
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

    public class TowerLevelEnemyDB
    {
        public int id;
        public List<int> enemyIDLst = new List<int>();
    }

    public class TowerShopItemDB
    {
        public int id;
        public bool buyed;
    }


}