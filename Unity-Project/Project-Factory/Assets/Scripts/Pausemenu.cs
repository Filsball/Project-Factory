using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Pausemenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused;
    public TodController todController;
    private Scene scene;
    public static Slider slider;
    private AudioListener audioListener;
    public bool pauseAn = false;
    
    void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
        scene = SceneManager.GetActiveScene();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            isPaused = !isPaused;
        }
        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            if (!todController.getTod())
            {
                if (pauseMenuUI.transform.localPosition.Equals(new Vector3(0, 350, 0)))
                {
                    ShowMenue();
                }
                if (pauseAn)
                {
                    DeactivateMenu();
                }
                
            }
            
        }

    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        pauseAn = true;


    }
    public void DeactivateMenu()
    {
        pauseAn = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = true;
        SceneManager.LoadScene(scene.name);
    }

    public void Hauptmenue()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
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
    
    public void HideMenue()
    {
        pauseMenuUI.transform.localPosition = new Vector3(0, 800, 0);
    }

    public void ShowMenue()
    {
        pauseMenuUI.transform.localPosition = new Vector3(0, 0, 0);
    }
    
}
