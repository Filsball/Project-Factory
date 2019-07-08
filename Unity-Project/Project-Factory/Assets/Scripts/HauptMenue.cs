using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HauptMenue : MonoBehaviour
{
    public Image bild1;
    public Image bild2;
    public Image bild3;
    private Vector3 bild1Position;
    private Vector3 bild2Position;
    private Vector3 bild3Position;
    private Vector3 tmpPosition;
    private bool fade1;
    private bool fade2;
    private bool fade3;
    private Color c;
    private AudioSource audio;
    public static HauptMenue instance;
    public static float volume;
    private float pufferzeit;
    private int fadetime;

    public void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        instance = this;
        audio = GetComponent<AudioSource>();
        audio.Play();
        fade1 = true;
        fade2 = false;
        fade3 = false;
        bild1Position = bild1.transform.localPosition;
        bild2Position = bild2.transform.localPosition;
        bild3Position = bild3.transform.localPosition;
        pufferzeit = 0f;
        fadetime = 30;
    }
    public void Update()
    {
        if (fade1 && bild1.color.a < 0.4f && pufferzeit < 0.5f)
        {
            tmpPosition = bild1.transform.localPosition;
            tmpPosition += new Vector3(-.3f, -.2f, 0);
            bild1.transform.localPosition = tmpPosition;
            tmpPosition = bild3.transform.localPosition;
            tmpPosition += new Vector3(-.4f, .3f, 0);
            bild3.transform.localPosition = tmpPosition;
            c = bild3.color;
            c.a -= Time.deltaTime / fadetime;
            if (c.a > 0.4f) c.a = 0.4f;
            bild3.color = c;
            c = bild1.color;
            c.a += Time.deltaTime / fadetime;
            if (c.a < 0f) c.a = 0f;
            bild1.color = c;
            pufferzeit += Time.deltaTime / fadetime;
        }
        else if (fade1 && bild1.color.a >= 0.4f)
        {
            fade1 = false;
            fade2 = true;
            bild2.transform.localPosition = bild2Position;
            bild3.color = new Color(bild3.color.r, bild3.color.g, bild3.color.b, 0);
            pufferzeit = 0f;

        }
        if (fade2 && bild2.color.a < 0.4f && pufferzeit < 0.5f)
        {
            tmpPosition = bild1.transform.localPosition;
            tmpPosition += new Vector3(-.3f, -.2f, 0);
            bild1.transform.localPosition = tmpPosition;
            tmpPosition = bild2.transform.localPosition;
            tmpPosition += new Vector3(+.4f, -.2f, 0);
            bild2.transform.localPosition = tmpPosition;
            c = bild1.color;
            c.a -= Time.deltaTime / fadetime;
            if(c.a > 0.4f) c.a = 0.4f; if (c.a < 0f) c.a = 0f;
            bild1.color = c;
            c = bild2.color;
            c.a += Time.deltaTime / fadetime;
            if(c.a < 0f) c.a = 0f;
            bild2.color = c;
            pufferzeit += Time.deltaTime / fadetime;

        }
        else if (fade2 && bild2.color.a >= 0.4f)
        {
            fade2 = false;
            fade3 = true;
            bild3.transform.localPosition = bild3Position;
            pufferzeit = 0f;
        }
        if (fade3 && bild3.color.a < 0.4f && pufferzeit < 0.5f)
        {
            tmpPosition = bild2.transform.localPosition;
            tmpPosition += new Vector3(+.4f, -.2f, 0);
            bild2.transform.localPosition = tmpPosition;
            tmpPosition = bild3.transform.localPosition;
            tmpPosition += new Vector3(-.4f, .3f, 0);
            bild3.transform.localPosition = tmpPosition;
            c = bild2.color;
            c.a -= Time.deltaTime / fadetime;
            if (c.a > 0.4f) c.a = 0.4f;
            bild2.color = c;
            c = bild3.color;
            c.a += Time.deltaTime / fadetime;
            if (c.a < 0f) c.a = 0f;
            bild3.color = c;
            pufferzeit += Time.deltaTime / fadetime;
        }
        else if (fade3 && bild3.color.a >= 0.4f)
        {
            fade3 = false;
            fade1 = true;
            bild1.transform.localPosition = bild1Position;
            pufferzeit = 0f;
        }
    }

    public void StarteSpiel()
    {
        volume = AudioListener.volume;
        StartCoroutine(SceneLoader());
    }

    public void Beenden()
    {
        Application.Quit();
    }

    public IEnumerator SceneLoader()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
    
}
