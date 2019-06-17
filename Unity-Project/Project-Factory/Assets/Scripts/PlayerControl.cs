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
    private Camera headCamera;
    private Camera activeCamera;
    FirstPersonController fpc;
    private bool isInRiddle = false;


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
        headCamera = GetComponentInChildren<Camera>(); 
        fpc = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInRiddle)
        {
            if (lookedAtObject != null)
            {
                lookedAtObject.Selected = false;
            }

            DeterminLookedAtObject();

            if (lookedAtObject != null)
            {
                lookedAtObject.Selected = true;
                hud.OpenMsgPanel(lookedAtObject.ToolTip);
            }
            else
            {
                hud.CloseMsgPanel();
            }
        }
        else
        {

        }

        HandleInput();
    }

    public void SwapBackToPlayer()
    {
        isInRiddle = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        headCamera.gameObject.SetActive(true);
        fpc.enabled = true;
        if (activeCamera != null)
        {
            activeCamera.gameObject.SetActive(false);
        }

        hud.CloseInventory();
        hud.OpenCrossHairPanel();
    }

    public void SwapToCamera(Camera c)
    {
        isInRiddle = true;
        headCamera.gameObject.SetActive(false);
        fpc.enabled = false;
        if (c != null)
        {
            activeCamera = c;
            c.gameObject.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hud.OpenInventory();
        hud.CloseMsgPanel();
        hud.CloseCrossHairPanel();
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
        if (!isInRiddle)
        {
            if (Input.GetKeyDown(KeyCode.I))
                inventarVerwalten();

            if (Input.GetKeyDown(KeyCode.G))
                dropItem();

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (itemPickUp != null)
                {
                    inventory.addItem(itemPickUp);
                    itemPickUp.OnPickUp();
                    itemPickUp = null;
                    hud.CloseMsgPanel();
                }

                if (lookedAtObject != null)
                {
                    lookedAtObject.Interact();
                }
            }
        }
        else
        {
            // Escape Riddle
            if (Input.GetKeyDown(KeyCode.F))
            {
                SwapBackToPlayer();
                hud.CloseInventory();
            }

        }
    }

    private void dropItem()
    {
        //inventory.RemoveItem(0);
    }

    private void inventarVerwalten()
    {

        InvOpen = !InvOpen;
        if (InvOpen)
        {
            hud.OpenInventory();
            //GetComponent<FirstPersonController>().enabled = false;
            //mouseLock.SetCursorLock(false);
        }
        else
        {
            hud.CloseInventory();
            //GetComponent<FirstPersonController>().enabled = true;
        }
        
        
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
