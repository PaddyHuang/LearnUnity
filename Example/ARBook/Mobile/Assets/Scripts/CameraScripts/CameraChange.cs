using UnityEngine;

public class CameraChange : MonoBehaviour
{
    #region Mono-Singleton

    private static CameraChange instance;
    public static CameraChange Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<CameraChange>();
            return instance;
        }
    }

    #endregion

    public Camera CurrentCam => rendererCamera;

    [SerializeField] private Camera mainCamera, singleCamera, rendererCamera;

    [SerializeField] private LayerMask noDefaultLayerMask;

    private const string SingleStr = "SingleLayer";

    private LayerMask origLayerMask;
    private LayerMask saveMask;

    private Transform saveChanging;

    private AroundCameraMobile m_AroundCamera;

    private Quaternion targetOrigRotate;

    void Start()
    {
        m_AroundCamera = AroundCameraMobile.Instance;

        if (!ReferenceEquals(mainCamera, null))
        {
            origLayerMask = mainCamera.cullingMask;
        }

        if (singleCamera == null)
        {
            Debug.LogError("Single is Null");
        }
    }

    public void ChangeToSingle(Transform obj)
    {
        // mainCamera.cullingMask = noDefaultLayerMask;
        if (!ReferenceEquals(obj, null))
        {
            saveMask = rendererCamera.cullingMask;

            saveChanging = obj;
            saveChanging.gameObject.layer = LayerMask.NameToLayer(SingleStr);
            rendererCamera.cullingMask = 1 << LayerMask.NameToLayer(SingleStr);

            //从本地旋转换为世界
            targetOrigRotate = saveChanging.localRotation;
            m_AroundCamera.Target.position = saveChanging.position;
        }
    }

    public void ChangeToDefault()
    {
        if (!ReferenceEquals(saveChanging, null))
        {
            saveChanging.gameObject.layer = LayerMask.NameToLayer("Default");
            rendererCamera.cullingMask = saveMask;

            saveChanging = null;

            m_AroundCamera.SetDefault();
        }
    }

    //public void ChangeToSingle(Transform obj)
    //{
    //    mainCamera.cullingMask = noDefaultLayerMask;
    //    if (!ReferenceEquals(obj, null))
    //    {
    //        saveChanging = obj;
    //        saveChanging.gameObject.layer = LayerMask.NameToLayer(SingleStr);
    //        //从本地旋转换为世界
    //        targetOrigRotate = saveChanging.localRotation;
    //        saveChanging.rotation = targetOrigRotate;
    //        //ObjectControl.SetTarget(obj);
    //        ObjectControl.Pause();

    //        rendererCamera.gameObject.SetActive(false);

    //        m_AroundCamera.target = saveChanging;
    //        m_AroundCamera.enabled = true;
    //    }
    //}

    //public void ChangeToDefault()
    //{
    //    mainCamera.cullingMask = origLayerMask;
    //    if (!ReferenceEquals(saveChanging, null))
    //    {
    //        saveChanging.gameObject.layer = LayerMask.NameToLayer("Default");
    //        saveChanging.localRotation = targetOrigRotate;

    //        saveChanging = null;

    //        ObjectControl.Continue();

    //        m_AroundCamera.enabled = false;

    //        m_AroundCamera.target = null;

    //        rendererCamera.gameObject.SetActive(true);
    //    }
    //}
}