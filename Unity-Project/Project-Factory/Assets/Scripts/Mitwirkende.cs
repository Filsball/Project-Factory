using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mitwirkende : MonoBehaviour
{
    private static Mitwirkende instance;
    public Image bild;
    public Text text;
    public Text text2;
    public Text text3;
    private int geschwindigkeit = 80;
    private AudioSource audio;
    public AudioSource audioParent;
    private Vector3 startPosition;
    private Color startFarbe;
    public bool mitwirkendeAktiv;
    public bool mitwirkendeBeendet;
    private Color c = new Color(0, 0, 0, 0);
    private Vector3 v = new Vector3(0, 0, 0);

    // Update is called once per frame
    void Update()
    {
        if (mitwirkendeAktiv)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || mitwirkendeBeendet)
            {
                mitwirkendeBeendet = true;
                c = bild.color;
                c.a -= Time.deltaTime / 2;
                if (c.a < 0) c.a = 0;
                bild.color = c;
                c = text.color;
                c.a -= Time.deltaTime / 2;
                if (c.a < 0) c.a = 0;
                text.color = c;
                c = text2.color;
                c.a -= Time.deltaTime / 2;
                if (c.a < 0) c.a = 0;
                text2.color = c;
                c = text3.color;
                c.a -= Time.deltaTime / 2;
                if (c.a < 0) c.a = 0;
                text3.color = c;
                v = gameObject.transform.position;
                v.y += Time.deltaTime * geschwindigkeit;
                gameObject.transform.position = v;
                audioParent.volume += Time.deltaTime / 3;
                if (audioParent.volume > 0.7) audioParent.volume = 0.7f;
                audio.volume -= Time.deltaTime / 3;
                if (audio.volume < 0) audio.volume = 0;
                zuHauptmenue();
            } 
            else if(gameObject.transform.position.y < 1600 || bild.color.a < 1 || audioParent.volume > 0 || audio.volume < 1)
            {
                c = bild.color;
                c.a += Time.deltaTime / 3;
                if (c.a > 1) c.a = 1;
                bild.color = c;
                v = gameObject.transform.position;
                v.y += Time.deltaTime * geschwindigkeit;
                gameObject.transform.position = v;
                audioParent.volume -= Time.deltaTime / 3;
                if (audioParent.volume < 0) audioParent.volume = 0;
                audio.volume += Time.deltaTime / 3;
                if (audio.volume > 1) audio.volume = 1;
            }
            else
            {
                mitwirkendeBeendet = true;
            }
        }
    }

    public void ButtonMitwirkende()
    {
        startPosition = gameObject.transform.position;
        startFarbe = this.bild.color;
        audio = GetComponent<AudioSource>();
        audio.volume = 0;
        if(!audio.isPlaying) audio.Play();
        bild.gameObject.SetActive(true);
        mitwirkendeAktiv = true;
        text.color = new Color(1f, 1f, 1f, 1f);
        text2.color = new Color(1f, 1f, 1f, 1f);
        text3.color = new Color(1f, 1f, 1f, 1f);
    }

    void zuHauptmenue()
    {
        if(bild.color.a <= 0.001 && audioParent.volume >= 0.699 && audio.volume <= 0.001)
        {
            bild.color = startFarbe;
            gameObject.transform.position = startPosition;
            mitwirkendeAktiv = false;
            mitwirkendeBeendet = false;
            bild.gameObject.SetActive(false);
        }
    }
}
