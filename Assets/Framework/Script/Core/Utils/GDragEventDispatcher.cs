using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class GDragEventDispatcher : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    ScrollRect anotherScrollRect;

    private Image thisRaycast;

    void Start()
    {
        FindScrollRect(gameObject);
        if (anotherScrollRect)
        {
            thisRaycast = gameObject.GetComponent<Image>();
        }
    }

    private void FindScrollRect(GameObject obj)
    {
        GameObject tempObj = obj.transform.parent.gameObject;
        anotherScrollRect = tempObj.GetComponent<ScrollRect>();
        if (anotherScrollRect)
        {
            return;
        }
        else
        {
            FindScrollRect(tempObj);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (anotherScrollRect)
        {
            anotherScrollRect.OnBeginDrag(eventData);
        }
        if (thisRaycast)
        {
            thisRaycast.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (anotherScrollRect)
        {
            anotherScrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (anotherScrollRect)
        {
            anotherScrollRect.OnEndDrag(eventData);
        }
        if (thisRaycast)
        {
            thisRaycast.raycastTarget = true;
        }
    }
}