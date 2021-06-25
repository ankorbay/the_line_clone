using UnityEngine;
using DG.Tweening;
using Zenject;

public class PooledBlock : MonoBehaviour
{
    const int NUMBER_OF_COLS_PER_ROW = 7;


    public bool isDestroyable;
    public bool isAnimationComplete;

    SpriteRenderer renderer;
    Camera camera;

    Vector3 blockScale;
    Vector2 blocksMovementStep;


    void OnEnable()
    {
        camera = Camera.main;
        blockScale = ScreenData.CalculateBlockScale();
        transform.localScale = blockScale;
    }
    void Start()
    {
        blocksMovementStep = ScreenData.CalculateBlockStep(NUMBER_OF_COLS_PER_ROW);
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.y < -blocksMovementStep.y * 4f)
        {
            Disable();
        }
    }


    public void Disable()
    {
        BlockPool.Instance.Return(this);
        gameObject.SetActive(false);
        transform.DOKill();
    }


    #region Collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.gameObject.GetComponent<Player>().IsDestroyBlocksMode)
        {
            AnimateBlockCollision();
        }
    }

    void AnimateBlockCollision()
    {
        DOTween.KillAll();

        float playTime = 0.2f;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(renderer.DOColor(Color.yellow, playTime));
        mySequence.Append(renderer.DOColor(Color.red, playTime));
        mySequence.Append(renderer.DOColor(Color.yellow, playTime));
        mySequence.Append(renderer.DOColor(Color.red, playTime));
        mySequence.Append(renderer.DOColor(Color.yellow, playTime));
        mySequence.Append(renderer.DOColor(Color.red, playTime));
        mySequence.AppendCallback(SetAnimationComplete);
    }

    void SetAnimationComplete()
    {
        isAnimationComplete = true;
    }

    #endregion


    public class Factory : PlaceholderFactory<PooledBlock>
    {
    }
}