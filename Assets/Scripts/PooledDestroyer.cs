using UnityEngine;
using DG.Tweening;
using Zenject;

public class PooledDestroyer : MonoBehaviour
{
    public void DestoyOnTimeOut(float seconds)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.PrependInterval(seconds);
        sequence.AppendCallback(Destroy);
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
        DestroyerPool.Instance.Return(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy();
    }

    public class Factory : PlaceholderFactory<PooledDestroyer>
    {
    }
}
