using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zahnrad_Manager : MonoBehaviour
{
    private List<ZahnradAufsatz> aufsaetze = new List<ZahnradAufsatz>();

    // Start is called before the first frame update
    void Start()
    {
        
        foreach(Transform child in transform)
        {
            if(child.GetComponent<ZahnradAufsatz>() != null)
            {
                aufsaetze.Add(child.GetComponent<ZahnradAufsatz>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
