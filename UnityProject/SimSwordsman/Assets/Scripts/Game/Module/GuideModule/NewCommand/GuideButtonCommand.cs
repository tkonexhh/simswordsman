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

        private bool m_NeedShowWorldGuidePanel = false; //�Ƿ���Ҫ���絼��
        private Vector3 m_UIChangeOffset = Vector3.zero; //���絼��ʱ��λ��
        private string m_MerhodName = ""; //���絼��ʱ�ķ���

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
                    m_NeedShowWorldGuidePanel = Helper.String2Bool((string)pv[2]);

                    m_UIChangeOffset = Helper.String2Vector3((string)pv[3], '|');
                }

                if(pv.Length > 4)
                {
                    m_MerhodName = (string)pv[4];
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
            //StartCommand(0, guideStep.stepID);
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
                //if (GuideBtn.GetComponent<LongClickBtn>())
                //{
                //    GuideBtn.GetComponent<LongClickBtn>().onLongClick.RemoveListener(OnGuideBtnClick);
                //}

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
              
                if (m_NeedShowWorldGuidePanel)
                {
                    UIMgr.S.ClosePanelAsUIID(UIID.WorldGuideClickPanel);
                }
            }
            UIMgr.S.ClosePanelAsUIID(UIID.MyGuidePanel);
            EventSystem.S.UnRegister(EventID.GuideDelayStart, StartCommand);
           // UIMgr.S.ClosePanelAsUIID(UIID.MyGuidePanel);
            m_ClickTime = 0;
        }
        private void StartCommand(int key, params object[] param)
        {
            //Log.e((int)param[0] + " --- " + guideStep.stepID);
            if (param.Length < 0 || guideStep.stepID != (int)param[0])
            {
                Log.e("�����������޷�����" + guideStep.stepID);
                return;
            }

            if (m_NeedShowWorldGuidePanel) //���ȿ�������ٲ���
            {
                UIMgr.S.OpenPanel(UIID.WorldGuideClickPanel, m_MerhodName);
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

            //������������ת��һ������
            if (m_NeedShowWorldGuidePanel && m_UIChangeOffset != Vector3.zero)
            {
                Camera UICamera = GameObject.Find("UIRoot").GetComponent<UIRoot>().uiCamera;
                if(UICamera == null)
                {
                    UICamera = Camera.main;
                }
                GameExtensions.ScenePosition2UIPosition(Camera.main, UICamera, m_UIChangeOffset, m_GuideBtn.transform);
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

            if (m_NeedClickTime != 0)
            {
                UIMgr.S.OpenTopPanel(UIID.MyGuidePanel, null, targetNode, guideStep.stepID);
            }
            else //���c��t�������ό�
            {
                UIMgr.S.OpenPanel(UIID.MyGuidePanel, targetNode, guideStep.stepID, GuideMethod.NoMessage);
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

