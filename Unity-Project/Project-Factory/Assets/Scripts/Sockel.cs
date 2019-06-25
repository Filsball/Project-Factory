﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sockel : InteractableObject
{
    [SerializeField] Light Gluehbirne;
    public Inventory inventory;

    private void Start()
    {
        Gluehbirne.enabled = false;
    }
    public override void Interact()
    {
        Debug.Log("Sockel Interact");
        if (!Gluehbirne.enabled)
        {
            Debug.Log("In()");
            In();
        }
    }
    private void In() {
        Debug.Log("In() aufgerufen");
        // Hier moeglicherweise prüfen ob Generator aktiviert/ Strom vorhanden
        InventoryItem birne = inventory.containsGluehbirne();
        Debug.Log("In If birne im Inventar");
        if (birne != null)
        {
            inventory.removeItem(birne);
            Gluehbirne.enabled = true;
        }
    }
    
}
