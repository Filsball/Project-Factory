using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Birne : InventoryItem
{
    public override void OnDrop()
    {

    }

    public override void OnPickUp()
    {
        gameObject.SetActive(false);
    }
}
