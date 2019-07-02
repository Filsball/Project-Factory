using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLaterne : InteractableObject
{
    public GameObject fpc_Lantern;
    public override void Interact()
    {
        gameObject.SetActive(false);
        if(fpc_Lantern != null)
        {
            fpc_Lantern.SetActive(true);
        }
    }

}
