using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerControl : MonoBehaviour
{
    private CharacterController characterController;

    //   public float speed = 5.0f;

    //    public float rotationspeed = 240.0f;

    //   private float gravity = 20.0f;

    //    private Vector3 moveDir = Vector3.zero;

    public HUD hud;

    public Inventory inventory;

    private InventoryItem itemPickUp = null;

   // private bool mLockPickUp;

    private bool InvOpen;

   // private MouseLook mouseLock;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemPickUp != null && Input.GetKeyDown(KeyCode.F))
        {
            inventory.addItem(itemPickUp);
            itemPickUp.OnPickUp();
            itemPickUp = null;
            hud.CloseMsgPanel();
        }

        if (Input.GetKeyDown(KeyCode.I))
            inventarVerwalten();

        if (Input.GetKeyDown(KeyCode.G))
            dropItem();
    }

    private void dropItem()
    {
        inventory.RemoveItem(0);
    }

    private void inventarVerwalten()
    {

        if (InvOpen) {
            hud.CloseInventory();
            //GetComponent<FirstPersonController>().enabled = true;
        }
        else
        {
            hud.OpenInventory();
            //GetComponent<FirstPersonController>().enabled = false;
            //mouseLock.SetCursorLock(false);
        }
        InvOpen = !InvOpen;
        
        
    }

    private void OnTriggerEnter(Collider collider)
    {
         InventoryItem inventoryItem = collider.GetComponent<InventoryItem>();
         if (inventoryItem != null)
         {
         //    if (mLockPickUp)
         //        return;

             itemPickUp = inventoryItem;
             hud.OpenMsgPanel("");
         }
       

    }
    private void OnTriggerExit(Collider collider)
    {
        InventoryItem inventoryItem = collider.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            hud.CloseMsgPanel();
            itemPickUp = null;
        }
    }
}
