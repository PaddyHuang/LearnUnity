using UnityEngine;

public class UIScanEffect : MonoBehaviour
{
    private float origStartPos;
    private RectTransform rc;

    private float elapsed, duration;

    void Start()
    {
        rc = GetComponent<RectTransform>();

        origStartPos = rc.rect.height;
        rc.anchoredPosition = new Vector2(0, origStartPos);

        duration = 2.4f;
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed <= duration)
        {
            rc.anchoredPosition = Vector2.Lerp(new Vector2(0, origStartPos), new Vector2(0, -origStartPos), elapsed / duration);
        }
        else
        {
            elapsed = 0;
        }
    }
}