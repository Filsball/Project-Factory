using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {

    private int Slots = 4;

    private List<InventoryItem> ItemList = new List<InventoryItem>();

    public EventHandler<InventoryEventArgs> ItemAdded;

    public EventHandler<InventoryEventArgs> ItemRemoved;

    public void addItem(InventoryItem item)
    {
        if (ItemList.Count < Slots)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                GetComponent<Collider>().enabled = false;
                ItemList.Add(item);
                item.OnPickUp();
            }
            if (ItemAdded != null)
            {
                ItemAdded(this, new InventoryEventArgs(item));
            }
        }
    }
    public void removeItem(InventoryItem item) {
        if (ItemList.Contains(item)) {
            ItemList.Remove(item);
            item.OnDrop();
        }
        Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;
        if (ItemRemoved != null)
            ItemRemoved(this, new InventoryEventArgs(item));
    }
    
}

