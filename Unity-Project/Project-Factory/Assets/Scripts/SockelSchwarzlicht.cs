using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockelSchwarzlicht : SockelAbstract
{
    public override void Interact()
    {
        In(typeof(GluehbirneSchwarzlicht));
    }
}
