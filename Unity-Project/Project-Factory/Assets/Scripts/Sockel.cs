using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class Sockel : InteractableObject
{
    [SerializeField] Light Gluehbirne;
    //[SerializeField] Light GluehbirneX;
    [SerializeField] GameObject Lichtzone;

    public static bool StromAktiviert;

    public Inventory inventory;

    private new void Start()
    {
        Gluehbirne.enabled = false;
        //GluehbirneX.enabled = false;
        Lichtzone.SetActive(false);
        StromAktiviert = false;
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
        //if(StromAktiviert){
        InventoryItem birne = inventory.containsGluehbirne();
        Debug.Log("In If birne im Inventar");
        if (birne != null)
        {
            inventory.removeItem(birne);
            Gluehbirne.enabled = true;
            //GluehbirneX.enabled = true;
            Lichtzone.SetActive(true);
            currentlyInteractable = false;
        }
        //}
    }
    public static void StromAn() {
        StromAktiviert = true;
    }

}