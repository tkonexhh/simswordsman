using System;
using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace GameWish.Game
{
	public class CountdownData : DataDirtyHandler, IResetHandler
    {

        public List<Countdowner> countdownerData = new List<Countdowner>();

        public List<WorkCharacter> workCharacters = new List<WorkCharacter>();

       

        public void OnReset()
        {

        }

        public void SetDefaultValue()
        {
            countdownerData.Clear();
            workCharacters.Clear();

        }

    }

}