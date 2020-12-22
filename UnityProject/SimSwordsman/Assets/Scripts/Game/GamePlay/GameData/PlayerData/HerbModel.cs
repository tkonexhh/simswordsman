using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class HerbModel 
	{
		public int ID { set; get; }
		public int Number { set; get; }

		public HerbModel(int id,int number)
		{
			ID = id;
			Number = number;
		}
		public HerbModel()
		{
		}
	}
	
}