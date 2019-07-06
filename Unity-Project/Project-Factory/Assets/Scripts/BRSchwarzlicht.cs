using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRSchwarzlicht : MonoBehaviour
{
    [SerializeField] Notiz Leer;
    [SerializeField] Notiz Sichtbar;

    void Start()
    {
        Leer.enabled = true;
        Sichtbar.enabled = false;
    }
    public void SichtbarMachen() {
        Leer.enabled = false;
        Sichtbar.enabled = true;
    }
}
