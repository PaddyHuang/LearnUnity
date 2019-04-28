using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISizeSingleCreator : MonoBehaviour
{
    //缩放比
    private float uiToWorldScale;

    //模板
    private RectTransform lineTemplate;

    private CanvasGroup cg;

    [SerializeField] private Text valueText;

    private void Init()
    {
        lineTemplate = GetComponent<RectTransform>();

        if (!ReferenceEquals(lineTemplate, null))
            uiToWorldScale = lineTemplate.rect.width;
    }

    /// <summary> 
    /// 用于计算类型的尺寸创建  </summary>
    public void SetSizeLineCalcul(Transform from, Transform to, string value)
    {
        //初始化
        Init();

        //调位
        Vector3 eng = from.position + to.position;
        lineTemplate.position = eng / 2;

        //获取方向,指向to
        Vector3 ro = to.position - from.position;

        lineTemplate.right = ro;

        //拉伸
        float dic = ro.magnitude;
        lineTemplate.sizeDelta = new Vector2(dic * uiToWorldScale, lineTemplate.rect.height);

        valueText.text = value;
    }

    /// <summary> 
    /// 创建标尺线 </summary>
    public void SetSizeLine(Transform from, Transform to, string value)
    {
        //初始化
        Init();

        //调位
        Vector3 eng = from.position + to.position;
        lineTemplate.position = eng / 2;

        //获取方向,指向to
        Vector3 ro = to.position - from.position;

        //与两向量都垂直的向量
        Vector3 czV3 = Vector3.Cross(Vector3.forward, ro);

        lineTemplate.right = ro;
        //获取旋转角度
        float angle = Vector3.Angle(czV3.normalized, lineTemplate.forward.normalized);

        //矫正角度
        lineTemplate.Rotate(Vector3.right, angle);

        //拉伸
        float dic = ro.magnitude;
        lineTemplate.sizeDelta = new Vector2(dic * uiToWorldScale, lineTemplate.rect.height);

        //竖直角度是否与垂直向上接近0度,接近则旋转文字
        float angleScreen = Vector3.Angle(ro, Vector3.up);

        //if (Mathf.Abs(angleScreen - 180) < 15)
        if (Mathf.Abs(angleScreen) < 5)
        {
            valueText.transform.localRotation = Quaternion.Euler(0, 0, -90);
            valueText.rectTransform.pivot = new Vector2(0, 0.5f);
            valueText.alignment = TextAnchor.MiddleLeft;
            valueText.rectTransform.anchoredPosition = new Vector2(0, 0);
        }

        valueText.text = value;

        cg = transform.GetComponent<CanvasGroup>();
    }

    /// <summary> 
    /// 隐藏或显示   </summary>
    public void ActOrHideLine(bool hoa)
    {
        gameObject.SetActive(hoa);
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
            gameObject.SetActive(false);
        }
    }
}