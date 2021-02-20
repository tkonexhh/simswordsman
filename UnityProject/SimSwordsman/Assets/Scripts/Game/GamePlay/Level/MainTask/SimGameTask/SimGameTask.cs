using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private int taskId;
        private string tableName;
        private TaskState taskState;
        private int taskTime;

        public SimGameTask(int taskId, string tableName, TaskState taskState, int taskTime, List<int> recordCharacterID = null)
        {
            m_TaskId = taskId;
            if (recordCharacterID!=null)
            {
                foreach (var item in recordCharacterID)
                    m_RecordCharacterController.Add(MainGameMgr.S.CharacterMgr.GetCharacterController(item));
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
            return m_RecordCharacterController;
        }
        /// <summary>
        /// 清除弟子所拥有的当前任务ID
        /// </summary>
        private void ClearCharacterTaskID()
        {
            List<CharacterController> m_CharacterController = MainGameMgr.S.CharacterMgr.GetAllCharacterInTask(m_TaskDetailInfo.id);
            foreach (var item in m_CharacterController)
            {
                if (item.CharacterModel.GetCurTaskId()== m_TaskDetailInfo.id)
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

        //public void SetCurTaskFinished()
        //{
        //     m_TaskDetailInfo.taskState = TaskState.Finished;
        //}

        //public int GetCurSubType()
        //{
        //   return CommonTaskItemInfo.subType;
        //}

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
            //List<LevelReward> levelRewardList = new List<LevelReward>();
            //ExpCharacterReward exp = new ExpCharacterReward(RewardItemType.Exp_Role, CommonTaskItemInfo.expReward);
            //ExpKongfuReward KungfuExp = new ExpKongfuReward(RewardItemType.Exp_Kongfu, CommonTaskItemInfo.kongfuReward);
            //levelRewardList.Add(exp);
            //levelRewardList.Add(KungfuExp);
            //if (isSucess)
            //    levelRewardList.ForEach(i => i.ApplyReward(1));
            //else
            //    levelRewardList.ForEach(i => i.ApplyReward(2));
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

            // Item reward
            for (int i = 0; i < m_TaskDetailInfo.itemRewards.Count; i++)
            {
                int itemId = m_TaskDetailInfo.GetRewardId(i);
                int count = m_TaskDetailInfo.GetRewardValue(i);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);
            }

            // Special reward
            int random = Random.Range(0, 10000);
            if (random < m_TaskDetailInfo.specialRewardRate)
            {
                for (int i = 0; i < m_TaskDetailInfo.specialRewards.Count; i++)
                {
                    int itemId = m_TaskDetailInfo.GetSpecialRewardId(i);
                    int count = m_TaskDetailInfo.GetSpecialRewardValue(i);
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);
                }
            }

            CharacterIDs.Clear();
        }
    }
	
}