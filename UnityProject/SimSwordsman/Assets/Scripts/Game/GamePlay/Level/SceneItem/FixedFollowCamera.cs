using DG.Tweening;
using UnityEngine;

namespace GameWish.Game
{
    public class FixedFollowCamera : MonoBehaviour
    {
        // 需要跟随的目标对象
        public Transform target;

        // 需要锁定的坐标（可以实时生效）
        //public bool freazeX;
        //public bool freazeY;
        //public bool freazeZ;

        // 跟随的平滑时间（类似于滞后时间）
        public float smoothTime = 0.1f;
        private float xVelocity, yVelocity, zVelocity = 0.0F;

        // 跟随的偏移量
        private Vector3 offset;

        // 全局缓存的位置变量
        private Vector3 oldPosition;

        // 记录初始位置
        private Vector3 startPosition;

        public static FixedFollowCamera Instance;

        Camera m_Camera;
        private void Awake()
        {
            Instance = this;
            m_Camera = GetComponent<Camera>();
        }

        //void Start()
        //{
        //    startPosition = transform.position;
        //    offset = transform.position - target.position;
        //}

        void Update()
        {
            if (target == null) return;

            oldPosition = transform.position;

            //if (!freazeX)
            //{
            oldPosition.x = Mathf.SmoothDamp(transform.position.x, target.position.x + offset.x, ref xVelocity, smoothTime);
            oldPosition.x = Mathf.Clamp(oldPosition.x, -3f, 12f);
            //if (!freazeY)
            //{
            oldPosition.y = Mathf.SmoothDamp(transform.position.y, target.position.y + offset.y, ref yVelocity, smoothTime);
            oldPosition.y = Mathf.Clamp(oldPosition.y, -1.8f, 6.3f);
            //if (!freazeZ)
            //{
            //    oldPosition.z = Mathf.SmoothDamp(transform.position.z, target.position.z + offset.z, ref zVelocity, smoothTime);
            //}

            transform.position = oldPosition;
        }

        /// <summary>
        /// 用于重新开始游戏时直接重置相机位置
        /// </summary>
        public void ResetPosition()
        {
            transform.position = startPosition;
        }

        public void SetTarget(Transform tar)
        {
            target = tar;
            startPosition = transform.position;
            offset = Vector3.zero;
            //offset = transform.position - target.position;
        }

        public void TweenOrthoSize(float value)
        {
            m_Camera.DOOrthoSize(value, 0.8f).SetEase(Ease.OutCubic);
        }

        public void DestorySelf()
        {
            Destroy(this);
        }
    }
}
