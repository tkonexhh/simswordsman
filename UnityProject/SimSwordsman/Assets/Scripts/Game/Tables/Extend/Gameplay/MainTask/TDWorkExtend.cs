using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDWork
    {
        public List<string> workTalkLst;
        public void Reset()
        {
            workTalkLst = Helper.String2ListString(workTalk, ";");
        }
    }
}