using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    private bool useInputDataForPositioning = false;

    private float width;
    private float height;

    void Start()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        transform.position = new Vector3(0f, -1.5f, 0f);
    }

    void Update()
    {
        UpdatePlayerPos();
    }

    public void TurnOnControls()
    {
        useInputDataForPositioning = true;
    }

    public void TurnOffControls()
    {
        useInputDataForPositioning = false;
    }

    private void UpdatePlayerPos()
    {
        if(useInputDataForPositioning)
        {
            transform.position = GetLastTouchPos();
        }
    }

    private Vector3 GetLastTouchPos()
    {
        Vector3 posTracked = new Vector3(0f, 0f, 0f);
        if (useInputDataForPositioning) posTracked = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;

                posTracked.x = pos.x;
            }
        }

        return new Vector3(posTracked.x, -1.5f, 0f);
    }
}
