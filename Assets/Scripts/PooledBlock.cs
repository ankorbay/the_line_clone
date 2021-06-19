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

    public Vector3 CalculateBlockScale()
    {
        return new Vector3(camera.pixelWidth / 224f, camera.pixelHeight / 224f, 1f);
    }

    public Vector2 CalculateBlockStep(int rows, int cols)
    {
        Vector2 frustrumSize = GetFrustrumSize();

        float stepX = frustrumSize.x / cols;
        float stepY = frustrumSize.y / rows;

        return new Vector2(stepX, stepY);
    }

    public Vector2 GetFrustrumSize()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = camera.ViewportToWorldPoint(topRightCorner);

        float height = edgeVector.y * 2;
        float width = edgeVector.x * 2;

        return new Vector2(width, height);
    }

    private void OnEnable()
    {
        camera = Camera.main;
        blockScale = CalculateBlockScale();
        transform.localScale = blockScale;
    }
    private void Start()
    {

        blocksMovementStep = CalculateBlockStep(7, 7);
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