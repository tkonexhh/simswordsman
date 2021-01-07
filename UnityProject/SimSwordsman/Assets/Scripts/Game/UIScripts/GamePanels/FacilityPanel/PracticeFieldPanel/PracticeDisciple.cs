using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum PracticeFieldState
    {
        None,
        /// <summary>
        /// 空闲中
        /// </summary>
        Free,
        /// <summary>
        /// 未解锁
        /// </summary>
        NotUnlocked,
        /// <summary>
        /// 抄经中
        /// </summary>
        CopyScriptures,
        /// <summary>
        /// 练功中
        /// </summary>
        Practice,
    }


    public class PracticeDisciple : MonoBehaviour,ItemICom
    {
        [SerializeField]
        private Text m_PracticePos;
        [SerializeField]
        private Text m_Time;
        [SerializeField]
        private Text m_CurPractice;
        [SerializeField]
        private Image m_PracticeImg;
        [SerializeField]
        private Button m_PracticeBtn;
        private FacilityType m_CurFacilityType;
        private int m_CurLevel;
        private PracticeField m_PracticeFieldInfo = null;
        private int m_CountDown = 0;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            EventSystem.S.Register(EngineEventID.OnAfterApplicationFocusChange, HandleAddListenerEvent);
            m_PracticeFieldInfo = t as PracticeField;
            m_CurFacilityType = (FacilityType)obj[0];
            BindAddListenEvent();
            RefreshFixedInfo();
            m_PracticePos.text = "练功位:" + m_PracticeFieldInfo.Index;
            RefreshPracticeFieldState();
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {

            if ((bool)param[0])
            {
                //切换到前台时执行，游戏启动时执行一次
            }
            else
            {
                //切换到后台时执行

                RefreshPracticeFieldState();

            }
        }

        private void BindAddListenEvent()
        {
            m_PracticeBtn.onClick.AddListener(()=> {
                UIMgr.S.OpenPanel(UIID.ChooseDisciplePanel, m_PracticeFieldInfo, m_CurFacilityType, m_CurLevel);
            });
        }

        public PracticeFieldState GetPracticeFieldState()
        {
            return m_PracticeFieldInfo.PracticeFieldState;
        }

        public void IncreaseCountDown(int time)
        {
            CountDownMgr countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index);
            if (countDownMgr != null)
            {
                countDownMgr.IncreasTickTime(time);
            }
        }

        private void RefreshFixedInfo()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
        }

        public void RefreshPracticeFieldState()
        {

            switch (m_PracticeFieldInfo.PracticeFieldState)
            {
                case PracticeFieldState.None:
                    break;
                case PracticeFieldState.Free:
                    m_PracticeBtn.enabled = true;
                    m_CurPractice.text = "安排弟子";
                    m_Time.text = "空闲";
                    break;
                case PracticeFieldState.NotUnlocked:
                    m_PracticeBtn.enabled = false;
                    m_CurPractice.text = "练功场" + m_PracticeFieldInfo.UnlockLevel + "级后解锁";
                    m_Time.text = Define.COMMON_DEFAULT_STR;
                    break;
                case PracticeFieldState.CopyScriptures:
                    break;
                case PracticeFieldState.Practice:
                    RefreshFixedInfo();
                    m_CurPractice.text = "当前训练:" + m_PracticeFieldInfo.CharacterItem.name;
                    m_Time.text = SplicingTime(GetDuration());
                    CreateCountDown();
                    //TimeRemaining(m_PracticeFieldInfo.StartTime);
                    m_PracticeBtn.enabled = true;
                    break;
                default:
                    break;
            }
        }
        public string SplicingTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }

        private void CreateCountDown()
        {
            CountDownMgr countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index);
            if (countDownMgr == null)
            {
                m_CountDown = GetDuration();
                countDownMgr = new CountDownMgr(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index, m_CountDown);

            }
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            countDownMgr.OnSecondRefreshEvent = refresAction;
            if (countDownMgr.OnCountDownOverEvent == null)
                countDownMgr.OnCountDownOverEvent = m_PracticeFieldInfo.overAction;
        }

        private int GetDuration()
        {
            int duration = MainGameMgr.S.FacilityMgr.GetDurationForLevel(m_CurFacilityType, m_CurLevel);
            int takeTime = ComputingTime(m_PracticeFieldInfo.StartTime);
            return  duration - takeTime;
        }

        public void refresAction(string obj)
        {
            if (m_Time != null)
                m_Time.text = obj;
        }

        private int ComputingTime(string  time)
        {
            DateTime dateTime;
            DateTime.TryParse(time,out dateTime);
            if (dateTime!=null)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                return (int)timeSpan.TotalSeconds;
            }
            return 0;
        }

        public IEnumerator BattleCountdown()
        {
            while (m_CountDown >= 0)
            {
                if (m_CountDown == 0)
                {
                  //  AddExperience(m_PracticeFieldInfo.CharacterItem);
                    m_PracticeFieldInfo.TrainingIsOver();
                    StopCoroutine("BattleCountdown");
                    break;
                }
               // m_Time.text = SplicingTime(m_CountDown);
                yield return new WaitForSeconds(1);
                m_CountDown--;
            }
        }

        public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            EventSystem.S.UnRegister(EngineEventID.OnAfterApplicationFocusChange, HandleAddListenerEvent);
        }
    }
}