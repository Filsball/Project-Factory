using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HauptMenue : MonoBehaviour
{
    
    private AudioSource audio;
    public static HauptMenue instance;
    public static float volume;

    public void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        instance = this;
        audio = GetComponent<AudioSource>();
        audio.Play();
    }
    public void Update()
    {
    }

    public void StarteSpiel()
    {
        audio.Stop();
        volume = AudioListener.volume;
        SceneManager.LoadScene(1);
        this.gameObject.SetActive(false);
    }

    public void Beenden()
    {
        Application.Quit();
    }
}
