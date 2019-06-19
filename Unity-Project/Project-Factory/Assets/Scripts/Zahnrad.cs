using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider))]
public class Zahnrad : InventoryItem
{
    new public void Start()
    {
        base.Start();
        _name = "Zahnrad";
        
    }
    public override void OnDrop()
    {
        gameObject.SetActive(true);
    }

    public override void OnPickUp()
    {
        gameObject.SetActive(false);
    }
}
