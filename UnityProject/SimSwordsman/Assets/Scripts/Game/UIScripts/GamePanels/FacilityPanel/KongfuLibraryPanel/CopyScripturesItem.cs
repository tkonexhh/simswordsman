using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class CopyScripturesItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Text m_CopyScripturesPos;
        [SerializeField]
        private Text m_Time;
        [SerializeField]
        private Text m_CurCopyScriptures;
        [SerializeField]
        private Text m_ArrangeDisciple;
        [SerializeField]
        private Text m_Free;
        [SerializeField]
        private Image m_DiscipleImg;
        [SerializeField]
        private Image m_DiscipleHead;
        [SerializeField]
        private Image m_Lock;
        [SerializeField]
        private Image m_Plus;
        [SerializeField]
        private Button m_CopyScripturesBtn;

        private int m_CountDown = 0;
        private int m_CurLevel;
        private FacilityType m_CurFacility;
        private KungfuLibraySlot m_KungfuLibraySlot = null;
        private AddressableAssetLoader<Sprite> m_Loader;
        private KongfuLibraryPanel m_KongfuLibraryPanel;
        // public void LoadClanPrefabs(string prefabsName)
        // {
        //     m_Loader = new AddressableAssetLoader<Sprite>();
        //     m_Loader.LoadAssetAsync(prefabsName, (obj) =>
        //     {
        //         //Debug.Log(obj);
        //         m_DiscipleHead.sprite = obj;
        //     });
        // }
        private void OnDestroy()
        {
            if (m_Loader != null)
            {
                m_Loader.Release();
            }
        }
        // private void OnDisable()
        // {
        // }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            BindAddListenerEvent();
            m_CurFacility = (FacilityType)obj[0];
            m_KongfuLibraryPanel = obj[1] as KongfuLibraryPanel;
            m_KungfuLibraySlot = t as KungfuLibraySlot;
            m_CopyScripturesPos.text = "抄经位:" + m_KungfuLibraySlot.Index;
            RefreshPracticeFieldState();
        }

        public SlotState GetPracticeFieldState()
        {
            return m_KungfuLibraySlot.slotState;
        }
        public void IncreaseCountDown(int time)
        {
            CountDownItem countDown = null;
            countDown = TimeUpdateMgr.S.IsHavaITimeObserver(m_KungfuLibraySlot.FacilityType.ToString() + m_KungfuLibraySlot.Index);
            if (countDown != null)
                countDown.IncreasTickTime(time);
        }
        private void BindAddListenerEvent()
        {
            m_CopyScripturesBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (CheckIsEmpty())
                    UIMgr.S.OpenPanel(UIID.KungfuChooseDisciplePanel, m_KungfuLibraySlot, m_CurFacility);
                else
                {
                    FloatMessage.S.ShowMsg("暂时没有空闲的弟子，等会儿再试试吧");
                }
            });
        }

        private bool CheckIsEmpty()
        {
            bool isEntry = false;
            List<CharacterItem> characterItems = MainGameMgr.S.CharacterMgr.GetAllCharacterList();

            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            int maxLevel = TDFacilityLobbyTable.GetPracticeLevelMax(lobbyLevel);
            for (int i = 0; i < characterItems.Count; i++)
            {
                if (characterItems[i].IsFreeState() && characterItems[i].level < Define.CHARACTER_MAX_LEVEL)
                    isEntry = true;
            }
            return isEntry;
        }

        public void SetButtonEvent(Action<object> action)
        {
        }

        public void RefreshPracticeFieldState()
        {

            switch (m_KungfuLibraySlot.slotState)
            {
                case SlotState.None:
                    break;
                case SlotState.Free:
                    m_CopyScripturesBtn.enabled = true;
                    m_ArrangeDisciple.text = "安排弟子";
                    m_CurCopyScriptures.text = Define.COMMON_DEFAULT_STR;
                    m_Time.gameObject.SetActive(false);
                    m_Free.gameObject.SetActive(false);
                    m_Plus.gameObject.SetActive(true);
                    m_Lock.gameObject.SetActive(false);
                    m_DiscipleHead.gameObject.SetActive(false);
                    break;
                case SlotState.NotUnlocked:
                    m_Plus.gameObject.SetActive(false);
                    m_Lock.gameObject.SetActive(true);
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    m_CopyScripturesBtn.enabled = false;
                    m_CurCopyScriptures.text = Define.COMMON_DEFAULT_STR;
                    m_Free.text = "抄经位" + m_KungfuLibraySlot.UnlockLevel + "级后解锁";
                    m_Time.gameObject.SetActive(false);
                    m_DiscipleHead.gameObject.SetActive(false);
                    break;
                case SlotState.CopyScriptures:
                    m_Plus.gameObject.SetActive(false);
                    m_Lock.gameObject.SetActive(false);
                    m_DiscipleHead.gameObject.SetActive(true);
                    RefreshFixedInfo();
                    m_CurCopyScriptures.text = "当前抄经:" + m_KungfuLibraySlot.CharacterItem.name;
                    m_Time.gameObject.SetActive(true);
                    m_Time.text = GameExtensions.SplicingTime(GetDuration());
                    m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
                    m_Free.text = Define.COMMON_DEFAULT_STR;
                    CreateCountDown();
                    m_DiscipleHead.sprite = m_KongfuLibraryPanel.FindSprite(GetLoadDiscipleName(m_KungfuLibraySlot.CharacterItem));
                    //(m_PracticeFieldInfo.StartTime);
                    m_CopyScripturesBtn.enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CreateCountDown()
        {
            CountDownItem countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(m_KungfuLibraySlot.FacilityType.ToString() + m_KungfuLibraySlot.Index);
            if (countDownMgr == null)
            {
                m_CountDown = GetDuration();
                countDownMgr = new CountDownItem(m_KungfuLibraySlot.FacilityType.ToString() + m_KungfuLibraySlot.Index, m_CountDown);
            }
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            countDownMgr.OnSecondRefreshEvent = refresAction;
            if (countDownMgr.OnCountDownOverEvent == null)
                countDownMgr.OnCountDownOverEvent = m_KungfuLibraySlot.overAction;
        }
        public void refresAction(string obj)
        {
            if (m_Time != null)
                m_Time.text = obj;
        }
        private void RefreshFixedInfo()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacility);
        }

        private int GetDuration()
        {
            return m_KungfuLibraySlot.GetDurationTime();
        }
    }

}