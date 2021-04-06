using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class DeliverSystemMgr : TSingleton<DeliverSystemMgr>,IAssetPreloader
    {
        private ResLoader m_DeliverCarLoader = null;

        private const string DeliverCarPrefabName = "DeliverCar";

        public void OnInit()
        {
            //ºÏ≤‚—∫Ô⁄»ŒŒÒ
            List<SingleDeliverDetailData> dataList = GameDataMgr.S.GetClanData().GetDaliverData();
            var tmpDataList = dataList.Where(x => x.DaliverState == DeliverState.HasBeenSetOut).ToList();
            if (tmpDataList != null && tmpDataList.Count > 0) 
            {
                for (int i = 0; i < tmpDataList.Count; i++)
                {
                    SingleDeliverDetailData data = tmpDataList[i];

                    SetCharacterStateDeliver(data.CharacterIDList, false);

                    CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetTotalTimeSeconds(), null, (remainTime) => 
                    {
                        GetDeliverReward(data);
                    });

                    data.SetCountDownID(countDownItem.GetCountDownID());
                }
            }
        }
        private void SetCharacterStateDeliver(List<int> characterIDList,bool isFindPathToTargetPos = true) 
        {
            if (characterIDList != null) 
            {
                for (int i = 0; i < characterIDList.Count; i++)
                {
                    int characterID = characterIDList[i];
                    CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterID);
                    if (characterController != null) {
                        characterController.SetState(CharacterStateID.Deliver, FacilityType.Deliver, string.Empty, -1, isFindPathToTargetPos);                        
                    }
                }
            }
        }
        private void GetDeliverReward(SingleDeliverDetailData data)
        {
            GameDataMgr.S.GetClanData().RemoveDeliverDataByID(data.DaliverID);

            List<int> characterIDList = data.CharacterIDList;
            for (int i = 0; i < characterIDList.Count; i++)
            {
                int characterID = characterIDList[i];
                CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterID);
                if (characterController != null)
                {
                    characterController.SetState(CharacterStateID.Wander);
                }
            }

            var RewadDataList = data.RewadDataList;
            List<RewardBase> rewardList = new List<RewardBase>();
            for (int i = 0; i < RewadDataList.Count; i++)
            {
                DeliverRewadData tmpData = RewadDataList[i];
                rewardList.Add(RewardMgr.S.GetRewardBase(tmpData.RewardType, tmpData.RewardID, tmpData.RewardCount));
            }
            UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewardList);
        }
        public void StartDeliver(int deliverID,List<DeliverRewadData> rewardDataList,List<int> characterIDList) 
        {
            SingleDeliverDetailData data = GameDataMgr.S.GetClanData().AddOrUpdateDeliverData(deliverID, DeliverState.HasBeenSetOut, rewardDataList, characterIDList);

            DeliverCar car = SpawnDeliverCar();
            car.Init(deliverID, data.CharacterIDList, new Vector3(0, -3.6f, 0));

            SetCharacterStateDeliver(data.CharacterIDList);

            CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetTotalTimeSeconds(), null, (remainTime) => 
            {
                GetDeliverReward(data);
            });

            data.SetCountDownID(countDownItem.GetCountDownID());
        }

        #region preload
        public void StartPreload()
        {
            if (m_DeliverCarLoader == null) 
            {
                m_DeliverCarLoader = ResLoader.Allocate("DeliverCarLoader");
            }

            GameObject go = m_DeliverCarLoader.LoadSync(DeliverCarPrefabName) as GameObject;

            GameObjectPoolMgr.S.AddPool(DeliverCarPrefabName, go, 4, 2);

            AssetPreloaderMgr.S.OnLoadDone();
        }
        #endregion

        public DeliverCar SpawnDeliverCar() 
        {
            GameObject go = GameObjectPoolMgr.S.Allocate(DeliverCarPrefabName);
            go.transform.SetParent(null);

            DeliverCar car = go.GetComponent<DeliverCar>();

            return car;
        }
    }
}