using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Qarth;

namespace GameWish.Game
{
    public class CharacterStateDeliver : CharacterState
    {
        private CharacterController m_Controller = null;

        private bool m_IsReachTargetPos = false;
        private bool m_IsDeliverStart = false;

        private DeliverCar m_DeliverCar = null;

        /// <summary>
        /// 人物跟随镖车的一个距离
        /// </summary>
        private float m_CharacterFollowDeliverCarLength = 1;

        public CharacterStateDeliver(CharacterStateID stateEnum) : base(stateEnum)
        {
        }

        public override void Enter(ICharacterStateHander handler)
        {
            Qarth.EventSystem.S.Register(EventID.OnDeliverCarStartGoOut, OnDeliverStartGoOutCallBack);
            Qarth.EventSystem.S.Register(EventID.OnDeliverCarStartComeBack, OnDeliverStartComeBackCallBack);

            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_IsReachTargetPos = false;
            m_IsDeliverStart = false;

            m_DeliverCar = DeliverSystemMgr.S.GetDeliverCarByDeliverID(m_Controller.GetDeliverID());

            if (m_DeliverCar != null)
            {
                Vector3 targetPos = m_DeliverCar.GetGatherPointPosition(m_Controller.CharacterId);

                m_CharacterFollowDeliverCarLength = (m_DeliverCar.transform.position - targetPos).magnitude;

                m_Controller.RunTo(targetPos, OnReachDestination);
            }
            else {
                //吧人物移出场景
                m_Controller.SetPosition(DeliverSystemMgr.S.GoOutSidePos + new Vector3(Random.Range(-2f, 2f), Random.Range(-3f, 3f)));
                
                m_Controller.StopNavAgent();

                m_IsReachTargetPos = true;

                m_IsDeliverStart = true;
            }

            m_Controller.CharacterView.RemoveTouch();
        }

        private void OnDeliverStartGoOutCallBack(int key, object[] param)
        {
            if (param != null && param.Length > 0) 
            {
                int deliverID = int.Parse(param[0].ToString());

                if (deliverID == m_Controller.GetDeliverID()) 
                {
                    m_IsDeliverStart = true;

                    m_Controller.CharacterView.PlayWalkAnim();
                }
            }
        }
        private void OnDeliverStartComeBackCallBack(int key, object[] param)
        {
            if (param != null && param.Length > 0)
            {
                int deliverID = int.Parse(param[0].ToString());

                if (deliverID == m_Controller.GetDeliverID())
                {
                    if (m_DeliverCar == null) 
                    {
                        m_DeliverCar = DeliverSystemMgr.S.GetDeliverCarByDeliverID(m_Controller.GetDeliverID());

                        m_Controller.CharacterView.PlayWalkAnim();

                        Vector3 targetPos = m_DeliverCar.GetGatherPointPosition(m_Controller.CharacterId);

                        m_CharacterFollowDeliverCarLength = (m_DeliverCar.transform.position - targetPos).magnitude;
                    }                    
                }
            }
        }        

        private void OnReachDestination()
        {
            m_IsReachTargetPos = true;

            m_Controller.CharacterView.PlayIdleAnim();

            Qarth.EventSystem.S.Send(EventID.OnCharacterReachDeliverCarGatherPoint, m_Controller.CharacterId);
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            if (m_IsReachTargetPos == false) return;

            if (m_IsDeliverStart == false) return;

            if (m_DeliverCar != null && m_DeliverCar.IsMoving) 
            {
                Vector3 pos = m_DeliverCar.transform.position + m_DeliverCar.MoveDir * m_CharacterFollowDeliverCarLength;

                m_Controller.FollowDeliver(pos);
            }
        }

        public override void Exit(ICharacterStateHander handler)
        {
            Qarth.EventSystem.S.UnRegister(EventID.OnDeliverCarStartGoOut, OnDeliverStartGoOutCallBack);
            Qarth.EventSystem.S.UnRegister(EventID.OnDeliverCarStartComeBack, OnDeliverStartComeBackCallBack);

            m_Controller.CharacterView.AddTouch();
        }
    }
}