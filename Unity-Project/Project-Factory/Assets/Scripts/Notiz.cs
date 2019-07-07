using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Notiz : InteractableObject
{
    public new void Start()
    {
        base.Start();
        _name = "Zettel";
        _interactionName = "lesen";
    }
    public void ChangeLight(Light light)
    {
        bool currentlyActive = OilLightFaker.enabled;
        DisableFakeLight();

        OilLightFaker = light;
        if (currentlyActive)
        {
            EnableFakeLight();
        }
    }

    public override void Interact()
    {
        RiddleInteract();
    }
    
}
