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
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(true);
            gameObject.transform.position = hit.point;
        }
    }
}
