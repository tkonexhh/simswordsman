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
	
	    private void Awake()
	    {
	        Instance = this;
	    }
	
	    //void Start()
	    //{
	    //    startPosition = transform.position;
	    //    offset = transform.position - target.position;
	    //}
	
	    void LateUpdate()
	    {
	        oldPosition = transform.position;

            //if (!freazeX)
            //{
            oldPosition.x = Mathf.SmoothDamp(transform.position.x, target.position.x + offset.x, ref xVelocity, smoothTime);
            //Mathf.Clamp(Mathf.SmoothDamp(transform.position.x, target.position.x + offset.x, ref xVelocity, smoothTime), MainGameMgr.S.MainCamera.m_CameraBottomLeft.x, MainGameMgr.S.MainCamera.m_CameraTopRight.x);
            //}
            oldPosition.x = Mathf.Clamp(oldPosition.x, MainGameMgr.S.MainCamera.m_CameraBottomLeft.x, MainGameMgr.S.MainCamera.m_CameraTopRight.x);
            //if (!freazeY)
            //{
            oldPosition.y = Mathf.SmoothDamp(transform.position.y, target.position.y + offset.y, ref yVelocity, smoothTime);
                //Mathf.Clamp(Mathf.SmoothDamp(transform.position.y, target.position.y + offset.y, ref yVelocity, smoothTime), MainGameMgr.S.MainCamera.m_CameraBottomLeft.y, MainGameMgr.S.MainCamera.m_CameraTopRight.y);
                //}
            oldPosition.y = Mathf.Clamp(oldPosition.y, MainGameMgr.S.MainCamera.m_CameraBottomLeft.y, MainGameMgr.S.MainCamera.m_CameraTopRight.y);
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

        public void DestorySelf()
        {
            Destroy(this);
        }
	}
}