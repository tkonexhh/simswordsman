using System;
using UnityEngine;
using Qarth;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class GuideButtonCommand : AbstractGuideCommand
    {
        private IUINodeFinder m_Finder; //按钮节点 
        private int m_NeedClickTime = 1; //按钮点击次数

        private bool m_NeedShowWorldGuidePanel = false; //是否需要世界导航
        private Vector3 m_UIChangeOffset = Vector3.zero; //世界导航时的位置
        private string m_MerhodName = ""; //世界导航时的方法

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
            Debug.Log("gui commond 开始");

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
                Log.e("不满足条件无法开启" + guideStep.stepID);
                return;
            }

            if (m_NeedShowWorldGuidePanel) //率先开启面板再查找
            {
                UIMgr.S.OpenPanel(UIID.WorldGuideClickPanel, m_MerhodName);
            }

            RectTransform targetNode = m_Finder.FindNode(false) as RectTransform;

            if (targetNode == null)
            {
                Log.e("引导按钮为空" + guideStep.stepID);
                OnGuideBtnClick();
                return;
            }
            if (!(m_GuideBtn = targetNode.GetComponent<Button>()) && m_NeedClickTime != 0)
            {
                m_GuideBtn = targetNode.gameObject.AddComponent<Button>();
            }           

            //根据世界坐标转换一下坐标
            if (m_NeedShowWorldGuidePanel && m_UIChangeOffset != Vector3.zero)
            {
                Camera UICamera = GameObject.Find("UIRoot").GetComponent<UIRoot>().uiCamera;
                if(UICamera == null)
                {
                    UICamera = Camera.main;
                }
                GameExtensions.ScenePosition2UIPosition(Camera.main, UICamera, m_UIChangeOffset, m_GuideBtn.transform);
            }

            if (m_NeedClickTime != 0) //点击引导 才能给上完成回调
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
            else //非點擊則不為最上層
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

