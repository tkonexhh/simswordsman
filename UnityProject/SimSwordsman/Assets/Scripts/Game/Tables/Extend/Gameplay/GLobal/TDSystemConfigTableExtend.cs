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
        /// ����
        /// </summary>
        SysStudent,
        /// <summary>
        /// �����
        /// </summary>
        SysInform,
        /// <summary>
        /// ǩ��
        /// </summary>
        SysSignin,
        /// <summary>
        /// ����
        /// </summary>
        SysWork,
        /// <summary>
        /// �ռ�
        /// </summary>
        SysCollect,
        /// <summary>
        /// ��ս
        /// </summary>
        SysChallenge,
        /// <summary>
        /// ��ħ��
        /// </summary>
        SysTower,
        /// <summary>
        /// ����
        /// </summary>
        SysHeroTrial,
        /// <summary>
        /// ����
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
        /// ��ȡϵͳ�Ľ����ȼ�
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