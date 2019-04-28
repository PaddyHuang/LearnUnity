using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemStruct))]
public class CameraHighLighting : MonoBehaviour
{
    private event Action<Transform> OnHighLightHandler;
    public event Action<Transform> OnHighLight
    {
        add
        {
            OnHighLightHandler -= value;
            OnHighLightHandler += value;
        }
        remove { OnHighLightHandler -= value; }
    }

    private ItemStruct m_Struct;

    public LayerMask TargetingLayerMask = -1;

    private float targetingRayLength = Mathf.Infinity;

    private Camera cam;

    private bool isPause;

    void Awake()
    {
        cam = CameraChange.Instance.CurrentCam;
    }

    void Start()
    {
        m_Struct = GetComponent<ItemStruct>();
    }

    void Update()
    {
        if (isPause)
            return;

#if UNITY_STANDALONE
        TargetingRaycastPC();
#elif UNITY_ANDROID
        TargetingRaycastMb();
#endif

        // TargetingRaycast();
    }

    private void TargetingRaycastPC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (ReferenceEquals(cam, null))
                return;

            RaycastHit hitInfo;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Transform targetTransform = null;

            if (Physics.Raycast(ray, out hitInfo, targetingRayLength, TargetingLayerMask.value))
            {
                targetTransform = hitInfo.collider.transform;
            }

            if (targetTransform != null)
            {
                OnHighLightHandler?.Invoke(targetTransform);

                m_Struct.GetHighlightInfo(targetTransform);
            }
        }
    }

    private void TargetingRaycastMb()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return;
            //如果不是一次点击 直接返回
            switch (touch.phase)
            {
                case TouchPhase.Stationary: break;
                default: return;
            }

            if (ReferenceEquals(cam, null))
                return;

            RaycastHit hitInfo;

            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
            Transform targetTransform = null;

            if (Physics.Raycast(ray, out hitInfo, targetingRayLength, TargetingLayerMask.value))
            {
                targetTransform = hitInfo.collider.transform;
            }

            if (targetTransform != null)
            {
                OnHighLightHandler?.Invoke(targetTransform);

                m_Struct.GetHighlightInfo(targetTransform);
            }
        }
    }

    private void ResetState()
    {
        OnHighLightHandler?.Invoke(null);
    }

    /// <summary> 
    /// 暂停执行Update并且重置 </summary>
    public void HighLightPause()
    {
        ResetState();
        isPause = true;
    }

    /// <summary> 
    /// 开始执行Update并且重置 </summary>
    public void HighLightStart()
    {
        ResetState();
        isPause = false;
    }
}