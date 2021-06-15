using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public bool isDestroyBlocksMode = false;
    public bool isSizeReducerMode = false;

    private bool isAlive = true;
    private bool isAnimationPlaying = true;
    private bool useInputDataForPositioning = false;
    private new Rigidbody2D rigidbody;
    private PooledBlock blockCollided;
    private float width;
    private float height;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        transform.position = new Vector3(0f, -1.5f, 0f);
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            transform.position = GetLastTouchPos();
        }

        if(blockCollided != null)
        {
            if (blockCollided.isAnimationComplete) isAnimationPlaying = false;
        }
    }

    #region Modes and State Management

    private void TurnOnDestroyMode()
    {
        isDestroyBlocksMode = true;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(10f);
        sequence.AppendCallback(TurnOffDestroyMode);
    }

    private void TurnOffDestroyMode()
    {
        isDestroyBlocksMode = false;
    }

    private void TurnOnSizeReducerMode()
    {
        isSizeReducerMode = true;
        ScaleDown();
    }

    private void ScaleDown()
    {
        Tween scaleTween = gameObject.transform.DOScale(gameObject.transform.localScale * 0.4f, 1f);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(scaleTween);
        sequence.AppendInterval(10f);
        sequence.AppendCallback(ScaleUp);
    }

    private void ScaleUp()
    {
        gameObject.transform.DOScale(gameObject.transform.localScale / 0.4f, 1f).OnComplete(TurnOffSizeReducerMode);
    }

    private void TurnOffSizeReducerMode()
    {
        isSizeReducerMode = false;
    }

    public void TurnOnControls()
    {
        useInputDataForPositioning = true;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public bool IsAnimationPlaying()
    {
        return isAnimationPlaying;
    }

    public void Disactivate(Vector3 pos)
    {
        rigidbody.simulated = false;
        isAlive = false;
        gameObject.transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliderGO = collision.collider.gameObject;
        ContactPoint2D contact = collision.GetContact(0);
        if (colliderGO.name == "PooledBlock(Clone)")
        {
            blockCollided = collision.collider.gameObject.GetComponent<PooledBlock>();

            if (isDestroyBlocksMode)
            {
                blockCollided.Disable();
            }
            else
            {
                Disactivate(contact.point);
            }
        }
        else if (colliderGO.name == "Destroyer(Clone)")
        {
            TurnOnDestroyMode();
        }
        else if (colliderGO.name == "SizeReducer(Clone)")
        {
            TurnOnSizeReducerMode();
        }
    }

    #endregion

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
