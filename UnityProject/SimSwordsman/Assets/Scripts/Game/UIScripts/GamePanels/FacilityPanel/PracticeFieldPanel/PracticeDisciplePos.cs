using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PracticeDisciplePos : MonoBehaviour,ItemICom
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
        private Image m_DiscipleHead;
        [SerializeField]
        private Text m_ArrangeDisciple;
        [SerializeField]
        private Text m_State;
        [SerializeField]
        private Button m_PracticeBtn;
        private FacilityType m_CurFacilityType;
        private int m_CurLevel;
        private int m_CountDown = 0;
        private List<Sprite> m_Sprites;
        private PracticeFieldLevelInfo m_PracticeFieldLevelInfo = null;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
        private PracticeField m_PracticeFieldInfo = null;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            EventSystem.S.Register(EngineEventID.OnAfterApplicationFocusChange, HandleAddListenerEvent);
            m_CurFacilityType = (FacilityType)obj[0];
            BindAddListenEvent();
            GetInformationForNeed();

            m_PracticeFieldInfo = t as PracticeField;
            m_Sprites = (List<Sprite>)obj[1];
            RefreshFixedInfo();
            m_PracticePos.text = "练功位:" + m_PracticeFieldInfo.Index;
            RefreshPracticeFieldState();
        }
        public void LoadClanPrefabs(string prefabsName)
        {
            AddressableAssetLoader<Sprite> loader = new AddressableAssetLoader<Sprite>();
            loader.LoadAssetAsync(prefabsName, (obj) =>
            {
                //Debug.Log(obj);
                m_DiscipleHead.sprite = obj;
            });
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType/*, m_SubID*/);
            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_PracticeFieldLevelInfo = (PracticeFieldLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);

        }
        private Sprite GetSprite(string name)
        {
            return m_Sprites.Where(i => i.name.Equals(name)).FirstOrDefault();
        }
        private void HandleAddListenerEvent(int key, object[] param)
        {

        }

        private void BindAddListenEvent()
        {
            m_PracticeBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.ChooseDisciplePanel, m_PracticeFieldInfo, m_CurFacilityType, m_CurLevel);
            });
        }

        public SlotState GetPracticeFieldState()
        {
            return m_PracticeFieldInfo.slotState;
        }

        public void IncreaseCountDown(int time)
        {
            CountDownItem countDown = null;
            countDown = TimeUpdateMgr.S.IsHavaITimeObserver(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index);
            if (countDown != null)
                countDown.IncreasTickTime(time);
        }

        private void RefreshFixedInfo()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
        }

        public void RefreshPracticeFieldState()
        {
            switch (m_PracticeFieldInfo.slotState)
            {
                case SlotState.None:
                    break;
                case SlotState.Free:
                    m_PracticeBtn.enabled = true;
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_CurPractice.text = Define.COMMON_DEFAULT_STR;
                    m_Time.enabled = false;
                    //m_PracticeImg.sprite = ""
                    m_ArrangeDisciple.text = "安排弟子";
                    m_State.text = "空闲";
                    break;
                case SlotState.NotUnlocked:
                    m_PracticeBtn.enabled = false;
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_State.text = "练功场" + m_PracticeFieldInfo.UnlockLevel + "级后解锁";
                    m_PracticeImg.sprite = GetSprite("Lock2");
                    m_Time.enabled = false;
                    m_CurPractice.text = Define.COMMON_DEFAULT_STR;
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    break;
                case SlotState.Practice:
                    m_Time.enabled = true;
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_PracticeBtn.enabled = false;
                    m_State.text = Define.COMMON_DEFAULT_STR;
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    RefreshFixedInfo();
                    m_CurPractice.text = "当前训练:" + m_PracticeFieldInfo.CharacterItem.name;
                    m_Time.text = SplicingTime(GetDuration());
                    CreateCountDown();
                    LoadClanPrefabs(GetLoadDiscipleName(m_PracticeFieldInfo.CharacterItem));
                    //TimeRemaining(m_PracticeFieldInfo.StartTime);
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
            CountDownItem countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index);
            if (countDownMgr == null)
            {
                m_CountDown = GetDuration();
                countDownMgr = new CountDownItem(m_PracticeFieldInfo.FacilityType.ToString() + m_PracticeFieldInfo.Index, m_CountDown);
            }
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            countDownMgr.OnSecondRefreshEvent = OnRefresAction;
            if (countDownMgr.OnCountDownOverEvent == null)
                countDownMgr.OnCountDownOverEvent = m_PracticeFieldInfo.overAction;
        }

        private int GetDuration()
        {
            int duration = MainGameMgr.S.FacilityMgr.GetDurationForLevel(m_CurFacilityType, m_CurLevel);
            int takeTime = ComputingTime(m_PracticeFieldInfo.StartTime);
            return  duration - takeTime;
        }

        public void OnRefresAction(string obj)
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