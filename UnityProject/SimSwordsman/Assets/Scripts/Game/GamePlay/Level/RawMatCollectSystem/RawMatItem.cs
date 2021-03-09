using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class RawMatItem : MonoBehaviour
    {
        public CollectedObjType collectedObjType = CollectedObjType.None;
        public List<Transform> collectPos = new List<Transform>();
        public GameObject bubble = null;

        private DateTime m_LastShowBubbleTime;

        private bool m_IsBubbleShowed = false;
        private bool m_IsCharacterCollected = false;

        private List<Transform> m_UsedCollectPos = new List<Transform>();
        private WorkConfigItem m_WorkConfigItem = null;
        private bool m_IsUnlocked = false;
        private bool m_IsWorking = false;
        public void OnInit()
        {
            HideBubble();

            m_LastShowBubbleTime = DateTime.Parse(GameDataMgr.S.GetClanData().GetLastShowBubbleTime(collectedObjType));
            m_WorkConfigItem = TDWorkTable.GetWorkConfigItem(collectedObjType);
            if (m_WorkConfigItem == null)
            {
                Log.e("Work config item is null: " + collectedObjType.ToString());
                return;
            }

            CheckUnlocked();

            RegisterEvents();
        }

        public void OnUpdate()
        {

        }

        public void Refresh()
        {
            CheckUnlocked();

            if (!m_IsUnlocked)
                return;
            if (m_IsWorking)
                return;

            TimeSpan timeSpan = DateTime.Now - m_LastShowBubbleTime;

            if (m_IsBubbleShowed && !m_IsCharacterCollected)
            {
                if (!(KongfuLibraryPanel.isOpened || PracticeFieldPanel.isOpened))
                {
                    if (timeSpan.TotalSeconds > m_WorkConfigItem.waitingTime)
                    {
                        AutoSelectCharacter();
                    }
                }
            }

            if (!m_IsBubbleShowed && !m_IsCharacterCollected)
            {
                if (timeSpan.TotalSeconds > m_WorkConfigItem.workInterval)
                {
                    ShowBubble();
                }
            }
        }

        private bool IsFoodEnough()
        {
            int curFood = GameDataMgr.S.GetPlayerData().GetFoodNum();
            return curFood >= Define.WORK_NEED_FOOD_COUNT;
        }

        public void OnClicked()
        {
            if (IsFoodEnough() == false && GuideMgr.S.IsGuideFinish(8) && GuideMgr.S.IsGuideFinish(14))
            {
                FloatMessage.S.ShowMsg("食物不足");
                return;
            }

            CharacterController character = SelectIdleCharacterToCollectRes(true);

            if (character == null)
            {
                //UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "无空闲弟子！");
                FloatMessage.S.ShowMsg("无空闲弟子");
            }
        }

        private CharacterController SelectIdleCharacterToCollectRes(bool manual)
        {
            CharacterController character = MainGameMgr.S.CharacterMgr.CharacterControllerList.FirstOrDefault(i => i.CharacterModel.IsIdle());
            if (character != null)
            {
                character.CollectObjType = collectedObjType;
                character.ManualSelectedToCollectObj = manual;
                character.SetState(CharacterStateID.CollectRes);

                HideBubble();

                m_IsCharacterCollected = true;
                m_IsWorking = true;

                GameDataMgr.S.GetPlayerData().ReduceFoodNum(Define.WORK_NEED_FOOD_COUNT);
            }

            return character;
        }

        public void ShowBubble()
        {
            m_IsBubbleShowed = true;

            bubble.SetActive(true);

            m_LastShowBubbleTime = DateTime.Now.AddSeconds(m_WorkConfigItem.workTime);

            GameDataMgr.S.GetClanData().SetLastShowBubbleTime(collectedObjType, m_LastShowBubbleTime);
        }

        public void SetCharacterSelected(bool selected)
        {
            m_IsCharacterCollected = selected;
        }

        public void HideBubble()
        {
            m_IsBubbleShowed = false;
            bubble.SetActive(false);
        }

        public Transform GetRandomCollectPos()
        {
            List<Transform> list = new List<Transform>();
            foreach (Transform t in collectPos)
            {
                if (!m_UsedCollectPos.Contains(t))
                {
                    list.Add(t);
                }
            }

            if (list.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                return list[index];
            }

            return collectPos[0];
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnEndUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void CheckUnlocked()
        {
            m_IsUnlocked = MainGameMgr.S.FacilityMgr.GetFacilityState(FacilityType.Lobby) == FacilityState.Unlocked 
                && MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby) >= m_WorkConfigItem.unlockHomeLevel;
        }

        private void AutoSelectCharacter()
        {
            if (IsFoodEnough() == false)
            {
                m_LastShowBubbleTime = DateTime.Now.AddSeconds(m_WorkConfigItem.workTime); // Check next interval
                return;
            }

            CharacterController character = SelectIdleCharacterToCollectRes(false);
            if (character != null)
            {
                HideBubble();
            }
        }

        private void HandleEvent(int key, object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnEndUpgradeFacility:
                    FacilityType facilityType = (FacilityType)param[0];
                    if (facilityType == FacilityType.Lobby)
                    {
                        CheckUnlocked();
                    }
                    break;
                case (int)EventID.OnTaskObjCollected:
                    CollectedObjType collectedObjType = (CollectedObjType)param[0];
                    if (collectedObjType == this.collectedObjType)
                    {
                        m_LastShowBubbleTime = DateTime.Now;
                        m_IsCharacterCollected = false;
                        m_IsWorking = false;
                    }
                    break;
            }
        }
    }
	
}