using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZahnradAufsatz : MonoBehaviour
{
    public Zahnrad zahnrad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        positionZahnrad();
    }

    void positionZahnrad()
    {
        if(zahnrad == null) { return; }

        zahnrad.transform.position = transform.position + new Vector3(0, transform.localScale.y, 0);
    }
}
