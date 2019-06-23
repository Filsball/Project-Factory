﻿using System.Collections;
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
                collider.enabled = false;
                ItemList.Add(item);
                item.OnPickUp();
                item.Selected = false;
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
        ItemRemoved?.Invoke(this, new InventoryEventArgs(item));
    }
    
}

