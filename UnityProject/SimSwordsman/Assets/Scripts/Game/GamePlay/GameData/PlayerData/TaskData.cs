using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TaskData : IDataClass, IDailyResetData
    {
        public DailyTaskData dailyTaskData = new DailyTaskData();

        public override void InitWithEmptyData()
        {

        }

        public override void OnDataLoadFinish()
        {

        }

        public void ResetDailyData()
        {
            dailyTaskData.ResetDailyData();
        }
    }

    public class DailyTaskData : IDataClass, IDailyResetData
    {
        public List<int> completeTaskIdLst = new List<int>();

        public void ResetDailyData()
        {
            completeTaskIdLst.Clear();
        }

        public bool AddCompleteID(int id)
        {
            if (completeTaskIdLst.Contains(id))
            {
                return false;
            }
            completeTaskIdLst.Add(id);
            SetDataDirty();
            return true;
        }

        public bool HasCompleteID(int id)
        {
            return completeTaskIdLst.Contains(id);
        }

    }

}