using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class CopyScripturesItem : MonoBehaviour//, ItemICom
    {
        [SerializeField] private Text m_CopyScripturesPos;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_CurCopyScriptures;
        [SerializeField] private Text m_ArrangeDisciple;
        [SerializeField] private Text m_Free;
        [SerializeField] private Image m_DiscipleImg;
        [SerializeField] private Image m_DiscipleHead;
        [SerializeField] private Image m_Lock;
        [SerializeField] private Image m_Plus;
        [SerializeField] private Button m_CopyScripturesBtn;

        private int m_CountDown = 0;

        private CDBaseSlot m_KungfuLibraySlot = null;
        private KongfuLibraryPanel m_KongfuLibraryPanel;
        private int m_Index;


        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }

        public void Init(int index, KongfuLibraryPanel panel)
        {
            BindAddListenerEvent();
            m_Index = index;
            m_CopyScripturesPos.text = "����λ:" + index;
            m_KongfuLibraryPanel = panel;
            KongfuLibraryController kongFuController = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.KongfuLibrary);
            m_KungfuLibraySlot = kongFuController.GetSlotByIndex(m_Index);

            RefreshPracticeFieldState();
        }

        // private bool CheckUnlock()
        // {
        //     int kongfuLibraryLvl = GameDataMgr.S.GetClanData().ownedFacilityData.GetFacilityData(FacilityType.KongfuLibrary).level;
        //     int unlockSeat = TDFacilityKongfuLibraryTable.GetData(kongfuLibraryLvl).seat;
        //     bool isUnlock = m_Index < unlockSeat;
        //     return isUnlock;
        // }

        public SlotState GetPracticeFieldState()
        {
            if (m_KungfuLibraySlot != null)
                return m_KungfuLibraySlot.slotState;
            return SlotState.NotUnlocked;
        }
        public void IncreaseCountDown(int time)
        {
            CountDownItem countDown = null;
            // countDown = TimeUpdateMgr.S.IsHavaITimeObserver(m_KungfuLibraySlot.FacilityType.ToString() + m_KungfuLibraySlot.Index);
            countDown = TimeUpdateMgr.S.IsHavaITimeObserver(GetCountDownKey());
            if (countDown != null)
                countDown.IncreasTickTime(time);
        }
        private void BindAddListenerEvent()
        {
            m_CopyScripturesBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (CheckIsEmpty())
                {
                    UIMgr.S.OpenPanel(UIID.KungfuChooseDisciplePanel, m_KungfuLibraySlot);
                }
                else
                {
                    FloatMessage.S.ShowMsg("��ʱû�п��еĵ��ӣ��Ȼ�������԰�");
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

        public void RefreshPracticeFieldState()
        {
            if (m_KungfuLibraySlot == null)
            {
                UILocked();
                return;
            }

            // if (!CheckUnlock())
            // {
            //     UILocked();
            //     return;
            // }

            //������û���������index��reading
            var allCharacter = MainGameMgr.S.CharacterMgr.CharacterControllerList;
            CharacterItem characterModel = null;
            for (int i = 0; i < allCharacter.Count; i++)
            {
                // Debug.LogError(allCharacter[i].CharacterModel.CharacterItem.GetTargetFacilityType() + "--" + allCharacter[i].CharacterModel.CharacterItem.GetTargetFacilityIndex());
                if (allCharacter[i].CharacterModel.IsAtSlot(FacilityType.KongfuLibrary, m_KungfuLibraySlot.Index))
                {
                    Debug.LogError("Select");
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
            m_Plus.gameObject.SetActive(false);
            m_Lock.gameObject.SetActive(true);
            m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
            m_CopyScripturesBtn.enabled = false;
            m_CurCopyScriptures.text = Define.COMMON_DEFAULT_STR;
            //FIXME �����ȼ�
            // m_Free.text = "����λ" + m_Data.unlockLevel + "�������";
            m_Time.gameObject.SetActive(false);
            m_DiscipleHead.gameObject.SetActive(false);
        }

        private void UIFree()
        {
            m_CopyScripturesBtn.enabled = true;
            m_ArrangeDisciple.text = "���ŵ���";
            m_CurCopyScriptures.text = Define.COMMON_DEFAULT_STR;
            m_Time.gameObject.SetActive(false);
            m_Free.gameObject.SetActive(false);
            m_Plus.gameObject.SetActive(true);
            m_Lock.gameObject.SetActive(false);
            m_DiscipleHead.gameObject.SetActive(false);
        }

        private void UIDuring(CharacterItem characterItem)
        {
            m_Plus.gameObject.SetActive(false);
            m_Lock.gameObject.SetActive(false);
            m_DiscipleHead.gameObject.SetActive(true);
            m_CurCopyScriptures.text = "��ǰ����:" + characterItem.name;
            m_Time.gameObject.SetActive(true);
            m_Time.text = GameExtensions.SplicingTime(GetDuration());
            m_ArrangeDisciple.text = Define.COMMON_DEFAULT_STR;
            m_Free.text = Define.COMMON_DEFAULT_STR;
            CreateCountDown();
            m_DiscipleHead.sprite = m_KongfuLibraryPanel.FindSprite(GetLoadDiscipleName(characterItem));
            //(m_PracticeFieldInfo.StartTime);
            m_CopyScripturesBtn.enabled = false;
        }


        private void CreateCountDown()
        {
            CountDownItem countDownMgr = null;
            countDownMgr = TimeUpdateMgr.S.IsHavaITimeObserver(GetCountDownKey());
            if (countDownMgr == null)
            {
                m_CountDown = GetDuration();
                countDownMgr = new CountDownItem(GetCountDownKey(), m_CountDown);
            }
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            countDownMgr.OnSecondRefreshEvent = refresAction;
            if (countDownMgr.OnCountDownOverEvent == null)
                countDownMgr.OnCountDownOverEvent = m_KungfuLibraySlot.overAction;
        }

        private string GetCountDownKey()
        {
            if (m_KungfuLibraySlot != null)
                return FacilityType.KongfuLibrary.ToString() + m_KungfuLibraySlot.Index;

            throw new ArgumentNullException("m_KungfuLibraySlot");
        }

        private void refresAction(string obj)
        {
            if (m_Time != null)
                m_Time.text = obj;
        }


        private int GetDuration()
        {
            return m_KungfuLibraySlot.GetDurationTime();
        }
    }

}