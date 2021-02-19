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
            if (m_IsBubbleShowed)
                return;

            TimeSpan timeSpan = DateTime.Now - lastShowBubbleTime;
            if (m_IsUnlocked && timeSpan.Seconds > m_WorkConfigItem.workInterval)
            {
                ShowBubble();
            }
        }

        public void OnClicked()
        {
            CharacterController character = MainGameMgr.S.CharacterMgr.CharacterControllerList.FirstOrDefault(i => i.CurState == CharacterStateID.Wander || i.CurState == CharacterStateID.EnterClan || i.CurState == CharacterStateID.None);
            if (character != null)
            {
                character.CollectObjType = collectedObjType;
                character.SetState(CharacterStateID.CollectRes);
            }
            else
            {
                UIMgr.S.OpenPanel(UIID.LogPanel, "提示", "无空闲弟子！");
            }
        }

        private void ShowBubble()
        {
            m_IsBubbleShowed = true;
            bubble.SetActive(true);
        }

        private void HideBubble()
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
        }

        private void CheckUnlocked()
        {
            m_IsUnlocked = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby) >= m_WorkConfigItem.unlockHomeLevel;
        }

        private void HandleEvent(int key, object[] param)
        {
            FacilityType facilityType = (FacilityType)param[0];
            if (facilityType == FacilityType.Lobby)
            {
                CheckUnlocked();
            }
        }
    }
	
}