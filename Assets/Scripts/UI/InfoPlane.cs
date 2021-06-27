using UnityEngine;
using Zenject;

public class InfoPlane : MonoBehaviour
{
    const float INFO_PLANE_SCALE = 0.04f;

    RectTransform rectTransform;
    float newAncoredPositionY;
    float frustrumHeight;
    float touchPlaneHeight;
    float newInfoPlaneHeight;
    TouchPlane touchPlane;

    [Inject]
    public void Init(TouchPlane touchPlane)
    {
        this.touchPlane = touchPlane;
    }


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        frustrumHeight = (int)Screen.height;

        touchPlaneHeight = frustrumHeight * touchPlane.GetTouchPlaneScale();
        newInfoPlaneHeight = frustrumHeight * INFO_PLANE_SCALE;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newInfoPlaneHeight);

        newAncoredPositionY = frustrumHeight / 6f + touchPlaneHeight/2f + newInfoPlaneHeight/2f;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newAncoredPositionY);
    }
}
