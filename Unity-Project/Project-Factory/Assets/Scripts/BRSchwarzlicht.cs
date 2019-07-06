﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRSchwarzlicht : MonoBehaviour
{
    [SerializeField] Texture Leer;
    [SerializeField] Texture Sichtbar;

    public Renderer rend;

    void Start()
    {
        rend.material.SetTexture("_MainTex", Leer);
    }

    public void SichtbarMachen() {
        rend.material.SetTexture("_MainTex", Sichtbar);
    }
}
