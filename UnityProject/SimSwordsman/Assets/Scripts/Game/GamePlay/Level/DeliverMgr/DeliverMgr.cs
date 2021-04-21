using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class DeliverMgr : MonoBehaviour, IMgr
    {
        public List<SingleDeliverDetailData> DaliverDetailDataList = null;

        public void OnDestroyed()
        {
        }

        public void OnInit()
        {
            DaliverDetailDataList = GameDataMgr.S.GetClanData().DeliverData.GetSingleDeliverDetailDataList();
            if (DaliverDetailDataList != null && DaliverDetailDataList.Count == 0)
            {
                List<DeliverConfig> deliverConfigs = new List<DeliverConfig>();
                deliverConfigs.AddRange(TDDeliverTable.GetDeliverConfigList());
                for (int i = 0; i < deliverConfigs.Count; i++)
                {
                    GameDataMgr.S.GetClanData().DeliverData.AddDeliverData(deliverConfigs[i].level, DeliverState.Unlock, GetRandomReward(deliverConfigs[i]),new  List<int>());
                }
            }
        }

        private List<DeliverRewadData> GetRandomReward(DeliverConfig deliverConfig)
        {
            List<DeliverRewadData> deliverRewadDatas = new List<DeliverRewadData>();
            RandomWeightHelper<RewardBase> randomWeightHelper = new Game.RandomWeightHelper<RewardBase>();
            foreach (var item in deliverConfig.normalReward)
                randomWeightHelper.AddWeightItem(item);

            deliverRewadDatas.Add(new DeliverRewadData(randomWeightHelper.GetRandomWeightDeleteValue()));
            deliverRewadDatas.Add(new DeliverRewadData(randomWeightHelper.GetRandomWeightDeleteValue()));
            randomWeightHelper.ClearAll();

            foreach (var item in deliverConfig.RareReward)
                randomWeightHelper.AddWeightItem(item);
            deliverRewadDatas.Add(new DeliverRewadData(randomWeightHelper.GetRandomWeightDeleteValue()));
            randomWeightHelper.ClearAll();

            return deliverRewadDatas;
        }


        public void OnUpdate()
        {
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}