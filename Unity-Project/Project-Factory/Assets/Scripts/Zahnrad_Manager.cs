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
            ZahnradAufsatz za = child.GetComponent<ZahnradAufsatz>();
            if (za != null)
            {
                aufsaetze.Add(za);
                za.imUZSdrehen = i++ % 2 == 0;
                if (za.imUZSdrehen)
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
        int i = 0;
        foreach(ZahnradAufsatz aufsatz in aufsaetze)
        {
            aufsatz.spinning = nextOneSpinning && (aufsatz.zahnrad != null || i==0);
            nextOneSpinning = aufsatz.zahnrad != null && aufsatz.spinning;
            i++;
        }
    }
}
