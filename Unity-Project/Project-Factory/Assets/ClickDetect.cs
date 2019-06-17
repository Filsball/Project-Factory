using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetect : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("INIT");
    }
    private void OnMouseDown()
    {
        Debug.Log("CUBE CLICKED");
    }
}
