using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, Interactable
{
    public bool currentlyInteractable = true;
    protected string _interactionName = "interagieren";
    public string ToolTip { get => Name + (Name != "" ? ":\n":"") + "Drücke Linke Maustaste zum "+_interactionName; } // set => _toolTip = value; }

    [SerializeField]
    protected bool _selected = false;
    public bool Selected { get => _selected; set => _selected = value; }

    protected string _name = "";
    public string Name { get => _name; set => _name = value; }

    new public Renderer renderer;

    private Material mat;
    private float glow = 0.5f;
    public float glowPower = 0.75f;
    private int fadeDirection = 1;

    public bool isFocused = false;
    public Light OilLightFaker;

    public Collider col;
    public Camera riddleCam;

    public abstract void Interact();

    // unschön, sollte eigentlich in eigener Klasse Riddle extends InteractableObject ausgelagert werden. Aber egal Zeit ist knapp
    public void RiddleInteract()
    {
        PlayerControl pc = GameObject.Find("FPSController").GetComponent<PlayerControl>();
        pc.SwapToCamera(riddleCam, this);
        if (pc.LightOn)
        {
            EnableFakeLight();
        }
        else
        {
            DisableFakeLight();
        }
    }

    public void Start()
    {
        gameObject.isStatic = false;
        foreach (Transform t in transform)
        {
            t.gameObject.isStatic = false;
        }

        if (OilLightFaker == null)
        {
            OilLightFaker = GetComponentInChildren<Light>();
        }
        if (OilLightFaker != null)
        {
            OilLightFaker.enabled = false;
        }

        if (riddleCam == null)
        {
            riddleCam = GetComponentInChildren<Camera>();
        }
        if (riddleCam != null)
        {
            riddleCam.gameObject.SetActive(false);
        }

        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
            if(renderer == null)
            {
                renderer = GetComponentInChildren<Renderer>();
                if(renderer == null)
                {
                    Debug.Log("Konnte bei " + Name + " kein Renderer finden.");
                }
            }
        }
        col = GetComponent<Collider>();
        mat = renderer.material;
    }

    private void CycleHighlighting()
    {
        if (mat != null)
        {
            mat.EnableKeyword("_EMISSION");
            Color emissiveColor = new Color(glow, glow, glow, 0.05f);
            mat.SetColor("_EmissionColor", emissiveColor);

            //Debug.Log("HIGHLIGHTING the Object: " + gameObject.name + " with Color "+emissiveColor);
            if (glow >= glowPower  || glow <= 0)
            {
                fadeDirection *= -1;
            }
            glow += glowPower * fadeDirection * Time.deltaTime;
        }
    }


    private void StopHighlighting()
    {
        glow = glowPower/2;
        fadeDirection = 1;
        if (mat != null)
        {
            mat.DisableKeyword("_EMISSION");
        }
    }

    public void SwapBack()
    {
        DisableFakeLight();
    }

    public void Update()
    {
        if (isFocused)
        {
            PlayerControl pc = GameObject.Find("FPSController").GetComponent<PlayerControl>();
            if (pc.Oil <= 0)
            {
                DisableFakeLight();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    SwapEnableFakeLight();
                }
            }
        }
        if (_selected)
        {
            CycleHighlighting();
        }
        else
        {
            StopHighlighting();
        }
    }
    
    public void SwapEnableFakeLight()
    {
        OilLightFaker.enabled = !OilLightFaker.enabled;
    }

    public void EnableFakeLight()
    {
        OilLightFaker.enabled = true;
    }

    public void DisableFakeLight()
    {
        OilLightFaker.enabled = false;
    }
}
