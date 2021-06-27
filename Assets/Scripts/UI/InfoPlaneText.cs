﻿using UnityEngine;
using UnityEngine.UI;

public class InfoPlaneText : MonoBehaviour
{
    Text txt;

    public void MakeVisible()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.transform.parent.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetText(string text)
    {
        txt = GetComponent<Text>();
        if(txt != null) txt.text = text;
    }    
}
