using UnityEngine;

public class TouchPlane : MonoBehaviour
{
    const float TOUCH_PLANE_SCALE = 0.14f;

    float worldYPos;
    float worldValueHeight;
    float newPlaneHeight;

    RectTransform rectTransform;
    float frustrumHeight;
    float newPosY;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        frustrumHeight = (int)Screen.height;

        newPlaneHeight = frustrumHeight * TOUCH_PLANE_SCALE;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newPlaneHeight);

        worldYPos = Camera.main.ScreenToWorldPoint(transform.position).y;
        worldValueHeight = Camera.main.ScreenToWorldPoint(new Vector3(rectTransform.sizeDelta.y, 0f)).x;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, frustrumHeight / 6);
    }


    public Vector2 GetWorldPosTopAndBottomLimits()
    {
        return new Vector2(worldYPos + worldValueHeight / 2f, worldYPos - worldValueHeight / 2f);
    }
}
