using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    public bool isDestroyBlocksMode = false;
    public bool isSizeReducerMode = false;

    private int modeActiveTimeLeft;
    private bool isAlive = true;
    private bool isAnimationPlaying = true;

    private new Rigidbody2D rigidbody;
    private PooledBlock blockCollided;

    void Start()
    {
        float width = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        transform.localScale = new Vector3(width / 15f, width / 15f, 1f);
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfCollisionAnimationComplete();
    }

    public int GetModeActiveTimeLeft()
    {
        if(isDestroyBlocksMode || isSizeReducerMode)
        {
            return modeActiveTimeLeft;
        } else
        {
            return 0;
        }
    }

    private void CheckIfCollisionAnimationComplete()
    {
        if (blockCollided != null)
        {
            if (blockCollided.isAnimationComplete) isAnimationPlaying = false;
        }
    }

    #region Modes and State Management

    private void TurnOnDestroyMode(int modeDuration)
    {
        isDestroyBlocksMode = true;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(modeDuration);
        for (int i = 0; i <= modeDuration; i++)
        {
            int timeLeft = modeDuration - i;
            sequence.InsertCallback( i, ()=>UpdateTimeLeft(timeLeft));
        }
        sequence.AppendCallback(TurnOffDestroyMode);
    }

    private void UpdateTimeLeft(int i)
    {
        modeActiveTimeLeft = i;
    }

    private void TurnOffDestroyMode()
    {
        isDestroyBlocksMode = false;
    }

    private void TurnOnSizeReducerMode()
    {
        isSizeReducerMode = true;
        ScaleDown(10);
    }

    private void ScaleDown(int modeDuration)
    {
        int scaleUpDownDuration = 1;
        Tween scaleTween = gameObject.transform.DOScale(gameObject.transform.localScale * 0.4f, scaleUpDownDuration);

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

    private void ScaleUp(int duration)
    {
        gameObject.transform.DOScale(gameObject.transform.localScale / 0.4f, 1f).OnComplete(TurnOffSizeReducerMode);
    }

    private void TurnOffSizeReducerMode()
    {
        isSizeReducerMode = false;
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
            TurnOnDestroyMode(10);
        }
        else if (colliderGO.name == "SizeReducer(Clone)")
        {
            TurnOnSizeReducerMode();
        }
    }

    #endregion


}
