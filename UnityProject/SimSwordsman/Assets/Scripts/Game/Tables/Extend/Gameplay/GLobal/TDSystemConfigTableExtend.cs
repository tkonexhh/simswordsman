using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public enum GameFunctionSystem
    {
        /// <summary>
        /// 弟子
        /// </summary>
        SysStudent,
        /// <summary>
        /// 公告榜
        /// </summary>
        SysInform,
        /// <summary>
        /// 签到
        /// </summary>
        SysSignin,
        /// <summary>
        /// 工作
        /// </summary>
        SysWork,
        /// <summary>
        /// 收集
        /// </summary>
        SysCollect,
        /// <summary>
        /// 挑战
        /// </summary>
        SysChallenge,
        /// <summary>
        /// 伏魔塔
        /// </summary>
        SysTower,
        /// <summary>
        /// 试炼
        /// </summary>
        SysHeroTrial,
        /// <summary>
        /// 神兽
        /// </summary>
        SysMonster,
    }

    public partial class TDSystemConfigTable
    {
        private static Dictionary<int, SystemConfigTable> m_SystemConfigTableDic = new Dictionary<int, SystemConfigTable>();
        static void CompleteRowAdd(TDSystemConfig tdData)
        {
            m_SystemConfigTableDic.Add(tdData.id, new SystemConfigTable(tdData));
        }

        /// <summary>
        /// 获取系统的解锁等级
        /// </summary>
        /// <param name="systemType"></param>
        /// <returns></returns>
        public static int GetLobbyLevelRequired(GameFunctionSystem systemType)
        {
            foreach (var item in m_SystemConfigTableDic.Values)
            {
                if (item.System== systemType)
                {
                    return item.LobbyLevelRequired;
                }
            }
            return -1;
        }
    }

    public class SystemConfigTable
    {
        public int ID { set; get; }
        public GameFunctionSystem System { set; get; }
        public string SystemName { set; get; }
        public int LobbyLevelRequired { set; get; }

        public SystemConfigTable(TDSystemConfig tdData)
        {
            ID = tdData.id;
            System = EnumUtil.ConvertStringToEnum<GameFunctionSystem>(tdData.system);
            SystemName = tdData.syeName;
            LobbyLevelRequired = tdData.lobbyLevelRequired;
        }
    }
}