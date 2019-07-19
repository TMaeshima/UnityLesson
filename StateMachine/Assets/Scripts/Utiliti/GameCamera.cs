using UnityEngine;

/// <summary>
/// 追いかけゲームカメラ
/// </summary>
public class GameCamera : MonoBehaviour
{
    [SerializeField] private float trackingLerp = 0.1f;
    [SerializeField] private float interestForwardDistance = 0f;
    [SerializeField] private float heightShiftThreashold = 2f;
    [SerializeField] private Transform initTarget;

    private Transform target;
    private Vector3 cameraOffset;
    private Transform trans;
    private float currentInterestY;

    void Awake()
    {
        cameraOffset = transform.position;
        trans = transform;
    }

    void Start()
    {
        // 初期追跡オブジェクトが設定されているときは、注視キャラとして設定。
        // シーンに配置されている状態をカメラの位置と回転とする。
        if (initTarget != null) {
            cameraOffset = trans.position - initTarget.position;
            SetTarget(initTarget);
        }
    }

    void LateUpdate()
    {
        UpdatePosition(trackingLerp);
    }

    public void SetTarget(Transform t)
    {
        target = t;
        currentInterestY = t.position.y;
        UpdatePosition(1.0f);
    }

    private void UpdatePosition(float lerp)
    {
        var tpos = target != null ? target.position : Vector3.zero;
        if (Mathf.Abs(tpos.y - currentInterestY) > heightShiftThreashold) 
        {
            currentInterestY = tpos.y;
        }
        tpos.y = currentInterestY;
        var fwd = target != null ? target.forward : Vector3.zero;
        trans.position = Vector3.Lerp(trans.position, tpos + fwd * interestForwardDistance + cameraOffset, lerp);
    }
}
