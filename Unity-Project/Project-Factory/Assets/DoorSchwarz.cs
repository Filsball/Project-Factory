using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSchwarz : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !GetComponent<Animation>().isPlaying)
        {
            PlayDoorAnim();
        }
    }
    private int m_lastIndex = 0;

    public void PlayDoorAnim()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            if (m_lastIndex == 0)
            {
                GetComponent<Animation>().Play("Schwarz_Open");
                m_lastIndex = 1;
            }
            else
            {
                GetComponent<Animation>().Play("Schwarz_Close");
                m_lastIndex = 0;
            }
        }
    }
}
