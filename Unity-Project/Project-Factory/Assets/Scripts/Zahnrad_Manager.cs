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

    public List<float> zahnradSizes;
    private float zrScaleRadius;

    public bool running = true;
    public bool solved = false;
    public bool instantiateGears = false;
    

    private Vector3 size;
    private bool playingAudio = false;
    private List<ZahnradAufsatz> aufsaetze = new List<ZahnradAufsatz>();
    [SerializeField]

    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();
        _interactionName = "näher zu treten";
        audioManager = FindObjectOfType<AudioManager>();
        boxCollider = (BoxCollider)col;
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
            float x = r - size.x/2; // coordinates of current Aufsatz
            float z = 0;
            if (i > 0)
            {
                float dist = r + zrScaleRadius * zahnradSizes[i - 1]/2 - 0.01f; // distance to previous Aufsatz
                float angle = 0;    // random angle to previous Aufsatz
                int errorCounter = -1; // to prevent while(true)
                //print("Distance: " + dist);
                do
                {
                    if (++errorCounter > 500000)
                    {
                        print("ERROR: Zahnraeder too big for ZahnradManager-Boundaries " + size +"! exiting ZahnradManager...");
                        return;
                    }
                    angle = 2 * Mathf.PI * (Random.Range(0.0f, 90.0f) - 45) / 360.0f;
                    //angle = 2 * Mathf.PI * (Random.Range(0.0f, 75.0f) - 30) / 360.0f;
                    x = (Mathf.Cos(angle) * dist) + aufsaetze[i - 1].transform.localPosition.x;
                    z = (Mathf.Sin(angle) * dist) + aufsaetze[i - 1].transform.localPosition.z;
                } while ( // while out of bounds
                    x + r >  size.x / 2 ||
                    x - r < -size.x / 2 ||
                    z + r >  size.z / 2 ||
                    z - r < -size.z / 2
                ) ;
        }
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
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();


        if(solved) {
            if (!playingAudio)
            {
                audioManager.generatorStarted = true;
                audioManager.Play("Zahnraeder", 0.1f, boxCollider.center + transform.position);
                audioManager.Play("GeneratorStartend", 1.0f, boxCollider.center + transform.position - new Vector3(2f, 2f, 2f));
                AudioManager.FadeCaller(audioManager.getSound("Saferoom").source, audioManager.getSound("Hintergrund").source, 0.7f, 1.5f, true);
                audioManager.setSaferoom(true);
                audioManager.setDunkelheit(false);
                audioManager.setHintergrund(false);
                Debug.Log("saferoom");
                playingAudio = true;
            }
        }
        for (int i= 0; i < aufsaetze.Count; i++)
        {
            ZahnradAufsatz cur = aufsaetze[i];

            //aufsatz.spinning = i == 0 || (nextOneSpinning && (aufsatz.zahnrad != null && aufsatz.zahnrad.transform.localScale.x == zahnradSizes[i]));
            if(i > 0)
            {
                ZahnradAufsatz pre = aufsaetze[i-1];

                //cur.spinning = pre.spinning && pre.zahnrad != null && pre.zahnrad.transform.localScale.x == zahnradSizes[i - 1] && cur.zahnrad.transform.localScale.x == zahnradSizes[i];
                float spaceBetweenOuter = ZahnradAufsatz.GetSpaceBetween(cur, pre)[1];
                cur.spinning = (pre.spinning && pre.zahnrad != null && spaceBetweenOuter < 0.01f);
                cur.spinningSpeed = aufsaetze[i - 1].spinningSpeed *  zahnradSizes[i-1] / zahnradSizes[i];
            }
            else
            {
                cur.spinning = true;
            }
        }
        solved = aufsaetze.Count > 0 && aufsaetze[aufsaetze.Count - 1].spinning; // last one spinns = puzzle solved
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
        RiddleInteract();
    }

    public List<ZahnradAufsatz> GetAdjacentAufsatz(ZahnradAufsatz za)
    {
        List<ZahnradAufsatz> neighbours = new List<ZahnradAufsatz>();
        int index = aufsaetze.IndexOf(za);
        if (index < 0) return neighbours;
        if (index - 1 >= 0)
        {
            neighbours.Add(aufsaetze[index - 1]);
        }
        if (index + 1 < aufsaetze.Count)
        {
            neighbours.Add(aufsaetze[index + 1]);
        }
        return neighbours;
    }
}
