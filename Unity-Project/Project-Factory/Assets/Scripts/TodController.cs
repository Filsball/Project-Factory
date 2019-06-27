using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TodController : MonoBehaviour
{
    [SerializeField] private GameObject TodUI;
    private Scene scene;
    private AudioListener audioListener;

    void Start()
    {
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
        TodUI.SetActive(true);
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Respawn()
    {
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

}