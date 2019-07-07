using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpOil : InteractableObject
{
    public int Fuellstand;

    public new void Start()
    {
        _name = "Öl-Ampulle";
        _interactionName = "Öllampe auffüllen";
    }
    public override void Interact()
    {   
       gameObject.SetActive(false);
    }
    public void Restfuellstand(int rest) {
        Fuellstand = rest;
    }
    public int getFuellstand()
    {
        return Fuellstand;
    }


}
