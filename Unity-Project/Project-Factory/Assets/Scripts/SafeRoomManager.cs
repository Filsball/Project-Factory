using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class SafeRoomManager : MonoBehaviour
{
    public Door finalDoor;
    public GameObject victoryPanel;
    public Text timeNeededText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateVictory()
    {
        GameObject player = GameObject.Find("FPSController");
        PlayerControl pc = player.GetComponent<PlayerControl>();
        Time.timeScale = 0;
        AudioListener.pause = true;
        int totalTime = (int)pc.TIME_NEEDED;

        int hours = totalTime / 3600;
        totalTime -= hours * 3600;

        int minutes = totalTime / 60;
        totalTime -= minutes * 60;

        timeNeededText.text = hours + "h " + minutes + "m " + totalTime +"s";
        victoryPanel.SetActive(true);

        player.GetComponent<FirstPersonController>().enabled = false;
        pc.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if (finalDoor.open)
            {
                finalDoor.Interact();
            }
            // gefangen im Saferoom hehe
            finalDoor.currentlyInteractable = false;

            ActivateVictory();
        }
    }
    public void Hauptmenue()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        AudioManager audio = FindObjectOfType<AudioManager>();
        audio.sounds[0].source.Stop();
        audio.sounds[1].source.Stop();
        audio.sounds[2].source.Stop();
        Destroy(audio);
        SceneManager.LoadScene(0);
    }
}
