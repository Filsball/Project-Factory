using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public bool animating = false;
    public bool open = false;
    public float openingAnle = 50f;
    public float openingSpeed = 100f;
    public float curRot = 0f;

    public override void Interact()
    {
        open = !open;
        animating = true;
    }

    public new void Start()
    {
        Name = "Tür";
        _interactionName = "öffnen";
        base.Start();
    }

    public new void Update()
    {
        base.Update();
        curRot = transform.localRotation.eulerAngles.y;
        if (animating)
        {
            if (open)
            {
                if (transform.localRotation.eulerAngles.y < 180 + openingAnle)
                {
                    transform.Rotate(new Vector3(0, 0, openingSpeed) * Time.deltaTime);
                }
            }
            else
            {
                if (transform.localRotation.eulerAngles.y > 180)
                {
                    transform.Rotate(new Vector3(0, 0, -openingSpeed) * Time.deltaTime);
                }
            }
        }
    }
}
