using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class Sockel : SockelAbstract
{
    [SerializeField] GameObject Lichtzone;
    //[SerializeField] GameObject Gluehbirne;
    //public static bool StromAktiviert;
    //public static List<Sockel> allSockets = new List<Sockel>();

    //public Inventory inventory;

    private new void Start()
    {
        //allSockets.Add(this);
        //Gluehbirne.SetActive(false);
        
        Lichtzone.SetActive(false);
        //StromAktiviert = false;
    }

   // public bool GluehbirneInSockelAktiviert() { return Gluehbirne.enabled; }

    public new void Update()
    {
        //  if (Gluehbirne.activeSelf)
        //  {
        //Gluehbirne.GetComponentInChildren<Light>().enabled = StromAktiviert;
        // Lichtzone.SetActive(StromAktiviert);
        // }
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
    /*private void In()
    {
        Debug.Log("In() aufgerufen");
        // Hier moeglicherweise pruefen ob Generator aktiviert/ Strom vorhanden
        InventoryItem birne = inventory.ContainsElementOfType(typeof(Gluehbirne));
        Debug.Log("In If birne im Inventar");
        if (birne != null)
        {
            inventory.removeItem(birne);
            Gluehbirne.SetActive(true);
            if (StromAktiviert)
            {
                //Lichtzone.SetActive(true);
                currentlyInteractable = false;
                Debug.Log("Alle Stücke Aktiviert");
            }
        }
    }
    */
   /* public static void StromAn() {
        StromAktiviert = true;
    }
    */
}