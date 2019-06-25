using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerControl : MonoBehaviour
{
    [Range(1,10)]
    public float maxInteractingDistance = 2.5f;
    public HUD hud;
    public Inventory inventory;


    private CharacterController characterController;
    private Transform head;
    private InteractableObject lookedAtObject;
    private InteractableObject cameraFocusedOn; // e.g. Riddle
    private InventoryItem itemPickUp = null;
    private Camera headCamera;
    private Camera activeCamera;
    FirstPersonController fpc;
    private bool isInRiddle = false;

    private static bool LightOn;
    private static int Oil { get; set; }
    [SerializeField] Light Oillamp;

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
        Oil = 100;
        LightOn = false;
        Oillamp.enabled = false;
    }

    public void ResetLookedAtObject()
    {
        if (lookedAtObject != null)
        {
            lookedAtObject.Selected = false;
        }
        lookedAtObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        ResetLookedAtObject();
        if (!isInRiddle)
        {
            DeterminLookedAtObject();

            if (lookedAtObject != null && lookedAtObject.currentlyInteractable)
            {
                lookedAtObject.Selected = true;
                hud.OpenMsgPanel(lookedAtObject.ToolTip);
            }
            else
            {
                hud.CloseMsgPanel();
            }
        }

        HandleInput();
    }

    public void SwapBackToPlayer()
    {
        cameraFocusedOn.col.enabled = true;
        Collider parentCol = cameraFocusedOn.gameObject.transform.parent.GetComponent<Collider>();
        if (parentCol != null)
        {
            parentCol.enabled = true;
        }
        cameraFocusedOn = null;
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

    public void SwapToCamera(Camera c, InteractableObject swapTo)
    {
        cameraFocusedOn = swapTo;
        cameraFocusedOn.col.enabled = false;
        Collider parentCol = cameraFocusedOn.gameObject.transform.parent.GetComponent<Collider>();
        if (parentCol != null)
        {
            parentCol.enabled = false;
        }
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

        Vector3 lookingDirection = head.TransformDirection(Vector3.forward);
        Ray interactionRay = new Ray(head.position + (lookingDirection * 0.25f), lookingDirection);
        RaycastHit hit;

        if (Physics.Raycast(interactionRay, out hit, maxInteractingDistance))
        {
            Debug.DrawRay(interactionRay.origin, interactionRay.direction * hit.distance, Color.red);
            lookedAtObject = hit.collider.transform.GetComponent<InteractableObject>();
        }
        else
        {
            lookedAtObject = null;
            Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * maxInteractingDistance, Color.red);
        }
    }

    private void HandleInput()
    {
        if (!isInRiddle)
        {
            if (Input.GetKeyDown(KeyCode.I))
                inventarVerwalten();

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (LightOn)
                    SwitchOillampOf();
                else
                    SwitchOillampOn();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (lookedAtObject != null)
                {
                    if (lookedAtObject is InventoryItem)
                    {
                        itemPickUp = (InventoryItem)lookedAtObject;
                        inventory.addItem(itemPickUp);
                        itemPickUp = null;
                        lookedAtObject = null;
                        hud.CloseMsgPanel();
                    }
                    else
                    {
                        lookedAtObject.Interact();
                    }
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

    IEnumerator LooseOil()
    {
        Debug.Log("LooseOil Gestartet");
        while (LightOn && Oil > 0)
        {
            yield return new WaitForSeconds(1);
            --Oil;
            Debug.Log("Oelstand:  " + Oil);
        }
        if (Oil <= 0)
        {
            Debug.Log("Lampe Leer");
            SwitchOillampOf();
        }
    }

    private void SwitchOillampOn() {
        if (Oil > 5)
            Oil -= 5;
        LightOn = true;
        Oillamp.enabled = true;
        Debug.Log("Oellampe aktiviert");
        StartCoroutine(LooseOil());
        Time.timeScale = 1;
    }

    private void SwitchOillampOf() {
        Debug.Log("Oellampe Deaktiviert");
        LightOn = false;
        Oillamp.enabled = false;
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
