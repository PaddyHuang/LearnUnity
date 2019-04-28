using UnityEngine;
using UnityEngine.UI;

public class UICalculShowSingle : MonoBehaviour
{
    public Text IndexStr, InfoText;

    public RectTransform LineShow;

    private bool pass;
    private Camera mainCamera;

    private Transform lockGameObject;

    private RectTransform thisTransform;

    public void Init(CalculationSingle cls, GameObject foundGo)
    {
        pass = true;
        lockGameObject = foundGo.transform;
        thisTransform = GetComponent<RectTransform>();

        bool isLeft = cls.LeftOrRight;
        if (isLeft)
        {
            //序号
            //IndexStr.rectTransform.pivot = new Vector2(1, 0.5f);
            //IndexStr.rectTransform.anchoredPosition = Vector2.zero;

            //线条
            LineShow.anchoredPosition *= -1;

            //todo:硬编码
            InfoText.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            InfoText.rectTransform.pivot = new Vector2(1, 0.5f);
            InfoText.rectTransform.anchoredPosition *= -1;
        }
        //scaleFactor = Screen.width / 1280f;

       // mainCamera = Camera.main;
        mainCamera = CameraChange.Instance.CurrentCam;

        IndexStr.text = cls.GameObjectIndex;

        InfoText.text = cls.Info;

        ////初始化时就更新位置 ，防止第一次点击时闪烁
        //Vector2 scrPos = mainCamera.WorldToScreenPoint(lockGameObject.position) / GameData.ScaleFactor;
        //thisTransform.anchoredPosition = scrPos;
    }

    void LateUpdate()
    {
        if (pass)
        {
            Vector2 scrPos = mainCamera.WorldToScreenPoint(lockGameObject.position) / GameData.Instance.ScaleFactor;
            thisTransform.anchoredPosition = scrPos;
        }
    }
}