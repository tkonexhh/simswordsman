using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public enum FacilityWorkingStateEnum
    {         
        /// <summary>
        /// 闲置状态
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 工作状态
        /// </summary>
        Working = 1,
        /// <summary>
        /// 显示气泡状态
        /// </summary>
        Bubble = 2,
    }
	public class ClickWorkingBubble : MonoBehaviour
	{
        public FacilityType Type;
        public GameObject BubbleView;
        public GameObject Tips;

        public GameObject WorkSprite;
        public GameObject RewardSprite;

        private FacilityController m_FacilityController = null;
        public FacilityController FacilityController
        {
            get {
                if (m_FacilityController == null) {
                    m_FacilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(Type);
                }
                return m_FacilityController;
            }
        }

        private DateTime m_LastShowBubbleTime;
        private void Awake()
        {
            EventSystem.S.Register(EventID.OnAddCanWorkFacility, ShowWorkBubble);
        }

        private void ShowWorkBubble(int key, object[] param)
        {
            if (FacilityController != null) 
            {
                FacilityType type = (FacilityType)param[0];

                if (type == Type && FacilityController.IsIdleState())
                {
                    FacilityController.ChangeFacilityWorkingState(FacilityWorkingStateEnum.Bubble);
                    FacilityController.CoundDownAutoStartWork(OnClicked,true);
                    BubbleView.SetActive(true);
                    WorkSprite.SetActive(true);
                    RewardSprite.SetActive(false);
                    DataAnalysisMgr.S.CustomEvent(DotDefine.work_generate, "Coin");

                    EventSystem.S.Send(EventID.OnSendWorkingBubbleFacility, Type,true);

                    m_LastShowBubbleTime = DateTime.Now;
                }
            }
        }

        private bool IsFoodEnough()
        {
            int curFood = GameDataMgr.S.GetPlayerData().GetFoodNum();
            return curFood >= Define.WORK_NEED_FOOD_COUNT;
        }

        public void OnClicked(bool isAuto = false)
        {
            if (FacilityController != null) 
            {
                if (IsFoodEnough() == false)
                {
                    if (!isAuto) 
                    {
                        int remaintCount = GameDataMgr.S.GetPlayerData().GetFoodRefreshTimesToday();
                        if (remaintCount > 0)
                        {
                            UIMgr.S.OpenPanel(UIID.SupplementFoodPanel);
                        }
                        else 
                        {
                            DataAnalysisMgr.S.CustomEvent(DotDefine.out_of_food);
                            FloatMessage.S.ShowMsg("食物不足");
                        }                        
                    }                    
                    return;
                }

                if (FacilityController.DispatchDiscipleStartWork(isAuto)) 
                {
                    BubbleView.SetActive(false);
                    GameDataMgr.S.GetPlayerData().ReduceFoodNum(Define.WORK_NEED_FOOD_COUNT);
                    DataAnalysisMgr.S.CustomEvent(DotDefine.work_enter, "Coin");

                    EventSystem.S.Send(EventID.OnSendWorkingBubbleFacility,Type,false);
                }
            }
        }
	}	
}