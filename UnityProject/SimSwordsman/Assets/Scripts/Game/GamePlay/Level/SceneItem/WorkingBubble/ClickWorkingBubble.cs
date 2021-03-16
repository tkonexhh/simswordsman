using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public enum FacilityWorkingStateEnum
    {         
        /// <summary>
        /// ����״̬
        /// </summary>
        Idle = 0,
        /// <summary>
        /// ����״̬
        /// </summary>
        Working = 1,
        /// <summary>
        /// ��ʾ����״̬
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
                    BubbleView.SetActive(true);
                    WorkSprite.SetActive(true);
                    RewardSprite.SetActive(false);

                    EventSystem.S.Send(EventID.OnSendWorkingBubbleFacility, Type, true);

                    m_LastShowBubbleTime = DateTime.Now;
                }
            }            
        }

        private bool IsFoodEnough()
        {
            int curFood = GameDataMgr.S.GetPlayerData().GetFoodNum();
            return curFood >= Define.WORK_NEED_FOOD_COUNT;
        }

        public void OnClicked()
        {
            if (FacilityController != null && FacilityController.IsShowBubble()) 
            {
                if (IsFoodEnough() == false)
                {
                    FloatMessage.S.ShowMsg("ʳ�ﲻ��");
                    return;
                }

                if (FacilityController.DispatchDiscipleStartWork()) 
                {
                    BubbleView.SetActive(false);
                    GameDataMgr.S.GetPlayerData().ReduceFoodNum(Define.WORK_NEED_FOOD_COUNT);
                }
            }
        }
	}	
}