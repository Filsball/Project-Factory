using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpOil : InteractableObject
{
    public int Fuellstand;
    public override void Interact()
    {   
            gameObject.SetActive(false);
    }
    public void Restfuellstand(int rest) {
        Fuellstand = rest;
        Debug.Log("Oilpot fuellstand: "+Fuellstand);
    }
    public int getFuellstand()
    {
        Debug.Log("get Oilpot fuellstand: " + Fuellstand);
        return Fuellstand;
    }


}
