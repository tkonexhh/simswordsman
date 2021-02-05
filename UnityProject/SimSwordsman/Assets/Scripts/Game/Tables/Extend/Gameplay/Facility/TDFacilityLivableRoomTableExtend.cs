using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDFacilityLivableRoomTable
    {
        public static List<LivableRoomLevelInfo> levelInfoList = new List<LivableRoomLevelInfo>();

        static void CompleteRowAdd(TDFacilityLivableRoom tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.id);
            levelInfo.level = tdData.level;
            LivableRoomLevelInfo roomLevelInfo = new LivableRoomLevelInfo();
            roomLevelInfo.Warp(levelInfo);
            roomLevelInfo.SetCurCapatity( tdData.capability);
            roomLevelInfo.roomId = tdData.houseId;

            levelInfoList.Add(roomLevelInfo);

        }

        public static LivableRoomLevelInfo GetLevelInfo(int roomId, int level)
        {
            LivableRoomLevelInfo info = levelInfoList.Where(i => i.roomId == roomId && i.level == level).FirstOrDefault();
            return info;
        }
        /// <summary>
        /// 获取某一屋舍某一等级的居住人数
        /// </summary>
        /// <param name="wareHouseID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetCapability(int wareHouseID, int level)
        {
            LivableRoomLevelInfo roomLevelInfo = levelInfoList.Where(i => i.roomId == wareHouseID && i.level == level).FirstOrDefault();
            if (roomLevelInfo!=null)
                return roomLevelInfo.GetCurCapacity();
            else
            {
                Log.w("LivableRoomLevelInfo is null,wareHouseID = {0}", wareHouseID);
                return -0;
            }
        }
        /// <summary>
        /// 获取某一屋舍最大等级
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static int GetMaxLevel(int roomId)
        {
            int maxLevel = 0;
            levelInfoList.ForEach(i=> {
                if (i.roomId == roomId)
                    maxLevel++;
            });
            return maxLevel;
        }

        private static FacilityLevelInfo PassLevelInfo(int id)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityLivableRoom item = null;
            bool haveData = m_DataCache.TryGetValue(id, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(id, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}