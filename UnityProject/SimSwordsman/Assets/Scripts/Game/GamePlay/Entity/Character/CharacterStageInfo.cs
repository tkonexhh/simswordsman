using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class CharacterStageInfo
	{
        public int stage;
        public int fromLevel;
        public int toLevel;
        public float baseAtk;
        public int growAtk;
        public List<CharacterStageReward> stageRewards;
        public int startExp;
        public int growExp;
	}
	
}