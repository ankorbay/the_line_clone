using UnityEngine;

public class TouchPlane : MonoBehaviour
{
    const float TOUCH_PLANE_SCALE = 0.14f;


    float worldYPos;
    float worldValueHeight;
    float newPlaneHeight;
    float frustrumHeight;

    RectTransform rectTransform;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        frustrumHeight = (int)Screen.height;

        newPlaneHeight = frustrumHeight * TOUCH_PLANE_SCALE;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newPlaneHeight);

        worldValueHeight = Camera.main.ScreenToWorldPoint(new Vector3(rectTransform.sizeDelta.y, 0f)).x;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, frustrumHeight / 6);
        worldYPos = Camera.main.ScreenToWorldPoint(rectTransform.anchoredPosition).y;
    }


    public float GetTouchPlaneScale()
    {
        return TOUCH_PLANE_SCALE;
    }

    public Vector2 GetWorldPosTopAndBottomLimits()
    {
        return new Vector2(worldYPos + worldValueHeight / 2f, worldYPos - worldValueHeight / 2f);
    }
}
