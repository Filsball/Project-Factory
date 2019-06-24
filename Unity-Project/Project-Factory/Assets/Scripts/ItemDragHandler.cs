using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public InventoryItem Item { get; set; }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData) {
        transform.localPosition = Vector3.zero;
    }
}
