using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableObject
{
    public override void Interact()
    {
    }



    // Start is called before the first frame update
    public void Start()
    {
        Name = "Knopf";
        Init();
    }
    
}
