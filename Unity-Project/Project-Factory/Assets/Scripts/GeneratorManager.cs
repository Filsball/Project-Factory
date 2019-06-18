using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public bool running = true;
    public Button[] buttonOrder;
    public Zahnrad_Manager zrManager;

    public GameObject rightDoor;
    public GameObject leftDoor;
    [Range(90, 180)]
    public int doorOpenAngle = 100;

    private bool doorsHaveOpened = false;
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

        if ((int)(rightDoor.transform.rotation.eulerAngles.y) != 360 - doorOpenAngle)
        {
            rightDoor.transform.Rotate(new Vector3(0, 0, -1));
            doorsHaveOpened = false;
        }
        if ((int)(leftDoor.transform.rotation.eulerAngles.y) != doorOpenAngle)
        {
            leftDoor.transform.Rotate(new Vector3(0, 0, 1));
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
    }
}
