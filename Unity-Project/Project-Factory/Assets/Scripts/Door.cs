using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public bool animating = false;
    public bool open = false;
    public float openingAngle = 50f;
    public float closingAngle = 50f;
    public float openingSpeed = 100f;
    public float curRot = 0f;

    public override void Interact()
    {
        if (!currentlyInteractable)
        {
            return;
        }
        open = !open;
        animating = true;
    }

    public new void Start()
    {
        glowPower = 0.2f;
        Name = "Tür";
        _interactionName = "öffnen";
        base.Start();

        if (openingAngle < 0)
        {
            openingAngle = 360 + openingAngle;
        }

        if (closingAngle < 0)
        {
            closingAngle = 360 + closingAngle;
        }
    }

    public new void Update()
    {
        base.Update();
        curRot = transform.localRotation.eulerAngles.y;
        if (animating)
        {
            if (open)
            {
                if (transform.localRotation.eulerAngles.y < openingAngle)
                {
                    transform.Rotate(new Vector3(0, 0, openingSpeed) * Time.deltaTime);
                }
            }
            else
            {
                if (transform.localRotation.eulerAngles.y > closingAngle)
                {
                    float before = transform.localRotation.eulerAngles.y;
                    transform.Rotate(new Vector3(0, 0, -openingSpeed) * Time.deltaTime);
                    if (transform.localRotation.eulerAngles.y > before)
                    {
                        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, closingAngle , transform.eulerAngles.z);
                    }
                }
            }
        }
    }
}
