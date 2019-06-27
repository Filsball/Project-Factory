using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class Sockel : InteractableObject
{
    [SerializeField] Light Gluehbirne;
    [SerializeField] GameObject Lichtzone;

    public Inventory inventory;

    private new void Start()
    {
        Gluehbirne.enabled = false;
        Lichtzone.SetActive(false);
    }

    public bool GluehbirneInSockelAktiviert() { return Gluehbirne.enabled; }

    public override void Interact()
    {
        Debug.Log("Sockel Interact");
        if (!Gluehbirne.enabled)
        {
            Debug.Log("In()");
            In();
        }
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
            Gluehbirne.enabled = true;
            Lichtzone.SetActive(true);
        }
    }

}