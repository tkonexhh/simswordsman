using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
namespace GameWish.Game
{
    public class PanelRoot : MonoBehaviour
    {
        [SerializeField] private Transform m_PanelRoot;
        [SerializeField] private GameObject m_ObjMaskBg;
        [SerializeField] private GameObject m_TopMask;
        [SerializeField] private GameObject m_BottomMask;
        private void Awake()
        {
            SetCamViewPortRect();
        }
        public void SetCamViewPortRect()
        {
            if (m_ObjMaskBg)
                m_ObjMaskBg.SetActive(false);


            AppropriateLongScreen();
            // #if UNITY_IOS
            //             if (ScreenAdjustMgr.S.GetScreenType() == ScreenType.IPhoneX)
            //             {
            //                 m_PanelRoot.rectTransform().offsetMin = new Vector2(0, 30);
            //                 m_PanelRoot.rectTransform().offsetMax = new Vector2(0, -55);
            //                 m_TopMask.SetActive(true);
            //                 m_BottomMask.SetActive(true);
            //                 if (m_ObjMaskBg != null)
            //                     m_ObjMaskBg.SetActive(true);
            //             }
            //             else
            //             {
            //                 m_PanelRoot.rectTransform().offsetMin = Vector2.zero;
            //                 m_PanelRoot.rectTransform().offsetMax = Vector2.zero;
            //                 m_TopMask.SetActive(false);
            //                 m_BottomMask.SetActive(false);
            //                 if (m_ObjMaskBg != null)
            //                     m_ObjMaskBg.SetActive(false);
            //             }
            // #endif
        }

        void AppropriateLongScreen()
        {
            if (Screen.height * 1.0f / Screen.width > 2)
            {

                var rectRoot = UIMgr.S.uiRoot.panelRoot.GetComponent<RectTransform>();
                var rect = Screen.safeArea;
                float canvasHeight = UIMgr.S.uiRoot.rootCanvas.rectTransform().rect.height;
                // Debug.LogError(Screen.height + ":" + rect.height + ":" + canvasHeight);
                float deltaY = (Screen.height - rect.height) / Screen.height * canvasHeight;
                UIMgr.S.panelOffset = -deltaY;
                rectRoot.offsetMax = new Vector2(rectRoot.offsetMax.x, UIMgr.S.panelOffset);
                rectRoot.offsetMin = new Vector2(rectRoot.offsetMin.x, 0);

                //下移了 需要上层遮挡
            }
        }
    }
}
