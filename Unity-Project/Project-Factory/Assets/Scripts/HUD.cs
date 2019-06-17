using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    public Inventory inventory;

    public GameObject MsgPanel;

    public GameObject InventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        inventory.ItemAdded += InventoryScript_ItemAdded;
        inventory.ItemRemoved += InventoryItemRemoved;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e) {

        Transform inventoryPanel = transform.Find("Inventory");

        foreach(Transform Slot in inventoryPanel){

            Transform imageTransform = Slot.GetChild(0).GetChild(0);
            Image img = imageTransform.GetComponent<Image>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            if (!img.enabled) {
                img.enabled = true;
                img.sprite = e.Item.Image;
                itemDragHandler.Item = e.Item;
                break;
            }
        }
    }

    private void InventoryItemRemoved(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");

        foreach (Transform Slot in inventoryPanel)
        {
            Transform imageTransform = Slot.GetChild(0).GetChild(0);
            Image img = imageTransform.GetComponent<Image>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            if (itemDragHandler.Item.Equals(e.Item))
            {
                img.enabled = false;
                img.sprite = null;
                itemDragHandler = null;
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

    internal void OpenInventory()
    {
        InventoryPanel.SetActive(true);
    }

    internal void CloseInventory()
    {
        InventoryPanel.SetActive(false);
    }
}
