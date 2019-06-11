﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory inventory;

    public GameObject MsgPanel;
    // Start is called before the first frame update
    void Start()
    {
        inventory.ItemAdded += InventoryScript_ItemAdded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e) {

        Transform inventoryPanel = transform.Find("Inventory");

        foreach(Transform Slot in inventoryPanel){
            Image img = Slot.GetChild(0).GetChild(0).GetComponent<Image>();
            if (!img.enabled) {
                img.enabled = true;
                img.sprite = e.Item.Image;
                break;
            }
        }
    }

    public void OpenMsgPanel(string text)
    {
        MsgPanel.SetActive(true);
    }

    public void CloseMsgPanel()
    {
        MsgPanel.SetActive(false);
    }
}