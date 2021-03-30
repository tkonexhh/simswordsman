using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDWorkTable
    {
        public static List<WorkConfigItem> workList = new List<WorkConfigItem>();

        static void CompleteRowAdd(TDWork tdData)
        {
            WorkConfigItem workInfo = new WorkConfigItem(tdData);

            workList.Add(workInfo);
        }

        public static WorkConfigItem GetWorkConfigItem(CollectedObjType collectedObjType)
        {
            WorkConfigItem workConfigItem = workList.FirstOrDefault(i => i.collectedObjType == collectedObjType);
            return workConfigItem;
        }
    }

    public class ItemTipsConfig
    {
        public string name = string.Empty;
        public string desc = string.Empty;

        public ItemTipsConfig()
        {
         
        }
        public ItemTipsConfig(string functionDesc)
        {
            string[] cont = functionDesc.Split(';');
            if (cont.Length == 2)
            {
                AddCont(cont[0], cont[1]);
            }
            else
                Log.w("cont.Length is error : " + cont);
        }
        public void AddCont(string name, string desc)
        {
            this.name = name;
            this.desc = desc;
        }
    }

    public class WorkConfigItem
    {
        public int workId;
        public int unlockHomeLevel;
        public CollectedObjType collectedObjType;
        public string workName;
        public List<string> workTalk;
        public List<TaskReward> itemRewards = new List<TaskReward>();
        public List<TaskReward> specialRewards = new List<TaskReward>();
        public ItemTipsConfig unlockDesc = new ItemTipsConfig ();
        public ItemTipsConfig functionDesc = new ItemTipsConfig ();
        public int workTime;
        public int workInterval;
        public int waitingTime;
        public int storeAmount;
        public int maxWorkManCount;

        public WorkConfigItem(TDWork tdData)
        {
            this.workId = tdData.workID;
            this.unlockHomeLevel = tdData.homeLevel;
            this.collectedObjType = EnumUtil.ConvertStringToEnum<CollectedObjType>(tdData.collectObjType);
            this.workName = tdData.workName;
            this.workTalk = tdData.workTalkLst;
            ParseReward(tdData.reward);
            ParseSpecialReward(tdData.speReward);
            ParseItemTipsDesc(unlockDesc, tdData.unlockDesc);
            ParseItemTipsDesc(functionDesc,tdData.functionDesc);
            this.workTime = tdData.workTime;
            this.workInterval = tdData.workInterval;
            this.waitingTime = tdData.waitingTime;
            this.storeAmount = tdData.storeAmount;
            this.maxWorkManCount = tdData.meanWhileWorkman;
        }

        private void ParseItemTipsDesc(ItemTipsConfig itemTipsConfig, string functionDesc)
        {
            string[] cont = functionDesc.Split(';');
            if (cont.Length == 2)
            {
                itemTipsConfig.AddCont(cont[0], cont[1]);
            }
            else
                Log.w("cont.Length is error : " + cont);
        }

        private void ParseReward(string reward)
        {
            string[] rewardStrs = reward.Split(';');
            foreach (string item in rewardStrs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    TaskReward taskReward = new TaskReward(item);
                    this.itemRewards.Add(taskReward);
                }
            }
        }

        private void ParseSpecialReward(string reward)
        {
            if (string.IsNullOrEmpty(reward))
                return;

            string[] rewardStrs = reward.Split(';');
            foreach (string item in rewardStrs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    TaskReward taskReward = new TaskReward(item);
                    this.specialRewards.Add(taskReward);
                }
            }
        }

        public string RandomTalk()
        {
            return this.workTalk[RandomHelper.Range(0, this.workTalk.Count)];
        }
    }
}