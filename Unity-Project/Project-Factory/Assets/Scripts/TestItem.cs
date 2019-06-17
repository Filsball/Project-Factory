using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : InventoryItem
{

    override public void OnPickUp()
    {
        gameObject.SetActive(false);
        //modify here
    }

    override public void OnDrop()
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
