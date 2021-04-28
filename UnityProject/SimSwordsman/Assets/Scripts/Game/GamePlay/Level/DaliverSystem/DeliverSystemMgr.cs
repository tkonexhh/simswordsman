using Qarth;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameWish.Game
{
    public class DeliverSystemMgr : TSingleton<DeliverSystemMgr>, IAssetPreloader
    {
        #region 字段
        private ResLoader m_DeliverCarLoader = null;

        private const string DeliverCarPrefabName = "DeliverCar";

        private List<SingleDeliverDetailData> m_DaliverDetailDataList = null;
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

        private Vector2 m_DeliverCarOriginPos = new Vector3(0.8f, -4.2f, 0);
        /// <summary>
        /// 镖车回来时的间隔时间，若同时回来，则间隔一段时间分别回来
        /// </summary>
        private float m_DeliverCarComeBackIntervalTime = 5.0f;

        private Queue<DeliverCar> m_DeliverCarComeBackQueue = new Queue<DeliverCar>();
        private Queue<DeliverCar> m_DeliverCarGoOutQueue = new Queue<DeliverCar>();
        #endregion

        #region public
        public void OnInit()
        {
            m_DaliverDetailDataList = GameDataMgr.S.GetClanData().DeliverData.GetSingleDeliverDetailDataList();
            if (m_DaliverDetailDataList != null && m_DaliverDetailDataList.Count <= TDDeliverTable.GetDeliverConfigList().Count)
            {
                List<DeliverConfig> deliverConfigs = new List<DeliverConfig>();
                deliverConfigs.AddRange(TDDeliverTable.GetDeliverConfigList());
                for (int i = 0; i < deliverConfigs.Count; i++)
                {
                    if (i < m_DaliverDetailDataList.Count)
                    {
                        if (m_DaliverDetailDataList[i].DeliverID == deliverConfigs[i].level)
                            continue;
                    }
                    GameDataMgr.S.GetClanData().DeliverData.AddDeliverData(deliverConfigs[i].level, DeliverState.Unlock, GetRandomReward(deliverConfigs[i]), new List<int>());
                }
            }

            EventSystem.S.Register(EventID.OnDeliverCarArrive, OnDeliverCarArriveCallBack);

            m_DeliverPath = GameObject.FindObjectOfType<DeliverPath>();

            //检测押镖任务
            List<SingleDeliverDetailData> dataList = GameDataMgr.S.GetClanData().GetAllDaliverData();
            var tmpDataList = dataList.Where(x => x.DaliverState == DeliverState.HasBeenGoOut).ToList();
            if (tmpDataList != null && tmpDataList.Count > 0)
            {
                var alreadyFinishedDeliverDataList = new List<SingleDeliverDetailData>();
                for (int i = 0; i < tmpDataList.Count; i++)
                {
                    SingleDeliverDetailData deliverData = tmpDataList[i];

                    if (deliverData.DaliverState == DeliverState.HasBeenGoOut)
                    {
                        CountDown countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(deliverData.GetRemainTimeSeconds(), null, (remainTime) =>
                        {
                            DeliverCar car = SpawnDeliverCar();
                            car.Init(deliverData.DeliverID, deliverData.CharacterIDList, GoOutSidePos);
                            AddDeliverCarToList(car);
                            //car.StartMoveComeBack();

                            m_DeliverCarComeBackQueue.Enqueue(car);
                        });
                        deliverData.SetCountDownID(countDownItem.GetCountDownID());
                        countDownItem.SetSpeedUpMultiply(deliverData.SpeedUpMultiple);
                    }
                }
            }

            MainGameMgr.S.StartCoroutine(DeliverCarWaitIntervalTimeComeBack());
        }
        public List<DeliverRewadData> GetRandomReward(DeliverConfig deliverConfig)
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
        public void StartDeliver(int deliverID)
        {
            //SingleDeliverDetailData data = GameDataMgr.S.GetClanData().AddDeliverData(deliverID, DeliverState.HasBeenSetOut, rewardDataList, characterIDList);
            SingleDeliverDetailData data = GameDataMgr.S.GetClanData().GetDeliverDataByDeliverID(deliverID);

            if (data != null && data.DaliverState != DeliverState.HasBeenGoOut)
            {
                data.DaliverState = DeliverState.HasBeenGoOut;

                data.UpdateStartTime();

                DeliverCar car = SpawnDeliverCar();
                car.Init(deliverID, data.CharacterIDList, m_DeliverCarOriginPos);
                AddDeliverCarToList(car);

                SetCharacterStateDeliver(data.CharacterIDList, data.DeliverID);

                CountDown countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTimeSeconds(), null, (remainTime) =>
                {
                    //car.StartMoveComeBack();

                    m_DeliverCarComeBackQueue.Enqueue(car);
                });

                data.SetCountDownID(countDownItem.GetCountDownID());
            }
            else
            {
                Debug.LogError("镖物数据为null或者已经出发 ：" + (data == null ? "null" : data.DaliverState.ToString()));
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
        public void UpdateDeliverSpeedUpMultiple(int deliverID, int speedUpMultiple = 2)
        {
            GameDataMgr.S.GetClanData().SetSpeedUpMultipleByDeliverID(deliverID, speedUpMultiple);
        }
        public void AddDeliverCarGoOut(DeliverCar car)
        {
            if (m_DeliverCarGoOutQueue.Contains(car) == false)
            {
                m_DeliverCarGoOutQueue.Enqueue(car);
            }
        }
        #endregion

        #region private
        /// <summary>
        /// 镖车间隔回来
        /// </summary>
        /// <param name="dataList">镖车数据</param>
        /// <returns></returns>
        private IEnumerator DeliverCarWaitIntervalTimeComeBack()
        {
            while (true)
            {
                if (m_DeliverCarComeBackQueue.Count > 0)
                {
                    DeliverCar car = m_DeliverCarComeBackQueue.Dequeue();
                    if (car != null)
                    {
                        car.StartMoveComeBack();
                    }
                    yield return new WaitForSeconds(m_DeliverCarComeBackIntervalTime);
                }
                else
                {
                    yield return null;
                }

                if (m_DeliverCarGoOutQueue.Count > 0)
                {
                    DeliverCar car = m_DeliverCarGoOutQueue.Dequeue();
                    if (car != null)
                    {
                        car.StartMoveGoOut();
                    }
                    yield return new WaitForSeconds(m_DeliverCarComeBackIntervalTime);
                }
                else
                {
                    yield return null;
                }
            }
        }
        /// <summary>
        /// 镖车到达后
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        private void OnDeliverCarArriveCallBack(int key, object[] param)
        {
            if (param != null && param.Length > 0)
            {
                int deliverID = int.Parse(param[0].ToString());

                SingleDeliverDetailData data = GameDataMgr.S.GetClanData().GetDeliverDataByDeliverID(deliverID);

                if (data != null)
                {
                    GetDeliverReward(data);

                    data.ResetData();
                }
            }
        }
        /// <summary>
        /// 将角色设置为押镖状态
        /// </summary>
        /// <param name="characterIDList">角色ID列表</param>
        /// <param name="deliverID">镖物ID</param>
        private void SetCharacterStateDeliver(List<int> characterIDList, int deliverID)
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
            //GameDataMgr.S.GetClanData().RemoveDeliverDataByID(data.DeliverID);
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
            rewardList.ForEach(x => x.AcceptReward());

            if (MainGameMgr.S.BattleFieldMgr.IsBattleing == false)
            {
                UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewardList);
            }
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
