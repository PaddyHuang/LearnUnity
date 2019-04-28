using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //private RectTransform thisRectTransform;

    private event Action<float> OnDragAddHandler;
    public event Action<float> OnDragAdd
    {
        add
        {
            OnDragAddHandler -= value;
            OnDragAddHandler += value;
        }
        remove { OnDragAddHandler -= value; }
    }

    private Vector3 offset;
    private Vector3 orig;

    private float scaleFactory = 0.0001f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = Input.mousePosition - transform.position;
        orig = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition - offset;
        DragAdd();
    }

    private void DragAdd()
    {
        //float x = Mathf.Abs(transform.position.normalized.x - orig.normalized.x);
        //float y = Mathf.Abs(transform.position.normalized.y - orig.normalized.y); 

        //Vector3 v = (transform.position - orig).normalized;

        float x = Mathf.Abs(transform.position.x - orig.x);
        float y = Mathf.Abs(transform.position.y - orig.y);
        orig = transform.position;

        OnDragAddHandler?.Invoke((x + y) * scaleFactory);

        //return x + y;
    }
}