using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    private static Transform target;
    private static bool isBegin;

    private static Quaternion origQuater;
    private static Vector3 origScale;
    private static Vector3 origPos;

    private float rotateSpeed = 5;

    private bool isPause;

    //void PCControl()
    //{
    //    if (!isBegin)
    //        return;

    //    //单点触摸， 水平上下旋转
    //    if (Input.GetMouseButton(0))
    //    {
    //        //Touch touch = Input.GetTouch(0);

    //        Vector2 deltaPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

    //        target.Rotate(Vector3.down * deltaPos.x /** Time.deltaTime*/ * rotateSpeed, Space.Self); //绕Y轴进行旋转

    //        float ag = target.up.AngleWithY(Vector3.up);

    //        if (ag > 90 && deltaPos.y < 0)
    //            return;
    //        if (ag < -90 && deltaPos.y > 0)
    //            return;

    //        target.Rotate(Vector3.right * deltaPos.y /** Time.deltaTime*/ * rotateSpeed, Space.World); //绕X轴进行旋转，下面我们还可以写绕Z轴进行旋转

    //        return;
    //    }

    //    float of = Input.GetAxis("Mouse ScrollWheel");

    //    if (Mathf.Abs(of) > 0)
    //    {
    //        //Touch touchZero = Input.GetTouch(0);
    //        //Touch touchOne = Input.GetTouch(1);

    //        //// Find the position in the previous frame of each touch.
    //        //Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
    //        //Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

    //        //// Find the magnitude of the vector (the distance) between the touches in each frame.
    //        //float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
    //        //float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

    //        //// Find the difference in the distances between each frame.
    //        //float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

    //        float scaleF = of;

    //        target.localScale += target.localScale * scaleF;
    //    }
    //}

    //void Update()
    //{
    //    PCControl();
    //    if (!isBegin)
    //        return;

    //    //没有触摸，就是触摸点为0
    //    if (Input.touchCount <= 0)
    //        return;

    //    //单点触摸， 水平上下旋转
    //    if (Input.touchCount == 1)
    //    {
    //        Touch touch = Input.GetTouch(0);

    //        Vector2 deltaPos = touch.deltaPosition;

    //        target.Rotate(Vector3.down * deltaPos.x * Time.deltaTime * rotateSpeed, Space.Self); //绕Y轴进行旋转

    //        float ag = target.up.AngleWithY(Vector3.up);

    //        if (ag > 90 && deltaPos.y < 0)
    //            return;
    //        if (ag < -90 && deltaPos.y > 0)
    //            return;

    //        target.Rotate(Vector3.right * deltaPos.y * Time.deltaTime * rotateSpeed, Space.World); //绕X轴进行旋转，下面我们还可以写绕Z轴进行旋转

    //        return;
    //    }

    //    if (Input.touchCount == 2)
    //    {
    //        Touch touchZero = Input.GetTouch(0);
    //        Touch touchOne = Input.GetTouch(1);

    //        // Find the position in the previous frame of each touch.
    //        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
    //        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

    //        // Find the magnitude of the vector (the distance) between the touches in each frame.
    //        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
    //        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

    //        // Find the difference in the distances between each frame.
    //        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

    //        float scaleF = deltaMagnitudeDiff / 1000f;

    //        target.localScale -= target.localScale * scaleF;
    //    }
    //}

    public static void SetTarget(Transform tar)
    {
        if (!ReferenceEquals(tar, null))
        {
            origQuater = tar.localRotation;
            origScale = tar.localScale;
            origPos = tar.localPosition;
            target = tar;

            isBegin = true;

            AroundCameraMobile.Instance.SetDefault();

        }
    }

    public static void SetDefault()
    {
        if (!ReferenceEquals(target, null))
        {
            target.localRotation = origQuater;
            target.localScale = origScale;
            target.localPosition = origPos;
            target = null;
            isBegin = false;

            AroundCameraMobile.Instance.SetDefault();

        }
    }

    public static void GameObjectLost()
    {
        SetDefault();
    }

    public static void Pause()
    {
        Debug.Log(target);
        if (!ReferenceEquals(target, null))
        {
            isBegin = false;
        }
    }

    public static void Continue()
    {
        if (!ReferenceEquals(target, null))
        {
            isBegin = true;
        }
    }

}