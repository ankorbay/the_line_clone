﻿using UnityEngine;
using DG.Tweening;
using Zenject;

public class PooledSizeReducer : MonoBehaviour
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
        SizeReducerPool.Instance.Return(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy();
    }

    public class Factory : PlaceholderFactory<PooledSizeReducer>
    {
    }

}
