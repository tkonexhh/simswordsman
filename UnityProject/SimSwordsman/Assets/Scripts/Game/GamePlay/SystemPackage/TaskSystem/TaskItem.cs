using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Qarth;
using System;

namespace GameWish.Game
{
	public class TaskItem
	{
        private int m_TaskId;
        //private int m_TaskSubId;
        private Type m_ClassType = null;
        private Action<TaskItem> m_StateChangedCallback = null;

        private bool m_IsFinished = false;
        private bool m_IsEventUnregistered = false;

        private List<IConditionChecker> m_ConditionCheckerList = new List<IConditionChecker>();
        private List<IRewardHandler> m_RewardHandlerList = new List<IRewardHandler>();
        private List<EventID> m_EventList = new List<EventID>();

        private List<string> m_RewardTypeList = new List<string>();
        private List<string> m_RewardValueList = new List<string>();

        public int TaskId { get => m_TaskId; }

        public TaskItem(int id, string tableName, Action<TaskItem> stateChangedCallback)
        {
            m_TaskId = id;
            //m_TaskSubId = subId;
            m_ClassType = Type.GetType(Define.NAME_SPACE_PREFIX + tableName);
            m_StateChangedCallback = stateChangedCallback;

            if (m_ClassType != null)
            {
                ParseTaskConditions();
                ParseTaskEvents();
                ParseTaskRewards();

                RegisterEvent();
            }
            else
            {
                Log.e("TaskTableType is null");
            }
        }

        //~TaskItem()
        //{
        //    UnregisterEvent();
        //}

        #region Public Set
        public void ClaimReward()
        {
            foreach (IRewardHandler rewardHandler in m_RewardHandlerList)
            {
                rewardHandler.OnRewardClaimed();
            }
        }

        public void Release()
        {
            UnregisterEvent();
        }

        #endregion

        #region Public Get
        public int GetId()
        {
            return m_TaskId;
        }

        public bool IsFinished()
        {
            return m_IsFinished;
        }

        public List<string> GetRewardTypeList()
        {
            return m_RewardTypeList;
        }

        public List<string> GetRewardValueList()
        {
            return m_RewardValueList;
        }

        public string GetCurValue()
        {
            return m_ConditionCheckerList[0].GetCurrentValue();
        }

        public string GetTargetValue()
        {
            return m_ConditionCheckerList[0].GetTargetValue();
        }

        public float GetProgressPercent()
        {
            return m_ConditionCheckerList[0].GetProgressPercent();
        }

        #endregion

        #region Private 
        private void RegisterEvent()
        {
            foreach (EventID eventID in m_EventList)
            {
                EventSystem.S.Register(eventID, HandleEvent);
            }
        }

        private void UnregisterEvent()
        {
            if (m_IsEventUnregistered == false)
            {
                foreach (EventID eventID in m_EventList)
                {
                    EventSystem.S.UnRegister(eventID, HandleEvent);
                }

                m_IsEventUnregistered = true;
            }
        }

        private void HandleEvent(int key, params object[] param)
        {
            //m_IsDirty = true;
            if (m_IsFinished == false)
            {
                m_IsFinished = CheckFinish();
                Log.i("Task:" + m_TaskId + " is finished: " + m_IsFinished);

                if (m_IsFinished)
                {
                    UnregisterEvent();
                }

                m_StateChangedCallback.Invoke(this);
            }
        }

        private bool CheckFinish()
        {
            //if (m_IsDirty)
            //{
            foreach (IConditionChecker taskCondition in m_ConditionCheckerList)
            {
                if (taskCondition.IsFinished() == false)
                {
                    //m_IsDirty = false;

                    return false;
                }
            }
            //}
            //else
            //{
            //    return false;
            //}

            return true;
        }

        private void ParseTaskConditions()
        {
            try
            {
                string conditionTypeStr = GetTableValue(TaskDefine.GET_CONDITION_TYPE);
                string[] conditionTypes = conditionTypeStr.Split('|');

                string conditionValueStr = GetTableValue(TaskDefine.GET_CONDITION_VALUE);
                string[] values = conditionValueStr.Split('|');

                for (int i =0; i < conditionTypes.Length; i++)
                {
                    string checkerClassName = Define.NAME_SPACE_PREFIX + conditionTypes[i] + "Checker";
                    string valueStr = values[i];

                    Type type = Type.GetType(checkerClassName);
                    if (type == null)
                    {
                        Log.e("ParseTaskConditions but class not found: " + checkerClassName);
                        continue;
                    }

                    object obj = Activator.CreateInstance(type, true);
                    MethodInfo methodInfo = type.GetMethod("Init");
                    object[] param = BuildParams(m_TaskId, valueStr);
                    methodInfo.Invoke(obj, param);
                    m_ConditionCheckerList.Add((IConditionChecker)obj);  
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        private void ParseTaskEvents()
        {
            try
            {
                string eventStr = GetTableValue(TaskDefine.GET_EVENT);
                string[] events = eventStr.Split('|');

                for (int i = 0; i < events.Length; i++)
                {
                    EventID eventId = StringHelper.ParseStringToEnum<EventID>(events[i]);
                    m_EventList.Add(eventId);
                }
            }
            catch (Exception e)
            {
               Log.e(e);
            }
        }

        private void ParseTaskRewards()
        {
            try
            {
                string str = GetTableValue(TaskDefine.GET_REWARD);
                string[] rewardStrs = str.Split(';');
                //string[] rewardTypes = str.Split('|');

                //string rewardValueStr = GetTableValue(TaskDefine.GET_REWARD_VALUE);
                //string[] values = rewardValueStr.Split('|');

                for (int i = 0; i < rewardStrs.Length; i++)
                {
                    string[] item = str.Split('|');
                    string rewardType = item[0];
                    string rewardValue = item[1];
                    m_RewardTypeList.Add(rewardType);
                    m_RewardValueList.Add(rewardValue);

                    string rewardClassName = Define.NAME_SPACE_PREFIX + rewardType + "RewardHandler";
                    string valueStr = rewardValue;

                    Type type = Type.GetType(rewardClassName);
                    if (type == null)
                    {
                        Log.e("ParseRewards but class not found: " + rewardClassName);
                        continue;
                    }

                    object obj = Activator.CreateInstance(type, true);
                    MethodInfo methodInfo = type.GetMethod("Init");
                    object[] param = BuildParams(m_TaskId, valueStr);
                    methodInfo.Invoke(obj, param);
                    m_RewardHandlerList.Add((IRewardHandler)obj);
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        private string GetTableValue(string colName)
        {
            object[] param = new object[] { m_TaskId };

            MethodInfo methodInfo = m_ClassType.GetMethod(colName);
            string value = (string)methodInfo.Invoke(null, param);
            return value;
        }

        private object[] BuildParams(int id, string value)
        {
            object[] objs;

            if (value.Contains("_"))
            {
                objs = new object[] { id, value };
            }
            else if (value.Contains("E+"))
            {
                objs = new object[] { id, double.Parse(value) };
            }
            else if (value.Contains("."))
            {
                objs = new object[] { id, float.Parse(value) };
            }
            else
            {
                objs = new object[] { id, int.Parse(value) };
            }

            return objs;
        }

        #endregion
    }

}