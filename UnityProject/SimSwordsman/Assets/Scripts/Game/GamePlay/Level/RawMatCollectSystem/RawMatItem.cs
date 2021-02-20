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
        public DateTime lastShowBubbleTime;
        public GameObject bubble = null;

        private bool m_IsBubbleShowed = false;
        private bool m_IsCharacterCollected = false;

        private List<Transform> m_UsedCollectPos = new List<Transform>();
        private WorkConfigItem m_WorkConfigItem = null;
        private bool m_IsUnlocked = false;

        public void OnInit()
        {
            HideBubble();

            lastShowBubbleTime = DateTime.Parse(GameDataMgr.S.GetClanData().GetLastShowBubbleTime(collectedObjType));
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

            TimeSpan timeSpan = DateTime.Now - lastShowBubbleTime;

            if (!m_IsBubbleShowed && !m_IsCharacterCollected)
            {
                if (timeSpan.TotalSeconds > m_WorkConfigItem.workInterval)
                {
                    ShowBubble();
                }
            }

            if (m_IsBubbleShowed && !m_IsCharacterCollected)
            {
                if (timeSpan.TotalSeconds > m_WorkConfigItem.waitingTime)
                {
                    AutoSelectCharacter();
                }
            }
        }

        public void OnClicked()
        {
            CharacterController character = SelectIdleCharacterToCollectRes();

            if (character == null)
            {
                UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "无空闲弟子！");
            }
        }

        private CharacterController SelectIdleCharacterToCollectRes()
        {
            CharacterController character = MainGameMgr.S.CharacterMgr.CharacterControllerList.FirstOrDefault(i => i.CurState == CharacterStateID.Wander || i.CurState == CharacterStateID.EnterClan || i.CurState == CharacterStateID.None);
            if (character != null)
            {
                character.CollectObjType = collectedObjType;
                character.SetState(CharacterStateID.CollectRes);

                HideBubble();

                m_IsCharacterCollected = true;
            }

            return character;
        }

        public void ShowBubble()
        {
            m_IsBubbleShowed = true;
            bubble.SetActive(true);

            lastShowBubbleTime = DateTime.Now;

            GameDataMgr.S.GetClanData().SetLastShowBubbleTime(collectedObjType, DateTime.Now);
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
            CharacterController character = SelectIdleCharacterToCollectRes();
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
                        m_IsCharacterCollected = false;
                    }
                    break;
            }
        }
    }
	
}