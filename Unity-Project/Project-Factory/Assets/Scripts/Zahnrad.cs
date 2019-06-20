using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider))]
public class Zahnrad : InventoryItem
{
    public BoxCollider colOuter;
    public BoxCollider colInner;
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

    override public int CheckDroppingOver(GameObject dropOverObject)
    {
        base.CheckDroppingOver(dropOverObject);
        if (dropOverObject.GetComponent<ZahnradAufsatz>() != null)
        {
            
            return CAN_DROP_OVER;
        }
        return ABORT_DROP_OVER;
    }

    public override int DropOver(GameObject dropOverObject)
    {
        base.DropOver(dropOverObject);

        ZahnradAufsatz za = dropOverObject.GetComponent<ZahnradAufsatz>();
        if (za == null) return ABORT_DROP_OVER;
        if (za.zahnrad != null) return ABORT_DROP_OVER;
        za.SetZahnrad(this);
        return CAN_DROP_OVER;
    }
}
