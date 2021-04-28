using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class ArenaData : IDataClass
    {
        public int coin;//擂台币
        public int nowLevel = 0;
        public int challengeCount = ArenaDefine.Max_ChallengeCount;
        public int adAddChallengeCount = ArenaDefine.Max_ADChallengeCount;

        public List<ArenaEnemyDB> enemyLst = new List<ArenaEnemyDB>();

        //商店信息
        public List<ArenaShopItemDB> shopInfoLst = new List<ArenaShopItemDB>();

        public override void InitWithEmptyData()
        {
            coin = 0;
        }

        public override void OnDataLoadFinish()
        {
        }

        public void Init()
        {
            enemyLst.Clear();
            if (enemyLst.Count == 0)
            {
                InitEnemy();
            }

        }

        private void Reset()
        {
            nowLevel = ArenaDefine.EnemyCount + 1;
            InitEnemy();
            adAddChallengeCount = ArenaDefine.Max_ADChallengeCount;
            challengeCount = ArenaDefine.Max_ChallengeCount;
        }

        public bool AddCoin(int delta)
        {
            if (delta + coin < 0)
            {
                return false;
            }
            coin += delta;
            coin = Mathf.Clamp(coin, 0, 99999);
            EventSystem.S.Send(EventID.OnRefeshArenaCoin);
            SetDataDirty();
            return true;
        }

        public void SetNowLevel(int level)
        {
            Debug.LogError(level);
            nowLevel = level;
            SetDataDirty();
        }

        public bool AddChallengeCount(int delta)
        {
            if (challengeCount + coin < 0)
            {
                return false;
            }
            challengeCount += delta;
            EventSystem.S.Send(EventID.OnRefeshArenaChallengeCount);
            SetDataDirty();
            return true;
        }

        public void ReduceADChallengeCount()
        {
            adAddChallengeCount--;
            SetDataDirty();
        }


        #region 敌人数据
        private void InitEnemy()
        {
            enemyLst.Clear();

            long totalAtk = MainGameMgr.S.CharacterMgr.GetCharacterATK();

            Debug.LogError(totalAtk);

            var enemyNames = TDArenaEnemyNameTable.GetRandomEnemy();
            for (int i = 0; i < ArenaDefine.EnemyCount; i++)
            {
                ArenaEnemyDB enemyDB = new ArenaEnemyDB();
                enemyDB.level = i + 1;
                enemyDB.name = enemyNames[i].name;
                float atkRate = TDArenaEnemyConfigTable.GetData(i + 1).enemyPowerRate;
                enemyDB.atk = (long)(atkRate * totalAtk);
                for (int j = 0; j < 5; j++)
                {
                    ArenaEnemyItemDB enemyItem = new ArenaEnemyItemDB();
                    enemyItem.quality = CharacterQuality.Good;
                    enemyItem.headId = 1;
                    enemyItem.bodyId = 1;
                    enemyDB.EnemyLst.Add(enemyItem);
                }
                enemyLst.Add(enemyDB);
            }

            SetDataDirty();
        }

        public ArenaEnemyDB GetArenaEnemyDBByIndex(int index)
        {
            if (index < 0 || index >= enemyLst.Count)
                throw new IndexOutOfRangeException();

            return enemyLst[index];
        }
        #endregion


        #region 商店相关
        public ArenaShopItemDB GetShopDataByIndex(int index)
        {
            if (index < 0 || index >= shopInfoLst.Count)
                return null;

            return shopInfoLst[index];
        }

        public void RandomShopData()
        {
            shopInfoLst.Clear();
            List<ArenaShopItemInfo> itemInfos = TDArenaShopTable.GetRandomShopItem();
            for (int i = 0; i < itemInfos.Count; i++)
            {
                ArenaShopItemDB shopItemDB = new ArenaShopItemDB();
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


    }


    public class ArenaShopItemDB : TowerShopItemDB
    {
    }

    public class ArenaEnemyDB
    {
        public int level;
        public string name;
        public long atk;
        public List<ArenaEnemyItemDB> EnemyLst = new List<ArenaEnemyItemDB>();
    }

    public class ArenaEnemyItemDB
    {
        public CharacterQuality quality;
        public int headId = 1; // Which head used
        public int bodyId = 1; // Which body used

    }

}