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
    private AudioManager audio;
    [SerializeField] GameObject Oillamp;
    Light oilLight;
    [SerializeField] Material lampGlassMaterial;

    // private bool mLockPickUp;

    private bool InvOpen;
    private Color enteredColor;

   // private MouseLook mouseLock;


    // Start is called before the first frame update
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        characterController = GetComponent<CharacterController>();
        head = transform.GetComponentInChildren<Camera>().transform;
        headCamera = GetComponentInChildren<Camera>(); 
        fpc = GetComponent<FirstPersonController>();
        Oil = 100;
        LightOn = false;
        oilLight = Oillamp.GetComponentInChildren<Light>();
        oilLight.enabled = false;
        lampGlassMaterial.DisableKeyword("_EMISSION");
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


        hud.UpdateOil(Oil/100f);
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
        if(LightOn)
            GeneratorManager.DisableFakeLight();
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
        if(LightOn)
            GeneratorManager.EnableFakeLight();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (c != null)
        {
            activeCamera = c;
            c.gameObject.SetActive(true);
        }

        hud.OpenInventory();
        if (Oillamp.activeSelf)
        {
            hud.OpenOilTankPanel();
        }
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
            if (Oillamp.activeSelf)
            {
                hud.OpenOilTankPanel();
            }
            audio.Play("InventarOeffnen", 1f);
            //GetComponent<FirstPersonController>().enabled = false;
            //mouseLock.SetCursorLock(false);
        }
        else
        {
            hud.CloseInventory();
            audio.Play("InventarSchließen", 1f);
            if (Oillamp.activeSelf  && !LightOn)
            {
                hud.CloseOilTankPanel();
            }
            //GetComponent<FirstPersonController>().enabled = true;
        }
        
        
    }

    IEnumerator LooseOil()
    {
        //Debug.Log("LooseOil Gestartet");
        while (LightOn && Oil > 0)
        {
            yield return new WaitForSeconds(1);
            --Oil;
            //Debug.Log("Oelstand:  " + Oil);
        }
        if (Oil <= 0)
        {
            //Debug.Log("Lampe Leer");
            SwitchOillampOf();
            GeneratorManager.DisableFakeLight();
        }
    }

    private void SwitchOillampOn() {
        if (!Oillamp.activeSelf) return;
        hud.OpenOilTankPanel();
        if (Oil > 5)
            Oil -= 5;
        
        if(Oil > 0)
        {
            LightOn = true;
            oilLight.enabled = true;
            //Debug.Log("Oellampe aktiviert");
            StartCoroutine(LooseOil());
            lampGlassMaterial.EnableKeyword("_EMISSION");
        }
        
        Time.timeScale = 1;


        // fuer Audio
        audio.Play("LampeAnschalten", 0.7f);
        if (Oil > 0)
        {
            audio.Play("LampeIstAn", 0.1f);
        }
        if (audio.getDunkelheit() && Oil > 0)
        {
            audio.HintergrundAktivierenMitLampe();
        }
    }

    private void SwitchOillampOf()
    {
        if (!Oillamp.activeSelf) return;
        if (!InvOpen)
        {
            hud.CloseOilTankPanel();
        }
        lampGlassMaterial.DisableKeyword("_EMISSION");
        Debug.Log("Oellampe Deaktiviert");
        LightOn = false;
        oilLight.enabled = false;
        audio.Stop("LampeIstAn");
        // fuer Audio
        if (audio.getDunkelheit())
        {
            audio.DunkelheitAktivierenMitLampe();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // in Lichtzone, Kein Schaden
        if (collider.tag == "Licht")
        {
            if (audio.getSaferoom())
            {
                audio.setHintergrund(true);
            }
            else
            {
                audio.HintergrundAktivieren();
            }
            
        }
        // in Saferoom
        if (collider.tag == "Saferoom")
        {
            audio.SaferoomAktivieren();
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        // Leite Tod ein wenn Oellampe nicht aktiv
        if(collider.tag == "Licht")
        {
            if(!LightOn && !audio.getSaferoom())
            {
                audio.DunkelheitAktivieren();
            }
        }
        // Saferoom verlassen
        if (collider.tag == "Saferoom")
        {
            if (audio.getHintergrund())
            {
                audio.HintergrundAktivieren();
            }
            else if(LightOn)
            {
                audio.HintergrundAktivierenMitLampe();
                audio.setDunkelheit(true);
                audio.setSaferoom(false);
            }
            else if(!audio.getHintergrund())
            {
                audio.DunkelheitAktivieren();
            }
            Debug.Log("Außerhalb von Saferoom");
        }

    }
}
