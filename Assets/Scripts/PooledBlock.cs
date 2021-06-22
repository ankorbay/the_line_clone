using UnityEngine;
using DG.Tweening;
using Zenject;

public class PooledBlock : MonoBehaviour
{
    public bool isDestroyable;
    public bool isAnimationComplete;

    private new SpriteRenderer renderer;
    private new Camera camera;
    private Vector3 blockScale;
    private Vector2 blocksMovementStep;

    public void Disable()
    {
        BlockPool.Instance.Return(this);
        gameObject.SetActive(false);
        transform.DOKill();
    }

    private void OnEnable()
    {
        camera = Camera.main;
        blockScale = ScreenData.CalculateBlockScale();
        transform.localScale = blockScale;
    }
    private void Start()
    {
        blocksMovementStep = ScreenData.CalculateBlockStep(7);
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (transform.position.y < -blocksMovementStep.y * 4f)
        {
            Disable();
        }
    }

    #region Collisions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.gameObject.GetComponent<Player>().isDestroyBlocksMode)
        {
            AnimateBlockCollision();
        }
    }

    private void AnimateBlockCollision()
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

    private void SetAnimationComplete()
    {
        isAnimationComplete = true;
    }

    #endregion

    public class Factory : PlaceholderFactory<PooledBlock>
    {
    }
}