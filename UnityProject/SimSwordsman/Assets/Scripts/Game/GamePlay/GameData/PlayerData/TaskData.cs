using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TaskData : IDataClass, IDailyResetData
    {
        public MainTaskData mainTaskData = new MainTaskData();
        public DailyTaskData dailyTaskData = new DailyTaskData();

        public override void InitWithEmptyData()
        {

        }

        public override void OnDataLoadFinish()
        {
            mainTaskData.SetDirtyRecorder(m_Recorder);
            dailyTaskData.SetDirtyRecorder(m_Recorder);
        }

        public void ResetDailyData()
        {
            dailyTaskData.ResetDailyData();
        }
    }

    public class MainTaskData : IDataClass
    {
        public int curIndex;

        public void FinishMainTask()
        {
            curIndex++;
            SetDataDirty();
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