using DG.Tweening;
using Qarth;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class CircleShaderControl : MonoBehaviour, ICanvasRaycastFilter
    {
        [SerializeField] private Shader m_GuideShader;

        private Tweener m_Tweener;
        private Tweener m_Tweener2;

        private bool m_IsGuide = false;

        /// <summary>
        /// 要高亮显示的目标
        /// </summary>
        public RectTransform m_Target;

        /// <summary>
        /// 镂空区域半径
        /// </summary>
        private float m_Radius;

        /// <summary>
        /// 遮罩材质
        /// </summary>
        private Material m_Material;

        /// <summary>
        /// 当前高亮区域的半径
        /// </summary>
        private float m_CurrentRadius;

        /// <summary>
        /// 区域范围缓存
        /// </summary>
        private Vector3[] m_Corners = new Vector3[4];

        /// <summary>
        /// 是否需要黑色遮罩
        /// </summary>
        private bool m_IsNeedBlackMask = true;

        /// <summary>
        /// 世界坐标向画布坐标转换
        /// </summary>
        /// <param name="canvas">画布</param>
        /// <param name="world">世界坐标</param>
        /// <returns>返回画布上的二维坐标</returns>
        private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
        {
            Vector2 position = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out position);

            return position;
        }

        private void ShowGuideAnimation()
        {
            //Debug.LogError("current radiu:" + m_CurrentRadius);
            m_Tweener = DOTween.To((value) =>
            {
                m_CurrentRadius = Mathf.Lerp(m_CurrentRadius, m_Radius, value);
            }, 0, 1, 0.5f).OnComplete(StartYoYo);
        }

        private void StartYoYo()
        {
            m_Tweener2 = DOTween.To((x) =>
            {
                m_CurrentRadius = Mathf.Lerp(m_Radius, m_Radius + m_Radius * 0.15f, x);

            }, 0, 1, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnEnable()
        {
            m_Material = new Material(m_GuideShader);
            GetComponent<Image>().material = m_Material;
        }

        private void Update()
        {
            if (!m_IsGuide) return;

            if (m_IsNeedBlackMask)
            {
                //m_CurrentRadius = Mathf.Clamp(m_CurrentRadius, 50, 250);
                m_Material.SetFloat("_Slider", m_CurrentRadius);
            }
        }

        private void OnDestroy()
        {
            if (m_Material != null)
            {
                GameObject.Destroy(m_Material);
            }
        }
        public void InitWithRectMask(RectTransform target)
        {
            m_Target = target;

            m_IsNeedBlackMask = false;
            //获取画布
            Canvas canvas = GameObject.Find("UIRoot").GetComponent<UIRoot>().rootCanvas;

            //获取高亮区域的四个顶点的界面坐标,中心为(0,0)
            m_Target.GetWorldCorners(m_Corners);

            float rectHeight = Vector2.Distance(WorldToCanvasPos(canvas, m_Corners[0]), WorldToCanvasPos(canvas, m_Corners[1])) * 0.5f;
            float rectWidth = Vector2.Distance(WorldToCanvasPos(canvas, m_Corners[0]), WorldToCanvasPos(canvas, m_Corners[3])) * 0.5f;

            if (m_Target.gameObject.name == "BtnBg")
            {
                m_Radius = 250f;
            }

            //计算高亮显示区域的圆心
            float x = m_Corners[0].x + ((m_Corners[3].x - m_Corners[0].x) / 2f);

            float y = m_Corners[0].y + ((m_Corners[1].y - m_Corners[0].y) / 2f);

            Vector3 centerWorld = new Vector3(x, y, 0);

            Vector2 center = WorldToCanvasPos(canvas, centerWorld);

            //设置遮罩材料中的圆心变量
            Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);

            //计算当前高亮显示区域的半径
            RectTransform canRectTransform = canvas.transform as RectTransform;

            if (canRectTransform != null)
            {
                //获取画布区域的四个顶点
                canRectTransform.GetWorldCorners(m_Corners);

                //将画布顶点距离高亮区域中心最远的距离作为当前高亮区域半径的初始值
                foreach (Vector3 corner in m_Corners)
                {
                    //_currentRadius = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corner), center),
                    //    _currentRadius);
                }
            }
            m_CurrentRadius = 2000;
            m_IsGuide = true;

            m_Material.SetFloat("_MaskMode", 2.0f);
            m_Material.SetVector("_Center", centerMat);
            m_Material.SetVector("_rectSize", new Vector4(rectWidth, rectHeight, 0, 0));

            //ShowGuideAnimation();
        }
        public void Init(RectTransform target)
        {
            m_Target = target;

            m_IsNeedBlackMask = true;
            //获取画布
            Canvas canvas = GameObject.Find("UIRoot").GetComponent<UIRoot>().rootCanvas;

            //获取高亮区域的四个顶点的界面坐标,中心为(0,0)
            m_Target.GetWorldCorners(m_Corners);

            //计算最终高亮显示区域的半径
            m_Radius = Vector2.Distance(WorldToCanvasPos(canvas, m_Corners[0]), WorldToCanvasPos(canvas, m_Corners[2])) / 2f;

            if (m_Target.gameObject.name == "BtnBg")
            {
                m_Radius = 250f;
            }

            //计算高亮显示区域的圆心
            float x = m_Corners[0].x + ((m_Corners[3].x - m_Corners[0].x) / 2f);

            float y = m_Corners[0].y + ((m_Corners[1].y - m_Corners[0].y) / 2f);

            Vector3 centerWorld = new Vector3(x, y, 0);

            Vector2 center = WorldToCanvasPos(canvas, centerWorld);

            //设置遮罩材料中的圆心变量
            Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);

            //计算当前高亮显示区域的半径
            RectTransform canRectTransform = canvas.transform as RectTransform;

            if (canRectTransform != null)
            {
                //获取画布区域的四个顶点
                canRectTransform.GetWorldCorners(m_Corners);

                //将画布顶点距离高亮区域中心最远的距离作为当前高亮区域半径的初始值
                foreach (Vector3 corner in m_Corners)
                {
                    //_currentRadius = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corner), center),
                    //    _currentRadius);
                }
            }
            m_CurrentRadius = 2000;
            m_IsGuide = true;

            m_Material.SetFloat("_MaskMode", 0.0f);
            m_Material.SetFloat("_Slider", m_CurrentRadius);
            m_Material.SetVector("_Center", centerMat);
            m_Material.SetFloat("_UseSecondCircle", 0);
            m_Material.SetFloat("_AlphaSlider", 0.167f);

            ShowGuideAnimation();
        }

        public void InitTwoTarget(RectTransform target, Vector2 targe2Pos)
        {
            m_Target = target;

            m_IsNeedBlackMask = true;
            //获取画布
            Canvas canvas = GameObject.Find("UIRoot").GetComponent<UIRoot>().rootCanvas;

            Vector2 center = GetCenterPos(canvas, target);

            Vector4 centerMat = GetCenterMat(center);

            float CurrentRadius = GetCurrentRadius(canvas, center);

            m_IsGuide = true;

            m_Material.SetFloat("_Slider", CurrentRadius);
            m_Material.SetVector("_Center", centerMat);
            m_Material.SetVector("_Center2", targe2Pos);
            m_Material.SetFloat("_UseSecondCircle", 1);
            m_Material.SetFloat("_AlphaSlider", 0.167f);

            ShowGuideAnimation();
        }
        private Vector2 GetCenterPos(Canvas canvas, RectTransform target)
        {
            //获取高亮区域的四个顶点的界面坐标,中心为(0,0)
            target.GetWorldCorners(m_Corners);

            //计算最终高亮显示区域的半径
            m_Radius = Vector2.Distance(WorldToCanvasPos(canvas, m_Corners[0]),
                WorldToCanvasPos(canvas, m_Corners[2])) / 2f;

            //计算高亮显示区域的圆心
            float x = m_Corners[0].x + ((m_Corners[3].x - m_Corners[0].x) / 2f);
            float y = m_Corners[0].y + ((m_Corners[1].y - m_Corners[0].y) / 2f);

            Vector3 centerWorld = new Vector3(x, y, 0);

            Vector2 center = WorldToCanvasPos(canvas, centerWorld);

            return center;
        }

        private Vector4 GetCenterMat(Vector2 center)
        {
            Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);

            return centerMat;
        }
        private float GetCurrentRadius(Canvas canvas, Vector2 center)
        {
            RectTransform canRectTransform = canvas.transform as RectTransform;

            if (canRectTransform != null)
            {
                //获取画布区域的四个顶点
                canRectTransform.GetWorldCorners(m_Corners);

                //将画布顶点距离高亮区域中心最远的距离作为当前高亮区域半径的初始值
                foreach (Vector3 corner in m_Corners)
                {
                    m_CurrentRadius = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corner), center),
                        m_CurrentRadius);
                }
            }

            return m_CurrentRadius;
        }

        public void EndGuide()
        {
            //Debug.LogError("EndGuide");
            if (m_Tweener != null)
            {
                m_Tweener.Kill();
            }
            if (m_Tweener2 != null)
            {
                m_Tweener2.Kill();
            }

            m_IsGuide = false;

            //m_CurrentRadius = 10f;

            m_Material.SetFloat("_Slider", m_CurrentRadius);
        }

        public void InitAsNoBlack(RectTransform target)
        {
            m_IsNeedBlackMask = false;
            m_IsGuide = true;
            m_Material.SetFloat("_Slider", m_CurrentRadius);
            m_Target = target;
        }

        //目标物体移除遮罩效果
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            //没有目标则捕捉事件渗透
            if (m_Target == null)
            {
                return true;
            }
            //在目标范围内做事件渗透
            return !RectTransformUtility.RectangleContainsScreenPoint(m_Target, sp, eventCamera);
        }
    }
}
