using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sockel : SockelAbstract
{
    [SerializeField] GameObject Lichtzone;
    public bool hasLightBulbFromBeginning = false;
    
    private new void Start()
    {
        allSockets.Add(this);
        Gluehbirne.SetActive(hasLightBulbFromBeginning);
    }

    public new void Update()
    {
      
        if (Gluehbirne.activeSelf)
        {
            Gluehbirne.GetComponentInChildren<Light>().enabled = StromAktiviert;
        }
        Lichtzone.SetActive(Gluehbirne.activeSelf && StromAktiviert);
    }

    public override void Interact()
    {
            In(typeof(Gluehbirne));
    }
}