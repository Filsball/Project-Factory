using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {

    private int Slots = 4;

    private List<InventoryItem> ItemList = new List<InventoryItem>();

    public EventHandler<InventoryEventArgs> ItemAdded;

    public EventHandler<InventoryEventArgs> ItemRemoved;

    private AudioManager audio;
    
        
        


    public void addItem(InventoryItem item)
    {
        audio = FindObjectOfType<AudioManager>();
        if (ItemList.Count < Slots)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                ItemList.Add(item);
                item.OnPickUp();
                audio.Play("ItemPickup", 0.15f, item.transform.position);
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
            Debug.Log("ItemRemoved");
        }
        Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;
        ItemRemoved?.Invoke(this, new InventoryEventArgs(item));
    }
    public InventoryItem ContainsGluehbirne() {   // Es wird die erste Glühbirne im Inventar ausgegeben
        for (int i = 0; i < ItemList.Count; ++i)
        {
            if (ItemList[i] is Gluehbirne)
                return ItemList[i];
        }
        return null;
    }
    public InventoryItem ContainsElementOfType(Type type) {
        Debug.Log("Objekttype  "+type.GetType());
        for (int i = 0; i < ItemList.Count; ++i)
        {
            if (ItemList[i].GetType() == type)
                return ItemList[i];
        }
        return null;
    }

}

