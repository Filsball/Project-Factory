using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluehbirne : InventoryItem
{
    public override void OnDrop()
    {
        
    }

    public override void OnPickUp()
    {
        gameObject.SetActive(false);
    }
}
