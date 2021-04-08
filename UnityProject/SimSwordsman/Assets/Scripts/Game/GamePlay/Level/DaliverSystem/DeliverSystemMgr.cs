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
        #region 字段
        private ResLoader m_DeliverCarLoader = null;

        private const string DeliverCarPrefabName = "DeliverCar";
        /// <summary>
        /// 镖车路径
        /// </summary>
        private DeliverPath m_DeliverPath;
        public DeliverPath DeliverPath
        {
            get 
            {
                return m_DeliverPath;
            }
        }

        private List<DeliverCar> m_AllDeliverCarList = new List<DeliverCar>();

        public Vector3 GoOutSidePos = new Vector3(-11f, -5f);

        private Vector2 m_DeliverCarOriginPos = new Vector3(0, -3.6f, 0);

        #endregion

        #region public
        public void OnInit()
        {
            EventSystem.S.Register(EventID.OnDeliverCarArrive, OnDeliverCarArriveCallBack);

            m_DeliverPath = GameObject.FindObjectOfType<DeliverPath>();

            //检测押镖任务
            List<SingleDeliverDetailData> dataList = GameDataMgr.S.GetClanData().GetAllDaliverData();
            var tmpDataList = dataList.Where(x => x.DaliverState == DeliverState.HasBeenSetOut).ToList();
            if (tmpDataList != null && tmpDataList.Count > 0)
            {
                for (int i = 0; i < tmpDataList.Count; i++)
                {
                    SingleDeliverDetailData data = tmpDataList[i];

                    if (data.DaliverState == DeliverState.HasBeenSetOut)
                    {
                        CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTimeSeconds(), null, (remainTime) =>
                        {
                            DeliverCar car = SpawnDeliverCar();
                            AddDeliverCarToList(car);
                            car.Init(data.DeliverID, data.CharacterIDList, GoOutSidePos);
                            car.StartMoveComeBack();
                        });
                        data.SetCountDownID(countDownItem.GetCountDownID());
                        countDownItem.SetSpeedUpMultiply(data.SpeedUpMultiple);
                    }
                }
            }
        }
        public DeliverCar SpawnDeliverCar()
        {
            GameObject go = GameObjectPoolMgr.S.Allocate(DeliverCarPrefabName);
            go.transform.SetParent(null);

            DeliverCar car = go.GetComponent<DeliverCar>();

            return car;
        }
        /// <summary>
        /// 添加镖车到缓存列表中
        /// </summary>
        /// <param name="deliverCar">镖车</param>
        public void AddDeliverCarToList(DeliverCar deliverCar)
        {
            DeliverCar car = m_AllDeliverCarList.Find(x => x.DeliverID == deliverCar.DeliverID);
            if (car == null)
            {
                m_AllDeliverCarList.Add(deliverCar);
            }
        }
        /// <summary>
        /// 缓存列表中移除镖车
        /// </summary>
        /// <param name="deliverID"></param>
        public void RemoveDeliverCar(int deliverID)
        {
            DeliverCar car = m_AllDeliverCarList.Find(x => x.DeliverID == deliverID);
            if (car != null)
            {
                m_AllDeliverCarList.Remove(car);
            }
        }
        /// <summary>
        /// 开始押镖
        /// </summary>
        /// <param name="deliverID">镖物id</param>
        /// <param name="rewardDataList">奖励列表</param>
        /// <param name="characterIDList">押镖弟子ID列表</param>
        public void StartDeliver(int deliverID, List<DeliverRewadData> rewardDataList, List<int> characterIDList)
        {
            //SingleDeliverDetailData data = GameDataMgr.S.GetClanData().AddDeliverData(deliverID, DeliverState.HasBeenSetOut, rewardDataList, characterIDList);
            SingleDeliverDetailData data = GameDataMgr.S.GetClanData().GetDeliverDataByDeliverID(deliverID);

            if (data != null) 
            {
                DeliverCar car = SpawnDeliverCar();
                AddDeliverCarToList(car);
                car.Init(deliverID, data.CharacterIDList, m_DeliverCarOriginPos);

                SetCharacterStateDeliver(data.CharacterIDList, data.DeliverID);

                CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTimeSeconds(), null, (remainTime) =>
                {
                    car.StartMoveComeBack();
                });

                data.SetCountDownID(countDownItem.GetCountDownID());
            }
        }

        /// <summary>
        /// 通过镖物ID获取镖车物体
        /// </summary>
        /// <param name="deliverID">镖物id</param>
        /// <returns></returns>
        public DeliverCar GetDeliverCarByDeliverID(int deliverID)
        {
            DeliverCar car = m_AllDeliverCarList.Find(x => x.DeliverID == deliverID);
            return car;
        }
        #endregion

        #region private
        private void OnDeliverCarArriveCallBack(int key, object[] param)
        {
            if (param != null && param.Length > 0) 
            {
                int deliverID = int.Parse(param[0].ToString());

                SingleDeliverDetailData data = GameDataMgr.S.GetClanData().GetDeliverDataByDeliverID(deliverID);

                if (data != null) 
                {
                    GetDeliverReward(data);
                }
            }
        }
        /// <summary>
        /// 将角色设置为押镖状态
        /// </summary>
        /// <param name="characterIDList">角色ID列表</param>
        /// <param name="deliverID">镖物ID</param>
        private void SetCharacterStateDeliver(List<int> characterIDList,int deliverID) 
        {
            if (characterIDList != null) 
            {
                for (int i = 0; i < characterIDList.Count; i++)
                {
                    int characterID = characterIDList[i];
                    CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterID);
                    if (characterController != null) 
                    {
                        characterController.SetDeliverID(deliverID);
                        characterController.SetState(CharacterStateID.Deliver, FacilityType.Deliver, string.Empty, -1);                        
                    }
                }
            }
        }
        /// <summary>
        /// 获取押镖的奖励
        /// </summary>
        /// <param name="data">镖物的数据类</param>
        private void GetDeliverReward(SingleDeliverDetailData data)
        {
            List<int> characterIDList = data.CharacterIDList;
            for (int i = 0; i < characterIDList.Count; i++)
            {
                int characterID = characterIDList[i];
                CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterID);
                if (characterController != null)
                {
                    characterController.SetDeliverID(-1);
                    characterController.StopNavAgent();
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
        #endregion

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
    }
}