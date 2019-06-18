using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, Interactable
{
    public bool currentlyInteractable = true;
    protected string _toolTip = "";
    public string ToolTip { get => _toolTip; } // set => _toolTip = value; }

    protected bool _selected = false;
    public bool Selected { get => _selected; set => _selected = value; }

    protected string _name = "";
    public string Name { get => _name; set => _name = value; }


    private Material mat;
    private float glow = 0.5f;
    private int fadeDirection = 1;

    protected Collider col;

    public abstract void Interact();

    public void Start()
    {
        col = GetComponent<Collider>();
        mat = GetComponent<Renderer>().material;
        _toolTip = Name + ":\n drücke F zum interagieren";
    }

    private void CycleHighlighting()
    {
        if (mat != null)
        {
            mat.EnableKeyword("_EMISSION");
            Color emissiveColor = new Color(glow, glow, glow, 0.05f);
            mat.SetColor("_EmissionColor", emissiveColor);

            //Debug.Log("HIGHLIGHTING the Object: " + gameObject.name + " with Color "+emissiveColor);
            if (glow >= 0.75f || glow <= 0)
            {
                fadeDirection *= -1;
            }
            glow += 0.75f * fadeDirection * Time.deltaTime;
        }
    }


    private void StopHighlighting()
    {
        glow = 0.5f;
        fadeDirection = 1;
        if (mat != null)
        {
            mat.DisableKeyword("_EMISSION");
        }
    }

    public void Update()
    {
        if (_selected)
        {
            CycleHighlighting();
        }
        else
        {
            StopHighlighting();
        }
    }

}
