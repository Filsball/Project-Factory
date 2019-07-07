using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class Sockel : InteractableObject
{
    [SerializeField] GameObject Gluehbirne;
    //[SerializeField] Light GluehbirneX;
    [SerializeField] GameObject Lichtzone;

    public static bool StromAktiviert;
    public static List<Sockel> allSockets = new List<Sockel>();

    public bool hasLightBulbFromBeginning = false;

    public Inventory inventory;

    private new void Start()
    {
        allSockets.Add(this);
        Gluehbirne.SetActive(hasLightBulbFromBeginning);
        currentlyInteractable = !hasLightBulbFromBeginning;
        Lichtzone.SetActive(Gluehbirne.activeSelf && StromAktiviert);
    }

   // public bool GluehbirneInSockelAktiviert() { return Gluehbirne.enabled; }

    public new void Update()
    {
        if (Gluehbirne.activeSelf)
        {
            Gluehbirne.GetComponentInChildren<Light>().enabled = StromAktiviert;
            Lichtzone.SetActive(StromAktiviert);
        }
    }

    public override void Interact()
    {
        Debug.Log("Sockel Interact");
     //   if (!Gluehbirne.enabled)
      //  {
            Debug.Log("In()");
            In();
      //  }
    }
    private void In()
    {
        Debug.Log("In() aufgerufen");
        // Hier moeglicherweise pruefen ob Generator aktiviert/ Strom vorhanden
        InventoryItem birne = inventory.containsGluehbirne();
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
    public static void StromAn() {
        StromAktiviert = true;
    }

}