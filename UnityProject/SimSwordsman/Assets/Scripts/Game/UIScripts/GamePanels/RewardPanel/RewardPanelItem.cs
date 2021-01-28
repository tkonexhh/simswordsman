using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class RewardPanelItem : MonoBehaviour
	{
        [SerializeField]
        Image m_Icon;
        [SerializeField]
        Text m_Count;
        [SerializeField]
        Text m_RewardName;

	    public void Init(Sprite icon, RewardBase reward)
        {
            m_Icon.sprite = icon;
            m_RewardName.text = reward.RewardName();
            m_Count.text = "+" + reward.Count;
        }
	}
}