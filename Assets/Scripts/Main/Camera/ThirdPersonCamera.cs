using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("跟随设置")]
    public Transform target;           // 跟随目标
    public Vector3 offset = new Vector3(0, 2f, -5f); // 基础偏移
    public float followSpeed = 5f;     // 跟随速度
    public AnimationCurve followCurve; // 跟随曲线（可选）
    public float followCurveScale = 1f; // 跟随曲线缩放（可选）

    [Header("距离控制")]
    public float minDistance = 2f;    // 最小相机距离
    public float maxDistance = 10f;   // 最大相机距离
    public float zoomSpeed = 5f;      // 缩放速度

    [Header("碰撞检测")]
    public LayerMask collisionMask;   // 碰撞检测层
    public float sphereRadius = 0.3f; // 检测球体半径
    public float collisionOffset = 0.2f; // 碰撞偏移

    private float currentDistance;    // 当前实际距离
    private float targetDistance;    // 目标距离（无碰撞时）
    private Vector3 cameraDirection;  // 相机相对方向

    [Header("旋转控制")]
    public float rotateSpeed = 3f;          // 旋转速度
    public float minVerticalAngle = -80f;   // 最低俯角
    public float maxVerticalAngle = 80f;    // 最高仰角
    public bool invertY = false;            // Y轴反转

    private float currentXRotation;         // 当前水平旋转
    private float currentYRotation;         // 当前垂直旋转
    void Start()
    {
        cameraDirection = offset.normalized;
        currentDistance = targetDistance = offset.magnitude;
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标
        // 初始化旋转角度
        Vector3 initialDirection = offset.normalized;
        currentXRotation = Mathf.Atan2(initialDirection.x, initialDirection.z) * Mathf.Rad2Deg;
        currentYRotation = -Mathf.Asin(initialDirection.y) * Mathf.Rad2Deg;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleRotationInput(); // 新增旋转输入处理
        HandleZoomInput();
        HandleCollision();
        UpdateCameraPosition();
    }
    
    // 新增旋转输入处理方法
    void HandleRotationInput()
    {
        if (Input.GetMouseButton(1)) // 右键按住旋转
        {
            // 获取鼠标输入
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * (invertY ? -1 : 1);

            // 更新旋转角度
            currentXRotation += mouseX;
            currentYRotation = Mathf.Clamp(
                currentYRotation + mouseY, 
                minVerticalAngle, 
                maxVerticalAngle
            );
        }

        // 计算新的相机方向
        Quaternion rotation = Quaternion.Euler(
            -currentYRotation, // 注意符号匹配坐标系
            currentXRotation, 
            0
        );
        cameraDirection = rotation * Vector3.forward;
    }


    // 处理鼠标滚轮输入
    void HandleZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetDistance = Mathf.Clamp(
            targetDistance - scroll * zoomSpeed, 
            minDistance, 
            maxDistance
        );
    }

    // 处理碰撞检测
    void HandleCollision()
    {
        RaycastHit hit;
        Vector3 rayOrigin = target.position;
        Vector3 rayDirection = -cameraDirection;

        // 使用球体射线检测
        if (Physics.SphereCast(
            rayOrigin, 
            sphereRadius, 
            rayDirection, 
            out hit, 
            targetDistance, 
            collisionMask))
        {
            currentDistance = hit.distance - collisionOffset;
        }
        else
        {
            currentDistance = Mathf.Lerp(
                currentDistance, 
                targetDistance, 
                Time.deltaTime * zoomSpeed
            );
        }
    }

    // 修改后的位置更新方法
    void UpdateCameraPosition()
    {
        Vector3 desiredPosition = target.position + cameraDirection * currentDistance;
        
        float curveDistance = currentDistance / followCurveScale;
        float lerp = 1;
        if (curveDistance > 0 && curveDistance < 1)
        {
            // 使用曲线进行平滑移动
            lerp = followCurve.Evaluate(curveDistance);
        }
        // 平滑移动
        transform.position = Vector3.Lerp(
            transform.position, 
            desiredPosition, lerp
        );
        
        // 保持注视目标（带垂直偏移）
        transform.LookAt(target.position + Vector3.up * offset.y);
    }

    // 在Scene视图显示检测球体（可选）
    void OnDrawGizmosSelected()
    {
        if (target == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, sphereRadius);
        Gizmos.DrawLine(target.position, transform.position);
    }
}