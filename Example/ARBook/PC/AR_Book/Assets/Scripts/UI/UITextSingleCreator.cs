using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// Text类 </summary>
public class UITextSingleCreator : MonoBehaviour
{
    //public Text InfoText;
    //public RectTransform LineShow;

    public GameObject LeftGameObject, RightGameObject;

    private RectTransform thisTransform;
    private Transform lockGameObject;
    private bool pass;
    private CanvasGroup cg;

    private Camera mainCamera;

    // private Vector3 offset;
    //private float scaleFactor;

    /// <summary> 
    /// 初始化 </summary>
    public void Init(StructSingle ss, GameObject foundGo)
    {
        pass = true;
        lockGameObject = foundGo.transform;
        thisTransform = GetComponent<RectTransform>();
        cg = thisTransform.GetComponent<CanvasGroup>();

        bool isLeft = ss.LeftOrRight;
        //if (isLeft)
        //{
        //    LineShow.pivot = InfoText.rectTransform.pivot = new Vector2(1, 0.5f);
        //    LineShow.anchoredPosition = Vector2.zero;
        //    
        //    InfoText.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
        //    InfoText.rectTransform.anchoredPosition = new Vector2(-60, 0);
        //}

        Text textshow;

        if (isLeft)
        {
            LeftGameObject.SetActive(true);
            RightGameObject.SetActive(false);
            textshow = LeftGameObject.GetComponentInChildren<Text>();
        }
        else
        {
            LeftGameObject.SetActive(false);
            RightGameObject.SetActive(true);
            textshow = RightGameObject.GetComponentInChildren<Text>();
        }

        //scaleFactor = Screen.width / 1920f;

        //mainCamera = Camera.main;
        mainCamera = CameraChange.Instance.CurrentCam;
        //InfoText.text = ss.CombinedStr;
        textshow.text = ss.CombinedStr;
    }

    private void LateUpdate()
    {
        if (pass)
        {
            Vector2 scrPos = mainCamera.WorldToScreenPoint(lockGameObject.position) / GameData.Instance.ScaleFactor;
            thisTransform.anchoredPosition = scrPos;
        }
    }

    public void ActiveFunc(bool b)
    {
        thisTransform.gameObject.SetActive(b);
    }

    /// <summary> 
    /// 动画过渡特效 </summary>
    public void AlphaEffect()
    {
        DOTween.To(() => cg.alpha, x => cg.alpha = x, 0.4f, 0.5f);
    }

    /// <summary>
    /// 重置透明度
    /// </summary>
    /// <param name="needDeAct">是否需要隐藏物体</param>
    public void ResetEffect(bool needDeAct)
    {
        cg.alpha = 1;
        if (needDeAct)
        {
            thisTransform.gameObject.SetActive(false);
        }
    }
}