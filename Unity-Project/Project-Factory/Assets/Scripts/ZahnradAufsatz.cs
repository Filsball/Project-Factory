﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZahnradAufsatz : MonoBehaviour
{
    public Zahnrad zahnrad;
    public bool spinning = true;
    public float spinningSpeed = 100f;
    public bool imUZSdrehen = true;

    private float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        positionZahnrad();
        if(spinning)
            transform.Rotate(Vector3.up, (imUZSdrehen ? 1 : -1) * spinningSpeed * Time.deltaTime);
    }

    void positionZahnrad()
    {
        if(zahnrad == null) { return; }
        zahnrad.transform.position = transform.position + Vector3.up * (transform.localScale.y - (1.5f * zahnrad.transform.localScale.y));
        zahnrad.transform.rotation = transform.rotation;
    }
}
