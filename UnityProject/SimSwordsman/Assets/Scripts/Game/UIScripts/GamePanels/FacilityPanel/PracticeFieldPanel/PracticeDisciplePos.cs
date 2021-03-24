using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PracticeDisciplePos : MonoBehaviour
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
        private int m_CountDown = 0;
        private PracticeFieldPanel m_PracticeFieldPanel;

        private CDBaseSlot m_Slot = null;

        public void Init(int index, FacilityType type, PracticeFieldPanel panel)
        {
            BindAddListenEvent();
            m_CurFacilityType = type;
            m_PracticeFieldPanel = panel;
            m_PracticePos.text = "练功位:" + index;
            PracticeFieldController controller = (PracticeFieldController)MainGameMgr.S.FacilityMgr.GetFacilityController(m_CurFacilityType);
            m_Slot = controller.GetSlotByIndex(index);
            RefreshPracticeFieldState();
        }

        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }

        private void BindAddListenEvent()
        {
            m_PracticeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (CheckIsEmpty())
                    UIMgr.S.OpenPanel(UIID.ChooseDisciplePanel, m_Slot, m_CurFacilityType);
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
                if (characterItems[i].IsFreeState() && characterItems[i].level < maxLevel)
                    isEntry = true;
            }
            return isEntry;
        }

        public SlotState GetSlotState()
        {
            if (m_Slot != null)
                return m_Slot.slotState;

            return SlotState.NotUnlocked;
        }

        public void IncreaseCountDown(int time)
        {
            if (m_Slot == null)
                return;

            CountDownItem countDown = null;
            countDown = TimeUpdateMgr.S.IsHavaITimeObserver(m_Slot.FacilityType.ToString() + m_Slot.Index);
            if (countDown != null)
                countDown.IncreasTickTime(time);
        }

        public void RefreshPracticeFieldState()
        {
            if (m_Slot == null)
            {
                UILocked();
                return;
            }

            //看看有没有人在这个index上reading
            var allCharacter = MainGameMgr.S.CharacterMgr.CharacterControllerList;
            CharacterItem characterModel = null;
            for (int i = 0; i < allCharacter.Count; i++)
            {
                // Debug.LogError(allCharacter[i].CharacterModel.CharacterItem.GetTargetFacilityType() + "--" + allCharacter[i].CharacterModel.CharacterItem.GetTargetFacilityIndex());
                if (allCharacter[i].CharacterModel.IsAtSlot(m_CurFacilityType, m_Slot.Index))
                {
                    characterModel = allCharacter[i].CharacterModel.CharacterItem;
                    break;
                }
            }

            if (characterModel != null)
            {
                UIDuring(characterModel);
            }
            else
            {
                UIFree();
            }
        }

        private void UILocked()
        {
            m_PracticeBtn.enabled = false;
            m_DiscipleHead.gameObject.SetActive(false);
            //FIXME 解锁等级
            m_State.text = "练功场" + m_Slot.UnlockLevel + "级后解锁";
            m_PracticeImg.sprite = m_PracticeFieldPanel.FindSprite("Lock2");
            m_Time.enabled = false;
            m_CurPractice.text = Define.COMMON_DEFAULT_STR;
            m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
        }

        private void UIFree()
        {
            m_PracticeBtn.enabled = true;
            m_DiscipleHead.gameObject.SetActive(false);
            m_CurPractice.text = Define.COMMON_DEFAULT_STR;
            m_Time.enabled = false;
            //m_PracticeImg.sprite = ""
            m_ArrangeDisciple.text = "安排弟子";
            m_State.text = "空闲";
        }

        private void UIDuring(CharacterItem characterItem)
        {
            m_Time.enabled = true;
            m_DiscipleHead.gameObject.SetActive(true);
            m_PracticeBtn.enabled = false;
            m_State.text = Define.COMMON_DEFAULT_STR;
            m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
            m_CurPractice.text = "当前训练:" + characterItem.name;
            m_Time.text = GameExtensions.SplicingTime(GetDuration());
            CreateCountDown();
            LoadClanPrefabs(GetLoadDiscipleName(characterItem));
        }

        private void LoadClanPrefabs(string prefabsName)
        {
            m_DiscipleHead.sprite = m_PracticeFieldPanel.FindSprite(prefabsName);
        }

        private void CreateCountDown()
        {
            CountDownItem countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(m_Slot.FacilityType.ToString() + m_Slot.Index);
            if (countDownMgr == null)
            {
                m_CountDown = GetDuration();
                countDownMgr = new CountDownItem(m_Slot.FacilityType.ToString() + m_Slot.Index, m_CountDown);
            }
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            countDownMgr.OnSecondRefreshEvent = OnRefresAction;
            if (countDownMgr.OnCountDownOverEvent == null)
                countDownMgr.OnCountDownOverEvent = m_Slot.overAction;
        }

        private int GetDuration()
        {
            return m_Slot.GetDurationTime();
        }

        public void OnRefresAction(string obj)
        {
            if (m_Time != null)
                m_Time.text = obj;
        }
    }
}