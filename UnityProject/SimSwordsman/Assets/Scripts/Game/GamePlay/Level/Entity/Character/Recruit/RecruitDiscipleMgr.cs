using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
    public class RecruitDiscipleMgr : MonoBehaviour, IMgr
    {

        private List<RecruitModel> m_RecruitModel = new List<RecruitModel>();

        private RecruitData m_RecruitData = null;
        public void OnDestroyed()
        {
            
        }

        public void OnInit()
        {
            EventSystem.S.Register(EventID.OnRecruitmentOrderIncrease,HandleAddListenerEvent);
            m_RecruitData = GameDataMgr.S.GetPlayerData().GetRecruitData();
            m_RecruitModel.Add(new RecruitModel(RecruitType.GoldMedal, m_RecruitData));
            m_RecruitModel.Add(new RecruitModel(RecruitType.SilverMedal, m_RecruitData));

        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRecruitmentOrderIncrease:
                    if ((RawMaterial)param[0] == RawMaterial.GoldOlder)
                        SetRecruitCount(RecruitType.GoldMedal, (int)param[1]);
                    else if ((RawMaterial)param[0] == RawMaterial.SilverOlder)
                        SetRecruitCount(RecruitType.SilverMedal, (int)param[1]);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ������ļ������
        /// </summary>
        /// <param name="recruitType"></param>
        /// <param name="delta"></param>
        private void SetRecruitCount(RecruitType recruitType,int delta)
        {
            RecruitModel recruitModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (recruitModel != null)
                recruitModel.IncreaseCurRecruitCount(delta);
        }

        public void OnUpdate()
        {
            
        }

        #region ��ļ����
        /// <summary>
        /// �������������ȡ��ǰ�����еĵ���
        /// </summary>
        /// <param name="recruitType"></param>
        /// <returns></returns>
        public CharacterItem GetRecruitForRecruitType(RecruitType recruitType)
        {
            RecruitModel reModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (reModel != null)
                return reModel.GetCurContCharacter();
            else
            {
                Log.w("Recruit Data is Warning");
                return null;
            }
        }
        /// <summary>
        /// ���õ��쿴������ļ����
        /// </summary>
        /// <param name="recruitType"></param>
        /// <param name="delta"></param>
        public void SetAdvertisementCount(RecruitType recruitType, int delta = 1)
        {
            RecruitModel reModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (reModel != null)
                reModel.SetAdvertisementCount(delta);
        }
        /// <summary>
        /// ��õ�ǰ���Ĵ���
        /// </summary>
        /// <param name="recruitType"></param>
        /// <returns></returns>
        public int GetAdvertisementCount(RecruitType recruitType)
        {
            RecruitModel reModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (reModel != null)
                return reModel.GetAdvertisementCount();
            return 0;
        }

        public void ResetAdvertisementCount(RecruitType recruitType)
        {
            RecruitModel reModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (reModel != null)
                reModel.ResetAdvertisementCount(recruitType);
        }

        /// <summary>
        /// ��ȡ�Ƿ��һ����ļ
        /// </summary>
        /// <param name="recruitType">��ļ����</param>
        /// <returns></returns>
        public bool GetIsFirstMedal(RecruitType recruitType)
        {
            RecruitModel ReModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (ReModel != null)
                return ReModel.GetmIsFirstRecruit();
            else
            {
                Log.w("RecruitModel is Null");
                return false;
            }
        }
        /// <summary>
        /// ���������������ļ����E
        /// </summary>
        /// <param name="recruitType"></param>
        public void SetCurRecruitCount(RecruitType recruitType)
        {
            RecruitModel ReModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (ReModel != null)
                ReModel.SetCurRecruitCount();
            else
            {
                Log.w("RecruitModel is Null");
            }
        }
        /// <summary>
        /// ���õ�ǰ��ļ����
        /// </summary>
        /// <param name="recruitType"></param>
        public void ResetCurRecruitCount(RecruitType recruitType)
        {
            RecruitModel ReModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (ReModel != null)
                ReModel.ResetCurRecruitCount();
            else
            {
                Log.w("RecruitModel is Null");
            }
        }
        /// <summary>
        /// �������ͻ�ȡ��ǰ��ļ����
        /// </summary>
        /// <param name="recruitType"></param>
        /// <returns></returns>
        public int GetCurRecruitCount(RecruitType recruitType)
        {
            RecruitModel ReModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (ReModel != null)
                return ReModel.GetCurRecruitCount();
            else
            {
                Log.w("RecruitModel is Null");
                return 0;
            }
        }

        public void SetIsFirstRecruit(RecruitType recruitType)
        {
            RecruitModel reModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (reModel != null)
                reModel.SetIsFirstRecruit();
        }
        /// <summary>
        /// ���������Ƴ���Ӧ����
        /// </summary>
        /// <param name="recruitType"></param>
        /// <param name="item"></param>
        public void RemoveCharacterList(RecruitType recruitType, CharacterItem item)
        {
            RecruitModel reModel = m_RecruitModel.Where(i => i.GetCurRecruitType() == recruitType).FirstOrDefault();
            if (reModel != null)
            {
                reModel.RemoveCharacterList(item);
            }
        }

        /// <summary>
        /// ˢ��RecruitData����
        /// </summary>
        public void RefreshRecruitData()
        {
            foreach (var item in m_RecruitModel)
            {
                switch (item.GetCurRecruitType())
                {
                    case RecruitType.GoldMedal:
                        m_RecruitData.SetIsFirstValue(RecruitType.GoldMedal);
                        m_RecruitData.SetRecruitGoldData(item);
                        break;
                    case RecruitType.SilverMedal:
                        m_RecruitData.SetIsFirstValue(RecruitType.SilverMedal);
                        m_RecruitData.SetRecruitSilverData(item);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion


        private void OnDisable()
        {
            EventSystem.S.Register(EventID.OnRecruitmentOrderIncrease,HandleAddListenerEvent);
        }
    }

}