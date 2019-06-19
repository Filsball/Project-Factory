using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Collider))]
public class Zahnrad_Manager : InteractableObject
{
    public GameObject zahnradPrefab;
    public GameObject aufsatzPrefab;
    public BoxCollider boxCollider;
    public AudioManager audioManager;
    private bool playingAudio = false;

    public List<float> zahnradSizes;
    private float zrScaleRadius;

    public bool running = true;
    public bool solved = false;
    public bool instantiateGears = false;

    public Camera riddleCam;

    private Vector3 size;
    private List<ZahnradAufsatz> aufsaetze = new List<ZahnradAufsatz>();

    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();
        _interactionName = "näher zu treten";
        audioManager = FindObjectOfType<AudioManager>();
        boxCollider = GetComponent<BoxCollider>();
        size = boxCollider.size;
        if (zahnradPrefab == null)
        {
            print("WARNING: ZahnradPrefab of ZahnradManager " + this + " is null. Setting Zahnrad-Boundaries to (1, 1, 1)");
            zrScaleRadius = 1;
        } else
        {
            //GameObject z = Instantiate(zahnradPrefab);
            zrScaleRadius = zahnradPrefab.GetComponent<BoxCollider>().size.x;
            //Destroy(z);
        }

        for (int i = 0; i < zahnradSizes.Count; i++)
        {
            GameObject aufsatzObject = Instantiate(aufsatzPrefab, transform); // instantiate (crate) a new Aufsatz with me as parent
            float r = zrScaleRadius * zahnradSizes[i] / 2; // radius
            //print("Radius: " + r);
            float x = r - size.x/2; // coordinates of current Aufsatz
            float z = 0;
            //print("Initial (x | z) = (" + x + " | " + z + " )");
            if (i > 0)
            {
                float dist = r + zrScaleRadius * zahnradSizes[i - 1]/2 - 0.01f; // distance to previous Aufsatz
                float angle = 0;    // random angle to previous Aufsatz
                int errorCounter = -1; // to prevent while(true)
                //print("Distance: " + dist);
                do
                {
                    if(++errorCounter > 50000)
                    {
                        print("ERROR: Zahnraeder too big for ZahnradManager-Boundaries " + size +"! exiting ZahnradManager...");
                        return;
                    }
                    angle = 2 * Mathf.PI * (Random.Range(0.0f, 90.0f) - 45) / 360.0f;
                    x = (Mathf.Cos(angle) * dist) + aufsaetze[i - 1].transform.localPosition.x;
                    z = (Mathf.Sin(angle) * dist) + aufsaetze[i - 1].transform.localPosition.z;
                    //if (errorCounter % 1000 == 0)
                    //{
                    //    print("Temporary (x | z) = (" + x + " | " + z + " )");
                    //}
                } while ( // while out of bounds
                    x + r >  size.x/2 ||
                    x - r < -size.x/2 ||
                    z + r >  size.z/2 || 
                    z - r < -size.z/2
                );
            }
            //print("Final (x | z) = (" + x + " | " + z + " )");
            Vector3 scale = aufsatzObject.transform.localScale;
            aufsatzObject.transform.localScale = new Vector3(scale.x, size.y/2, scale.z);
            aufsatzObject.transform.localPosition = new Vector3(x, 0, z);


            ZahnradAufsatz za = aufsatzObject.GetComponent<ZahnradAufsatz>();
            za.imUZSdrehen = i % 2 == 0;
            aufsaetze.Add(za);

            if (instantiateGears)
            {
                GameObject gear = Instantiate(zahnradPrefab);
                gear.transform.localScale = new Vector3(zahnradSizes[i], 1, zahnradSizes[i]);
                gear.name = zahnradSizes[i] + "er Zahnrad";
            }
        }
        audioManager.setPosition("Zahnraeder", position: boxCollider.center + transform.position);
        //int i = 0
        //foreach (Transform child in transform)
        //{
        //    ZahnradAufsatz za = child.GetComponent<ZahnradAufsatz>();
        //    if (za != null)
        //    {
        //        aufsaetze.Add(za);
        //        za.imUZSdrehen = i++ % 2 == 0;
        //        if (za.imUZSdrehen)
        //        {
        //            //child.transform.Rotate(child.transform.up, 45);
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();
        //return;
        bool nextOneSpinning = running;
        if(nextOneSpinning && aufsaetze[0].zahnrad != null) {
            if (!playingAudio)
            {
                audioManager.generatorStarted = true;
                audioManager.Play("Zahnraeder", 0.1f, audioManager.transform.position);
                audioManager.Play("GeneratorStartend", 1.0f, audioManager.transform.position - new Vector3(2f, 2f, 2f));
                playingAudio = true;
            }
        }
        for (int i= 0; i < aufsaetze.Count; i++)
        {
            ZahnradAufsatz aufsatz = aufsaetze[i];

            aufsatz.spinning = nextOneSpinning && (aufsatz.zahnrad != null && aufsatz.zahnrad.transform.localScale.x == zahnradSizes[i]);
            if(i > 0)
            {
                aufsatz.spinningSpeed = aufsaetze[i - 1].spinningSpeed *  zahnradSizes[i-1] / zahnradSizes[i];
            }
            nextOneSpinning = nextOneSpinning && aufsatz.spinning;
        }
        solved = aufsaetze.Count > 0 && aufsaetze[aufsaetze.Count - 1].spinning; // last one spinns = puzzle solved
        if (Input.GetButton("Fire1"))
        {
            //aufsaetze[Random.Range(0,aufsaetze.Count)].RemoveZR();
        }
    }

    void StartRotating()
    {
        running = true;
    }

    void StopRotating()
    {
        running = false;
    }

    public override void Interact()
    {
        GameObject.Find("FPSController").GetComponent<PlayerControl>().SwapToCamera(riddleCam);
    }
}
