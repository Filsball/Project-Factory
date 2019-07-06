using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Notiz : InteractableObject
{
    //public static Light OilLightFaker;
    //public Camera riddleCam;
    public BoxCollider boxCollider;


    public override void Interact()
    {
        RiddleInteract();
        //GameObject.Find("FPSController").GetComponent<PlayerControl>().SwapToCamera(riddleCam, this);
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        boxCollider = (BoxCollider)col;
        //OilLightFaker = GetComponentInChildren<Light>();
        //OilLightFaker.enabled = false;
    }
    
    //public static void EnableFakeLight(){ OilLightFaker.enabled = true;}

    //public static void DisableFakeLight(){ OilLightFaker.enabled = false;}
}
