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
        worldValueHeight = Camera.main.ScreenToWorldPoint(new Vector3(rectTransform.sizeDelta.y, 0f)).x;
        worldYPos = Camera.main.ScreenToWorldPoint(rectTransform.anchoredPosition).y;
    }


    public Vector2 GetWorldPosTopAndBottomLimits()
    {
        return new Vector2(worldYPos + worldValueHeight / 2f, worldYPos - worldValueHeight / 2f);
    }
}
