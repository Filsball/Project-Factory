using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerControl : MonoBehaviour
{
    private CharacterController characterController;
    private Transform head;
    private Interactable lookedAtObject;
    private InventoryItem itemPickUp = null;

    //   public float speed = 5.0f;

    //    public float rotationspeed = 240.0f;

    //   private float gravity = 20.0f;

    //    private Vector3 moveDir = Vector3.zero;

    public HUD hud;

    public Inventory inventory;


   // private bool mLockPickUp;

    private bool InvOpen;
    private Color enteredColor;

   // private MouseLook mouseLock;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        head = transform.GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookedAtObject != null)
        {
            lookedAtObject.Selected = false;
        }

        DeterminLookedAtObject();

        if(lookedAtObject != null)
        {
            lookedAtObject.Selected = true;
            hud.OpenMsgPanel(lookedAtObject.ToolTip);
        }
        else
        {
            hud.CloseMsgPanel();
        }


        HandleInput();
    }

    private void DeterminLookedAtObject()
    {

        Vector3 headLook = head.TransformDirection(Vector3.forward);
        Ray interactionRay = new Ray(head.position + (headLook * 0.25f), headLook);
        RaycastHit hit;

        if (Physics.Raycast(interactionRay, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(interactionRay.origin, interactionRay.direction * hit.distance, Color.red);
            lookedAtObject = hit.collider.transform.GetComponent<Interactable>();
        }
        else
        {
            Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * 100000, Color.red);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventarVerwalten();

        if (Input.GetKeyDown(KeyCode.G))
            dropItem();

        if (itemPickUp != null && Input.GetKeyDown(KeyCode.F))
        {
            inventory.addItem(itemPickUp);
            itemPickUp.OnPickUp();
            itemPickUp = null;
            hud.CloseMsgPanel();
        }
    }

    private void dropItem()
    {
        //inventory.RemoveItem(0);
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
         //InventoryItem inventoryItem = collider.GetComponent<InventoryItem>();
         //if (inventoryItem != null)
         //{
         //   //    if (mLockPickUp)
         //   //        return;

         //   inventoryItem.Selected = true;
         //   itemPickUp = inventoryItem;
         //    hud.OpenMsgPanel("");
         //}
       

    }
    private void OnTriggerExit(Collider collider)
    {
        //InventoryItem inventoryItem = collider.GetComponent<InventoryItem>();
        //if (inventoryItem != null)
        //{
        //    inventoryItem.Selected = false;
        //    hud.CloseMsgPanel();
        //    itemPickUp = null;
        //}
    }
}
