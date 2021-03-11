using System;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class GuideClickBubbleCommand : AbstractGuideCommand
    {
        Vector3 targetPos;//Ŀ����λ��
        Vector3 guideTipsPos;//�ı�λ��
        string textContentID;//������ʾ��id��Language��

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
                Camera.main.transform.position = new Vector3(targetPos.x, targetPos.y, Camera.main.transform.position.z);
                if (pv.Length > 1)
                {
                    guideTipsPos = Helper.String2Vector3((string)pv[1], '|');
                }
                if (pv.Length > 2)
                {
                    textContentID = (string)pv[2];
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
            Debug.Log("GuideClickWorldCommand ��ʼ");
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
                Log.e("�����������޷�����" + guideStep.stepID);
                return;
            }
            Action action = OnGuideClick;
            //UIMgr.S.OpenTopPanel(UIID.MaskClickWorldPanel, null, targetPos, guideTipsPos, textContentID, action, true);
            UIMgr.S.OpenPanel(UIID.MaskClickWorldPanel, targetPos, guideTipsPos, textContentID, action, true);
        }

        private void OnGuideClick()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Bubble"));
            if (hit.collider != null)
            {
                WorkingBubbleView bubble = hit.collider.GetComponent<WorkingBubbleView>();
                if (bubble != null)
                    bubble.BubbleCtrl.OnClicked();
            }
            FinishStep();
        }
    }
}

