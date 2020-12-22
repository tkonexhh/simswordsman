using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCharacterNameTable
    {
        private static RandomName randomName = new RandomName();

        static void CompleteRowAdd(TDCharacterName tdData)
        {
            randomName.AddTDName(tdData);
        }

        public static string GetRandomName()
        {
            return randomName.GetRandomName();
        }

    }


    public class RandomName
    {
        private static List<string> m_FamilyNameList = new List<string>();
        private static List<string> m_FirstNameList = new List<string>();


        public RandomName() { }

        public void AddTDName(TDCharacterName tDCharacter)
        {
            if (!m_FamilyNameList.Contains(tDCharacter.familyName))
                m_FamilyNameList.Add(tDCharacter.familyName);
            if (!m_FirstNameList.Contains(tDCharacter.familyName))
                m_FirstNameList.Add(tDCharacter.firstName);
        }

        public string GetRandomName()
        {
            int familyIndex = UnityEngine.Random.Range(0, m_FamilyNameList.Count);
            string family = m_FamilyNameList[familyIndex];
            int firstIndex = UnityEngine.Random.Range(0, m_FirstNameList.Count);
            string first = m_FirstNameList[firstIndex];
            return family + first;
        }
    }
}