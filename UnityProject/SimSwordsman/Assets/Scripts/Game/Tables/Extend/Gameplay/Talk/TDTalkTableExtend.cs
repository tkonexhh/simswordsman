using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTalkTable
    {
        public static Dictionary<int, List<string>> m_TalkDic = new Dictionary<int, List<string>>();
        static void CompleteRowAdd(TDTalk tdData)
        {
            if (m_TalkDic.ContainsKey(tdData.lobbyLevel))
            {
                List<string> talkList;
                m_TalkDic.TryGetValue(tdData.lobbyLevel, out talkList);
                talkList.Add(tdData.talkWords);
            }
            else
            {
                List<string> talkList = new List<string>();
                if (tdData.lobbyLevel >= 2)
                {
                    for (int i = 0; i < m_TalkDic.Count; i++)
                    {
                        foreach (var item in m_TalkDic[i + 1])
                        {
                            talkList.Add(item);
                        }
                    }
                }
                talkList.Add(tdData.talkWords);
                m_TalkDic.Add(tdData.lobbyLevel, talkList);
            }

        }

        public static string GetRangeWords(int level)
        {
            List<string> talkList;
            if (m_TalkDic.TryGetValue(level, out talkList))
            {
                return talkList[RandomHelper.Range(0, talkList.Count)];
            }
            else
            {
                return null;
            }
            
        }
    }
}