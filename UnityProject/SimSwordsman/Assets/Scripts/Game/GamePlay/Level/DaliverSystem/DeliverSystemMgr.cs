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
        #region �ֶ�
        private ResLoader m_DeliverCarLoader = null;

        private const string DeliverCarPrefabName = "DeliverCar";

        private List<SingleDeliverDetailData> m_DaliverDetailDataList = null;
        /// <summary>
        /// �ڳ�·��
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
        /// <summary>
        /// �ڳ�����ʱ�ļ��ʱ�䣬��ͬʱ����������һ��ʱ��ֱ����
        /// </summary>
        private float m_DeliverCarComeBackIntervalTime = 5.0f;

        private Queue<DeliverCar> m_DeliverCarComeBackQueue = new Queue<DeliverCar>();
        private Queue<DeliverCar> m_DeliverCarGoOutQueue = new Queue<DeliverCar>();
        #endregion

        #region public
        public void OnInit()
        {
            m_DaliverDetailDataList = GameDataMgr.S.GetClanData().DeliverData.GetSingleDeliverDetailDataList();
            if (m_DaliverDetailDataList != null && m_DaliverDetailDataList.Count == 0)
            {
                List<DeliverConfig> deliverConfigs = new List<DeliverConfig>();
                deliverConfigs.AddRange(TDDeliverTable.GetDeliverConfigList());
                for (int i = 0; i < deliverConfigs.Count; i++)
                {
                    GameDataMgr.S.GetClanData().DeliverData.AddDeliverData(deliverConfigs[i].level, DeliverState.Unlock, GetRandomReward(deliverConfigs[i]), new List<int>());
                }
            }

            EventSystem.S.Register(EventID.OnDeliverCarArrive, OnDeliverCarArriveCallBack);

            m_DeliverPath = GameObject.FindObjectOfType<DeliverPath>();

            //���Ѻ������
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
                        CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(deliverData.GetRemainTimeSeconds(), null, (remainTime) =>
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
        /// ����ڳ��������б���
        /// </summary>
        /// <param name="deliverCar">�ڳ�</param>
        public void AddDeliverCarToList(DeliverCar deliverCar)
        {
            DeliverCar car = m_AllDeliverCarList.Find(x => x.DeliverID == deliverCar.DeliverID);
            if (car == null)
            {
                m_AllDeliverCarList.Add(deliverCar);
            }
        }
        /// <summary>
        /// �����б����Ƴ��ڳ�
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
        /// ��ʼѺ��
        /// </summary>
        /// <param name="deliverID">����id</param>
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

                CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTimeSeconds(), null, (remainTime) =>
                {
                    //car.StartMoveComeBack();

                    m_DeliverCarComeBackQueue.Enqueue(car);
                });

                data.SetCountDownID(countDownItem.GetCountDownID());
            }
            else {
                Debug.LogError("��������Ϊnull�����Ѿ����� ��" + (data == null ? "null" : data.DaliverState.ToString()));
            }
        }
        /// <summary>
        /// ͨ������ID��ȡ�ڳ�����
        /// </summary>
        /// <param name="deliverID">����id</param>
        /// <returns></returns>
        public DeliverCar GetDeliverCarByDeliverID(int deliverID)
        {
            DeliverCar car = m_AllDeliverCarList.Find(x => x.DeliverID == deliverID);
            return car;
        }
        public void UpdateDeliverSpeedUpMultiple(int deliverID,int speedUpMultiple = 2)
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
        /// �ڳ��������
        /// </summary>
        /// <param name="dataList">�ڳ�����</param>
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
                else {
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
        /// �ڳ������
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
        /// ����ɫ����ΪѺ��״̬
        /// </summary>
        /// <param name="characterIDList">��ɫID�б�</param>
        /// <param name="deliverID">����ID</param>
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
        /// ��ȡѺ�ڵĽ���
        /// </summary>
        /// <param name="data">�����������</param>
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