using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLaterne : InteractableObject
{
    public GameObject fpc_Lantern;
    private AudioManager audio;
    public override void Interact()
    {
        audio = FindObjectOfType<AudioManager>();
        audio.Play("ItemPickup", 0.15f, fpc_Lantern.transform.position);
        gameObject.SetActive(false);
        if(fpc_Lantern != null)
        {
            
            fpc_Lantern.SetActive(true);
        }
    }

}
