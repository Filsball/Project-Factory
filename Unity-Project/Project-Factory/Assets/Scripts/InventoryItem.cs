using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : InteractableObject, Inventoriable
{
    public Sprite _Image = null;
    public Sprite Image { get { return _Image; } }

    public void Start()
    {
        Init();
        _name = "Basic inventory item";
        _toolTip = Name + ": Drücke F zum aufheben";
    }
    public override void Interact()
    {
        // nothing to do here
    }
    public void OnPickUp() {
        gameObject.SetActive(false);
        //modify here
    }

    public void OnDrop()
    {
        gameObject.SetActive(true);
    }
}
