using System;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class GuideClickWorldCommand : AbstractGuideCommand
    {
        Vector3 handPos;//提示手位置
        Vector3 targetPos;//目标点击位置
        Vector3 cameraPos;//相机位置
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
                handPos = Helper.String2Vector3((string)pv[0], '|');

                if (pv.Length > 1)
                {
                    targetPos = Helper.String2Vector3((string)pv[1], '|');
                }
                if (pv.Length > 2)
                {
                    cameraPos = Helper.String2Vector3((string)pv[2], '|');
                    Camera.main.transform.position = new Vector3(cameraPos.x, cameraPos.y, Camera.main.transform.position.z);
                }
                if (pv.Length > 3)
                {
                    textContentID = (string)pv[3];
                }
                if (pv.Length > 4)
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
            UIMgr.S.OpenTopPanel(UIID.MaskClickWorldPanel, null, handPos, targetPos, textContentID, action, isNotForce);
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

