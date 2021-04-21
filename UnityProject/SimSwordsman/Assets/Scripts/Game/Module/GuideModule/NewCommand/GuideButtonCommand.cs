using System;
using UnityEngine;
using Qarth;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class GuideButtonCommand : AbstractGuideCommand
    {
        private IUINodeFinder m_Finder; //��ť�ڵ� 
        private int m_NeedClickTime = 1; //��ť�������
        
        private bool m_IsNotForce = false;

        private Button m_GuideBtn;
        private int m_ClickTime = 0;

        public override void SetParam(object[] pv)
        {
            if (pv.Length == 0)
            {
                return;
            }

            try
            {
                m_Finder = pv[0] as IUINodeFinder;

                if(pv.Length > 1)
                {
                    m_NeedClickTime = Helper.String2Int((string)pv[1]);
                }

                if(pv.Length > 2)
                {
                    m_IsNotForce = true;
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
            Debug.Log("gui commond ��ʼ");

            AppLoopMgr.S.onUpdate += Update;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_NeedClickTime == 0)
                {
                    OnGuideBtnClick();
                }
            }
        }

        protected override void OnFinish(bool forceClean)
        {
            AppLoopMgr.S.onUpdate -= Update;
            if (m_GuideBtn != null)
            {
                if (m_NeedClickTime != 0)
                {
                    if (m_NeedClickTime == 1)
                    {
                        m_GuideBtn.onClick.RemoveListener(OnGuideBtnClick);
                    }
                    else
                    {
                        m_GuideBtn.onClick.RemoveListener(OnGuideBtnClickTimes);
                    }     
                }
            }
            UIMgr.S.ClosePanelAsUIID(UIID.MyGuidePanel);
            EventSystem.S.UnRegister(EventID.GuideDelayStart, StartCommand);
            m_ClickTime = 0;
        }
        private void StartCommand(int key, params object[] param)
        {
            if (param.Length < 0 || guideStep.stepID != (int)param[0])
            {
                Log.e("�����������޷�����" + guideStep.stepID);
                return;
            }
            RectTransform targetNode = m_Finder.FindNode(false) as RectTransform;

            if (targetNode == null)
            {
                Log.e("������ťΪ��" + guideStep.stepID);
                OnGuideBtnClick();
                return;
            }
            if (!(m_GuideBtn = targetNode.GetComponent<Button>()) && m_NeedClickTime != 0)
            {
                m_GuideBtn = targetNode.gameObject.AddComponent<Button>();
            }
            if (m_NeedClickTime != 0) //������� ���ܸ�����ɻص�
            {
                if (m_NeedClickTime == 1)
                {
                    m_GuideBtn.onClick.AddListener(OnGuideBtnClick);
                }
                else
                {
                    m_GuideBtn.onClick.AddListener(OnGuideBtnClickTimes);
                }
            }
            Action action = FinishStep;
            if (m_NeedClickTime != 0)
            {
                UIMgr.S.OpenTopPanel(UIID.MyGuidePanel, null, targetNode, guideStep.stepID, m_IsNotForce, action);
            }
            else //���c��t�������ό�
            {
                UIMgr.S.OpenPanel(UIID.MyGuidePanel, targetNode, guideStep.stepID/*, GuideMethod.NoMessage*/);
            }
        }

        private void OnGuideBtnClick()
        {
            FinishStep();
        }

        private void OnGuideBtnClickTimes()
        {
            m_ClickTime++;

            if (m_ClickTime >= m_NeedClickTime)
            {
                FinishStep();
            }

        }
    }
}

