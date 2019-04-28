using UnityEngine;

public class ItemDragContainer : MonoBehaviour
{
    //public UIDrag CurrentGrab;
    //public int Index;

    public RectTransform LineShow, Container;

    private Transform bindingGo; //绑定的游戏物体
    private Camera mainCamera; //主相机
    private RectTransform thisTransform; //该物体本身

    private bool isPass; //是否找到了游戏物体

    public UIDrop UIDrop;

    public void CreatContainer(Transform bt, bool isLeft)
    {
        if (bt != null)
        {
            bindingGo = bt;
            isPass = true;
        }

        mainCamera = CameraChange.Instance.CurrentCam;
        thisTransform = GetComponent<RectTransform>();

        //在左边显示
        if (isLeft)
        {
            LineShow.anchoredPosition = new Vector2(-LineShow.anchoredPosition.x, LineShow.anchoredPosition.y);

            Vector2 orig = Container.anchoredPosition;
            Container.pivot = new Vector2(1, 0.5f);
            Container.anchoredPosition = new Vector2(-orig.x, orig.y);
        }

        //Container.gameObject.AddComponent<DragContainerSymbol>().BindingContainer(this);
        UIDrop = StaticClass.FindInChild<UIDrop>(gameObject);
        if (UIDrop == null)
            Debug.LogError("Can't find child");
    }

    //public void SetCurrentGrab(UIDrag uid)
    //{
    //    CurrentGrab = uid;
    //}

    private void LateUpdate()
    {
        if (isPass)
        {
            Vector2 scrPos = mainCamera.WorldToScreenPoint(bindingGo.position) / GameData.Instance.ScaleFactor;
            thisTransform.anchoredPosition = scrPos;
        }
    }
}

public class DragContainerSymbol : MonoBehaviour
{
    private ItemDragContainer m_Container;

    public void BindingContainer(ItemDragContainer bd)
    {
        m_Container = bd;
    }

    public void GrabGragItem(UIDrag uid)
    {
        // m_Container.SetCurrentGrab(uid);
    }
}