﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public bool running = true;
    public Button[] buttonOrder;
    public Zahnrad_Manager zrManager;
    public float motorMaxSpeed = 100;
    public float motorSpeed = 0;

    public GameObject rightDoor;
    public GameObject leftDoor;
    public GameObject turbine;
    [Range(90, 180)]
    public int doorOpenAngle = 100;
    [Range(0.5f, 5)]
    public float doorOpenSpeed = 2;

    private bool doorsHaveOpened = false;
    [SerializeField]
    private bool doorsOpening = false;
   
    List<Button> buttonsPressed = new List<Button>();
    private bool buttonsSolved = false;
    
    // Start is called before the first frame update
    void Start()
    {
        zrManager = GetComponentInChildren<Zahnrad_Manager>();
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

    // Update is called once per frame
    void Update()
    {
        if (!buttonsSolved)
        {
            bool canCheckButtons = buttonsPressed.Capacity > 0;
            foreach (Button b in buttonOrder)
            {
                if (b.pressed)
                {
                    if (buttonsPressed.Contains(b)) continue;
                    b.currentlyInteractable = false;
                    buttonsPressed.Add(b);
                    canCheckButtons = false;
                } else
                {
                    canCheckButtons = false;
                }
            }

            if (canCheckButtons)
            {
                buttonsSolved = CheckButtons();
                if (buttonsSolved)
                {
                    doorsOpening = true;
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
        if (zrManager.solved)
        {
            turbine.transform.Rotate(new Vector3(0, 0, motorSpeed * Time.deltaTime));
            if(motorSpeed < motorMaxSpeed)
            {
                motorSpeed+=0.25f;
                motorSpeed *= 1.01f;
            }
        }
    }
}
