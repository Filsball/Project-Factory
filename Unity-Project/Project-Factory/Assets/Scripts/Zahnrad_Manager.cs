using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zahnrad_Manager : MonoBehaviour
{
    public bool running = true;
    private List<ZahnradAufsatz> aufsaetze = new List<ZahnradAufsatz>();

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ZahnradAufsatz>() != null)
            {
                aufsaetze.Add(child.GetComponent<ZahnradAufsatz>());
                if(i++ % 2 == 0)
                {
                    child.transform.Rotate(Vector3.up, 45);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool nextOneSpinning = running;
        foreach(ZahnradAufsatz aufsatz in aufsaetze)
        {
            aufsatz.spinning = nextOneSpinning;
            nextOneSpinning = aufsatz.zahnrad != null && aufsatz.spinning;
        }
    }
}
