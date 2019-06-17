using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour, InventoryItem
{
    public string Name { get { return "TestItem"; } }

    public Sprite _Image = null;

    public Sprite Image { get { return _Image; } }

    public void OnPickUp() {
        gameObject.SetActive(false);
        //modify here
    }

    public void OnDrop()
    {
        gameObject.SetActive(true);
    }
}
