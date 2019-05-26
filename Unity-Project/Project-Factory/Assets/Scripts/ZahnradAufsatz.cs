using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZahnradAufsatz : MonoBehaviour
{
    public Zahnrad zahnrad;
    public bool spinning = true;
    public float spinningSpeed = 100f;
    public bool imUZSdrehen = true;

    private bool positioned = false;
    private Vector3 initialScale;

    private readonly float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (zahnrad != null)
        {
            float zr_height = zahnrad.GetComponent<BoxCollider>().size.y/2;
            zahnrad.transform.position = transform.position + transform.up * ((zr_height * zahnrad.transform.localScale.y) - (transform.localScale.y));
            zahnrad.transform.localRotation = transform.rotation;
            transform.localScale = new Vector3(initialScale.x * zahnrad.transform.localScale.x, initialScale.y, initialScale.z * zahnrad.transform.localScale.z);
        }
        else
        {
            transform.localScale = initialScale;
        }
        if (spinning)
        {
            transform.Rotate(Vector3.up, (imUZSdrehen ? 1 : -1) * spinningSpeed * Time.deltaTime);
        }
    }

    public void SetZahnrad(Zahnrad z)
    {
        this.zahnrad = z;
    }
    private void OnMouseDown()
    {
        RemoveZR();
    }
    public void RemoveZR()
    {
        if (zahnrad != null)
        {
            zahnrad.transform.position = new Vector3(0, 0, 0);
            zahnrad.transform.rotation = new Quaternion(0, 0, 0, 0);
            SetZahnrad(null);
        }
    }
}
