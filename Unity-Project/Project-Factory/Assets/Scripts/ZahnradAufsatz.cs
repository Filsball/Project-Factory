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
   
    public static float[] GetSpaceBetween(ZahnradAufsatz a, ZahnradAufsatz b)
    {
        float dist = DistanceBetween(a, b);
        float spaceBetweenInner = dist;
        float spaceBetweenOuter = dist;
        if (a.zahnrad != null)
        {
            spaceBetweenInner -= (a.zahnrad.colInner.size.x * a.zahnrad.transform.localScale.x) / 2;
            spaceBetweenOuter -= (a.zahnrad.colOuter.size.x * a.zahnrad.transform.localScale.x) / 2;
        }
        if (b.zahnrad != null)
        {
            spaceBetweenInner -= (b.zahnrad.colInner.size.x * b.zahnrad.transform.localScale.x) / 2;
            spaceBetweenOuter -= (b.zahnrad.colOuter.size.x * b.zahnrad.transform.localScale.x) / 2;
        }
        return new float[] { spaceBetweenInner, spaceBetweenOuter};
    }

    public static float DistanceBetween(ZahnradAufsatz a, ZahnradAufsatz b)
    {
        return Mathf.Sqrt((a.transform.position - b.transform.position).sqrMagnitude);
    }

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
        zahnrad = z;
        if (z != null)
        {
            z.GetComponent<Rigidbody>().useGravity = false;
            z.GetComponent<Rigidbody>().isKinematic = true;

            Zahnrad_Manager manager = gameObject.transform.parent.gameObject.GetComponent<Zahnrad_Manager>();
            if (manager != null)
            {
                List<ZahnradAufsatz> neighbours = manager.GetAdjacentAufsatz(this);
                foreach (ZahnradAufsatz neighbour in neighbours)
                {
                    float[] spaceBetween = GetSpaceBetween(this, neighbour);
                    if (spaceBetween[0] < 0)
                    {
                        RemoveZR();
                        break;
                    }
                }
            }
        }
    }
    private void OnMouseDown()
    {
        if (!gameObject.transform.parent.gameObject.GetComponent<Zahnrad_Manager>().solved)
        {
            RemoveZR();
        }
    }
    public void RemoveZR()
    {
        if (zahnrad != null)
        {
            float zr_height = zahnrad.GetComponent<BoxCollider>().size.y / 2;
            zahnrad.transform.position = transform.position + transform.up * ((zr_height * zahnrad.transform.localScale.y) + (transform.localScale.y));
            zahnrad.GetComponent<Rigidbody>().useGravity = true;
            zahnrad.GetComponent<Rigidbody>().isKinematic = false;


            //zahnrad.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.up));
            //zahnrad.transform.rotation = new Quaternion(0, 0, 0, 0);
            SetZahnrad(null);
        }
    }
}
