using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    const float SCALE_DOWN_MODE_COEFF = 0.5f;


    public bool IsDestroyBlocksMode
    {
        get => isDestroyBlocksMode;
        private set => isDestroyBlocksMode = value;
    }

    public bool IsSizeReducerMode
    {
        get => isSizeReducerMode;
        private set => isSizeReducerMode = value;
    }

    bool isDestroyBlocksMode = false;
    bool isSizeReducerMode = false;
    bool isAlive = true;
    bool isAnimationPlaying = true;
    int modeActiveTimeLeft;

    Rigidbody2D rigidbody;
    PooledBlock blockCollided;
    SpriteRenderer renderer;


    void Start()
    {
        float screenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        transform.localScale = new Vector3(screenWidth / 15f, screenWidth / 15f, 1f);

        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckIfCollisionAnimationComplete();
    }


    public bool IsAlive()
    {
        return isAlive;
    }

    public bool IsAnimationPlaying()
    {
        return isAnimationPlaying;
    }

    public int GetModeActiveTimeLeft()
    {
        if(IsDestroyBlocksMode || IsSizeReducerMode)
        {
            return modeActiveTimeLeft;
        } else
        {
            return 0;
        }
    }


    #region Modes and State Management
    void CheckIfCollisionAnimationComplete()
    {
        if (blockCollided != null)
        {
            if (blockCollided.isAnimationComplete) isAnimationPlaying = false;
        }
    }

    void TurnOnDestroyMode(int modeDuration)
    {
        IsDestroyBlocksMode = true;

        AnimatePlayer(modeDuration);

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(modeDuration);
        for (int i = 0; i <= modeDuration; i++)
        {
            int timeLeft = modeDuration - i;
            sequence.InsertCallback(i, () => UpdateTimeLeft(timeLeft));
        }
        sequence.AppendCallback(TurnOffDestroyMode);
    }

    void AnimatePlayer(int modeDuration)
    {
        Color baseColor = renderer.color;

        float playTime = 1f;
        Sequence mySequence = DOTween.Sequence();
        for (int i = 0; i < (int)modeDuration/2; i++)
        {
            mySequence.Append(renderer.DOColor(Color.cyan, playTime));
            mySequence.Append(renderer.DOColor(baseColor, playTime));
        }
    }

    void UpdateTimeLeft(int i)
    {
        modeActiveTimeLeft = i;
    }

    void TurnOffDestroyMode()
    {
        IsDestroyBlocksMode = false;
    }

    void TurnOnSizeReducerMode()
    {
        IsSizeReducerMode = true;
        ScaleDown(10);
    }

    void ScaleDown(int modeDuration)
    {
        int scaleUpDownDuration = 1;
        Tween scaleTween = gameObject.transform.DOScale(gameObject.transform.localScale * SCALE_DOWN_MODE_COEFF, scaleUpDownDuration);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(scaleTween);
        sequence.AppendInterval(modeDuration);
        for (int i = 0; i <= modeDuration; i++)
        {
            int timeLeft = modeDuration - i;
            sequence.InsertCallback(i, () => UpdateTimeLeft(timeLeft));
        }
        sequence.AppendCallback(()=>ScaleUp(scaleUpDownDuration));
    }

    void ScaleUp(int duration)
    {
        gameObject.transform.DOScale(gameObject.transform.localScale / SCALE_DOWN_MODE_COEFF, duration).OnComplete(TurnOffSizeReducerMode);
    }

    void TurnOffSizeReducerMode()
    {
        IsSizeReducerMode = false;
    }

    void Disactivate(Vector3 pos)
    {
        rigidbody.simulated = false;
        isAlive = false;
        gameObject.transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliderGO = collision.collider.gameObject;
        ContactPoint2D contact = collision.GetContact(0);
        if (colliderGO.name == "PooledBlock(Clone)")
        {
            blockCollided = collision.collider.gameObject.GetComponent<PooledBlock>();

            if (IsDestroyBlocksMode)
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
            TurnOnDestroyMode(10);
        }
        else if (colliderGO.name == "SizeReducer(Clone)")
        {
            TurnOnSizeReducerMode();
        }
    }
    #endregion
}
