using System;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class GuideClickWorldCommand : AbstractGuideCommand
    {
        Vector3 targetPos;//目标点击位置
        Vector3 guideTipsPos;//文本位置
        string textContentID;//气泡显示的id（Language表）
        bool isNotForce = false;

        public override void SetParam(object[] pv)
        {
            if (pv.Length == 0)
            {
                return;
            }
            try
            {
                Transform tar = GameplayMgr.S.transform.Find((string)pv[0]);
                targetPos = tar.position;
                //EventSystem.S.Send(EventID.InGuideProgress, true);
                Camera.main.transform.position = new Vector3(targetPos.x, targetPos.y, Camera.main.transform.position.z);
                //EventSystem.S.Send(EventID.InGuideProgress, false);
                if (pv.Length > 1)
                {
                    guideTipsPos = Helper.String2Vector3((string)pv[1], '|');
                }
                if (pv.Length > 2)
                {
                    textContentID = (string)pv[2];
                    isNotForce = true;
                }
                if (pv.Length > 3)
                {
                    isNotForce = true;
                }
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }

        protected override void OnStart()
        {
            EventSystem.S.Register(EventID.GuideDelayStart, StartCommand);
            Debug.Log("GuideClickWorldCommand 开始");
        }
        

        protected override void OnFinish(bool forceClean)
        {
            UIMgr.S.ClosePanelAsUIID(UIID.MaskClickWorldPanel);
            
            EventSystem.S.UnRegister(EventID.GuideDelayStart, StartCommand);
        }
        private void StartCommand(int key, params object[] param)
        {
            if (param.Length < 0 || guideStep.stepID != (int)param[0])
            {
                Log.e("不满足条件无法开启" + guideStep.stepID);
                return;
            }
            Action action = OnGuideClick;
            UIMgr.S.OpenTopPanel(UIID.MaskClickWorldPanel, null, targetPos, guideTipsPos, textContentID, action, isNotForce);
        }

        private void OnGuideClick()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Facility"));
            if (hit.collider != null)
            {
                IFacilityClickedHandler handler = hit.collider.GetComponent<IFacilityClickedHandler>();
                if (handler != null)
                {
                    handler.OnClicked();
                    FinishStep();
                }
            }
        }
    }
}

