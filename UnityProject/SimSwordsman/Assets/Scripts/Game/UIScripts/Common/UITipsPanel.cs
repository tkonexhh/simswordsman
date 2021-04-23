using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class UITipsPanel : AbstractAnimPanel
    {
        [SerializeField] private Transform m_ImgBg;
        [SerializeField] private Text m_TipName;
        [SerializeField] private Text m_Need;
        [SerializeField] private Text m_Desc;

        [SerializeField] private Button m_BtnJump;

        private int TimerID;
        private JumpToRes m_JumpToRes;
        private UpgradeResItemToUITipsPanel m_InfoStruct;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_BtnJump.onClick.AddListener(OnClickJump);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_InfoStruct = (UpgradeResItemToUITipsPanel)args[0];
            m_ImgBg.transform.position = m_InfoStruct.pos + new Vector3(0.5f, 0.8f, 0);
            m_TipName.text = m_InfoStruct.name;
            m_Need.text = m_InfoStruct.need;
            m_Desc.text = m_InfoStruct.desc;

            Timer.S.Cancel(TimerID);
            TimerID = Timer.S.Post2Really((i) =>
            {
                HideSelfWithAnim();
            }, 2.0f, -1);


            int itemID = m_InfoStruct.itemID;
            // Debug.LogError(itemID);
            m_JumpToRes = JumpToResFactory.Create(itemID);
            m_BtnJump.gameObject.SetActive(m_JumpToRes != null);

            DataAnalysisMgr.S.CustomEvent(DotDefine.Tap_item_detail, itemID);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        private void OnClickJump()
        {
            if (m_JumpToRes == null)
                return;

            DataAnalysisMgr.S.CustomEvent(DotDefine.Tap_item_detail, m_InfoStruct.itemID);

            m_InfoStruct.parentPanel.CloseSelfPanel();

            m_JumpToRes.Jump();
        }
    }

    public class JumpToResFactory
    {
        public static JumpToRes Create(int id)
        {
            switch (id)
            {
                case 1001: return new JumpToRes_QingRock();
                case 1002: return new JumpToRes_WuWood();
                case 1003: return new JumpToRes_CloudRock();
                case 1004: return new JumpToRes_SilverWood();
                case 1005: return new JumpToRes_Vine();
                case 1006: return new JumpToRes_Iron();

                case 1010: return new JumpToRes_LotusRoot();
                case 1012: return new JumpToRes_Fish();
                case 1013: return new JumpToRes_LotusLeaf();
                case 1014: return new JumpToRes_Chicken();
                case 1016: return new JumpToRes_RoyalJelly();
                case 1017: return new JumpToRes_Lotus();
                case 1018: return new JumpToRes_Well();

                case 1020:
                case 1021:
                    return new JumpToRes_Ganoderma();

                case 1007:
                case 1008:
                case 3101:
                case 3102:
                    return new JumpToRes_Deliver();
            }
            return null;
        }
    }

    public abstract class JumpToRes
    {
        protected Transform m_Target;

        public void Jump()
        {
            Init();
            if (m_Target == null) return;

            WorldUIPanel.S.ShowHandTips(m_Target);
            MainGameMgr.S.MainCamera.LookAtTarget(m_Target);
        }

        protected abstract void Init();
    }


    public class JumpToRes_WuWood : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.WuWood);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_QingRock : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.QingRock);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_CloudRock : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.CloudRock);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Vine : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.Vine);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_SilverWood : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.SilverWood);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Iron : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.Iron);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Deliver : JumpToRes
    {
        protected override void Init()
        {
            m_Target = MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Deliver).view.transform;
        }
    }

    public class JumpToRes_LotusRoot : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.CollectItemSystem.GetRawMatItem(CollectedObjType.LotusRoot);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Fish : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.Fish);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_LotusLeaf : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.CollectItemSystem.GetRawMatItem(CollectedObjType.LotusLeaf);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Chicken : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.Chicken);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_RoyalJelly : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.CollectItemSystem.GetRawMatItem(CollectedObjType.RoyalJelly);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Lotus : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.CollectItemSystem.GetRawMatItem(CollectedObjType.Lotus);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Well : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(CollectedObjType.Well);
            if (target == null) return;
            m_Target = target.transform;
        }
    }

    public class JumpToRes_Ganoderma : JumpToRes
    {
        protected override void Init()
        {
            var target = MainGameMgr.S.CollectItemSystem.GetRawMatItem(CollectedObjType.Ganoderma);
            if (target == null) return;
            m_Target = target.transform;
        }
    }


}