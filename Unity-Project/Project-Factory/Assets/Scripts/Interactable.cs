using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    string Name { get; }
    bool Selected { set; get; }
    string ToolTip { get; }
    void Interact();
}
