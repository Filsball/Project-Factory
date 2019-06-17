using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableObject
{
    public override void Interact()
    {
        Interact();
    }

    // Start is called before the first frame update
    public void Start()
    {
        Init();
        _name = "Knopf";
    }

    // Update is called once per frame
    public void Update()
    {
        //base.Update();
    }
}
