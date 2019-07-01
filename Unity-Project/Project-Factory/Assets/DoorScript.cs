using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    // Start is called before the first frame update
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
                GetComponent<Animation>().Play("DoorAnimation");
                m_lastIndex = 1;
            }
            else
            {
                GetComponent<Animation>().Play("BackDoorAnimation");
                m_lastIndex = 0;
            }
        }
    }
}
