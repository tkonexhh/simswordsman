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

        protected override void OnUIInit()
        {
            S = this;

            GameObjectPoolMgr.S.AddPool("WalkTalk", m_WalkTalk.gameObject, -1, 5);
        }


        public void ShowWorkText(Transform character, string talk)
        {
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

    }
}
