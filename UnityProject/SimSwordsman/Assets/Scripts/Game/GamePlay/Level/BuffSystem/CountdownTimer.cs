using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace GameWish.Game
{
    
    public class CountdownTimer : ITimeObserver
    {
        public int TickCount;
        public int TotalSecondds;

        public Action OnTickAction;

        public int GetTickCount()
        {
            return -1;
        }

        public int GetTickInterval()
        {
            return 1;
        }

        public int GetTotalSeconds()
        {
            return TotalSecondds;
        } 

        public void OnFinished()
        {
           
        }

        public void OnPause()
        {
            
        }

        public void OnResume()
        {
           
        }

        public void OnStart()
        {
           
        }

        public void OnTick(int count)
        {
            OnTickAction?.Invoke();
        }

        public bool ShouldRemoveWhenMapChanged()
        {
            return false;
        }
    }

}