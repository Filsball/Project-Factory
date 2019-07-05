using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArea : MonoBehaviour
{
    [SerializeField] GameObject ExplosiveLights;
    // Start is called before the first frame update
    void Start()
    {
        ExplosiveLights.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Explode() {
        ExplosiveLights.SetActive(true);
    }
}
