using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            ItemDragHandler itemDragged = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if(itemDragged != null)
            {
                RaycastHit hit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction);
                if(Physics.Raycast(ray, out hit, 1000))
                {
                    GameObject dropOverObject = hit.collider.gameObject;
                    int dropStatus = itemDragged.Item.CheckDroppingOver(dropOverObject);
                    if(dropStatus == InventoryItem.ABORT_DROP_OVER) return;

                    if(dropStatus == InventoryItem.DROP_ON_GROUND)
                    {
                        itemDragged.Item.transform.position = hit.point;
                        GetComponent<Inventory>().removeItem(itemDragged.Item);
                    }

                    if(dropStatus == InventoryItem.CAN_DROP_OVER)
                    {
                        dropStatus = itemDragged.Item.DropOver(dropOverObject);
                        if (dropStatus == InventoryItem.ABORT_DROP_OVER) return;
                        if (dropStatus == InventoryItem.DROP_ON_GROUND)
                        {
                            itemDragged.Item.transform.position = hit.point;
                        }
                        GetComponent<Inventory>().removeItem(itemDragged.Item);
                    }
                }
            }
        }

    }

}
