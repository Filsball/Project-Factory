using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public bool running = true;
    Zahnrad_Manager zrManager;

    List<Button> buttons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        zrManager = GetComponentInChildren<Zahnrad_Manager>();

        foreach (Transform child in transform)
        {
            Button b = child.GetComponent<Button>();
            if (b == null) continue;
            buttons.Add(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        zrManager.running = running;
    }
}
