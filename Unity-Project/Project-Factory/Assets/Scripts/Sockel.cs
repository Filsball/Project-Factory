using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sockel : InteractableObject
{
    [SerializeField] Light Gluehbirne;
    [SerializeField] GameObject Lichtzone;

    public Inventory inventory;

    private void Start()
    {
        Gluehbirne.enabled = false;
        Lichtzone.SetActive(false);
    }

    public bool GluehbirneInSockelAktiviert() { return Gluehbirne.enabled; }

    public override void Interact()
    {
        if (!Gluehbirne.enabled)
        {
            In();
        }
    }
    private void In() {
        // Hier moeglicherweise prüfen ob Generator aktiviert/ Strom vorhanden
        InventoryItem birne = inventory.containsGluehbirne();
        if (birne != null)
        {
            inventory.removeItem(birne);
            Gluehbirne.enabled = true;
            Lichtzone.SetActive(true);
        }
    }
}
