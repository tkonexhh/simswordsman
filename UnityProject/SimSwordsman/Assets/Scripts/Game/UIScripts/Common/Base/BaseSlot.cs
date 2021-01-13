using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class BaseSlot 
	{
		public int Index { set; get; }
		public int UnlockLevel { set; get; }
		public CharacterItem CharacterItem { set; get; }
		public SlotState PracticeFieldState { set; get; }
		public FacilityType FacilityType { set; get; }
		public string StartTime { set; get; }
	}
}