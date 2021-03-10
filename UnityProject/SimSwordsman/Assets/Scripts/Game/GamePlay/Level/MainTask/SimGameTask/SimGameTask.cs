using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public abstract class SimGameTask
    {
        protected int m_TaskId;
        protected CommonTaskItemInfo m_TaskDetailInfo = null;
        //protected string m_TaskStartTime = string.Empty;

        public CommonTaskItemInfo CommonTaskItemInfo { get { return m_TaskDetailInfo; } }

        //public string TaskStartTime { get => m_TaskStartTime;}
        public int TaskId { get => m_TaskId; }

        public List<int> CharacterIDs = new List<int>();
        public List<CharacterController> m_RecordCharacterController = new List<CharacterController>();


        public SimGameTask(int taskId, string tableName, TaskState taskState, int taskTime, List<int> recordCharacterID = null)
        {
            m_TaskId = taskId;
            if (recordCharacterID != null)
            {
                foreach (var item in recordCharacterID)
                {
                    CharacterController controller = MainGameMgr.S.CharacterMgr.GetCharacterController(item);
                    if (controller != null)
                    {
                        m_RecordCharacterController.Add(controller);
                    }
                }
            }


            m_TaskDetailInfo = TDCommonTaskTable.GetMainTaskItemInfo(taskId);
            if (m_TaskDetailInfo == null)
            {
                Debug.LogError("Task info not found, id: " + taskId);
            }

            m_TaskDetailInfo.taskState = taskState;
            m_TaskDetailInfo.taskTime = taskTime;
        }

        /// <summary>
        /// 记录参与任务的角色ID
        /// </summary>
        /// <param name="values"></param>
        public void RecordDiscipleID(Dictionary<int, CharacterItem> values)
        {
            //ClearCharacterTaskID();
            //m_RecordCharacterController = Transformation(values);
            List<int> IDList = new List<int>();
            foreach (var item in values.Values)
                IDList.Add(item.id);
            m_RecordCharacterController = Transformation(values);
            GameDataMgr.S.GetCommonTaskData().SetRecordChracterID(m_TaskId, IDList);
        }

        public List<CharacterController> GetRecordCharacterController()
        {
            return m_RecordCharacterController.Where(i => i != null).ToList();
        }

        public void RemoveCharacter(int id)
        {
            CharacterController character = m_RecordCharacterController.FirstOrDefault(i => i.CharacterId == id);
            if (character != null)
            {
                m_RecordCharacterController.Remove(character);
            }
        }
        /// <summary>
        /// 清除弟子所拥有的当前任务ID
        /// </summary>
        private void ClearCharacterTaskID()
        {
            List<CharacterController> m_CharacterController = MainGameMgr.S.CharacterMgr.GetAllCharacterInTask(m_TaskDetailInfo.id);
            foreach (var item in m_CharacterController)
            {
                if (item.CharacterModel.GetCurTaskId() == m_TaskDetailInfo.id)
                    item.CharacterModel.ClearCurTask(this);
            }
        }
        private List<CharacterController> Transformation(Dictionary<int, CharacterItem> m_SelectedDiscipleDic)
        {
            List<CharacterController> characterController = new List<CharacterController>();
            foreach (var item in m_SelectedDiscipleDic.Values)
                characterController.Add(MainGameMgr.S.CharacterMgr.GetCharacterController(item.id));
            return characterController;
        }

        public TaskState GetCurTaskState()
        {
            return m_TaskDetailInfo.taskState;
        }
        public SimGameTaskType GetCurTaskType()
        {
            return CommonTaskItemInfo.taskType;
        }

        public virtual void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            foreach (var item in selectedCharacters)
                CharacterIDs.Add(item.CharacterId);
            m_TaskDetailInfo.taskState = TaskState.Running;
            GameDataMgr.S.GetCommonTaskData().OnTaskStarted(TaskId);
        }

        public void ClaimReward(bool isSucess)
        {
            List<CharacterController> characters = MainGameMgr.S.CharacterMgr.GetAllCharacterInTask(m_TaskId);
            float ratio = isSucess ? 1 : 0.5f;

            // Add exp
            characters.ForEach(i =>
            {
                i.AddExp((int)(CommonTaskItemInfo.expReward * ratio));
            });

            // Add kongfu exp
            characters.ForEach(i =>
            {
                i.CharacterModel.DistributionKungfuExp((int)(CommonTaskItemInfo.kongfuReward * ratio));
            });
            int allWeight = 0;

            if (!isSucess)
                return;

            foreach (var item in m_TaskDetailInfo.itemRewards)
            {
                allWeight += item.weight;
            }

            int randomNum = Random.Range(0, allWeight);
            allWeight = 0;
            List<RewardBase> rewards = new List<RewardBase>();
            // Item reward
            foreach (var item in m_TaskDetailInfo.itemRewards)
            {
                allWeight += item.weight;
                if (randomNum < allWeight)
                {
                    switch (item.rewardType)
                    {
                        case TaskRewardType.Coin:
                            GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(item.count1);
                            rewards.Add(new CoinReward(item.count1));
                            break;
                        case TaskRewardType.Item:
                            int itemRan = Random.Range(item.count1, item.count1 + 1);
                            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)item.id), itemRan);
                            rewards.Add(new ItemReward( item.id, itemRan));
                            break;
                        case TaskRewardType.Medicine:
                            int medicRan = Random.Range(item.count1, item.count1 + 1);
                            MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)item.id), medicRan);
                            rewards.Add(new MedicineReward(item.id, medicRan));
                            break;
                        case TaskRewardType.Kongfu:
                            int kungfuRan = Random.Range(item.count1, item.count1 + 1);
                            MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KungfuType)item.id), kungfuRan);
                            rewards.Add(new KongfuReward(item.id, kungfuRan));
                            break;
                        case TaskRewardType.Food:
                            GameDataMgr.S.GetPlayerData().AddFoodNum(item.count1);
                            rewards.Add(new FoodsReward( item.count1));
                            break;
                        default:
                            break;
                    }
                }
            }
            EventSystem.S.Send(EventID.OnReciveRewardList, rewards);
            CharacterIDs.Clear();
        }
    }

}