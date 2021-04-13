using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTowerConfig
    {
        public void Reset()
        {

        }

        public bool CanRevive()
        {
            return m_CanRevive == 1;
        }
    }
}