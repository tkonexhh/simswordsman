using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{

    public class WorldUIPanel : AbstractPanel
    {
        // private static WorldUIPanel instance;
        public static WorldUIPanel S;
        public WorldUI_WorkTalk m_WalkTalk;


        private bool m_IsBattle = false;

        protected override void OnUIInit()
        {
            S = this;

            GameObjectPoolMgr.S.AddPool("WalkTalk", m_WalkTalk.gameObject, -1, 5);
        }

        protected override void OnOpen()
        {
            RegisterEvent(EventID.OnEnterBattle, HandleEvent);
            RegisterEvent(EventID.OnExitBattle, HandleEvent);
        }


        public void ShowWorkText(Transform character, string talk)
        {
            if (m_IsBattle) return;

            //TODO issue 战斗的时候也能看到工作气泡
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

        private void HandleEvent(int key, params object[] args)
        {
            switch (key)
            {
                case (int)EventID.OnEnterBattle:
                    m_IsBattle = true;
                    break;
                case (int)EventID.OnExitBattle:
                    m_IsBattle = false;
                    break;
            }
        }

    }
}
