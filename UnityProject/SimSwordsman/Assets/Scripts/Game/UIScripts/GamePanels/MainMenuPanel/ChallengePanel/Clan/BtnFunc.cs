using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public enum ChallengeBtnState
	{
		/// <summary>
		/// 锁住
		/// </summary>
		Lock,
		/// <summary>
		/// 已结束
		/// </summary>
		Over,
		/// <summary>
		/// 可以战斗
		/// </summary>
		Battle,
	}

	public class BtnFunc : MonoBehaviour
	{
		private Button m_Self;
		[SerializeField]
		private Text m_Number;
		[SerializeField]
		private Image m_Over;
		[SerializeField]
		private Image m_Battle;	
		[SerializeField]
		private Image m_Lock;
		private ChallengeBtnState m_ChallengeBtnState;
		private int index;
		private ChapterConfigInfo m_CurChapterConfigInfo = null;
		private LevelConfigInfo m_CurChapterLevelConfigInfo = null;
		public void RefreshBtnInfo(ChallengeBtnState state, int number, ChapterConfigInfo chapterConfigInfo, LevelConfigInfo levelConfigInfo)
		{
			index = number;
			m_CurChapterConfigInfo = chapterConfigInfo;
			m_CurChapterLevelConfigInfo = levelConfigInfo;
			m_ChallengeBtnState = state;

			switch (m_ChallengeBtnState)
            {
                case ChallengeBtnState.Lock:
					m_Lock.gameObject.SetActive(true);
					m_Over.gameObject.SetActive(false);
					m_Battle.gameObject.SetActive(false);
					m_Number.text = Define.COMMON_DEFAULT_STR;
					break;
                case ChallengeBtnState.Over:
					m_Lock.gameObject.SetActive(false);
					m_Over.gameObject.SetActive(true);
					m_Battle.gameObject.SetActive(false);
					m_Number.text = (number+1).ToString();
					break;
                case ChallengeBtnState.Battle:
					m_Lock.gameObject.SetActive(false);
					m_Over.gameObject.SetActive(false);
					m_Battle.gameObject.SetActive(true);
					m_Number.text = (number + 1).ToString();
					break;
                default:
                    break;
            }
        }

		public int GetIndex()
		{
			return index;
		}
		// Start is called before the first frame update
		void Start()
	    {
			m_Self = GetComponent<Button>();
			m_Self.onClick.AddListener(()=> { 
				 UIMgr.S.OpenPanel(UIID.IdentifyChallengesPanel, m_CurChapterConfigInfo, m_CurChapterLevelConfigInfo);
			});
		}
	
	    // Update is called once per frame
	    void Update()
	    {
	        
	    }
	}
	
}