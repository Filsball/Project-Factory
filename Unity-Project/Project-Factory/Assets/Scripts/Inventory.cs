using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {

    private int Slots = 4;

    private List<InventoryItem> ItemList = new List<InventoryItem>();

    public EventHandler<InventoryEventArgs> ItemAdded;

    public EventHandler<InventoryEventArgs> ItemDeleted;

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
    
}

