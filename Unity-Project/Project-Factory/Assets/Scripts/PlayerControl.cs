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
    [SerializeField] Light FakeLight;
    [SerializeField] Material lampGlassMaterial;
    private bool InExpZone;
    private ExplosiveArea ExpArea;

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
        FakeLight.enabled = false;
        RefactorFakeLight();
        lampGlassMaterial.DisableKeyword("_EMISSION");
        
    }

    private void RefactorFakeLight()
    {
        FakeLight.transform.position = transform.position;
        Vector3 vTemp = FakeLight.transform.position;
        vTemp.y += 2;
        FakeLight.transform.position = vTemp;
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
        if (LightOn)
        {
            //GeneratorManager.DisableFakeLight();
            //Notiz.DisableFakeLight();
            FakeLight.enabled = false;
        }
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
        if (LightOn)
        {
            //GeneratorManager.EnableFakeLight();
            //Notiz.EnableFakeLight();
            RefactorFakeLight();
            FakeLight.enabled = true;
        }
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (LightOn)
            {
                SwitchOillampOf();
                if (isInRiddle)
                {
                    //GeneratorManager.DisableFakeLight();
                    //Notiz.DisableFakeLight();
                    FakeLight.enabled = false;
                }
            }
            else
            {
                SwitchOillampOn();
                if (isInRiddle && Oil > 0)
                {
                    //GeneratorManager.EnableFakeLight();
                    //Notiz.EnableFakeLight();
                    FakeLight.enabled = true;
                }
            }
        }
        if (!isInRiddle)
        {
            if (Input.GetKeyDown(KeyCode.E))
                inventarVerwalten();

            if (Input.GetKeyDown(KeyCode.Mouse0))
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
                    else if (lookedAtObject is PickUpOil) {
                        PickUpOil oilpot = (PickUpOil)lookedAtObject;
                        if (oilpot.getFuellstand() + Oil > 100)
                        {
                            oilpot.Restfuellstand(oilpot.getFuellstand()+Oil-100);
                            Oil = 100;
                        }
                        else
                        {
                            Oil += oilpot.getFuellstand();
                            oilpot.Restfuellstand(0);
                            lookedAtObject.Interact();
                        }
                        
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
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
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
            if (InExpZone) {
                ExpArea.Explode();
                audio.Play("Explosion", 0.8f);
                //Position konnte bisher noch nicht gesetzt werden
                //audio.Play("Explosion", 0.8f, ExpArea.getExplosiveLights().transform.position);
                AudioManager.GameOverCallerMitPP(2f, audio.getPPB());
                // @Dennis Direkten Tod + Audio Explosion einfügen
            }
            yield return new WaitForSeconds(1);
            --Oil;
            //Debug.Log("Oelstand:  " + Oil);
        }
        if (Oil <= 0)
        {
            //Debug.Log("Lampe Leer");
            SwitchOillampOf();
            //GeneratorManager.DisableFakeLight();
            //Notiz.DisableFakeLight();
            FakeLight.enabled = false; ;
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
            Debug.Log("InLichtzone");
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
        if (collider.tag == "Faesser")
        {
            InExpZone = true;
            ExpArea = collider.GetComponent<ExplosiveArea>();
            Debug.Log("ExpArea");
             if(LightOn)
             {
                ExpArea.Explode();
                // @Dennis Direkten Tod Einleiten, Audio Explosion
            }
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
        if (collider.tag == "Faesser") {
            InExpZone = false;
            ExpArea = null;
        }

    }

    public bool GetIsInRiddle()
    {
        return isInRiddle;
    }
}
