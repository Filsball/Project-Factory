using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockelSchwarzlicht : SockelAbstract
{
    public BRSchwarzlicht Raetsel;

    public override void Interact()
    {
        In(typeof(GluehbirneSchwarzlicht));
    }
    public new void Update()
    {
        if (Gluehbirne.activeSelf)
        {
            Gluehbirne.GetComponentInChildren<Light>().enabled = StromAktiviert;
            Raetsel.SichtbarMachen();
        }
    }
}
