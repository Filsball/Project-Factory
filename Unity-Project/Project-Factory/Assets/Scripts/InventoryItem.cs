using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : InteractableObject, Inventoriable
{
    public Sprite _Image = null;
    public Sprite Image { get { return _Image; } }

    public abstract void OnDrop();
    public abstract void OnPickUp();

    new public void Start()
    {
        base.Start();
        _name = "Basic inventory item";
        _toolTip = Name + ": Drücke F zum aufheben";
    }

    override public void Interact()
    {
        // Interacting with Inventory Object is realized in OnPickUp/OnDrop
    }
}
