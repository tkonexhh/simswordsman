using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{

    public class WorldUIPanel : AbstractPanel
    {
        public static WorldUIPanel S;
        public WorldUI_WorkTalk m_WalkTalk;
        public WorldUI_HandTips m_HandTips;
        public ItemTips m_ItemTips;


        private int m_TimerHandTips = -1;

        // private bool m_IsBattle = false;

        protected override void OnUIInit()
        {
            S = this;

            GameObjectPoolMgr.S.AddPool("WalkTalk", m_WalkTalk.gameObject, -1, 5);
            GameObjectPoolMgr.S.AddPool("ItemTips", m_ItemTips.gameObject, -1, 5);
            GameObjectPoolMgr.S.AddPool("HandTips", m_HandTips.gameObject, -1, 2);
        }

        // protected override void OnOpen()
        // {
        //     // RegisterEvent(EventID.OnEnterBattle, HandleEvent);
        //     // RegisterEvent(EventID.OnExitBattle, HandleEvent);
        // }


        public void ShowWorkText(Transform character, string talk)
        {
            // if (m_IsBattle) return;

            var workTalkGo = GameObjectPoolMgr.S.Allocate("WalkTalk");
            workTalkGo.transform.SetParent(transform);
            workTalkGo.transform.localPosition = Vector3.zero;
            workTalkGo.transform.localScale = Vector3.one;

            WorldUI_WorkTalk workTalk = workTalkGo.GetComponent<WorldUI_WorkTalk>();
            workTalk.followTransform = character;
            workTalk.SetText(talk);
            Timer.S.Post2Scale(i =>
            {
                GameObjectPoolMgr.S.Recycle(workTalkGo);
            }, 3.0f);
        }

        public void ShowWorkText(Transform character, string name, string cont, Sprite icom)
        {
            var itemTipsGo = GameObjectPoolMgr.S.Allocate("ItemTips");
            itemTipsGo.transform.SetParent(transform);
            itemTipsGo.transform.localPosition = Vector3.zero;
            itemTipsGo.transform.localScale = Vector3.one;

            ItemTips itemTips = itemTipsGo.GetComponent<ItemTips>();
            itemTips.followTransform = character;
            itemTips.SetText(name, cont, icom);
            Timer.S.Post2Scale(i =>
            {
                GameObjectPoolMgr.S.Recycle(itemTipsGo);
            }, 3.0f);
        }

        public void ShowHandTips(Transform target)
        {
            if (m_TimerHandTips != -1)
            {
                Timer.S.Cancel(m_TimerHandTips);
            }

            m_HandTips.followTransform = target;

            m_HandTips.gameObject.SetActive(true);

            m_TimerHandTips = Timer.S.Post2Scale(i =>
                {
                    m_HandTips.gameObject.SetActive(false);
                    m_HandTips.followTransform = null;
                    m_HandTips.transform.localPosition = new Vector3(5000, 5000, 0);
                    m_HandTips.transform.localScale = Vector3.one;
                }, 2.0f);
        }

        // private void HandleEvent(int key, params object[] args)
        // {
        //     switch (key)
        //     {
        //         case (int)EventID.OnEnterBattle:
        //             m_IsBattle = true;
        //             break;
        //         case (int)EventID.OnExitBattle:
        //             m_IsBattle = false;
        //             break;
        //     }
        // }

    }
}
