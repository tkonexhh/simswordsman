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

        private bool m_IsBubbleShowed = false;

        private List<Transform> m_UsedCollectPos = new List<Transform>();

        public void OnInit()
        {
            lastShowBubbleTime = DateTime.Parse( GameDataMgr.S.GetClanData().GetLastShowBubbleTime(collectedObjType));
        }

        public void OnUpdate()
        {

        }

        public void Refresh()
        {
            if (m_IsBubbleShowed)
                return;

            TimeSpan timeSpan = DateTime.Now - lastShowBubbleTime;
            if (timeSpan.Seconds > 10)
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
            //
        }

        private void HideBubble()
        {

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
    }
	
}