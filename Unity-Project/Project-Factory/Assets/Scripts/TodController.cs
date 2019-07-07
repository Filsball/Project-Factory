using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TodController : MonoBehaviour
{
    [SerializeField] public GameObject todUI;
    public bool isTod = false;
    private Scene scene;
    private AudioListener audioListener;
    private static AudioManager audio;
    public Image background;

    void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
        scene = SceneManager.GetActiveScene();
        todUI.SetActive(false);
        audio = FindObjectOfType<AudioManager>();

    }

    private void Update()
    {
        if (isTod)
        {
            Debug.Log("ist tod");
            ActivateTod();
            audio.getSound("GameOver").source.ignoreListenerPause = true;
            audio.Play("GameOver", 0.5f);
            
        }
    }
    
    public void ActivateTod()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        todUI.SetActive(true);
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        isTod = false;
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        isTod = false;
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = true;
        SceneManager.LoadScene(scene.name);
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

    public void setTod()
    {
        isTod = !isTod;
    }

    public bool getTod()
    {
        return isTod;
    }

}