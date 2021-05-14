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
        public bool getRewarded = false;//是否领取过排行奖励
        public bool hasReward = false;//是否需要发放奖励
        public string lastRefeshTime;

        public List<ArenaEnemyDB> enemyLst = new List<ArenaEnemyDB>();

        //商店信息
        public List<ArenaShopItemDB> shopInfoLst = new List<ArenaShopItemDB>();

        public override void InitWithEmptyData()
        {
            coin = 0;
            nowLevel = ArenaDefine.EnemyCount + 1;
        }

        public override void OnDataLoadFinish()
        {
            if (shopInfoLst.Count == 0)
                RandomShopData();
        }

        public void Init()
        {
            // Debug.LogError("Init:" + lastRefeshTime);
            //判读是否需要重置数据
            if (string.IsNullOrEmpty(lastRefeshTime))//首次进入
            {
                // Debug.LogError("lastRefeshTime is Null Reset");
                Reset();
            }
            else
            {
                //判断是否间隔1天
                DateTime lastRefesh = DateTime.Now;
                DateTime now = DateTime.Now;
                DateTime.TryParse(lastRefeshTime, out lastRefesh);

                int offsetDays = (now - lastRefesh).Days;
                // Debug.LogError("AAAA:" + offsetDays + "-----" + lastRefesh + "------" + now);
                if (offsetDays >= 1)
                {
                    // Debug.LogError("Passday Reset");
                    Reset();
                }
                else
                {
                    int lastLevel = nowLevel;
                    //如果当前的时间是刷新时间的十点以后
                    //并且间隔没有超过2天
                    //并且当前排名>24
                    if (now >= new DateTime(now.Year, now.Month, now.Day).AddHours(ArenaDefine.EndTime)
                        && now <= new DateTime(now.Year, now.Month, now.Day).AddDays(1).AddHours(ArenaDefine.StartTime)
                        // && offsetDays == 1
                        && lastLevel <= ArenaDefine.EnemyCount)
                    {
                        if (getRewarded == false)
                        {
                            hasReward = true;
                        }

                        SetDataDirty();
                    }
                }
            }

            // Reset();
        }

        private void Reset()
        {
            var now = DateTime.Now;
            var startTime = new DateTime(now.Year, now.Month, now.Day).AddHours(ArenaDefine.StartTime);
            lastRefeshTime = startTime.ToString();//DateTime.Now.ToString().Substring(0, 9) + ' ' + string.Format("{0:D2}:00:00", ArenaDefine.StartTime);
            // Debug.LogError("Reset:" + lastRefeshTime);
            nowLevel = ArenaDefine.EnemyCount + 1;
            InitEnemy();
            adAddChallengeCount = ArenaDefine.Max_ADChallengeCount;
            challengeCount = ArenaDefine.Max_ChallengeCount;
            getRewarded = false;
            hasReward = false;

            RandomShopData();
            SetDataDirty();
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
            // Debug.LogError(level);
            nowLevel = level;
            SetDataDirty();
        }

        public bool AddChallengeCount(int delta)
        {
            if (challengeCount + delta < 0)
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
                enemyDB.headID = UnityEngine.Random.Range(1, TDAvatarTable.count + 1);
                enemyDB.level = i + 1;
                enemyDB.name = enemyNames[i].name;
                float atkRate = TDArenaEnemyConfigTable.GetData(i + 1).enemyPowerRate;
                enemyDB.atk = (long)(atkRate * totalAtk);
                for (int j = 0; j < 5; j++)
                {
                    ArenaEnemyItemDB enemyItem = new ArenaEnemyItemDB();
                    enemyItem.quality = (CharacterQuality)(UnityEngine.Random.Range((int)CharacterQuality.Normal, (int)CharacterQuality.Hero));
                    enemyItem.bodyId = UnityEngine.Random.Range(1, CharacterDefine.Max_Body);
                    enemyItem.headId = UnityEngine.Random.Range(1, CharacterDefine.Max_Head);
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
            EventSystem.S.Send(EventID.OnRefeshArenaShop);
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

        public void SetRankRewarded()
        {
            getRewarded = true;
            hasReward = false;
        }

    }


    public class ArenaShopItemDB : TowerShopItemDB
    {
    }

    public class ArenaEnemyDB
    {
        public int headID = 1;
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