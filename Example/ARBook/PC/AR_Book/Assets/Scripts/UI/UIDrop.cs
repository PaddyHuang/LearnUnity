using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image ContainerImage;

    //public Image ReceivingImage;

    public Text ReceivingText;

    public Color HighlightColor = Color.green;
    private Color normalColor;

    public void OnEnable()
    {
        if (ContainerImage != null)
            normalColor = ContainerImage.color;
    }

    public void OnDrop(PointerEventData data)
    {
        ContainerImage.color = normalColor;

        //if (ReceivingImage == null)
        //return;

        //Sprite dropSprite = GetDropSprite(data);
        //if (dropSprite != null)
        //    ReceivingImage.overrideSprite = dropSprite;

        string getStr = GetDropText(data);
        if (getStr != null)
        {
            ReceivingText.text = getStr;
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (ContainerImage == null)
            return;

        string getStr = GetDropText(data);
        if (getStr != null)
            ContainerImage.color = HighlightColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (ContainerImage == null)
            return;

        ContainerImage.color = normalColor;
    }

    private Sprite GetDropSprite(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<UIDrag>();
        if (dragMe == null)
            return null;

        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null)
            return null;

        return srcImage.sprite;
    }

    private string GetDropText(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<UIDrag>();
        if (dragMe == null)
            return null;

        return dragMe.BringStr;
    }
}