using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPlane : MonoBehaviour
{
    const float TOUCH_PLANE_SCALE = 0.14f;
    const float INFO_PLANE_SCALE = 0.04f;

    RectTransform rectTransform;
    float newAncoredPositionY;
    float frustrumHeight;
    float touchPlaneHeight;
    float newInfoPlaneHeight;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        frustrumHeight = (int)Screen.height;

        touchPlaneHeight = frustrumHeight * TOUCH_PLANE_SCALE;
        newInfoPlaneHeight = frustrumHeight * INFO_PLANE_SCALE;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newInfoPlaneHeight);

        newAncoredPositionY = frustrumHeight / 6f + touchPlaneHeight/2f + newInfoPlaneHeight/2f;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newAncoredPositionY);
    }
}
