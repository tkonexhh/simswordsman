using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    /// <summary>
    /// ��Ҫ�ֶ��������ݿ��е�����finished��״̬
    /// </summary>
	public class ManualFinishChecker : ConditionCheckerBase<int>
	{
        public override bool IsFinished()
        {
            MainTaskItemData item = GameDataMgr.S.GetMainTaskData().GetMainTaskItemData(m_TaskId);
            if (item != null)
            {
                Log.e("Manual task is finished");

                return item.taskState == TaskState.Unclaimed;
            }

            return false;
        }
    }
	
}