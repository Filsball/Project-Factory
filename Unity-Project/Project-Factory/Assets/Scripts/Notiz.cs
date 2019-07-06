using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Notiz : InteractableObject
{
    public override void Interact()
    {
        RiddleInteract();
    }
}
