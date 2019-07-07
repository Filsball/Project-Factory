using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockelAbstract : InteractableObject
{
    public GameObject Gluehbirne;
    public static bool StromAktiviert;
    public static List<SockelAbstract> allSockets = new List<SockelAbstract>();
    public Inventory inventory;

    private new void Start()
    {
        allSockets.Add(this);
        Gluehbirne.SetActive(false);
        StromAktiviert = false;
        
    }
    public new void Update()
    {
        if (Gluehbirne.activeSelf)
        {
            Gluehbirne.GetComponentInChildren<Light>().enabled = StromAktiviert;
        }
    }
    public override void Interact()
    {
            In(typeof(Birne));
    }
    public void In(Type type)
    {
        Debug.Log("In() aufgerufen");
        // Hier moeglicherweise pruefen ob Generator aktiviert/ Strom vorhanden
        InventoryItem birne = inventory.ContainsElementOfType(type);
        Debug.Log("In If birne im Inventar");
        if (birne != null)
        {
            inventory.removeItem(birne);
            Gluehbirne.SetActive(true);
            currentlyInteractable = false;
        }
    }
    public static void StromAn()
    {
        StromAktiviert = true;
    }
}
