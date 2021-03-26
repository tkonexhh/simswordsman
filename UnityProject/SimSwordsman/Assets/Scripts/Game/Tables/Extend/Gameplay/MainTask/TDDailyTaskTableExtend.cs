using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDDailyTaskTable
    {
        private static Dictionary<int, List<TDDailyTask>> m_DailyTaskLvlMap = new Dictionary<int, List<TDDailyTask>>();
        static void CompleteRowAdd(TDDailyTask tdData)
        {
            int level = tdData.homeLevel;
            List<TDDailyTask> taskLst;
            m_DailyTaskLvlMap.TryGetValue(level, out taskLst);
            if (taskLst == null)
            {
                taskLst = new List<TDDailyTask>();
                taskLst.Add(tdData);
                m_DailyTaskLvlMap.Add(level, taskLst);
            }
            else
            {
                taskLst.Add(tdData);
            }
        }

        public static List<TDDailyTask> GetDailyTasksByLvl(int level)
        {
            List<TDDailyTask> lst = new List<TDDailyTask>();
            for (int i = 1; i <= level; i++)
            {
                List<TDDailyTask> tempLst;
                m_DailyTaskLvlMap.TryGetValue(i, out tempLst);
                if (tempLst != null)
                    lst.AddRange(tempLst);
            }
            return lst;
        }
    }
}