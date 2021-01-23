using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class MedicinalPowderMgr : MonoBehaviour, IMgr
    {
        private Dictionary<int, PlayerDataHerb> m_Herbs = new Dictionary<int, PlayerDataHerb>();

        #region Interface
        public void OnDestroyed()
        {
            throw new System.NotImplementedException();
        }

        public void OnInit()
        {
            List<HerbModel>  herbsModel = GameDataMgr.S.GetPlayerData().GetArchiveHerb();
            foreach (var item in herbsModel)
            {
                if (!m_Herbs.ContainsKey(item.ID))
                    m_Herbs.Add(item.ID, new PlayerDataHerb(GetHerbForId(item.ID), item.Number));
            }
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region Private

        private HerbConfig GetHerbForId(int id)
        {
            return TDHerbConfigTable.GetHerbForId(id);
        }

        #endregion

        #region Public
        /// <summary>
        /// 返回当前所有的草药
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, PlayerDataHerb> GetAllHerbs()
        {
            return m_Herbs;
        }
        /// <summary>
        /// 添加药草
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        public void AddHerb(int id,int number = 0)
        {
            if (!m_Herbs.ContainsKey(id))
                m_Herbs.Add(id, new PlayerDataHerb(GetHerbForId(id), number));
            else
                m_Herbs[id].Number += number;
            GameDataMgr.S.GetPlayerData().AddArchiveHerb(id, number);
        }
        #endregion
    }

    public class PlayerDataHerb : HerbConfig
    {
        public int Number { set; get; }
        public PlayerDataHerb(TDHerbConfig tDHerb) : base(tDHerb) { }

        public PlayerDataHerb(HerbConfig herb, int number):base(herb)
        {
            Number = number;
        }
    }

}