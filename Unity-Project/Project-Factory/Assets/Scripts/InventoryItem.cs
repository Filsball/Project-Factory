using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : InteractableObject, Inventoriable
{
    public static int CAN_DROP_OVER = 0;
    public static int ABORT_DROP_OVER = 1;
    public static int DROP_ON_GROUND = 2;

    public Sprite _Image= null;
    public Sprite Image { get { return _Image; } }

    public abstract void OnDrop();
    public abstract void OnPickUp();

    new public void Start()
    {
        base.Start();
        _name = "Basic inventory item";
        _interactionName = "aufheben";
    }

    virtual public int CheckDroppingOver(GameObject dropOverObject) // virtual means, can be overritten by childs with "override"
    {
        //Debug.Log("Trying to drop :" + name + " on " + dropOverObject);
        return DROP_ON_GROUND;
    }

    virtual public int DropOver(GameObject dropOverObject)
    {
        return DROP_ON_GROUND;
    }

    override public void Interact()
    {
        // Interacting with Inventory Object is realized in OnPickUp/OnDrop
    }
}
