using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sockel : SockelAbstract
{
    [SerializeField] GameObject Lichtzone;
    
    private new void Start()
    {
        allSockets.Add(this);
        Gluehbirne.SetActive(false);
        Lichtzone.SetActive(false);
        Gluehbirne.SetActive(false);
    }

    public new void Update()
    {
      
        if (Gluehbirne.activeSelf)
        {
            Gluehbirne.GetComponentInChildren<Light>().enabled = StromAktiviert;
            Lichtzone.SetActive(true);
        }
    }

    public override void Interact()
    {
            In(typeof(Gluehbirne));
    }
}