using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableObject
{
    public bool pressed = false;

    private Vector3 translateTo;
    [Range(0,10)]
    public float translationSpeed;
    private bool translating;

    public override void Interact()
    {
        if (!currentlyInteractable) return;
        if (!pressed){
            translateTo = transform.position + new Vector3(-col.bounds.size.x / 2, 0, 0);
        }
        else
        {
            translateTo = transform.position + new Vector3(col.bounds.size.x / 2, 0, 0);
        }
        pressed = !pressed;
        translating = true;
    }

    new public void Update()
    {
        base.Update();
        if (translating)
        {
            float distance1 = Vector3.Distance(transform.position, translateTo);

            transform.Translate((translateTo - transform.position).normalized * translationSpeed/10 * Time.deltaTime);

            float distance2 = Vector3.Distance(transform.position, translateTo);
        

            if (distance1 <= distance2)
            {
                transform.position = translateTo;
                translating = false;
            }
            
        }


    }

    // Start is called before the first frame update
    new public void Start()
    {
        Name = "Knopf";
        base.Start();
        //translationSpeed = 0.5f;
        translating = false;
    }
    
}
