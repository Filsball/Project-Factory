using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZahnradAufsatz : MonoBehaviour
{
    public Zahnrad zahnrad;
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
        transform.Rotate(Vector3.up, (imUZSdrehen ? 1 : -1) * 100f * Time.deltaTime);
    }

    void positionZahnrad()
    {
        if(zahnrad == null) { return; }

        zahnrad.transform.position = transform.position + new Vector3(0, transform.localScale.y - zahnrad.transform.localScale.y/2, 0);
        zahnrad.transform.rotation = transform.rotation;
    }
}
