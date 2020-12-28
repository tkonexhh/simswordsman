using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDSectNameTable
    {
        public static List<string> m_FamilyNameList = new List<string>();
        public static List<string> m_FirstList = new List<string>();

        static void CompleteRowAdd(TDSectName tdData)
        {
            if (!tdData.familyName.ToLower().Equals("none") && !m_FamilyNameList.Contains(tdData.familyName))
            {
                m_FamilyNameList.Add(tdData.familyName);
            }
            if (!tdData.firstName.ToLower().Equals("none") && !m_FirstList.Contains(tdData.firstName))
            {
                m_FirstList.Add(tdData.firstName);
            }
        }
    }
}