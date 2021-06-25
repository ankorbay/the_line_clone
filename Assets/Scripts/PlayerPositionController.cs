using UnityEngine;
using Zenject;

public class PlayerPositionController : MonoBehaviour
{
    bool useInputDataForPositioning = false;

    float width;
    float height;

    TouchPlane touchPlane;

    [Inject]
    void Construct(TouchPlane touchPlane)
    {
        this.touchPlane = touchPlane;
    }


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

    void UpdatePlayerPos()
    {
        if(useInputDataForPositioning)
        {
            transform.position = GetLastTouchPos();
        }
    }

    Vector3 GetLastTouchPos()
    {
        Vector2 limits = touchPlane.GetWorldPosTopAndBottomLimits();
        Vector3 lastPos = transform.position;
        Vector3 posTracked = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 resultPos = lastPos;

        if (posTracked.y >= limits.x && posTracked.y <= limits.y) // tracking mouse position
        {
            resultPos = new Vector3(posTracked.x, -1.5f, 0f);
        }

        if (Input.touchCount > 0) // tracking mobile touches
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;

                if (pos.y >= limits.x && pos.y <= limits.y)
                {
                    resultPos.x = pos.x;
                }
            }
        }

        return resultPos;
    }
}
