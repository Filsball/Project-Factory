using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaferoomOnEnter : MonoBehaviour
{
    private AudioManager audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audio.saferoomIstAktiv();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            audio.saferoomIstNichtAktiv();
        }
    }
}
