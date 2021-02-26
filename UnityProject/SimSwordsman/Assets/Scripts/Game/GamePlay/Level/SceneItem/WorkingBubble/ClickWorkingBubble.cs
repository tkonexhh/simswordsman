using UnityEngine;
using Qarth;

namespace GameWish.Game
{
	public class ClickWorkingBubble : MonoBehaviour
	{
        public FacilityType Type;
        public GameObject BubbleView;
        public GameObject Tips;

        public GameObject WorkSprite;
        public GameObject RewardSprite;

        short state = 0;//0������ʾ���� 1�����Թ��������� 2�� ���Ի�ý���������


        private void Awake()
        {
            state = 0;
            EventSystem.S.Register(EventID.OnAddCanWorkFacility, CanWorkBubble);
            //EventSystem.S.Register(EventID.OnAddWorkingRewardFacility, RewardBubble);
        }

        private void CanWorkBubble(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            if (type == Type)
            {
                state = 1;
                BubbleView.SetActive(true);
                WorkSprite.SetActive(true);
                RewardSprite.SetActive(false);

                EventSystem.S.Send(EventID.OnSendWorkingBubbleFacility, Type,true);
            }
        }

        private void RewardBubble(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            if (type == Type)
            {
                state = 2;
                BubbleView.SetActive(true);
                WorkSprite.SetActive(false);
                RewardSprite.SetActive(true);
            }
        }

        private bool IsFoodEnough()
        {
            int curFood = GameDataMgr.S.GetPlayerData().GetFoodNum();
            return curFood >= Define.WORK_NEED_FOOD_COUNT;
        }

        public void OnClicked()
        {
            if (state == 1)
            {
                if (IsFoodEnough() == false)
                {
                    FloatMessage.S.ShowMsg("ʳ�ﲻ��");
                    return;
                }

                if (!WorkSystem.S.StartWork(Type))
                    FloatMessage.S.ShowMsg("�޿��е���");
                //UIMgr.S.OpenPanel(UIID.LogPanel, "��ʾ", "�޿��е��ӣ�");
                else
                {
                    state = 0;
                    BubbleView.SetActive(false);
                    EventSystem.S.Send(EventID.OnSendWorkingBubbleFacility, Type, false);
                    EventSystem.S.Send(EventID.OnAddRawMaterialEvent);

                    GameDataMgr.S.GetPlayerData().AddFoodNum(-Define.WORK_NEED_FOOD_COUNT);
                }
            }
            else if(state == 2)
            {
                state = 0;
                BubbleView.SetActive(false);
                WorkSystem.S.GetReward(Type);
            }
        }
	}
	
}