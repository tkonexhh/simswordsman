using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using BitBenderGames;
using Qarth;
using System;

namespace GameWish.Game
{
    public class MainCamera : MonoBehaviour, IMgr, IInputObserver
    {
        private TouchInputController m_TouchInput;
        private MobileTouchCamera m_MobileTouchCamera;
        private Camera m_Camera;

        private CameraProperty m_BattleProperty;
        private CameraProperty m_SimProperty;

        public CameraProperty battleProperty
        {
            get { return m_BattleProperty; }
        }

        private bool m_CameraMoveSwitch = true;
        #region IMgr

        public void OnInit()
        {
            InputMgr.S.AddTouchObserver(this);

            m_TouchInput = GetComponent<TouchInputController>();
            m_MobileTouchCamera = GetComponent<MobileTouchCamera>();
            m_Camera = GetComponent<Camera>();

            m_BattleProperty = new CameraProperty(new Vector3(41, 0.4f, -10), InitBattleSize());
            m_SimProperty = new CameraProperty(new Vector3(6, 2.2f, -10), 10);

            RegisterEvents();

        }

        private float InitBattleSize()
        {
            return 6.0f;
        }

        public void OnUpdate()
        {
        }

        public void OnDestroyed()
        {
            UnregisterEvents();
        }

        #endregion

        #region IInput
        public bool On_Drag(Gesture gesture, bool isTouchStartFromUI)
        {
            return false;
        }

        public bool On_LongTap(Gesture gesture)
        {
            return false;
        }

        public bool On_Swipe(Gesture gesture)
        {
            return false;
        }

        public bool On_TouchUp(Gesture gesture)
        {
            return false;
        }

        public bool BlockInput()
        {
            return true;
        }

        public int GetSortingLayer()
        {
            return 1;
        }

        public bool On_TouchDown(Gesture gesture)
        {
            //#if UNITY_EDITOR
            //            if (!m_CameraMoveSwitch)
            //                return false;

            //            float x = transform.position.x - gesture.deltaPosition.x * m_MoveSpeed;
            //            float y = transform.position.y - gesture.deltaPosition.y * m_MoveSpeed;

            //            if (x > m_CameraBottomLeft.x && x < m_CameraTopRight.x && y > m_CameraBottomLeft.y && y < m_CameraTopRight.y)
            //            {
            //                transform.position -= m_MoveSpeed * new Vector3(gesture.deltaPosition.x, gesture.deltaPosition.y, 0);
            //            }

            //            return true;
            //#endif
            return false;
        }

        public bool On_TouchStart(Gesture gesture)
        {
            return false;
        }


        #endregion

        #region Public

        public void LookAtTarget(Transform target)
        {
            //TODO 修改实现方式
            var followCamera = gameObject.AddMissingComponent<FixedFollowCamera>();
            followCamera.SetTarget(target);
            Timer.S.Post2Scale(i => followCamera.DestorySelf(), 0.5f);
        }

        public void OpenGameCamera()
        {
            m_Camera.enabled = true;
        }

        public void CloseGameCamera()
        {
            m_Camera.enabled = false;
        }

        #endregion

        #region Private

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnEnterBattle, HandleEvent);
            EventSystem.S.Register(EventID.OnExitBattle, HandleEvent);
            EventSystem.S.Register(EventID.InGuideProgress, CameraMoveWwitch);
            EventSystem.S.Register(EventID.OnLimitCameraTouchMove, OnLimitCameraTouchMove);
        }

        private void OnLimitCameraTouchMove(int key, object[] param)
        {
            bool isLimitTouch = false;

            if (param.Length > 0)
            {
                isLimitTouch = bool.Parse(param[0].ToString());
            }

            m_TouchInput.enabled = !isLimitTouch;
            m_MobileTouchCamera.enabled = !isLimitTouch;
            m_SimProperty.Apply(m_Camera);
        }

        private void CameraMoveWwitch(int key, object[] param)
        {
            if (param.Length > 0)
            {
                m_CameraMoveSwitch = (bool)param[0];
                if (m_CameraMoveSwitch)
                {
                    if (!m_TouchInput.enabled)
                        m_TouchInput.enabled = true;
                    //if (!m_MobileTouchCamera.enabled)
                    //    m_MobileTouchCamera.enabled = true;
                }
                else
                {
                    if (m_TouchInput.enabled)
                        m_TouchInput.enabled = false;
                    //if (m_MobileTouchCamera.enabled)
                    //    m_MobileTouchCamera.enabled = false;
                }
            }
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnEnterBattle, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnExitBattle, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnEnterBattle:
                    m_TouchInput.enabled = false;
                    m_MobileTouchCamera.enabled = false;
                    m_BattleProperty.Apply(m_Camera);
                    break;
                case (int)EventID.OnExitBattle:
                    m_TouchInput.enabled = true;
                    m_MobileTouchCamera.enabled = true;
                    m_SimProperty.Apply(m_Camera);
                    break;
            }
        }

        #endregion
    }

    public class CameraProperty
    {
        public Vector3 pos;
        public float size;

        public CameraProperty(Vector3 pos, float size)
        {
            this.pos = pos;
            this.size = size;
        }

        public void Apply(Camera camera)
        {
            camera.transform.position = pos;
            camera.orthographicSize = size;
        }
    }
}