using UnityEngine;

public class TouchPlane : MonoBehaviour
{
    float worldYPos;
    float worldValueHeight;


    void Start()
    {
        worldYPos = Camera.main.ScreenToWorldPoint(transform.position).y;
        worldValueHeight = Camera.main.ScreenToWorldPoint(new Vector3(GetComponent<RectTransform>().sizeDelta.y,0f)).x;
    }


    public Vector2 GetWorldPosTopAndBottomLimits()
    {
        return new Vector2(worldYPos + worldValueHeight / 2f, worldYPos - worldValueHeight / 2f);
    }
}
