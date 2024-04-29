using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage_btn : MonoBehaviour
{
    public GameObject Panel;
    private bool isPanelOpen;

    public void Storage()
    {
        if (!isPanelOpen)
        {
            Panel.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            Panel.GetComponent<CanvasGroup>().alpha = 1;
        }
        isPanelOpen = !isPanelOpen;
    }
}
