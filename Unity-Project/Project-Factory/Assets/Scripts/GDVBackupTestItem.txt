using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour, InventoryItem
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string Name { get { return "TestItem"; } }

    public Sprite _Image = null;

    public Sprite Image { get { return _Image; } }

    public void OnPickUp() {
        gameObject.SetActive(false);
        //modify here
    }
}
