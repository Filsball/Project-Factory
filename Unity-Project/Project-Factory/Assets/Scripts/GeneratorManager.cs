using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public Door door;
    public bool running = true;
    public Button[] buttonOrder;
    public Zahnrad_Manager zrManager;
    public float motorMaxSpeed = 100;
    public float motorSpeed = 0;

    public GameObject turbine;
    public GameObject rightDoor;
    public GameObject leftDoor;
    [Range(90, 180)]
    public int doorOpenAngle = 100;
    [Range(0.5f, 5)]
    public float doorOpenSpeed = 2;

    private AudioManager audio;
    private bool doorsHaveOpened = false;
    [SerializeField]
    private bool doorsOpening = false;

    List<Button> buttonsPressed = new List<Button>();

    private bool buttonsSolved = false;


    public bool SOLVED_ONLY_FOR_DEBUGGING = false;
    private bool generatorStarted = false;

     [SerializeField]
    Material LightBulbGlassMaterial;
    [SerializeField]
    Material LightBulbWireMaterial;



    // Start is called before the first frame update
    void Start()
    {
        zrManager = GetComponentInChildren<Zahnrad_Manager>();
        audio = FindObjectOfType<AudioManager>();
        LightBulbWireMaterial.DisableKeyword("_EMISSION");
        LightBulbGlassMaterial.DisableKeyword("_EMISSION");
    }

    void OpenDoors()
    {
        doorsHaveOpened = true;

        if ((int)(rightDoor.transform.localRotation.eulerAngles.y) != 360 - doorOpenAngle)
        {
            rightDoor.transform.Rotate(new Vector3(0, 0, -doorOpenSpeed));
            doorsHaveOpened = false;
        }
        if ((int)(leftDoor.transform.localRotation.eulerAngles.y) < doorOpenAngle)
        {
            leftDoor.transform.Rotate(new Vector3(0, 0, doorOpenSpeed));
            doorsHaveOpened = false;
        }
        

        doorsOpening = !doorsHaveOpened;
    }

    private bool CheckButtons()
    {
        for(int i=0; i<buttonOrder.Length; i++)
        {
            if (!buttonOrder[i].Equals(buttonsPressed[i]))
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckDreiButtons()
    {

        Debug.Log(buttonOrder.Length);
        Debug.Log(buttonsPressed.Count);
        if (buttonsPressed.Count < buttonOrder.Length - 1)
        {

            return false;
        }
        for (int i = 0; i < buttonOrder.Length-1; i++)
        {

            if (!buttonOrder[i].Equals(buttonsPressed[i]))
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!buttonsSolved)
        {
            foreach (Button b in buttonOrder)
            {
                if (b.pressed)
                {
                    if (buttonsPressed.Contains(b)) continue;
                    b.currentlyInteractable = false;
                    buttonsPressed.Add(b);
                }
            }

            if (buttonsPressed.Count == buttonOrder.Length)
            {
                buttonsSolved = CheckButtons();
                if (buttonsSolved)
                {
                    doorsOpening = true;
                    audio.Play("ButtonMitEinrasten", 0.8f, buttonOrder[0].transform.position, true);
                    audio.Play("MetalltuerGenerator", 0.7f, rightDoor.transform.position, true);
                }
                else
                {
                    foreach(Button b in buttonsPressed)
                    {
                        b.currentlyInteractable = true;
                        b.Interact();
                    }
                    buttonsPressed.Clear();
                }
            }
        }

        if (!doorsHaveOpened && doorsOpening)
        {
            OpenDoors();
        }

        zrManager.running = running;
        door.currentlyInteractable = zrManager.solved || SOLVED_ONLY_FOR_DEBUGGING;
        if ((zrManager.solved || SOLVED_ONLY_FOR_DEBUGGING ) && !generatorStarted)
        {
            // nur einmal starten, nicht in jedem Update-Call
            generatorStarted = true;
            audio.Play("GeneratorStartend", 1.0f, transform.position, false);
            audio.generatorStarted = true;
            LightBulbGlassMaterial.EnableKeyword("_EMISSION");
            LightBulbWireMaterial.EnableKeyword("_EMISSION");
            SockelAbstract.StromAn();
        }
        if (generatorStarted)
        {
            // Turbine drehen
            turbine.transform.Rotate(new Vector3(0, 0, motorSpeed * Time.deltaTime));
            if (motorSpeed < motorMaxSpeed)
            {
                motorSpeed += 0.25f;
                motorSpeed *= 1.01f;
            }
        }
    }

}
