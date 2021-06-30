using UnityEngine;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    public void FadeIn()
    {
        canvasGroup.DOFade(1, 1f);
        canvasGroup.interactable = true;
    }
}