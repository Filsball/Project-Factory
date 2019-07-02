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
    public Image background;

    void Start()
    {
        gameObject.SetActive(false);
        audioListener = FindObjectOfType<AudioListener>();
        scene = SceneManager.GetActiveScene();

    }

    private void Update()
    {
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
        isTod = true;
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
        Time.timeScale = 0;
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