using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 画面ポインティング入力とマーカー表示。
/// </summary>
public class PointInput : MonoBehaviour
{
    [SerializeField] private Transform pointerPrefab;
    [SerializeField] private Transform targetMarkerPrefab;
    [SerializeField] private Camera cameraObj;

    public PointerEvent onPoint = new PointerEvent();
    public PointerEvent onUpdatePoint = new PointerEvent();
    public PointerEvent onDrag = new PointerEvent();

    [System.Serializable] public class PointerEvent : UnityEvent<Vector3> { }

    private Transform pointerObj = null;
    private Transform targetMarkerObj = null;

    void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        if (cameraObj == null) {
            cameraObj = GetComponent<Camera>();
        }
    }

    void Start()
    {
        if (pointerPrefab != null) {
            pointerObj = Instantiate(pointerPrefab);
        }
        if (targetMarkerPrefab != null) {
            targetMarkerObj = Instantiate(targetMarkerPrefab);
        }
        HideTarget();
    }

    void LateUpdate()
    {
        RaycastHit hit;
        var ray = cameraObj.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);
        if (Physics.Raycast(ray, out hit)) {
            OnUpdatePoint(hit.point);
            if (Input.GetMouseButtonUp(0)) {
                OnPoint(hit.point);
            }
            if (Input.GetMouseButton(0)) {
                OnDrag(hit.point);
            }
        }
    }

    // 画面上をドラッグ。矢印マーカーは消す。
    private void OnDrag(Vector3 pos)
    {
        HideTarget();
        onDrag.Invoke(pos);
    }

    // 画面上をクリック/タッチ。矢印マーカーを出す
    private void OnPoint(Vector3 pos)
    {
        ShowTarget(pos);
        onPoint.Invoke(pos);
    }

    // 画面上をポインタ移動
    private void OnUpdatePoint(Vector3 pos)
    {
        if (pointerObj != null) {
            pointerObj.position = pos;
        }
        onUpdatePoint.Invoke(pos);
    }

    private void ShowTarget(Vector3 pos)
    {
        if (targetMarkerPrefab != null) {
            targetMarkerObj.gameObject.SetActive(true);
            targetMarkerObj.position = pos;
        }
    }

    // 矢印ポインタ隠す
    private void HideTarget()
    {
        if (targetMarkerPrefab != null) {
            targetMarkerObj.gameObject.SetActive(false);
        }
    }
}