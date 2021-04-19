using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
	public enum LoadDiscipleHeadPortrait
	{
		Head,
		Body,
	}

	public enum UserPanel
	{
		PromotionPanel,
	}

	public class DiscipleHeadPosAndScale
	{
		public UserPanel UserPanel;
		public Vector2 Pos;
		public Vector2 Scale;
		/// <summary>
		/// 身体和头的差值
		/// </summary>
		public float BodyHeadInterpolation;

		public DiscipleHeadPosAndScale() { }

		public void AddPosAndScale(UserPanel userPanel,float bodyHeadInterpolation, Vector2 pos, Vector2 scale)
		{
			UserPanel = userPanel;
			Pos = pos;
			Scale = scale;
			BodyHeadInterpolation = bodyHeadInterpolation;
		}
    }

	public class DiscipleHeadPortraitMgr : TSingleton<DiscipleHeadPortraitMgr>
	{
		public List<DiscipleHeadPosAndScale> DiscipleHeadPos = new List<DiscipleHeadPosAndScale>();

		public override void OnSingletonInit()
        {
            base.OnSingletonInit();
			DiscipleHeadPosAndScale siscipleHeadPosAndScale = new DiscipleHeadPosAndScale();
			siscipleHeadPosAndScale.AddPosAndScale(UserPanel.PromotionPanel,-137, new Vector2(-9, 105), new Vector2(0.6f, 0.6f));

			DiscipleHeadPos.Add(siscipleHeadPosAndScale);
		}
		
    }

	public class DiscipleHeadPortrait : MonoBehaviour
	{
		[SerializeField]
		private Image m_Head;
		[SerializeField]
		private Image m_Body;
			 
		private CharacterItem m_CharacterItem;
		void Start()
        {
			
		}

        // Update is called once per frame
        void Update()
	    {
	        
	    }

		public void SetHeadPortrait(CharacterItem characterItem, UserPanel userPanel)
		{
			m_CharacterItem = characterItem;
			m_Head.sprite = CommonMethod.GetDiscipleHeadPortrait( LoadDiscipleHeadPortrait.Head, characterItem);
			m_Body.sprite = CommonMethod.GetDiscipleHeadPortrait(LoadDiscipleHeadPortrait.Body, characterItem);
			DiscipleHeadPosAndScale discipleHeadPosAndScale = DiscipleHeadPortraitMgr.S.DiscipleHeadPos.Where(i => i.UserPanel == userPanel).FirstOrDefault();
			transform.localPosition = discipleHeadPosAndScale.Pos;
			transform.localScale = discipleHeadPosAndScale.Scale;
		}
	}
	
}