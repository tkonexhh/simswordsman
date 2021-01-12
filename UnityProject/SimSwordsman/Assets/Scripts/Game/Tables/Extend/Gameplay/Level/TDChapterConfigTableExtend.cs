using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDChapterConfigTable
    {
        public static Dictionary<int, ChapterConfigInfo> chapterConfigInfoDic = new Dictionary<int, ChapterConfigInfo>();

        static void CompleteRowAdd(TDChapterConfig tdData)
        {
            if (!chapterConfigInfoDic.ContainsKey(tdData.chapter))
            {
                ChapterConfigInfo chapterInfo = new ChapterConfigInfo();
                chapterInfo.chapterId = tdData.chapter;
                chapterInfo.desc = tdData.desc;
                
                chapterInfo.clanType = EnumUtil.ConvertStringToEnum<ClanType>(tdData.clanType);

                string[] unlockConditionStr = tdData.unlockPrecondition.Split('_');
                if (unlockConditionStr[0].Equals("Null"))
                    chapterInfo.unlockPrecondition = new ChapterUnlockPrecondition();
                else
                {
                    int chapter = int.Parse(unlockConditionStr[1]);
                    int level = int.Parse(unlockConditionStr[2]);
                    chapterInfo.unlockPrecondition = new ChapterUnlockPrecondition(chapter, level);

                }
                chapterConfigInfoDic.Add(tdData.chapter, chapterInfo);
            }
        }

        public static ChapterConfigInfo GetChapterConfigInfo(int id)
        {
            if (chapterConfigInfoDic.ContainsKey(id))
            {
                return chapterConfigInfoDic[id];
            }

            return null;
        }
    }

  
}