using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBlock : MonoBehaviour
{
    List<Button> allButtons = new List<Button>();
    List<Button> buttonsPressed = new List<Button>();
    public Button[] buttonOrder;
    private bool buttonsSolved = false;
    private AudioManager audio;

    public GameObject door;

    public bool doorsOpening = false;

    void Start()
    {
        foreach (Transform t in transform)
        {
            Button b = t.GetComponent<Button>();
            if (b != null)
            {
                allButtons.Add(b);
            }
        }
        audio = FindObjectOfType<AudioManager>();
        audio.setPosition("Button", position: buttonOrder[2].transform.position);
        audio.setPosition("ButtonMitEinrasten", position: buttonOrder[0].transform.position);

    }
    private bool CheckButtons()
    {
        for (int i = 0; i < buttonOrder.Length; i++)
        {
            if (!buttonOrder[i].Equals(buttonsPressed[i]))
            {
                return false;
            }
        }
        return true;
    }


    public void Update()
    {
        if (!buttonsSolved)
        {
            foreach (Button b in allButtons)
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
                    Debug.Log("Code richtig");
                    doorsOpening = true;
                    audio.Play("ButtonMitEinrasten", 0.7f);
                    door.GetComponent<Rigidbody>().AddForce(Vector3.forward * 20);
                    foreach (Button b in allButtons)
                    {
                        b.currentlyInteractable = false;
                    }
                }
                else
                {
                    foreach (Button b in buttonsPressed)
                    {
                        b.currentlyInteractable = true;
                        b.Interact();
                    }
                    buttonsPressed.Clear();
                }
            }
        }
    }

}
