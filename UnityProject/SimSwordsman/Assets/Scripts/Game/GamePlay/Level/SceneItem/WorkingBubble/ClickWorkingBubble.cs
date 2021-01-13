using UnityEngine;
using Qarth;

namespace GameWish.Game
{
	public class ClickWorkingBubble : MonoBehaviour
	{
        public FacilityType Type;
        public GameObject BubbleView;

        public GameObject WorkSprite;
        public GameObject RewardSprite;

        short state = 0;//0������ʾ���� 1�����Թ��������� 2�� ���Ի�ý���������


        private void Awake()
        {
            state = 0;
            EventSystem.S.Register(EventID.OnAddCanWorkFacility, CanWorkBubble);
            EventSystem.S.Register(EventID.OnAddWorkingRewardFacility, RewardBubble);
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

        public void OnClicked()
        {
            if (state == 1)
            {
                if (!WorkSystem.S.StartWork(Type))
                    UIMgr.S.OpenPanel(UIID.LogPanel, "��ʾ", "�޿��е��ӣ�");
                else
                {
                    state = 0;
                    BubbleView.SetActive(false);
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