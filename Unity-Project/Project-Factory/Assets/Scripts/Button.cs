using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableObject
{
    public bool pressed = false;

    private Vector3 translateTo;
    public BoxCollider boxCollider;
    [Range(0,10)]
    public float translationSpeed;
    private bool translating;
    private AudioManager audio;

    public override void Interact()
    {
        if (!currentlyInteractable) return;
        if (!pressed){
            translateTo = transform.position - col.bounds.size.x / 4 * transform.right;
            GeneratorManager gm = gameObject.GetComponentInParent<GeneratorManager>();
            if (gm == null || !gm.CheckDreiButtons())
            {
                audio.Play("Button", 0.7f, gameObject.transform.position, true);
            }
            
        }
        else
        {
            translateTo = transform.position + col.bounds.size.x / 4 * transform.right;
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
        boxCollider = GetComponent<BoxCollider>();
        audio = FindObjectOfType<AudioManager>();
        audio.setPosition("Button", position: boxCollider.center + transform.position);
        Name = "Knopf";
        base.Start();
        //translationSpeed = 0.5f;
        translating = false;
    }
    
}
