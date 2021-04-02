using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class MainTaskUIHandler : MonoBehaviour
    {
        [SerializeField] private Animator m_AnimCtrl;
        [SerializeField] private Image m_ImgTaskStatus;
        [SerializeField] private Text m_TxtTaskTitle;
        [SerializeField] private Image m_ImgRewardIcon;
        [SerializeField] private Image m_ImgKongfuNameIcon;
        [SerializeField] private Text m_TxtRewardNum;
        [SerializeField] private Button m_BtnHide;
        [SerializeField] private Button m_BtnShow;

        [SerializeField, Header("Uncomplete")] private GameObject m_ObjUncomplete;
        [SerializeField, Header("Completed")] private GameObject m_ObjCompleted;
        [SerializeField] private Button m_BtnReward;

        private const string ANIMNAME_SHOW = "Show";
        private const string ANIMNAME_Hide = "Hide";
        private int m_AnimHash_Show;
        private int m_AnimHash_Hide;
        private bool m_Animing = false;

        private AbstractPanel m_Panel;

        public void Init(AbstractPanel panel)
        {
            m_Panel = panel;
            m_AnimHash_Show = Animator.StringToHash(ANIMNAME_SHOW);
            m_AnimHash_Hide = Animator.StringToHash(ANIMNAME_Hide);
            m_BtnReward.onClick.AddListener(OnClickReward);
            m_BtnHide.onClick.AddListener(OnClickHide);
            m_BtnShow.onClick.AddListener(OnClickShow);
            EventSystem.S.Register(EventID.OnRefeshMainTask, Refesh);
            m_AnimCtrl.Play(m_AnimHash_Hide);
            RefeshTask();
        }

        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EventID.OnRefeshMainTask, Refesh);
        }

        private void Refesh(int key, params object[] args)
        {
            RefeshTask();
        }

        private void RefeshTask()
        {
            //主线任务是否全部做完 做完全部隐藏
            var task = MainGameMgr.S.TaskMgr.mainTaskController.curTask;
            if (task == null || task.reward == null)
            {
                gameObject.SetActive(false);
                return;
            }

            var reward = task.reward;

            m_TxtRewardNum.text = reward.Count.ToString();
            bool isComplete = task.IsComplete();
            m_ImgTaskStatus.sprite = SpriteHandler.S.GetSprite("MainTaskAtlas", isComplete ? "MainTask_tip2" : "MainTask_tip1");
            m_ImgTaskStatus.SetNativeSize();
            if (isComplete)
                DataAnalysisMgr.S.CustomEvent(DotDefine.m_task_finish, task.id.ToString());
            m_ObjUncomplete.SetActive(!isComplete);
            m_ObjCompleted.SetActive(isComplete);
            m_TxtTaskTitle.text = task.taskSubTitle;

            if (reward.RewardItem == RewardItemType.Kongfu && reward.KeyID.HasValue)
            {
                KungfuQuality kungfuQuality = TDKongfuConfigTable.GetKungfuConfigInfo((KungfuType)reward.KeyID).KungfuQuality;

                switch (kungfuQuality)
                {
                    case KungfuQuality.Normal:
                        m_ImgRewardIcon.sprite = m_Panel.FindSprite("Introduction");
                        break;
                    case KungfuQuality.Super:
                        m_ImgRewardIcon.sprite = m_Panel.FindSprite("Advanced");
                        break;
                    case KungfuQuality.Master:
                        m_ImgRewardIcon.sprite = m_Panel.FindSprite("Excellent");
                        break;
                    default:
                        break;
                }

                m_ImgKongfuNameIcon.sprite = m_Panel.FindSprite(reward.SpriteName());
                m_ImgKongfuNameIcon.gameObject.SetActive(true);
            }
            else
            {
                m_ImgKongfuNameIcon.gameObject.SetActive(false);
                m_ImgRewardIcon.sprite = m_Panel.FindSprite(reward.SpriteName());
            }

            m_ImgRewardIcon.SetNativeSize();
        }

        private void OnClickShow()
        {
            //if (m_Animing)
            //    return;
            Debug.Log("OnClickShow");
            m_Animing = true;
            m_AnimCtrl.Play(m_AnimHash_Show);
        }

        private void OnClickHide()
        {
            //if (m_Animing)
            //    return;
            Debug.Log("OnClickHide");
            m_Animing = true;
            m_AnimCtrl.Play(m_AnimHash_Hide);
        }

        private void OnClickReward()
        {
            var task = MainGameMgr.S.TaskMgr.mainTaskController.curTask;
            DataAnalysisMgr.S.CustomEvent(DotDefine.m_task_receive, task.id.ToString());
            MainGameMgr.S.TaskMgr.mainTaskController.FinishCurTask();
        }

        #region  Anim Event
        public void OnShowComplete()
        {
            m_Animing = false;
        }

        public void OnHideComplete()
        {
            m_Animing = false;
        }
        #endregion
    }

}